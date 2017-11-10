namespace Scrapbook {
    using UnityEngine;

    using Zenject;

    /// <summary>
    /// Represents a dependency injection installer for a ScrapbookManager
    /// </summary>
    public class ScrapbookManagerInstaller : MonoInstaller {
        /// <summary>
        /// The settings for the scrapbook manager.
        /// </summary>
        [SerializeField]
        private ScrapbookManager.Settings settings;

        /// <summary>
        /// Installs the bindings for this ScrapbookManager.
        /// </summary>
        public override void InstallBindings() {
            this.Container.Bind<ScrapbookManager.Settings>().FromInstance(this.settings);
        }
    }
}
