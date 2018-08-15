// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies
{
    public enum ProjectTreeFlagsEnum: ulong
    {
        Empty = 0,
        // These values come from ProjectTreeFlags.Common, make sure the values match
        //these should be hex values 
        FxAssemblyProjectFlags = 0x0001,
        ProjectNodeFlags = 0x0002,
        ShowEmptyProviderRootNode = 0x0004,
        SupportsHierarchy = 0x0008,
        BubbleUp = ProjectTreeFlags.Common.BubbleUp,
        SupportsRuleProperties = 0x0020,
        GenericResolvedDependencyFlags = ResolvedDependencyFlags | ResolvedReferenceFlags | GenericDependencyFlags,
        GenericUnresolvedDependencyFlags = UnresolvedDependencyFlags | UnresolvedReferenceFlags | GenericDependencyFlags,
        SupportsRemove = 0x0100,
        DiagnosticNodeFlags = 0x0200,
        DiagnosticErrorNodeFlags = 0x0400,
        DiagnosticWarningNodeFlags = 0x0800,
        PackageNodeFlags = 0x1000,
        FrameworkAssembliesNodeFlags = 0x2000,
        SharedProjectFlags = 0x4000 | SharedProjectImportReferenceFlag,
        DependencyFlags = ProjectTreeFlags.Common.VirtualFolder | ProjectTreeFlags.Common.BubbleUp | SupportsRuleProperties | SupportsRemove,
        SubTreeRootNodeFlags = 0x00010000,
        ResolvedFlags = 0x00020000,
        UnresolvedFlags = 0x00040000,
        GenericDependencyFlags = 0x00080000,
        BaseReferenceFlags = ProjectTreeFlags.Common.Reference,
        ReferencesFolder = ProjectTreeFlags.Common.ReferencesFolder,
        AnalyzerSubTreeRootNodeFlags = 0x00400000,
        AnalyzerSubTreeNodeFlags = 0x00800000,
        AssemblySubTreeRootNodeFlags = 0x01000000,
        AssemblySubTreeNodeFlags = 0x02000000,
        SharedProjectImportReferenceFlag = ProjectTreeFlags.Common.SharedProjectImportReference,
        ComSubTreeNodeFlags = 0x08000000,
        VirtualFolder = ProjectTreeFlags.Common.VirtualFolder,
        TargetNodeFlags = 0x20000000,
        NuGetSubTreeRootNodeFlags = 0x40000000,
        ProjectSubTreeRootNodeFlags = 0x80000000,
        SdkSubTreeRootNodeFlags = 0x0100000000,
        HiddenProjectItem = 0x0200000000,
        DependenciesRootNode = 0x0400000000,
        SdkSubTreeNodeFlags = 0x0800000000,
        NuGetSubTreeNodeFlags = 0x1000000000,
        ComSubTreeRootNodeFlags = 0x2000000000,
        UnresolvedReferenceFlags = BaseReferenceFlags | ProjectTreeFlags.Common.BrokenReference,
        ResolvedReferenceFlags = BaseReferenceFlags | ProjectTreeFlags.Common.ResolvedReference,
        UnresolvedDependencyFlags = UnresolvedFlags | DependencyFlags,
        ResolvedDependencyFlags = ResolvedFlags | DependencyFlags,
        DependenciesRootNodeFlags = BubbleUp | ReferencesFolder | VirtualFolder | DependenciesRootNode,
    }

    internal static class ProjectTreeFlagsEnumExtension
    {
        public static ProjectTreeFlagsEnum Except(this ProjectTreeFlagsEnum source, ProjectTreeFlagsEnum toBeRemoved)
        {
            return source & ~toBeRemoved;
        }

        public static bool Contains(this ProjectTreeFlagsEnum source, ProjectTreeFlagsEnum contained)
        {
            return (source & contained) == contained;
        }

        public static ProjectTreeFlagsEnum Union(this ProjectTreeFlagsEnum source, ProjectTreeFlagsEnum added)
        {
            return source | added;
        }

        public static ProjectTreeFlags ToProjectTreeFlags(this ProjectTreeFlagsEnum? source)
        {
            return source == null
                ? ProjectTreeFlags.Empty
                : ToProjectTreeFlags(source.Value);
        }

        public static ProjectTreeFlags ToProjectTreeFlags(this ProjectTreeFlagsEnum source)
        {
            // TODO: Implement this
            //throw new NotImplementedException();
            if (source.Contains(ProjectTreeFlagsEnum.BubbleUp)) {
                return ProjectTreeFlags.BubbleUp;
            } 
            return ProjectTreeFlags.Empty;
        }
    }
}
