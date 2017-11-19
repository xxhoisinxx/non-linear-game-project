namespace Player {
    using System;

    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The player.
    /// </summary>
    public class Player : IInitializable {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="movementSettings">
        /// The movement settings.
        /// </param>
        /// <param name="componentSettings">
        /// The component settings.
        /// </param>
        [Inject]
        internal Player(
            Settings.Movement movementSettings,
            Settings.Components componentSettings) {
            this.MovementSpeed = movementSettings.Speed;
            this.Transform = componentSettings.Transform;
            this.RigidBody = componentSettings.RigidBody;
        }

        /// <summary>
        /// Gets or sets the movement speed.
        /// </summary>
        public FloatReactiveProperty MovementSpeed { get; set; }

        /// <summary>
        /// Gets the rigid body.
        /// </summary>
        public Rigidbody RigidBody { get; }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// The initialize.
        /// </summary>
        public void Initialize() { }

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
                /// Gets the rigid body.
                /// </summary>
                internal Rigidbody RigidBody {
                    get {
                        return this.rigidBody;
                    }
                }

                /// <summary>
                /// Gets the transform.
                /// </summary>
                internal Transform Transform {
                    get {
                        return this.transform;
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
                private FloatReactiveProperty speed;

                [SerializeField]
                private IntReactiveProperty raycastLayer;

                /// <summary>
                /// Gets or sets the speed.
                /// </summary>
                internal FloatReactiveProperty Speed {
                    get {
                        return this.speed;
                    }
                }

                /// <summary>
                /// Gets the raycast layer.
                /// </summary>
                internal IntReactiveProperty RaycastLayer {
                    get {
                        return this.raycastLayer;
                    }
                }
            }
        }
    }
}