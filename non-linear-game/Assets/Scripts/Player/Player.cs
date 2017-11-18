namespace Player {
    using System;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The player.
    /// </summary>
    public class Player : IInitializable {
        /// <summary>
        /// The movement settings.
        /// </summary>
        [Inject]
        private Settings.Movement movementSettings;

        [Inject]
        private Settings.Components componentSettings;

        /// <summary>
        /// The initialize.
        /// </summary>
        public void Initialize() {
        }

        /// <summary>
        /// The settings.
        /// </summary>
        [Serializable]
        internal class Settings {
            /// <summary>
            /// The components.
            /// </summary>
            [Serializable]
            internal class Components {
                /// <summary>
                /// The rigid body.
                /// </summary>
                [SerializeField]
                private Rigidbody rigidBody;

                /// <summary>
                /// The transform.
                /// </summary>
                [SerializeField]
                private Transform transform;

                /// <summary>
                /// Gets or sets the rigid body.
                /// </summary>
                internal Rigidbody RigidBody {
                    get {
                        return this.rigidBody;
                    }

                    set {
                        this.rigidBody = value;
                    }
                }

                /// <summary>
                /// Gets or sets the transform.
                /// </summary>
                internal Transform Transform {
                    get {
                        return this.transform;
                    }

                    set {
                        this.transform = value;
                    }
                }
            }

            /// <summary>
            /// The movement.
            /// </summary>
            [Serializable]
            internal class Movement {
                /// <summary>
                /// The speed.
                /// </summary>
                [SerializeField]
                private float speed;

                /// <summary>
                /// Gets or sets the speed.
                /// </summary>
                internal float Speed {
                    get {
                        return this.speed;
                    }

                    set {
                        this.speed = value;
                    }
                }
            }
        }
    }
}
