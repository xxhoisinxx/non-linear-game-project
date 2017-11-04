/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Memes are Dreams Studios. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Scripts.Timeline {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The sectored cylinder.
    /// </summary>
    public class Timeline : IInitializable, ITickable {
        /// <summary>
        /// The default float.
        /// </summary>
        public static readonly float DefaultFloat = 0.001f;

        /// <summary>
        /// The timeline Pieces.
        /// </summary>
        private readonly LinkedList<TimelinePiece.TimelinePiece> timelinePieces;

        /// <summary>
        /// The uniform sector depth.
        /// </summary>
        private float uniformSectorDepth;

        /// <summary>
        /// The uniform sector depth.
        /// </summary>
        private float uniformSectorHeight;

        /// <summary>
        /// The component settings.
        /// </summary>
        [Inject]
        private Settings.Components componentSettings;

        /// <summary>
        /// The dimensions settings.
        /// </summary>
        [Inject]
        private Settings.Dimensions dimensionsSettings;

        /// <summary>
        /// The radius settings.
        /// </summary>
        [Inject]
        private Settings.Radius radiusSettings;

        /// <summary>
        /// The speed settings.
        /// </summary>
        [Inject]
        private Settings.Speeds speedSettings;

        /// <summary>
        /// The radius.
        /// </summary>
        private float radius;

        /// <summary>
        /// The radius.
        /// </summary>
        private float minRadius;

        /// <summary>
        /// The radius.
        /// </summary>
        private float maxRadius;

        /// <summary>
        /// The pulsation interval.
        /// </summary>
        private float pulsationInterval;

        private float pulsationRate;

        private int rotationSpeed;

        /// <summary>
        /// The sector factory.
        /// </summary>
        [Inject]
        private TimelinePiece.TimelinePiece.Pool timelineFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timeline"/> class.
        /// </summary>
        public Timeline() {
            this.timelinePieces = new LinkedList<TimelinePiece.TimelinePiece>();
        }

        /// <summary>
        /// Gets the number of timeline pieces.
        /// </summary>
        public int NumberOfTimelinePieces {
            get {
                return this.timelinePieces.Count;
            }
        }

        /// <summary>
        /// Gets or sets the uniform sector depth.
        /// </summary>
        public float UniformSectorDepth {
            get {
                return this.uniformSectorDepth;
            }

            set {
                value = Mathf.Clamp(value, DefaultFloat, float.MaxValue);
                this.uniformSectorDepth = value;
                if (Math.Abs(this.dimensionsSettings.UniformSectorDepth.Value - value) > float.Epsilon) {
                    this.dimensionsSettings.UniformSectorDepth.Value = value;
                }
                foreach (var piece in this.timelinePieces) {
                    piece.Sector.Depth = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the uniform sector height.
        /// </summary>
        public float UniformSectorHeight {
            get {
                return this.uniformSectorHeight;
            }

            set {
                value = Mathf.Clamp(value, DefaultFloat, float.MaxValue);
                this.uniformSectorHeight = value;
                if (Math.Abs(this.dimensionsSettings.UniformSectorHeight.Value - value) > float.Epsilon) {
                    this.dimensionsSettings.UniformSectorHeight.Value = value;
                }
                foreach (var piece in this.timelinePieces) {
                    piece.Sector.Height = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the transform.
        /// </summary>
        public Transform Transform {
            get {
                return this.componentSettings.Transform;
            }

            set {
                this.componentSettings.Transform = value;
            }
        }

        /// <summary>
        /// Gets or sets the min radius.
        /// </summary>
        public float MinRadius {
            get {
                return this.minRadius;
            }

            set {
                value = Mathf.Clamp(value, 0, this.MaxRadius);
                this.minRadius = value;
                if (Math.Abs(this.radiusSettings.MinRadius.Value - value) > float.Epsilon) {
                    this.radiusSettings.MinRadius.Value = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the max radius.
        /// </summary>
        public float MaxRadius {
            get {
                return this.maxRadius;
            }

            set {
                value = Mathf.Clamp(value, DefaultFloat, float.MaxValue);
                value = Mathf.Clamp(value, this.MinRadius, float.MaxValue);
                this.maxRadius = value;
                if (Math.Abs(this.radiusSettings.MaxRadius.Value - value) > float.Epsilon) {
                    this.radiusSettings.MaxRadius.Value = value;
                }
            }
        }

        /// <summary>
        /// Gets the radius.
        /// </summary>
        public float Radius {
            get {
                return this.radius;
            }

            private set {
                this.radius = Mathf.Clamp(value, this.MinRadius, this.MaxRadius);
                if (Math.Abs(this.radiusSettings.CurrentRadius.Value - value) > float.Epsilon) {
                    this.radiusSettings.CurrentRadius.Value = value;
                }
                foreach (var piece in this.timelinePieces) {
                    Debug.DrawRay(piece.Sector.Transform.position, piece.Sector.Transform.forward * 10, Color.red);
                    piece.Sector.Transform.localPosition = Vector3.zero;
                    piece.Sector.Transform.Translate(piece.Sector.Transform.forward * this.radius, Space.World);
                }
            }
        }

        /// <summary>
        /// Gets or sets the rotation speed.
        /// </summary>
        public int RotationSpeed {
            get {
                return this.rotationSpeed;
            }

            set {
                this.rotationSpeed = value;
                if (this.speedSettings.RotationSpeed.Value != value) {
                    this.speedSettings.RotationSpeed.Value = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the pulsation rate.
        /// </summary>
        public float PulsationRate {
            get {
                return this.pulsationRate;
            }

            set {
                value = Mathf.Clamp(value, 0, float.MaxValue);
                this.pulsationRate = value;
                if (Math.Abs(this.speedSettings.PulsationRate.Value - value) > float.Epsilon) {
                    this.speedSettings.PulsationRate.Value = value;
                }
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public void Initialize() {
            this.speedSettings.PulsationRate.Subscribe(value => {
                if (Math.Abs(this.PulsationRate - value) > float.Epsilon) {
                    this.PulsationRate = value;
                }
            });
            this.PulsationRate = this.speedSettings.PulsationRate.Value;

            this.speedSettings.RotationSpeed.Subscribe(value => {
                if (this.RotationSpeed != value) {
                    this.RotationSpeed = value;
                }
            });
            this.RotationSpeed = this.speedSettings.RotationSpeed.Value;

            this.radiusSettings.MinRadius.Subscribe(value => {
                if (Math.Abs(this.MinRadius - value) > float.Epsilon) {
                    this.MinRadius = value;
                }
            });
            this.MinRadius = this.radiusSettings.MinRadius.Value;

            this.radiusSettings.MaxRadius.Subscribe(value => {
                if (Math.Abs(this.MaxRadius - value) > float.Epsilon) {
                    this.MaxRadius = value;
                }
            });
            this.MaxRadius = this.radiusSettings.MaxRadius.Value;

            this.radiusSettings.CurrentRadius.Subscribe(value => {
                if (Math.Abs(this.Radius - value) > float.Epsilon) {
                    this.Radius = value;
                }
            });
            this.Radius = this.radiusSettings.CurrentRadius.Value;

            this.dimensionsSettings.UniformSectorDepth.Subscribe(value => {
                if (Math.Abs(this.UniformSectorDepth - value) > float.Epsilon) {
                    this.UniformSectorDepth = value;
                }
            });
            this.UniformSectorDepth = this.dimensionsSettings.UniformSectorDepth.Value;

            this.dimensionsSettings.UniformSectorHeight.Subscribe(value => {
                if (Math.Abs(this.UniformSectorHeight - value) > float.Epsilon) {
                    this.UniformSectorHeight = value;
                }
            });
            this.UniformSectorHeight = this.dimensionsSettings.UniformSectorHeight.Value;

            this.dimensionsSettings.NumberOfTimelinePieces.Subscribe(
                value => {
                    value = Mathf.Clamp(value, 3, int.MaxValue);
                    var diff = Mathf.Abs(value - this.NumberOfTimelinePieces);
                    if (diff == 0) {
                        return;
                    }
                    if (value > this.NumberOfTimelinePieces) {
                        for (var i = 0; i < diff; i++) {
                            this.AddLast();
                        }
                    }
                    else if (value < this.NumberOfTimelinePieces) {
                        for (var i = 0; i < diff; i++) {
                            this.RemoveLast();
                        }
                    }
                    var counter = 0;
                    foreach (var piece in this.timelinePieces) {
                        piece.Sector.Slice = value;
                        piece.Sector.Transform.localPosition = Vector3.zero;
                        piece.Sector.Transform.localRotation = Quaternion.identity;
                        piece.Sector.Transform.Rotate(this.Transform.up, counter * 360.0f / this.NumberOfTimelinePieces);
                        piece.Sector.Transform.Translate(piece.Sector.Transform.forward * this.Radius);
                        counter++;
                    }
                });

            this.pulsationInterval = Mathf.Lerp(
                this.MinRadius,
                this.MaxRadius,
                this.Radius / (this.MaxRadius - this.MinRadius));
        }

        /// <summary>
        /// The tick.
        /// </summary>
        public void Tick() {
            this.dimensionsSettings.NumberOfTimelinePieces.Value = this.NumberOfTimelinePieces;
            this.pulsationInterval += this.PulsationRate * Time.deltaTime;
            this.Radius = Mathf.Lerp(this.MinRadius, this.MaxRadius, Mathf.PingPong(this.pulsationInterval, 1));
            this.Transform.Rotate(this.Transform.up, this.RotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// The remove piece.
        /// </summary>
        /// <param name="pieceGameObject">
        /// The piece game object.
        /// </param>
        /// <returns>
        /// The <see cref="TimelinePiece"/>.
        /// </returns>
        public TimelinePiece.TimelinePiece RemovePiece(GameObject pieceGameObject) {
            var piece = this.timelinePieces.FirstOrDefault(p => p.Sector.Transform.gameObject == pieceGameObject);
            if (piece == null) {
                return null;
            }
            this.timelinePieces.Remove(piece);
            this.ResetPieces();
            return piece;
        }

        public void RemoveLast() {
            this.timelineFactory.Despawn(this.timelinePieces.Last.Value);
            this.timelinePieces.RemoveLast();
        }

        public void InsertLast(TimelinePiece.TimelinePiece piece) {
            this.timelinePieces.AddLast(piece);
            ResetPieces();
        }

        public TimelinePiece.TimelinePiece AddLast() {
            var piece = this.timelineFactory.Spawn(
                this.UniformSectorDepth,
                this.UniformSectorHeight,
                this.NumberOfTimelinePieces + 1);
            this.timelinePieces.AddLast(piece);
            return piece;
        }
/*
        public TimelinePie*/

        public void InsertAndSwapPiece(TimelinePiece.TimelinePiece swap, TimelinePiece.TimelinePiece piece) {

        }

        private void ResetPieces() {
            var counter = 0;
            foreach (var piece in this.timelinePieces) {
                piece.Sector.Slice = this.NumberOfTimelinePieces;
                piece.Sector.Transform.localPosition = Vector3.zero;
                piece.Sector.Transform.localRotation = Quaternion.identity;
                piece.Sector.Transform.Rotate(this.Transform.up, counter * 360.0f / this.NumberOfTimelinePieces);
                piece.Sector.Transform.Translate(piece.Sector.Transform.forward * this.Radius);
                counter++;
            }
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
                /// The game object.
                /// </summary>
                [SerializeField]
                private GameObject timelineGameObject;

                /// <summary>
                /// The transform.
                /// </summary>
                [SerializeField]
                private Transform transform;

                /// <summary>
                /// Gets or sets the game object.
                /// </summary>
                internal GameObject TimelineGameObject {
                    get {
                        return this.timelineGameObject;
                    }

                    set {
                        this.timelineGameObject = value;
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
            /// The dimensions.
            /// </summary>
            [Serializable]
            internal class Dimensions {
                /// <summary>
                /// The number of timelinePieces.
                /// </summary>
                [SerializeField]
                private IntReactiveProperty numberOfTimelinePieces;

                /// <summary>
                /// The sector depth.
                /// </summary>
                [SerializeField]
                private FloatReactiveProperty uniformSectorDepth;

                /// <summary>
                /// The sector height.
                /// </summary>
                [SerializeField]
                private FloatReactiveProperty uniformSectorHeight;

                /// <summary>
                /// Gets or sets the number of timelinePieces.
                /// </summary>
                internal IntReactiveProperty NumberOfTimelinePieces {
                    get {
                        return this.numberOfTimelinePieces;
                    }

                    set {
                        this.numberOfTimelinePieces = value;
                    }
                }

                /// <summary>
                /// Gets or sets the uniform sector depth.
                /// </summary>
                internal FloatReactiveProperty UniformSectorDepth {
                    get {
                        return this.uniformSectorDepth;
                    }

                    set {
                        this.uniformSectorDepth = value;
                    }
                }

                /// <summary>
                /// Gets or sets the uniform sector height.
                /// </summary>
                internal FloatReactiveProperty UniformSectorHeight {
                    get {
                        return this.uniformSectorHeight;
                    }

                    set {
                        this.uniformSectorHeight = value;
                    }
                }
            }

            /// <summary>
            /// The radius.
            /// </summary>
            [Serializable]
            internal class Radius {
                /// <summary>
                /// The initial radius.
                /// </summary>
                [SerializeField]
                private FloatReactiveProperty currentRadius;

                /// <summary>
                /// The min radius.
                /// </summary>
                [SerializeField]
                private FloatReactiveProperty minRadius;

                /// <summary>
                /// The max radius.
                /// </summary>
                [SerializeField]
                private FloatReactiveProperty maxRadius;

                /// <summary>
                /// Gets or sets the initial radius.
                /// </summary>
                internal FloatReactiveProperty CurrentRadius {
                    get {
                        return this.currentRadius;
                    }

                    set {
                        this.currentRadius = value;
                    }
                }

                /// <summary>
                /// Gets or sets the min radius.
                /// </summary>
                internal FloatReactiveProperty MinRadius {
                    get {
                        return this.minRadius;
                    }

                    set {
                        this.minRadius = value;
                    }
                }

                /// <summary>
                /// Gets or sets the max radius.
                /// </summary>
                internal FloatReactiveProperty MaxRadius {
                    get {
                        return this.maxRadius;
                    }

                    set {
                        this.maxRadius = value;
                    }
                }
            }

            /// <summary>
            /// The speeds.
            /// </summary>
            [Serializable]
            internal class Speeds {
                /// <summary>
                /// The pulsation rate.
                /// </summary>
                [SerializeField]
                private FloatReactiveProperty pulsationSpeed;

                /// <summary>
                /// The rotation speed.
                /// </summary>
                [SerializeField]
                private IntReactiveProperty rotationSpeed;

                /// <summary>
                /// Gets or sets the pulsation rate.
                /// </summary>
                internal FloatReactiveProperty PulsationRate {
                    get {
                        return this.pulsationSpeed;
                    }

                    set {
                        this.pulsationSpeed = value;
                    }
                }

                /// <summary>
                /// Gets or sets the rotation speed.
                /// </summary>
                internal IntReactiveProperty RotationSpeed {
                    get {
                        return this.rotationSpeed;
                    }

                    set {
                        this.rotationSpeed = value;
                    }
                }
            }
        }
    }
}
