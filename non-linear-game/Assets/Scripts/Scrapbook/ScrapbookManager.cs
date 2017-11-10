namespace Scrapbook {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

        /// <summary>
        /// The database of image targets.
        /// </summary>
        private XmlDocument database;

        /// <summary>
        /// The collection of disposable action listeners.
        /// </summary>
        private LinkedList<IDisposable> disposableActionListeners;

        /// <summary>
        /// Gets the page dictionary.
        /// </summary>
        public Dictionary<string, TrackableBehaviour> PageDictionary { get; private set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName {
            get {
                return this.databaseName;
            }

            set {
                var filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Vuforia", value + ".xml");
                try {
                    this.database.Load(filePath);
                }
                catch (DirectoryNotFoundException e) {
                    if (this.database.DocumentElement != null) {
                        Log.WarnFormat("Unable to load {0}", filePath);
                    }
                    else {
                        Log.ErrorFormat("Unable to load {0}", filePath);
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
                    string imageName;
                    try {
                        // ReSharper disable once PossibleNullReferenceException
                        imageName = node.Attributes["name"].Value;
                    }
                    catch (NullReferenceException) {
                        Log.WarnFormat("ImageTarget does not have a name attribute in {0}", filePath);
                        continue;
                    }
                    var enumerator = this.trackerManager.GetStateManager().GetTrackableBehaviours().GetEnumerator();
                    while (enumerator.MoveNext()) {
                        // ReSharper disable once PossibleNullReferenceException
                        if (enumerator.Current.TrackableName.Equals(imageName)) {
                            this.PageDictionary.Add(imageName, enumerator.Current);
                        }
                    }

                    enumerator.Dispose();
                }
            }
        }

        /// <summary>
        /// Initializes the dependencies for this <see cref="ScrapbookManager"/>.
        /// </summary>
        /// <param name="tm">
        /// The <see cref="Vuforia.TrackerManager"/> instance.
        /// </param>
        [Inject]
        public void Construct(TrackerManager tm) {
            this.trackerManager = tm;
        }

        /// <summary>
        /// Initializes the <see cref="ScrapbookManager"/>.
        /// </summary>
        protected void Awake() {
            this.database = new XmlDocument();
            this.PageDictionary = new Dictionary<string, TrackableBehaviour>();
            this.disposableActionListeners = new LinkedList<IDisposable>();
        }

        /// <summary>
        /// The start.
        /// </summary>
        protected void Start() {
            this.disposableActionListeners.AddLast(this.settings.DatabaseName.Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(
                dbName => { this.DatabaseName = dbName; }));

            var derp = this.trackerManager.GetStateManager().GetTrackableBehaviours().GetEnumerator();
            while (derp.MoveNext()) {
                Log.Debug(derp.Current.TrackableName);
            }


/*            var enumerator = this.trackerManager.GetStateManager().GetTrackableBehaviours().GetEnumerator();
            Log.Debug("Hello world!");
            while (enumerator.MoveNext()) {
                Log.Debug(enumerator.Current?.TrackableName);
                Log.Debug(enumerator.Current?.CurrentStatus);
            }

            enumerator.Dispose();*/
        }

        /// <summary>
        /// Cleans up any resources when this <see cref="ScrapbookManager"/> is disabled.
        /// </summary>
        protected void OnDisable() {
            var length = this.disposableActionListeners.Count;
            for (var i = 0; i < length; i++) {
                this.disposableActionListeners.Last.Value.Dispose();
                this.disposableActionListeners.RemoveLast();
            }
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
