namespace Scripts.Timeline.TimelinePiece {
    using System;

    using UnityEngine;

    using Zenject;

    using _3DPrimitives;

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
            this.Container.Bind<int>().FromInstance(this.settings.Layer);
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

            [SerializeField]
            private int layer;

            /// <summary>
            /// The mesh filter.
            /// </summary>
            public MeshFilter MeshFilter {
                get {
                    return this.meshFilter;
                }
            }

            /// <summary>
            /// The mesh collider.
            /// </summary>
            public MeshCollider MeshCollider {
                get {
                    return this.meshCollider;
                }
            }

            /// <summary>
            /// The transform.
            /// </summary>
            public Transform Transform {
                get {
                    return this.transform;
                }
            }

            public int Layer {
                get {
                    return this.layer;
                }
            }
        }
    }
}
