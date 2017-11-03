using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.TimelinePiece {
    using UnityEngine;

    using Zenject;

    using _3DPrimitives;

    /// <summary>
    /// The timeline piece.
    /// </summary>
    public class TimelinePiece : MonoBehaviour {
        /// <summary>
        /// Gets the model.
        /// </summary>
        public Sector Sector { get; private set; }

        /// <summary>
        /// The construct.
        /// </summary>
        /// <param name="sector">
        /// The Sector.
        /// </param>
        [Inject]
        public void Construct(Sector sector) {
            this.Sector = sector;
        }

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
            }
        }
    }
}
