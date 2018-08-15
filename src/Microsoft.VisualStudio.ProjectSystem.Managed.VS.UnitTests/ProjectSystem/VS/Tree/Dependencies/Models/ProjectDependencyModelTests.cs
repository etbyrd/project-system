﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;

using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Models;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Snapshot;

using Xunit;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies
{
    [Trait("UnitTest", "ProjectSystem")]
    public class ProjectDependencyModelTests
    {
        [Fact]
        public void Resolved()
        {
            var properties = ImmutableStringDictionary<string>.EmptyOrdinal.Add("myProp", "myVal");

            var flag = ProjectTreeFlagsEnum.Empty;
            var model = new ProjectDependencyModel(
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
            Assert.Equal(KnownMonikers.Application, model.Icon);
            Assert.Equal(KnownMonikers.Application, model.ExpandedIcon);
            Assert.Equal(ManagedImageMonikers.ApplicationWarning, model.UnresolvedIcon);
            Assert.Equal(ManagedImageMonikers.ApplicationWarning, model.UnresolvedExpandedIcon);
            Assert.True(model.Flags.Contains(ProjectTreeFlagsEnum.SupportsHierarchy));
            Assert.True(model.Flags.Contains(flag));
        }

        [Fact]
        public void Unresolved()
        {
            var properties = ImmutableStringDictionary<string>.EmptyOrdinal.Add("myProp", "myVal");

            var flag = ProjectTreeFlagsEnum.Empty;
            var model = new ProjectDependencyModel(
                "myProvider",
                "c:\\myPath.dll",
                "myOriginalItemSpec",
                flags: flag,
                resolved: false,
                isImplicit: false,
                properties: properties);

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
            Assert.Equal(KnownMonikers.Application, model.Icon);
            Assert.Equal(KnownMonikers.Application, model.ExpandedIcon);
            Assert.Equal(ManagedImageMonikers.ApplicationWarning, model.UnresolvedIcon);
            Assert.Equal(ManagedImageMonikers.ApplicationWarning, model.UnresolvedExpandedIcon);
            Assert.True(model.Flags.Contains(ProjectTreeFlagsEnum.SupportsHierarchy));
            Assert.True(model.Flags.Contains(flag));
        }

        [Fact]
        public void Implicit()
        {
            var properties = ImmutableStringDictionary<string>.EmptyOrdinal.Add("myProp", "myVal");

            var flag = ProjectTreeFlagsEnum.Empty;
            var model = new ProjectDependencyModel(
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
            Assert.Equal(ManagedImageMonikers.ApplicationPrivate, model.Icon);
            Assert.Equal(ManagedImageMonikers.ApplicationPrivate, model.ExpandedIcon);
            Assert.Equal(ManagedImageMonikers.ApplicationWarning, model.UnresolvedIcon);
            Assert.Equal(ManagedImageMonikers.ApplicationWarning, model.UnresolvedExpandedIcon);
            Assert.True(model.Flags.Contains(ProjectTreeFlagsEnum.SupportsHierarchy));
            Assert.True(model.Flags.Contains(flag));
        }
    }
}
