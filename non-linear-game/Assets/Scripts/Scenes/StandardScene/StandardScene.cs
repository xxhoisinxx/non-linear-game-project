namespace Scenes.StandardScene {
    using System;

    using ButtonClickHandler;

    using UnityEngine;
    using UnityEngine.UI;

    using Zenject;

    /// <summary>
    /// Represents a standard level scene.
    /// </summary>
    public class StandardScene : MonoBehaviour {
        private LoadSceneButtonHandler.Pool loadSceneButtonFactory;

        private Tuple<Button, string> loadScrapBookSceneButton;

        [Inject]
        public void Construct(
                LoadSceneButtonHandler.Pool buttonFactory,
                Tuple<Button, string> scrapBookButton) {
            this.loadSceneButtonFactory = buttonFactory;
            this.loadScrapBookSceneButton = scrapBookButton;
        }

        protected void Awake() {
            this.loadSceneButtonFactory.Spawn(
                this.loadScrapBookSceneButton.Item1,
                this.loadScrapBookSceneButton.Item2);
        }
    }
}
