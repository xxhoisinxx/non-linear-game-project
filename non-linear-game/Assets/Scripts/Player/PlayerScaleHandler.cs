namespace Player {
    using System;
    using System.Reflection;

    using log4net;

    using UniRx;

    using UnityEngine;

    using Zenject;

    public class PlayerScaleHandler : UniRx.IObserver<long> {
        /// <summary>
        ///     The logger for this class.
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Camera camera;

        private readonly Vector3ReactiveProperty defaultScale;

        private readonly FloatReactiveProperty defaultZDistance;

        /// <summary>
        ///     The transform.
        /// </summary>
        private readonly Transform transform;

        [Inject]
        internal PlayerScaleHandler(
                Camera camera,
                PlayerInstaller.Settings.Components componentSettings,
                PlayerInstaller.Settings.Scale scaleSettings) {
            this.transform = componentSettings.Transform;
            this.defaultZDistance = scaleSettings.DefaultZDistance;
            this.defaultScale = scaleSettings.DefaultScale;
            this.camera = camera;
        }

        public void OnCompleted() {
            // throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            // throw new NotImplementedException();
        }

        public void OnNext(long value) {
            var newScale = Vector3.Distance(
                               new Vector3(
                                   0,
                                   0,
                                   this.camera.transform.position.z),
                               new Vector3(0, 0, this.transform.position.z))
                           / this.defaultZDistance.Value
                           * this.defaultScale.Value;
            this.transform.localScale = Vector3.Lerp(
                this.transform.localScale,
                newScale,
                Time.timeScale);
        }

        public class Pool : MemoryPool<PlayerScaleHandler> { }
    }
}