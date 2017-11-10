namespace Scrapbook {
    using System;
    using System.IO;
    using System.Xml;

    using log4net;

    using UniRx;

    using UnityEngine;

    using Vuforia;

    using Zenject;

    /// <summary>
    /// The scrapbook manager.
    /// </summary>
    public class ScrapbookManager : MonoBehaviour {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The tracker manager.
        /// </summary>
        private TrackerManager trackerManager;

        /// <summary>
        /// The settings for this <see cref="ScrapbookManager"/>.
        /// </summary>
        [Inject]
        private Settings settings;

        /// <summary>
        /// The database name.
        /// </summary>
        private string databaseName;

        private XmlDocument database;

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName {
            get {
                return this.databaseName;
            }

            set {
                var filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Vuforia", value + ".xml");
                try {
                    this.database.Load(filepath);
                }
                catch (DirectoryNotFoundException e) {
                    if (this.database.DocumentElement != null) {
                        Log.WarnFormat("Unable to load {0}", filepath);
                    }
                    else {
                        Log.ErrorFormat("Unable to load {0}", filepath);
                        throw;
                    }

                    return;
                }
                this.databaseName = value;
                var imageTargets = this.database.DocumentElement?.SelectNodes("Tracking/ImageTarget");
                if (imageTargets == null) {
                    Log.Warn("No image targets in database");
                    return;
                }
                foreach (XmlNode node in imageTargets) {
                    try {
                        Log.Debug(node.Attributes["name"].Value);
                    }
                    catch (NullReferenceException e) {
                        Log.WarnFormat("ImageTarget does not have a name attribute in {0}", filepath);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the dependencies for this scrapbook.
        /// </summary>
        /// <param name="tm">
        /// The <see cref="Vuforia.TrackerManager"/> instance.
        /// </param>
        [Inject]
        public void Construct(TrackerManager tm) {
            this.trackerManager = tm;
        }

        protected void Awake() {
            this.database = new XmlDocument();
        }

        /// <summary>
        /// The start.
        /// </summary>
        protected void Start() {
            this.settings.DatabaseName.Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(
                dbName => { this.DatabaseName = dbName; });



/*            var enumerator = this.trackerManager.GetStateManager().GetTrackableBehaviours().GetEnumerator();
            Log.Debug("Hello world!");
            while (enumerator.MoveNext()) {
                Log.Debug(enumerator.Current?.TrackableName);
                Log.Debug(enumerator.Current?.CurrentStatus);
            }

            enumerator.Dispose();*/
        }

        /// <summary>
        /// The settings for this class.
        /// </summary>
        [Serializable]
        internal class Settings {
            /// <summary>
            /// The database name.
            /// </summary>
            [SerializeField]
            private StringReactiveProperty databaseName;

            /// <summary>
            /// Gets the name of the database.
            /// </summary>
            internal StringReactiveProperty DatabaseName {
                get {
                    return this.databaseName;
                }
            }
        }

        protected void Update () {
        }


    }
}
