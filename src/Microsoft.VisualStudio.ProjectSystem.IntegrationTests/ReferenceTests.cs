// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Threading;

using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.ProjectSystem.VS
{
    [TestClass]
    public class ReferenceTests : TestBase
    {
        [TestMethod]
        public void RenamingTheReferencedProjectUpdatesTheP2PReference()
        {
            ProjectTestExtension referencingProject = default;
            ProjectTestExtension referencedProject = default;
            using (Scope.Enter("Create Projects"))
            {
                referencingProject = VisualStudio.ObjectModel.Solution.CreateProject(ProjectLanguage.CSharp, ProjectTemplate.CpsConsoleApplication);
                referencingProject.Rename("ReferencingProject");
                referencedProject = VisualStudio.ObjectModel.Solution.AddProject(ProjectLanguage.CSharp, ProjectTemplate.CpsConsoleApplication);
                referencedProject.Rename("ReferencedProject");
            }

            using (Scope.Enter("Verify Create Project"))
            {
                VisualStudio.ObjectModel.Solution.Verify.HasProject();
            }

            using (Scope.Enter("Adding Reference"))
            {
                referencingProject.References.Dte.AddProjectReference(referencedProject);
            }

            using (Scope.Enter("Verify Reference Added"))
            {
                Thread.Sleep(500);
                //Assert.IsTrue(referencingProject.References.TryFindReferenceByName("ReferencedProject", out var none));
            }

            using (Scope.Enter("Renaming project"))
            {
                referencedProject.Rename("ReferencedProjectRenamed");
                //referencedProject.Save();
            }

            using (Scope.Enter("Verify Referenes"))
            {
                bool originalReferenceFound = false;
                bool updatedReferenceFound = false;
                foreach (ReferenceTestExtension reference in referencingProject.References)
                {
                    if ("ReferencedProject".Equals(reference.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        originalReferenceFound = true;
                    }
                    else if ("ReferencedProjectRenamed".Equals(reference.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        updatedReferenceFound = true;
                    }
                }

                Assert.IsFalse(originalReferenceFound);
                Assert.IsTrue(updatedReferenceFound);
            }
        }
    }
}
