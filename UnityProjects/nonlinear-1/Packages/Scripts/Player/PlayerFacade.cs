namespace Scripts.Player {
    using UnityEngine;

    using Zenject;

    /// <inheritdoc />
    /// <summary>
    /// The player facade.
    /// </summary>
    public class PlayerFacade : MonoBehaviour {
        /// <summary>
        /// The model.
        /// </summary>
        private Player model;

        /// <summary>
        /// The construct.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        [Inject]
        public void Construct(Player model) {
            this.model = model;
        }
    }
}
