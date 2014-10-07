﻿using System;
using System.Diagnostics;
using NuGet.Client.ProjectSystem;
using NuGet.Versioning;

namespace NuGet.Client.VisualStudio.UI
{
    // This class is used to represent one of the following facts about a package:
    // - A version of the package is installed. In this case, property Version is not null. 
    //   Property IsSolution indicates if the package is installed in the solution or in a project.
    // - The package is not installed in a project/solution. In this case, property Version is null.
    public class PackageInstallationInfo
    {
        public NuGetVersion Version
        {
            get;
            private set;
        }

        public bool Selected
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public Project Project
        {
            get;
            private set;
        }

        private string _name;
        
        public bool IsSolution
        {
            get;
            private set;
        }

        public PackageInstallationInfo(Project project, NuGetVersion version, bool enabled)
        {
            Project = project;
            _name = Project.Name;
            Version = version;
            Enabled = enabled;
            IsSolution = false;
        }

        public PackageInstallationInfo(string name, NuGetVersion version, bool enabled, Project project)
        {
            _name = name;
            Version = version;
            Enabled = enabled;
            IsSolution = true;

            // this is just a placeholder and will not be really used. It's used to avoid
            // lots of null checks in our code.
            Project = project;
        }

        public override string ToString()
        {
            if (Version == null)
            {
                return _name;
            }
            else
            {
                return string.Format("{0} ({1})", _name,
                    Version.ToString());
            }
        }
    }
}