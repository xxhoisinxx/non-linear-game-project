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

        public void FixedTick() {
            if (!Input.GetMouseButtonDown(0) || this.pickedUpPiece.Piece != null) {
                return;
            }
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform.gameObject.CompareTag(this.tag)) {
                Debug.Log("Adding");
                this.pickedUpPiece.Piece = this.timeline.RemovePiece(hit.transform.gameObject);
                hit.transform.gameObject.layer = 2;
            }
        }
    }
}
