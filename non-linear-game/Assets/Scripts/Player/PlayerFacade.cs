namespace Player {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    ///     Represents a facade for the player.
    /// </summary>
    public class PlayerFacade : MonoBehaviour, IDisposable {
        /// <summary>
        ///     The player model.
        /// </summary>
        private Player model;

        private LinkedList<IDisposable> observers;

        /// <summary>
        ///     Finalizes an instance of the <see cref="PlayerFacade"/> class.
        /// </summary>
        ~PlayerFacade() {
            this.Dispose(false);
        }

        /// <inheritdoc />
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Initializes an instance of the
        ///     <see cref="PlayerFacade"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        [Inject]
        // ReSharper disable once ParameterHidesMember
        public void Construct(Player model) {
            this.model = model;
            this.observers.AddLast(Observable.EveryUpdate().Where(
                _ => Input.GetMouseButtonDown(0)).Subscribe());
        }

        /// <summary>
        ///     Called when script is loaded.
        /// </summary>
        protected void Awake() {
            this.observers = new LinkedList<IDisposable>();
        }

        protected void Start() {

        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing,
        ///     releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">
        ///     Specifies whether this is being called by the
        ///      <see cref="Dispose"/> method.
        /// </param>
        protected virtual void Dispose(bool isDisposing) {
            this.ReleaseUnmanagedResources();
        }

        /// <summary>
        ///     Releases unmanaged resources.
        /// </summary>
        private void ReleaseUnmanagedResources() {
            this.model = null;
        }
    }
}
