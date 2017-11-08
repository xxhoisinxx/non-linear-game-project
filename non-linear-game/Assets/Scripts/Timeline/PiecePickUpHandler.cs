namespace Scripts.Timeline {
    using System;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The piece pick up handler.
    /// </summary>
    public class PiecePickUpHandler : IFixedTickable {
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
        /// Initializes a new instance of the <see cref="PiecePickUpHandler"/> class.
        /// </summary>
        /// <param name="tag">
        /// The tag.
        /// </param>
        /// <param name="timeline">
        /// The timeline.
        /// </param>
        public PiecePickUpHandler(string tag, Timeline timeline) {
            this.tag = tag;
            this.timeline = timeline;
        }

        /// <summary>
        /// The fixed tick.
        /// </summary>
        public void FixedTick() {
            if (this.pickedUpPiece.Piece != null && Input.GetMouseButton(0)) {
                var v3 = Input.mousePosition;
                v3.z = 7.5f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                this.pickedUpPiece.Piece.Sector.Transform.position = v3;
            }
            if (!Input.GetMouseButtonDown(0) || this.pickedUpPiece.Piece != null) {
                return;
            }
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(
                    ray,
                    out hit,
                    100.0f,
                    LayerMask.GetMask(LayerMask.LayerToName(TimelinePiece.TimelinePiece.DefaultLayer)))
                        && hit.transform.gameObject.CompareTag(this.tag)) {
                this.pickedUpPiece.Piece = this.timeline.DeactivatePiece(hit.transform.gameObject);
                hit.transform.gameObject.layer = 2;
            }
        }
    }
}
