namespace Player {
    using System;
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

        private PlayerMovementHandler.Pool movementHandlerFactory;

        private PlayerMovementHandler movementHandler;

        /// <summary>
        ///     The observers.
        /// </summary>
        private LinkedList<IDisposable> observers;

        /// <summary>
        ///     Finalizes an instance of the <see cref="PlayerFacade" /> class.
        /// </summary>
        ~PlayerFacade() {
            this.Dispose(false);
        }

        /// <summary>
        ///     Initializes an instance of the
        ///     <see cref="PlayerFacade" /> class.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="movementHandlerFactory">
        ///     The movement handler factory.
        /// </param>
        [Inject]
        // ReSharper disable ParameterHidesMember
        public void Construct(
                Player model,
                PlayerMovementHandler.Pool movementHandlerFactory) {
        // ReSharper enable ParameterHidesMember
            this.observers = new LinkedList<IDisposable>();
            this.movementHandlerFactory = movementHandlerFactory;

            this.model = model;
            this.movementHandler = this.movementHandlerFactory.Spawn();
        }

        protected void Start() {
            this.observers.AddLast(
                Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0))
                    .Subscribe(this.movementHandler));
        }

        /// <inheritdoc />
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing,
        ///     releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">
        ///     Specifies whether this is being called by the
        ///     <see cref="Dispose" /> method.
        /// </param>
        protected virtual void Dispose(bool isDisposing) {
            if (!isDisposing) {
                return;
            }
            this.model = null;
            foreach (var o in this.observers) {
                o.Dispose();
                this.movementHandlerFactory.Despawn(this.movementHandler);
            }
        }
    }
}