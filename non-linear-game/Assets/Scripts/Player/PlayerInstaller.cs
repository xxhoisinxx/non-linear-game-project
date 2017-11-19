namespace Player {
    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    ///     The player installer.
    /// </summary>
    public class PlayerInstaller : MonoInstaller {
        /// <summary>
        ///     The component settings.
        /// </summary>
        [SerializeField]
        private Player.Settings.Components componentSettings;

        /// <summary>
        ///     The movement settings.
        /// </summary>
        [SerializeField]
        private Player.Settings.Movement movementSettings;

        /// <summary>
        ///     The install bindings.
        /// </summary>
        public override void InstallBindings() {
            // ReSharper disable StyleCop.SA1110
            this.Container.BindInterfacesAndSelfTo<Player>().AsSingle();
            this.Container.Bind<Player.Settings.Movement>()
                .FromInstance(this.movementSettings);
            this.Container.Bind<Player.Settings.Components>()
                .FromInstance(this.componentSettings);
            this.Container
                .BindMemoryPool<PlayerMovementHandler,
                    PlayerMovementHandler.Pool>();
        }
    }
}