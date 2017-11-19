namespace Player {
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using log4net;

    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    ///     Represents a facade for the player.
    /// </summary>
    public class PlayerFacade : MonoBehaviour {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        }

        public class Pool : MonoMemoryPool<Camera, PlayerFacade> {
            protected override void Reinitialize(
                    Camera camera,
                    PlayerFacade item) {
                item.movementHandler = item.movementHandlerFactory.Spawn(camera);
                item.scaleHandler = item.scaleHandlerFactory.Spawn(camera);
                item.observers.AddLast(
                    Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0))
                        .Subscribe(item.movementHandler));
                item.observers.AddLast(Observable.EveryUpdate().Subscribe(
                    item.scaleHandler));
            }

            protected override void OnDespawned(PlayerFacade item) {
                item.gameObject.SetActive(false);
                item.movementHandler?.Dispose();
                foreach (var o in item.observers) {
                    o.Dispose();
                }
                item.movementHandlerFactory.Despawn(item.movementHandler);
                item.scaleHandlerFactory.Despawn(item.scaleHandler);
            }
        }
    }
}