// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies
{
    public enum ProjectTreeFlagsEnum
    {
        Empty = 0,

        // These values come from ProjectTreeFlags.Common, make sure the values match
        BubbleUp = ProjectTreeFlags.Common.BubbleUp,
        BaseReferenceFlags = ProjectTreeFlags.Common.Reference,
        FxAssemblyProjectFlags = 1048577,
        ProjectNodeFlags = 1048578,
        ShowEmptyProviderRootNode = 1048579,
        SupportsHierarchy = 1048580,
        SupportsRuleProperties = 1048581,
        GenericResolvedDependencyFlags = 1048582,
        GenericUnresolvedDependencyFlags = 1048583,
        SupportsRemove = 1048584,
        DiagnosticNodeFlags = 1048585,
        DiagnosticErrorNodeFlags = 1048586,
        DiagnosticWarningNodeFlags = 1048587,
        PackageNodeFlags = 1048588,
        FrameworkAssembliesNodeFlags = 1048589,
        SharedProjectFlags = 1048590,
        DependencyFlags = 1048591,
        SubTreeRootNodeFlags = 1048592,
        ResolvedFlags = 1048593,
        UnresolvedFlags = 1048594,
        GenericDependencyFlags = 1048595,
        SdkSubTreeNodeFlags = 1048596,
        AnalyzerSubTreeRootNodeFlags = 1048597,
        AnalyzerSubTreeNodeFlags = 1048598,
        AssemblySubTreeRootNodeFlags = 1048599,
        AssemblySubTreeNodeFlags = 1048600,
        ComSubTreeRootNodeFlags = 1048601,
        ComSubTreeNodeFlags = 1048602,
        NuGetSubTreeNodeFlags = 1048603,
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
            throw new NotImplementedException();
        }
    }
}
