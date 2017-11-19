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

        private PlayerMovementHandler movementHandler;

        private PlayerMovementHandler.Pool movementHandlerFactory;

        /// <summary>
        ///     The observers.
        /// </summary>
        private LinkedList<IDisposable> observers;

        private PlayerScaleHandler scaleHandler;

        private PlayerScaleHandler.Pool scaleHandlerFactory;

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
        /// <param name="scaleHandlerFactory">
        ///     The scale Handler Factory.
        /// </param>
        [Inject]
        // ReSharper disable ParameterHidesMember
        public void Construct(
                Player model,
                PlayerMovementHandler.Pool movementHandlerFactory,
                PlayerScaleHandler.Pool scaleHandlerFactory) {
            // ReSharper enable ParameterHidesMember
            this.observers = new LinkedList<IDisposable>();
            this.movementHandlerFactory = movementHandlerFactory;
            this.scaleHandlerFactory = scaleHandlerFactory;

            this.model = model;
            this.movementHandler = this.movementHandlerFactory.Spawn();
            this.scaleHandler = this.scaleHandlerFactory.Spawn();
        }

        /// <inheritdoc />
        public void Dispose() {
            this.movementHandler?.Dispose();
            foreach (var o in this.observers) {
                o.Dispose();
            }
            this.movementHandlerFactory.Despawn(this.movementHandler);
            this.scaleHandlerFactory.Despawn(this.scaleHandler);
        }

        protected void Start() {
            this.observers.AddLast(
                Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0))
                    .Subscribe(this.movementHandler));
            this.observers.AddLast(Observable.EveryUpdate().Subscribe(
                this.scaleHandler));
        }
    }
}