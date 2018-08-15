// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;

using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Models;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Snapshot;

using Xunit;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies
{
    [Trait("UnitTest", "ProjectSystem")]
    public class SharedProjectDependencyModelTests
    {
        [Fact]
        public void SharedResolved()
        {
            var properties = ImmutableStringDictionary<string>.EmptyOrdinal.Add("myProp", "myVal");

            var flag = ProjectTreeFlagsEnum.Empty;
            var model = new SharedProjectDependencyModel(
                "myProvider",
                "c:\\myPath.dll",
                "myOriginalItemSpec",
                flags: flag,
                resolved: true,
                isImplicit: false,
                properties: properties);

            Assert.Equal("myProvider", model.ProviderType);
            Assert.Equal("c:\\myPath.dll", model.Path);
            Assert.Equal("myOriginalItemSpec", model.OriginalItemSpec);
            Assert.Equal("myPath", model.Caption);
            Assert.Equal(ResolvedProjectReference.SchemaName, model.SchemaName);
            Assert.True(model.Resolved);
            Assert.False(model.Implicit);
            Assert.Equal(properties, model.Properties);
            Assert.Equal(Dependency.ProjectNodePriority, model.Priority);
            Assert.Equal(ProjectReference.PrimaryDataSourceItemType, model.SchemaItemType);
            Assert.Equal(KnownMonikers.SharedProject, model.Icon);
            Assert.Equal(KnownMonikers.SharedProject, model.ExpandedIcon);
            Assert.Equal(ManagedImageMonikers.SharedProjectWarning, model.UnresolvedIcon);
            Assert.Equal(ManagedImageMonikers.SharedProjectWarning, model.UnresolvedExpandedIcon);
            Assert.True(model.Flags.Contains(ProjectTreeFlagsEnum.SharedProjectFlags));
            Assert.False(model.Flags.Contains(ProjectTreeFlagsEnum.SupportsRuleProperties));
            Assert.True(model.Flags.Contains(flag));
        }

        [Fact]
        public void Unresolved()
        {
            var properties = ImmutableStringDictionary<string>.EmptyOrdinal.Add("myProp", "myVal");

            //This was previously ProjectTreeFlags.Create("MyCustomFlag")
            // Question: Do we need to support custom flags? 

            //var flag = ProjectTreeFlagsEnum.Empty;
            var model = new SharedProjectDependencyModel(
                "myProvider",
                "c:\\myPath.dll",
                "myOriginalItemSpec",
                flags: ProjectTreeFlagsEnum.Empty,
                resolved: false,
                isImplicit: false,
                properties: properties);

            // the last tests were:
            // "VirtualFolder BubbleUp Unresolved MyCustomFlag GenericDependency BrokenReference 
            // Dependency SharedProjectDependency SupportsRemove SharedProjectImportReference Reference"
            // but we only have GenericUnresolvedDependencyFlags and SharedProjectFlags
            // that can't be right...
            Assert.Equal("myProvider", model.ProviderType);
            Assert.Equal("c:\\myPath.dll", model.Path);
            Assert.Equal("myOriginalItemSpec", model.OriginalItemSpec);
            Assert.Equal("myPath", model.Caption);
            Assert.Equal(ProjectReference.SchemaName, model.SchemaName);
            Assert.False(model.Resolved);
            Assert.False(model.Implicit);
            Assert.Equal(properties, model.Properties);
            Assert.Equal(Dependency.ProjectNodePriority, model.Priority);
            Assert.Equal(ProjectReference.PrimaryDataSourceItemType, model.SchemaItemType);
            Assert.Equal(KnownMonikers.SharedProject, model.Icon);
            Assert.Equal(KnownMonikers.SharedProject, model.ExpandedIcon);
            Assert.Equal(ManagedImageMonikers.SharedProjectWarning, model.UnresolvedIcon);
            Assert.Equal(ManagedImageMonikers.SharedProjectWarning, model.UnresolvedExpandedIcon);
            Assert.True(model.Flags.Contains(ProjectTreeFlagsEnum.SharedProjectFlags));
            Assert.False(model.Flags.Contains(ProjectTreeFlagsEnum.SupportsRuleProperties));
            //is this really neccesary if we are starting from empty? x & 0 will always be 0
            Assert.True(model.Flags.Contains(ProjectTreeFlagsEnum.Empty));
        }

        [Fact]
        public void Implicit()
        {
            var properties = ImmutableStringDictionary<string>.EmptyOrdinal.Add("myProp", "myVal");

            var flag = ProjectTreeFlagsEnum.Empty;
            var model = new SharedProjectDependencyModel(
                "myProvider",
                "c:\\myPath.dll",
                "myOriginalItemSpec",
                flags: flag,
                resolved: true,
                isImplicit: true,
                properties: properties);

            Assert.Equal("myProvider", model.ProviderType);
            Assert.Equal("c:\\myPath.dll", model.Path);
            Assert.Equal("myOriginalItemSpec", model.OriginalItemSpec);
            Assert.Equal("myPath", model.Caption);
            Assert.Equal(ResolvedProjectReference.SchemaName, model.SchemaName);
            Assert.True(model.Resolved);
            Assert.True(model.Implicit);
            Assert.Equal(properties, model.Properties);
            Assert.Equal(Dependency.ProjectNodePriority, model.Priority);
            Assert.Equal(ProjectReference.PrimaryDataSourceItemType, model.SchemaItemType);
            Assert.Equal(ManagedImageMonikers.SharedProjectPrivate, model.Icon);
            Assert.Equal(ManagedImageMonikers.SharedProjectPrivate, model.ExpandedIcon);
            Assert.Equal(ManagedImageMonikers.SharedProjectWarning, model.UnresolvedIcon);
            Assert.Equal(ManagedImageMonikers.SharedProjectWarning, model.UnresolvedExpandedIcon);
            Assert.True(model.Flags.Contains(ProjectTreeFlagsEnum.SharedProjectFlags));
            Assert.False(model.Flags.Contains(ProjectTreeFlagsEnum.SupportsRuleProperties));
            Assert.True(model.Flags.Contains(flag));
        }
    }
}
