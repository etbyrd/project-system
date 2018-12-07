﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.ProjectSystem.References;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.CrossTarget;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Models;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Snapshot;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies
{
    [Export(typeof(IDependenciesTreeViewProvider))]
    [AppliesTo(ProjectCapability.DependenciesTree)]
    [Order(Order)]
    internal class GroupedByTargetTreeViewProvider : TreeViewProviderBase
    {
        private const int Order = 1000;

        [ImportingConstructor]
        public GroupedByTargetTreeViewProvider(
            IDependenciesTreeServices treeServices,
            IDependenciesViewModelFactory viewModelFactory,
            IUnconfiguredProjectCommonServices commonServices)
            : base(commonServices.Project)
        {
            TreeServices = treeServices;
            ViewModelFactory = viewModelFactory;
            CommonServices = commonServices;
        }

        private IDependenciesTreeServices TreeServices { get; }
        private IDependenciesViewModelFactory ViewModelFactory { get; }
        private IUnconfiguredProjectCommonServices CommonServices { get; }

        /// <summary>
        /// Builds Dependencies tree for given dependencies snapshot
        /// </summary>
        public override async Task<IProjectTree> BuildTreeAsync(
            IProjectTree dependenciesTree,
            IDependenciesSnapshot snapshot,
            CancellationToken cancellationToken = default)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "BuildTreeAsync" });
            

            IProjectTree originalTree = dependenciesTree;

            var currentTopLevelNodes = new List<IProjectTree>();

            IProjectTree RememberNewNodes(IProjectTree rootNode, IEnumerable<IProjectTree> currentNodes)
            {
                if (currentNodes != null)
                {
                    currentTopLevelNodes.AddRange(currentNodes);
                }

                return rootNode;
            }

            if (snapshot.Targets.Count(x => !x.Key.Equals(TargetFramework.Any)) == 1)
            {
                foreach ((ITargetFramework _, ITargetedDependenciesSnapshot targetedSnapshot) in snapshot.Targets)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return originalTree;
                    }

                    dependenciesTree = await BuildSubTreesAsync(
                        rootNode: dependenciesTree,
                        snapshot.ActiveTarget,
                        targetedSnapshot,
                        RememberNewNodes);
                }
            }
            else
            {
                foreach ((ITargetFramework targetFramework, ITargetedDependenciesSnapshot targetedSnapshot) in snapshot.Targets)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return originalTree;
                    }

                    if (targetFramework.Equals(TargetFramework.Any))
                    {
                        dependenciesTree = await BuildSubTreesAsync(
                            rootNode: dependenciesTree,
                            snapshot.ActiveTarget,
                            targetedSnapshot,
                            RememberNewNodes);
                    }
                    else
                    {
                        IProjectTree node = dependenciesTree.FindChildWithCaption(targetFramework.FriendlyName);
                        bool shouldAddTargetNode = node == null;
                        IDependencyViewModel targetViewModel = ViewModelFactory.CreateTargetViewModel(targetedSnapshot);

                        node = CreateOrUpdateNode(
                            node,
                            targetViewModel,
                            rule: null,
                            isProjectItem: false,
                            additionalFlags: ProjectTreeFlags.Create(ProjectTreeFlags.Common.BubbleUp));

                        node = await BuildSubTreesAsync(
                            rootNode: node, 
                            snapshot.ActiveTarget, 
                            targetedSnapshot, 
                            CleanupOldNodes);

                        dependenciesTree = shouldAddTargetNode 
                            ? dependenciesTree.Add(node).Parent 
                            : node.Parent;

                        currentTopLevelNodes.Add(node);
                    }
                }
            }

            dependenciesTree = CleanupOldNodes(dependenciesTree, currentTopLevelNodes);

            // now update root Dependencies node status
            ProjectImageMoniker rootIcon = ViewModelFactory.GetDependenciesRootIcon(snapshot.HasUnresolvedDependency).ToProjectSystemType();
            return dependenciesTree.SetProperties(icon: rootIcon, expandedIcon: rootIcon);
        }

        public override IProjectTree FindByPath(IProjectTree root, string path)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "FindByPath" });
            if (root == null)
            {
                return null;
            }

            IProjectTree dependenciesNode = root.Flags.Contains(DependencyTreeFlags.DependenciesRootNodeFlags) 
                ? root 
                : root.GetSubTreeNode(DependencyTreeFlags.DependenciesRootNodeFlags);

            return dependenciesNode?.GetSelfAndDescendentsBreadthFirst()
                .FirstOrDefault((node, p) => string.Equals(node.FilePath, p, StringComparison.OrdinalIgnoreCase), path);
        }

        /// <summary>
        /// Builds all available sub trees under root: target framework or Dependencies node 
        /// when there is only one target.
        /// </summary>
        private async Task<IProjectTree> BuildSubTreesAsync(
            IProjectTree rootNode,
            ITargetFramework activeTarget,
            ITargetedDependenciesSnapshot targetedSnapshot,
            Func<IProjectTree, IEnumerable<IProjectTree>, IProjectTree> syncFunc)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "BuildSubTreesAsync" });
            var groupedByProviderType = new Dictionary<string, List<IDependency>>(StringComparers.DependencyProviderTypes);

            foreach (IDependency dependency in targetedSnapshot.TopLevelDependencies)
            {
                if (!dependency.Visible)
                {
                    if (dependency.Flags.Contains(DependencyTreeFlags.ShowEmptyProviderRootNode))
                    {
                        // if provider sends special invisible node with flag ShowEmptyProviderRootNode, we 
                        // need to show provider node even if it does not have any dependencies.
                        groupedByProviderType.Add(dependency.ProviderType, new List<IDependency>());
                    }

                    continue;
                }

                if (!groupedByProviderType.TryGetValue(dependency.ProviderType, out List<IDependency> dependencies))
                {
                    dependencies = new List<IDependency>();
                    groupedByProviderType.Add(dependency.ProviderType, dependencies);
                }

                dependencies.Add(dependency);
            }

            var currentNodes = new List<IProjectTree>(capacity: groupedByProviderType.Count);

            bool isActiveTarget = targetedSnapshot.TargetFramework.Equals(activeTarget);
            foreach ((string providerType, List<IDependency> dependencies) in groupedByProviderType)
            {
                IDependencyViewModel subTreeViewModel = ViewModelFactory.CreateRootViewModel(
                    providerType, targetedSnapshot.CheckForUnresolvedDependencies(providerType));
                IProjectTree subTreeNode = rootNode.FindChildWithCaption(subTreeViewModel.Caption);
                bool isNewSubTreeNode = subTreeNode == null;

                ProjectTreeFlags excludedFlags = targetedSnapshot.TargetFramework.Equals(TargetFramework.Any) 
                    ? ProjectTreeFlags.Create(ProjectTreeFlags.Common.BubbleUp) 
                    : ProjectTreeFlags.Empty;

                subTreeNode = CreateOrUpdateNode(
                    subTreeNode,
                    subTreeViewModel,
                    rule: null,
                    isProjectItem: false,
                    excludedFlags: excludedFlags);

                subTreeNode = await BuildSubTreeAsync(
                    subTreeNode,
                    targetedSnapshot,
                    dependencies,
                    isActiveTarget,
                    shouldCleanup: !isNewSubTreeNode);

                currentNodes.Add(subTreeNode);

                rootNode = isNewSubTreeNode 
                    ? rootNode.Add(subTreeNode).Parent 
                    : subTreeNode.Parent;
            }

            return syncFunc(rootNode, currentNodes);
        }

        /// <summary>
        /// Builds a sub tree under root: target framework or Dependencies node when there is only one target.
        /// </summary>
        private async Task<IProjectTree> BuildSubTreeAsync(
            IProjectTree rootNode,
            ITargetedDependenciesSnapshot targetedSnapshot,
            List<IDependency> dependencies,
            bool isActiveTarget,
            bool shouldCleanup)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "BuildSubTreeAsync" });
            List<IProjectTree> currentNodes = shouldCleanup 
                ? new List<IProjectTree>(capacity: dependencies.Count) 
                : null;

            foreach (IDependency dependency in dependencies)
            {
                IProjectTree dependencyNode = rootNode.FindChildWithCaption(dependency.Caption);
                bool isNewDependencyNode = dependencyNode == null;

                if (!isNewDependencyNode
                    && dependency.Flags.Contains(DependencyTreeFlags.SupportsHierarchy))
                {
                    if ((dependency.Resolved && dependencyNode.Flags.Contains(DependencyTreeFlags.UnresolvedFlags))
                        || (!dependency.Resolved && dependencyNode.Flags.Contains(DependencyTreeFlags.ResolvedFlags)))
                    {
                        // when transition from unresolved to resolved or vise versa - remove old node
                        // and re-add new  one to allow GraphProvider to recalculate children
                        isNewDependencyNode = true;
                        rootNode = dependencyNode.Remove();
                        dependencyNode = null;
                    }
                }

                dependencyNode = await CreateOrUpdateNodeAsync(dependencyNode, dependency, targetedSnapshot, isActiveTarget);

                currentNodes?.Add(dependencyNode);

                rootNode = isNewDependencyNode
                    ? rootNode.Add(dependencyNode).Parent
                    : dependencyNode.Parent;
            }

            return shouldCleanup 
                ? CleanupOldNodes(rootNode, currentNodes) 
                : rootNode;
        }

        /// <summary>
        /// Removes nodes that don't exist anymore
        /// </summary>
        private static IProjectTree CleanupOldNodes(IProjectTree rootNode, IEnumerable<IProjectTree> currentNodes)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "CleanupOldNodes" });
            foreach (IProjectTree nodeToRemove in rootNode.Children.Except(currentNodes))
            {
                rootNode = rootNode.Remove(nodeToRemove);
            }

            return rootNode;
        }

        private async Task<IProjectTree> CreateOrUpdateNodeAsync(
            IProjectTree node,
            IDependency dependency,
            ITargetedDependenciesSnapshot targetedSnapshot,
            bool isProjectItem,
            ProjectTreeFlags? additionalFlags = null,
            ProjectTreeFlags? excludedFlags = null)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "CreateOrUpdateNodeAsync" });
            IRule rule = null;
            if (dependency.Flags.Contains(DependencyTreeFlags.SupportsRuleProperties))
            {
                rule = await TreeServices.GetRuleAsync(dependency, targetedSnapshot.Catalogs);
            }

            return CreateOrUpdateNode(
                node,
                dependency.ToViewModel(targetedSnapshot),
                rule,
                isProjectItem,
                additionalFlags,
                excludedFlags);
        }

        private IProjectTree CreateOrUpdateNode(
            IProjectTree node,
            IDependencyViewModel viewModel,
            IRule rule,
            bool isProjectItem,
            ProjectTreeFlags? additionalFlags = null,
            ProjectTreeFlags? excludedFlags = null)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "CreateOrUpdateNode" });
            if (node != null)
            {
                return UpdateTreeNode(node, viewModel, rule);
            }

            return isProjectItem
                ? CreateProjectItemTreeNode(viewModel, rule, additionalFlags, excludedFlags)
                : CreateProjectTreeNode(viewModel, rule, additionalFlags, excludedFlags);
        }

        private IProjectTree CreateProjectTreeNode(
            IDependencyViewModel viewModel,
            IRule rule,
            ProjectTreeFlags? additionalFlags = null,
            ProjectTreeFlags? excludedFlags = null)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "CreateProjectTreeNode" });
            // For IProjectTree remove ProjectTreeFlags.Common.Reference flag, otherwise CPS would fail to 
            // map this node to graph node and GraphProvider would be never called. 
            // Only IProjectItemTree can have this flag
            ProjectTreeFlags flags = FilterFlags(viewModel.Flags.Except(DependencyTreeFlags.BaseReferenceFlags),
                additionalFlags,
                excludedFlags);
            string filePath = (viewModel.OriginalModel != null && viewModel.OriginalModel.TopLevel &&
                               viewModel.OriginalModel.Resolved)
                ? viewModel.OriginalModel.GetTopLevelId()
                : viewModel.FilePath;

            return TreeServices.CreateTree(
                caption: viewModel.Caption,
                filePath: filePath,
                visible: true,
                browseObjectProperties: rule,
                flags: flags,
                icon: viewModel.Icon.ToProjectSystemType(),
                expandedIcon: viewModel.ExpandedIcon.ToProjectSystemType());
        }

        private IProjectTree CreateProjectItemTreeNode(
            IDependencyViewModel viewModel,
            IRule rule,
            ProjectTreeFlags? additionalFlags = null,
            ProjectTreeFlags? excludedFlags = null)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "CreateProjectItemTreeNode" });
            ProjectTreeFlags flags = FilterFlags(viewModel.Flags, additionalFlags, excludedFlags);
            string filePath = (viewModel.OriginalModel != null && viewModel.OriginalModel.TopLevel &&
                               viewModel.OriginalModel.Resolved)
                ? viewModel.OriginalModel.GetTopLevelId()
                : viewModel.FilePath;

            var itemContext = ProjectPropertiesContext.GetContext(
                CommonServices.Project,
                file: filePath,
                itemType: viewModel.SchemaItemType,
                itemName: filePath);

            return TreeServices.CreateTree(
                caption: viewModel.Caption,
                itemContext: itemContext,
                propertySheet: null,
                browseObjectProperties: rule,
                icon: viewModel.Icon.ToProjectSystemType(),
                expandedIcon: viewModel.ExpandedIcon.ToProjectSystemType(),
                visible: true,
                flags: flags);
        }

        private IProjectTree UpdateTreeNode(
            IProjectTree node,
            IDependencyViewModel viewModel,
            IRule rule)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "UpdateTreeNode" });
            ProjectTreeCustomizablePropertyContext updatedNodeParentContext = GetCustomPropertyContext(node.Parent);

            var updatedValues = new ReferencesProjectTreeCustomizablePropertyValues
            {
                Caption = viewModel.Caption,
                Flags = viewModel.Flags,
                Icon = viewModel.Icon.ToProjectSystemType(),
                ExpandedIcon = viewModel.ExpandedIcon.ToProjectSystemType()
            };

            ApplyProjectTreePropertiesCustomization(updatedNodeParentContext, updatedValues);

            return node.SetProperties(
                caption: updatedValues.Caption,
                browseObjectProperties: rule,
                icon: updatedValues.Icon,
                expandedIcon: updatedValues.ExpandedIcon,
                flags: updatedValues.Flags);
        }

        private static ProjectTreeFlags FilterFlags(
            ProjectTreeFlags flags,
            ProjectTreeFlags? additionalFlags,
            ProjectTreeFlags? excludedFlags)
        {
            System.IO.File.AppendAllLines("C:\\Users\\etbyrd\\desktop\\log.txt", new[] { "FilterFlags" });
            if (additionalFlags.HasValue)
            {
                flags = flags.Union(additionalFlags.Value);
            }

            if (excludedFlags.HasValue)
            {
                flags = flags.Except(excludedFlags.Value);
            }

            return flags;
        }
    }
}
