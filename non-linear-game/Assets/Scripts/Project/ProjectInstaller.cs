﻿using Zenject;

namespace Project {
    using log4net;

    using Logging;

    using Scrapbook;

    using Vuforia;

    /// <summary>
    /// Represents a dependency injection installer for a project
    /// </summary>
    public class ProjectInstaller : MonoInstaller {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Installs the bindings for this project.
        /// </summary>
        public override void InstallBindings() {
            // Setup Log4Net
            LogConfigurationManager.ConfigureAllLogging();
            Log.Info("[Success] Logging configured");
            this.Container.BindInterfacesAndSelfTo<ScrapbookManager>().AsSingle();
            this.Container.Bind<TrackerManager>().FromInstance(TrackerManager.Instance);
            Log.Info("[Success] Project bindings installed");
/*            ((log4net.Repository.Hierarchy.Logger)log.Logger).Level = log4net.Core.Level.Debug;*/
        }
    }
}
