namespace Scripts.Timeline.TimelinePiece {
    using System;

    using UnityEngine;

    using Zenject;

    using _3DPrimitives;
    using System.Collections.Generic;

    using Random = UnityEngine.Random;

    /// <summary>
    /// The timeline piece.
    /// </summary>
    public class TimelinePiece : MonoBehaviour, IEquatable<TimelinePiece> {
        /// <summary>
        /// Gets the model.
        /// </summary>
        [Inject]
        public Sector Sector { get; }

        public MeshRenderer MeshRenderer { get; private set; }

        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        public static int DefaultLayer { get; private set; }

        [Inject]
        private void Construct(int layer, MeshRenderer meshRenderer) {
            DefaultLayer = layer;
            this.MeshRenderer = meshRenderer;

        }

        protected void Awake() {
            this.MeshRenderer.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }

        public bool IsActive { get; set; }

        public bool Equals(TimelinePiece other) {
            if (object.ReferenceEquals(null, other)) {
                return false;
            }
            if (object.ReferenceEquals(this, other)) {
                return true;
            }
            return this.Sector == other.Sector;
        }

        public override bool Equals(object obj) {
            if (object.ReferenceEquals(null, obj)) {
                return false;
            }
            if (object.ReferenceEquals(this, obj)) {
                return true;
            }
            return obj.GetType() == this.GetType() && this.Equals((TimelinePiece)obj);
        }

        public override int GetHashCode() {
            var hashCode = 749113007;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Sector>.Default.GetHashCode(this.Sector);
            return hashCode;
        }

        public static bool operator ==(TimelinePiece left, TimelinePiece right) => Equals(left, right);

        public static bool operator !=(TimelinePiece left, TimelinePiece right) => !Equals(left, right);

        /// <summary>
        /// The pool.
        /// </summary>
        public class Pool : MonoMemoryPool<float, float, int, TimelinePiece> {
            /// <summary>
            /// The reinitialize.
            /// </summary>
            /// <param name="depth">
            /// The depth.
            /// </param>
            /// <param name="height">
            /// The height.
            /// </param>
            /// <param name="slice">
            /// The slice.
            /// </param>
            /// <param name="piece">
            /// The piece.
            /// </param>
            protected override void Reinitialize(float depth, float height, int slice, TimelinePiece piece) {
                piece.Sector.Reinitialize(depth, height, slice);
                piece.Sector.Transform.gameObject.layer = TimelinePiece.DefaultLayer;
                piece.IsActive = true;
            }

            protected override void OnCreated(TimelinePiece piece) {
                base.OnCreated(piece);
                piece.IsActive = false;
            }

            protected override void OnSpawned(TimelinePiece piece) {
                base.OnSpawned(piece);
                piece.IsActive = true;
            }

            protected override void OnDespawned(TimelinePiece piece) {
                base.OnDespawned(piece);
                piece.IsActive = false;
            }
        }
    }
}
