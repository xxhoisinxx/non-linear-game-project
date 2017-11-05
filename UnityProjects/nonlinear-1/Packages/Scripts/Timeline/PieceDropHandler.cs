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

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            this.timeline.ReactivatePiece(this.pickedUpPiece.Piece);
            RaycastHit hit;
            Debug.DrawLine(ray.origin, ray.direction * 100.0f, Color.cyan, 10);
            if (Physics.Raycast(
                    ray,
                    out hit,
                    100.0f,
                    LayerMask.GetMask(LayerMask.LayerToName(TimelinePiece.TimelinePiece.DefaultLayer)))
                        && hit.transform.gameObject.CompareTag(this.tag)) {
                Debug.Log("Swap");
                this.timeline.SwapPiece(this.pickedUpPiece.Piece, hit.transform.gameObject);
                Debug.DrawLine(Input.mousePosition, hit.point, Color.blue, 10);
            }
            RaycastHit[] hits = Physics.RaycastAll(ray, 100.0f, LayerMask.GetMask(
                LayerMask.LayerToName(TimelinePiece.TimelinePiece.DefaultLayer)));
            Debug.Log(hits.Length);
            for (int i = 0; i < hits.Length; i++) {
                Debug.Log(hits[i]);
            }

            this.pickedUpPiece.Piece.transform.gameObject.layer = TimelinePiece.TimelinePiece.DefaultLayer;
            this.pickedUpPiece.Piece = null;

        }
    }
}
