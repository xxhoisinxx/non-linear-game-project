namespace Scenes.StandardScene {
    using System;

    using ButtonClickHandler;

    using UnityEngine;
    using UnityEngine.UI;

    using Zenject;

    /// <summary>
    ///     Represents a dependency injector for a standard scene.
    /// </summary>
    public class StandardSceneInstaller : MonoInstaller {
        [SerializeField]
        private Settings settings;

        /// <summary>
        ///     Installs the bindings for this scene.
        /// </summary>
        public override void InstallBindings() {
            this.Container.Bind<Tuple<Button, string>>().FromInstance(
                new Tuple<Button, string>(
                    this.settings.LoadScrapbookSceneButton,
                    this.settings.ScrapbookSceneName));
        }

        [Serializable]
        internal class Settings {
            [SerializeField]
            private Button loadScrapbookSceneButton;

            [SerializeField]
            private string scrapbookSceneName;

            internal Button LoadScrapbookSceneButton {
                get {
                    return this.loadScrapbookSceneButton;
                }
            }

            internal string ScrapbookSceneName {
                get {
                    return this.scrapbookSceneName;
                }
            }
        }
    }
}