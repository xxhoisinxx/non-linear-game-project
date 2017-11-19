namespace Project {
    using SceneLoadedHandlers;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// Represents the manager of the entire project.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class ProjectManager : MonoBehaviour {
        /// <summary>
        /// The scene loaded handler.
        /// </summary>
        private ISceneLoadedHandler sceneLoadedHandler;

        /// <summary>
        /// Initializes the dependencies of this <see cref="ProjectManager"/>.
        /// </summary>
        /// <param name="handler">
        /// The scene loaded handler.
        /// </param>
        [Inject]
        public void Construct(ISceneLoadedHandler handler) {
            this.sceneLoadedHandler = handler;
        }

        // Use this for initialization
        protected void Start() {

        }

        // Update is called once per frame
        protected void Update() {

        }


    }
}
