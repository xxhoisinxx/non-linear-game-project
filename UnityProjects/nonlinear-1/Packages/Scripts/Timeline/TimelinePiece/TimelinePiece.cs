namespace Scripts.Timeline.TimelinePiece {
    using System;

    using UnityEngine;

    using Zenject;

    using _3DPrimitives;

    /// <summary>
    /// The timeline piece.
    /// </summary>
    public class TimelinePiece : MonoBehaviour, IEquatable<TimelinePiece> {
        /// <summary>
        /// Gets the model.
        /// </summary>
        [Inject]
        public Sector Sector { get; }

        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        public int DefaultLayer { get; set; }

        /// <summary>
        /// The construct.
        /// </summary>
        /// <param name="Layer">
        /// The layer.
        /// </param>
        [Inject]
        public void Construct(int Layer) {
            this.DefaultLayer = Layer;
        }

        public bool Equals(TimelinePiece other) {
            if (object.ReferenceEquals(null, other)) {
                return false;
            }
            if (object.ReferenceEquals(this, other)) {
                return true;
            }
            return base.Equals(other) && object.Equals(this.Sector, other.Sector);
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

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode() {
            unchecked {
                return (base.GetHashCode() * 397) ^ (this.Sector != null ? this.Sector.GetHashCode() : 0);
            }
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
                piece.Sector.Transform.gameObject.layer = piece.DefaultLayer;
            }
        }
    }
}
