﻿using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Microsoft.HockeyApp.Services
{
    /// <summary>
    /// The application service
    /// </summary>
    internal sealed class ApplicationService : IApplicationService
    {
        /// <summary>
        /// The application identifier
        /// </summary>
        private string applicationId;

        /// <summary>
        /// The version
        /// </summary>
        private string version;

        /// <summary>
        /// Occurs when an app is resuming.
        /// </summary>
        public event EventHandler OnResuming;

        /// <summary>
        /// Occurs when an app is suspending.
        /// </summary>
        public event EventHandler OnSuspending;

        /// <summary>
        /// Initializes the service.
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// Indicates whether the application is installed in development mode.
        /// </summary>
        /// <returns>
        /// True if a package is installed in development mode, otherwise false.
        /// </returns>
        public bool IsDevelopmentMode() { return false; }

        /// <summary>
        /// Gets the version for the current application. If the version cannot be found, we will return the passed in default.
        /// </summary>
        /// <returns>
        /// The extracted data.
        /// </returns>
        public string GetVersion()
        {
            if (version == null)
            {
                try
                {
                    version = GetVersionUsingClickOnce().ToString();
                }
                catch (Exception)
                {
                    version = GetVersionUsingAssembly().ToString();
                }
            }
            return version;
        }

        /// <summary>
        /// Gets the version using click once.
        /// </summary>
        /// <returns>The version managed by ClickOnce</returns>
        private Version GetVersionUsingClickOnce()
        {
            var type = Type.GetType("System.Deployment.Application.ApplicationDeployment");
            object deployment = type.GetMethod("CurrentDeployment").Invoke(null, null);
            return type.GetMethod("CurrentVersion").Invoke(deployment, null) as Version;
        }

        /// <summary>
        /// Gets the version using assembly.
        /// </summary>
        /// <returns>The assembly version</returns>
        private Version GetVersionUsingAssembly()
        {
            return Assembly.GetEntryAssembly().GetName().Version;
        }

        /// <summary>
        /// Gets the application id, which is the namespace name for App class.
        /// </summary>
        /// <returns>
        /// Namespace name for App class.
        /// </returns>
        public string GetApplicationId()
        {
            if (applicationId == null)
            {
                applicationId = Application.Current.GetType().Namespace;
            }
            return applicationId;
        }

        /// <summary>
        /// Gets the application store region.
        /// </summary>
        /// <returns>
        /// The two-letter identifier for the user's region.
        /// </returns>
        public string GetStoreRegion() { return RegionInfo.CurrentRegion.TwoLetterISORegionName; }
    }
}