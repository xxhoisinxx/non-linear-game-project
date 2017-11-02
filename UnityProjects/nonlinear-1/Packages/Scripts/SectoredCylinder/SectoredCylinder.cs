/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Memes are Dreams Studios. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Scripts.SectoredCylinder {
    using System;
    using System.Collections.Generic;

    using _3DPrimitives;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The sectored cylinder.
    /// </summary>
    public class SectoredCylinder : IInitializable, ITickable {
        /// <summary>
        /// The sectors.
        /// </summary>
        private readonly Stack<Sector> sectors;

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
        /// The pulsation interval.
        /// </summary>
        private float pulsationInterval;

        /// <summary>
        /// The sector factory.
        /// </summary>
        [Inject]
        private Sector.Pool sectorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectoredCylinder"/> class.
        /// </summary>
        public SectoredCylinder() {
            this.sectors = new Stack<Sector>();
        }

        /// <summary>
        /// Gets or sets the number sectors.
        /// </summary>
        public int NumSectors {
            get {
                return this.dimensionsSettings.NumberOfSectors;
            }

            set {
                var diff = Mathf.Abs(value - this.sectors.Count);
                if (value > this.sectors.Count) {
                    for (var i = 0; i < diff; i++) {
                        this.sectors.Push(this.sectorFactory.Spawn(
                            this.dimensionsSettings.UniformSectorDepth,
                            this.dimensionsSettings.UniformSectorHeight,
                            this.dimensionsSettings.NumberOfSectors));
                    }
                }
                else if (value < this.sectors.Count) {
                    for (var i = 0; i < diff; i++) {
                        this.sectorFactory.Despawn(this.sectors.Pop());
                    }
                }
                this.dimensionsSettings.NumberOfSectors = value;
                var counter = 0;
                foreach (var sector in this.sectors) {
                    sector.Slice = value;
                    sector.Transform.localPosition = Vector3.zero;
                    sector.Transform.localRotation = Quaternion.identity;
                    sector.Transform.Rotate(this.Transform.up, counter * 360.0f / this.dimensionsSettings.NumberOfSectors);
                    sector.Transform.Translate(sector.Transform.forward * this.Radius);
                    counter++;
                }
            }
        }

        /// <summary>
        /// Gets or sets the uniform sector depth.
        /// </summary>
        public float UniformSectorDepth {
            get {
                return this.dimensionsSettings.UniformSectorDepth;
            }

            set {
                this.dimensionsSettings.UniformSectorDepth = value;
                foreach (var sector in this.sectors) {
                    sector.Depth = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the uniform sector height.
        /// </summary>
        public float UniformSectorHeight {
            get {
                return this.dimensionsSettings.UniformSectorHeight;
            }

            set {
                this.dimensionsSettings.UniformSectorHeight = value;
                foreach (var sector in this.sectors) {
                    sector.Height = value;
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
                return this.radiusSettings.MinRadius;
            }

            set {
                this.radiusSettings.MinRadius = value;
            }
        }

        /// <summary>
        /// Gets or sets the max radius.
        /// </summary>
        public float MaxRadius {
            get {
                return this.radiusSettings.MaxRadius;
            }

            set {
                this.radiusSettings.MaxRadius = value;
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
                var oldRadius = this.radius;
                this.radius = Mathf.Clamp(value, this.MinRadius, this.MaxRadius);
                foreach (var sector in this.sectors) {
                    Debug.DrawRay(sector.Transform.position, sector.Transform.forward * 10, Color.red);
                    sector.Transform.localPosition = Vector3.zero;
                    sector.Transform.Translate(sector.Transform.forward * this.radius, Space.World);
                }
            }
        }

        /// <summary>
        /// Gets or sets the rotation speed.
        /// </summary>
        public float RotationSpeed {
            get {
                return this.speedSettings.RotationSpeed;
            }

            set {
                this.speedSettings.RotationSpeed = value;
            }
        }

        /// <summary>
        /// Gets or sets the pulsation rate.
        /// </summary>
        public float PulsationRate {
            get {
                return this.speedSettings.PulsationRate;
            }

            set {
                this.speedSettings.PulsationRate = value;
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public void Initialize() {
            this.pulsationInterval = Mathf.Lerp(
                this.MinRadius,
                this.MaxRadius,
                this.Radius / (this.MaxRadius - this.MinRadius));
            for (var i = 0; i < this.dimensionsSettings.NumberOfSectors; i++) {
                var sector = this.sectorFactory.Spawn(
                    this.dimensionsSettings.UniformSectorDepth,
                    this.dimensionsSettings.UniformSectorHeight,
                    this.dimensionsSettings.NumberOfSectors);
                sector.Transform.Rotate(this.Transform.up, i * 360.0f / this.dimensionsSettings.NumberOfSectors);
                sector.Transform.Translate(sector.Transform.forward * this.Radius);
                this.sectors.Push(sector);
            }
        }

        /// <summary>
        /// The tick.
        /// </summary>
        public void Tick() {
            this.pulsationInterval += this.PulsationRate * Time.deltaTime;
            this.Radius = Mathf.Lerp(this.MinRadius, this.MaxRadius, Mathf.PingPong(this.pulsationInterval, 1));
            this.Transform.Rotate(this.Transform.up, this.RotationSpeed * Time.deltaTime);
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
                private GameObject sectorGameObject;

                /// <summary>
                /// The transform.
                /// </summary>
                [SerializeField]
                private Transform transform;

                /// <summary>
                /// Gets or sets the game object.
                /// </summary>
                internal GameObject SectorGameObject {
                    get {
                        return this.sectorGameObject;
                    }

                    set {
                        this.sectorGameObject = value;
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
                /// The number of sectors.
                /// </summary>
                [SerializeField]
                private int numberOfSectors;

                /// <summary>
                /// The sector depth.
                /// </summary>
                [SerializeField]
                private float uniformSectorDepth;

                /// <summary>
                /// The sector height.
                /// </summary>
                [SerializeField]
                private float uniformSectorHeight;

                /// <summary>
                /// Gets or sets the number of sectors.
                /// </summary>
                internal int NumberOfSectors {
                    get {
                        return this.numberOfSectors;
                    }

                    set {
                        this.numberOfSectors = value;
                    }
                }

                /// <summary>
                /// Gets or sets the uniform sector depth.
                /// </summary>
                internal float UniformSectorDepth {
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
                internal float UniformSectorHeight {
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
                private float initialRadius;

                /// <summary>
                /// The min radius.
                /// </summary>
                [SerializeField]
                private float minRadius;

                /// <summary>
                /// The max radius.
                /// </summary>
                [SerializeField]
                private float maxRadius;

                /// <summary>
                /// Gets or sets the initial radius.
                /// </summary>
                internal float InitialRadius {
                    get {
                        return this.initialRadius;
                    }

                    set {
                        this.initialRadius = value;
                    }
                }

                /// <summary>
                /// Gets or sets the min radius.
                /// </summary>
                internal float MinRadius {
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
                internal float MaxRadius {
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
                private float pulsationSpeed;

                /// <summary>
                /// The rotation speed.
                /// </summary>
                [SerializeField]
                private float rotationSpeed;

                /// <summary>
                /// Gets or sets the pulsation rate.
                /// </summary>
                internal float PulsationRate {
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
                internal float RotationSpeed {
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
