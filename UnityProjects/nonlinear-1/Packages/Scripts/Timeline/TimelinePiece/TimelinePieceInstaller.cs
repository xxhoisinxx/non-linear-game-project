namespace Scripts.TimelinePiece {
    using System;

    using UnityEngine;

    using _3DPrimitives;

    using Zenject;

    /// <summary>
    /// The timeline piece installer.
    /// </summary>
    public class TimelinePieceInstaller : MonoInstaller {
        /// <summary>
        /// The settings.
        /// </summary>
        [SerializeField]
        private Settings settings;

        /// <summary>
        /// The install bindings.
        /// </summary>
        public override void InstallBindings() {
            this.Container.Bind<Sector>().AsSingle();
            this.Container.Bind<MeshFilter>().FromInstance(this.settings.MeshFilter);
            this.Container.Bind<MeshCollider>().FromInstance(this.settings.MeshCollider);
            this.Container.Bind<Transform>().FromInstance(this.settings.Transform);
        }

        /// <summary>
        /// The settings.
        /// </summary>
        [Serializable]
        public class Settings {
            /// <summary>
            /// The mesh filter.
            /// </summary>
            [SerializeField]
            private MeshFilter meshFilter;

            /// <summary>
            /// The mesh collider.
            /// </summary>
            [SerializeField]
            private MeshCollider meshCollider;

            /// <summary>
            /// The transform.
            /// </summary>
            [SerializeField]
            private Transform transform;

            /// <summary>
            /// The mesh filter.
            /// </summary>
            public MeshFilter MeshFilter => this.meshFilter;

            /// <summary>
            /// The mesh collider.
            /// </summary>
            public MeshCollider MeshCollider => this.meshCollider;

            /// <summary>
            /// The transform.
            /// </summary>
            public Transform Transform => this.transform;
        }
    }
}
