namespace Scrapbook {
    using Vuforia;

    public class TrackablePage {
        private PageDetectionHandler pageDetectionHandler;

        public TrackablePage(TrackableBehaviour tb, PageDetectionHandler handler) {
            this.TrackableBehaviour = tb;
            this.PageDetectionHandler = handler;
        }

        internal TrackableBehaviour TrackableBehaviour { get; set; }

        internal PageDetectionHandler PageDetectionHandler {
            get {
                return this.pageDetectionHandler;
            }

            set {
                if (this.TrackableBehaviour == null) {
                    return;
                }
                if (this.pageDetectionHandler == null) {
                    this.TrackableBehaviour.RegisterTrackableEventHandler(value);
                    this.pageDetectionHandler = value;
                    return;
                }
                if (this.pageDetectionHandler == value) {
                    return;
                }
                this.TrackableBehaviour.UnregisterTrackableEventHandler(this.pageDetectionHandler);
                this.TrackableBehaviour.RegisterTrackableEventHandler(value);
                this.pageDetectionHandler = value;
            }
        }
    }
}
