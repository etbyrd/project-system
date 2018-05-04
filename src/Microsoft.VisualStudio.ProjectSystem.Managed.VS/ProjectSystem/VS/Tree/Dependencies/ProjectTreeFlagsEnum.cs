// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies
{
    public enum ProjectTreeFlagsEnum: ulong
    {
        Empty = 0,

        // These values come from ProjectTreeFlags.Common, make sure the values match
        
        
        FxAssemblyProjectFlags = 1,
        ProjectNodeFlags = 2,
        ShowEmptyProviderRootNode = 4,
        SupportsHierarchy = 8,
        BubbleUp = ProjectTreeFlags.Common.BubbleUp,
        SupportsRuleProperties = 32,
        GenericResolvedDependencyFlags = 64,
        GenericUnresolvedDependencyFlags = 128,
        SupportsRemove = 256,
        DiagnosticNodeFlags = 512,
        DiagnosticErrorNodeFlags = 1024,
        DiagnosticWarningNodeFlags = 2048,
        PackageNodeFlags = 4096,
        FrameworkAssembliesNodeFlags = 8192,
        SharedProjectFlags = 16384,
        DependencyFlags = 32768,
        SubTreeRootNodeFlags = 65536,
        ResolvedFlags = 131072,
        UnresolvedFlags = 262144,
        GenericDependencyFlags = 524288,
        BaseReferenceFlags = ProjectTreeFlags.Common.Reference,
        SdkSubTreeNodeFlags = 2097152,
        AnalyzerSubTreeRootNodeFlags = 4194304,
        AnalyzerSubTreeNodeFlags = 8388608,
        AssemblySubTreeRootNodeFlags = 16777216,
        AssemblySubTreeNodeFlags = 33554432,
        ComSubTreeRootNodeFlags = 67108864,
        ComSubTreeNodeFlags = 134217728,
        NuGetSubTreeNodeFlags = 268435456,
        TargetNodeFlags = 536870912,
        NuGetSubTreeRootNodeFlags = 1073741824,
        ProjectSubTreeRootNodeFlags = 2147483648,
        SdkSubTreeRootNodeFlags = 4294967296,
        HiddenProjectItem = 8589934592,
        DependenciesRootNodeFlags = 17179869184,
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
