﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.CrossTarget;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Snapshot;

using Moq;

using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.ProjectSystem.VS
{
    internal class IDependencyFactory
    {
        public static IDependency Create()
        {
            return Mock.Of<IDependency>();
        }

        public static Mock<IDependency> Implement(string providerType = null,
                                            string id = null,
                                            string originalItemSpec = null,
                                            string path = null,
                                            string fullPath = null,
                                            string name = null,
                                            string caption = null,
                                            string alias = null,
                                            IEnumerable<string> dependencyIDs = null,
                                            bool? resolved = null,
                                            bool? topLevel = null,
                                            bool? isImplicit = null,
                                            ProjectTreeFlagsEnum? flags = null,
                                            string setPropertiesCaption = null,
                                            bool? setPropertiesResolved = null,
                                            ProjectTreeFlagsEnum? setPropertiesFlags = null,
                                            bool? setPropertiesImplicit = null,
                                            bool? equals = null,
                                            IImmutableList<string> setPropertiesDependencyIDs = null,
                                            string setPropertiesSchemaName = null,
                                            ITargetFramework targetFramework = null,
                                            MockBehavior? mockBehavior = null)
        {
            var behavior = mockBehavior ?? MockBehavior.Strict;
            var mock = new Mock<IDependency>(behavior);

            if (providerType != null)
            {
                mock.Setup(x => x.ProviderType).Returns(providerType);
            }

            if (id != null)
            {
                mock.Setup(x => x.Id).Returns(id);
            }

            if (originalItemSpec != null)
            {
                mock.Setup(x => x.OriginalItemSpec).Returns(originalItemSpec);
            }

            if (fullPath != null)
            {
                mock.Setup(x => x.FullPath).Returns(fullPath);
            }

            if (path != null)
            {
                mock.Setup(x => x.Path).Returns(path);
            }

            if (name != null)
            {
                mock.Setup(x => x.Name).Returns(name);
            }

            if (caption != null)
            {
                mock.Setup(x => x.Caption).Returns(caption);
            }

            if (alias != null)
            {
                mock.Setup(x => x.Alias).Returns(alias);
            }

            if (dependencyIDs != null)
            {
                mock.Setup(x => x.DependencyIDs).Returns(ImmutableList<string>.Empty.AddRange(dependencyIDs));
            }

            if (resolved != null && resolved.HasValue)
            {
                mock.Setup(x => x.Resolved).Returns(resolved.Value);
            }

            if (topLevel != null && topLevel.HasValue)
            {
                mock.Setup(x => x.TopLevel).Returns(topLevel.Value);
            }

            if (isImplicit != null && isImplicit.HasValue)
            {
                mock.Setup(x => x.Implicit).Returns(isImplicit.Value);
            }

            if (flags != null && flags.HasValue)
            {
                mock.Setup(x => x.Flags).Returns(flags.Value);
            }

            if (targetFramework != null)
            {
                mock.Setup(x => x.TargetFramework).Returns(targetFramework);
            }

            if (setPropertiesCaption != null
                || setPropertiesDependencyIDs != null
                || setPropertiesResolved != null
                || setPropertiesFlags != null
                || setPropertiesImplicit != null)
            {
                mock.Setup(x => x.SetProperties(
                            setPropertiesCaption,
                            setPropertiesResolved,
                            setPropertiesFlags,
                            setPropertiesSchemaName,
                            setPropertiesDependencyIDs,
                            It.IsAny<ImageMoniker>(),
                            It.IsAny<ImageMoniker>(),
                            setPropertiesImplicit))
                    .Returns(mock.Object);
            }

            if (equals != null && equals.HasValue && equals.Value)
            {
                mock.Setup(x => x.Equals(It.IsAny<IDependency>())).Returns(true);
            }

            return mock;
        }

        public static IDependency FromJson(
            string jsonString,
            ProjectTreeFlagsEnum? flags = null,
            ImageMoniker? icon = null,
            ImageMoniker? expandedIcon = null,
            ImageMoniker? unresolvedIcon = null,
            ImageMoniker? unresolvedExpandedIcon = null,
            Dictionary<string, string> properties = null,
            IEnumerable<string> dependenciesIds = null,
            ITargetFramework targetFramework = null)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return null;
            }

            var json = JObject.Parse(jsonString);
            var data = json.ToObject<TestDependency>();

            if (flags != null && flags.HasValue)
            {
                data.Flags = data.Flags.Union(flags.Value);
            }

            if (icon != null && icon.HasValue)
            {
                data.Icon = icon.Value;
            }

            if (expandedIcon != null && expandedIcon.HasValue)
            {
                data.ExpandedIcon = expandedIcon.Value;
            }

            if (unresolvedIcon != null && unresolvedIcon.HasValue)
            {
                data.UnresolvedIcon = unresolvedIcon.Value;
            }

            if (unresolvedExpandedIcon != null && unresolvedExpandedIcon.HasValue)
            {
                data.UnresolvedExpandedIcon = unresolvedExpandedIcon.Value;
            }

            if (properties != null)
            {
                data.Properties = ImmutableStringDictionary<string>.EmptyOrdinal.AddRange(properties);
            }

            if (dependenciesIds != null)
            {
                data.DependencyIDs = ImmutableList<string>.Empty.AddRange(dependenciesIds);
            }

            if (targetFramework != null)
            {
                data.TargetFramework = targetFramework;
            }

            return data;
        }
    }
}
