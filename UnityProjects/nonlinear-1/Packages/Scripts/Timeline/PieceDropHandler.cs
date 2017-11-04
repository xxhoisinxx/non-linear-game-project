using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Timeline {
    using UnityEngine;

    using Zenject;
    /// <summary>
    /// The piece drop handler.
    /// </summary>
    public class PieceDropHandler : IFixedTickable {
        /// <summary>
        /// The tag.
        /// </summary>
        private readonly string tag;

        /// <summary>
        /// The piece.
        /// </summary>
        [Inject]
        private readonly TimelinePiece.TimelinePieceNode pickedUpPiece;

        /// <summary>
        /// The timeline.
        /// </summary>
        private Timeline timeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceDropHandler"/> class.
        /// </summary>
        /// <param name="timeline">
        /// The timeline.
        /// </param>
        public PieceDropHandler(string tag, Timeline timeline) {
            this.tag = tag;
            this.timeline = timeline;
        }

        public void FixedTick() {
            if (Input.GetMouseButton(0) || this.pickedUpPiece.Piece == null) {
                return;
            }
            this.timeline.InsertLast(this.pickedUpPiece.Piece);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
/*            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform.gameObject.CompareTag(this.tag)) {
                Debug.Log("INTERSECTION!");
                return;
            }*/
            Debug.Log("Remove");
            this.pickedUpPiece.Piece.transform.gameObject.layer = this.pickedUpPiece.Piece.DefaultLayer;
            this.pickedUpPiece.Piece = null;

        }
    }
}
