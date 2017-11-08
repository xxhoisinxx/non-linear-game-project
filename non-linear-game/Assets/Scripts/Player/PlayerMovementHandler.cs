namespace Scripts.Player {
    using System;
    using System.Collections;

    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The player movement handler.
    /// </summary>
    public class PlayerMovementHandler : IInitializable, IFixedTickable, IDisposable {
        /// <summary>
        /// The rigid body.
        /// </summary>
        [Inject]
        private Player.Settings.Components componentSettings;

        /// <summary>
        /// The movement settings.
        /// </summary>
        [Inject]
        private Player.Settings.Movement movementSettings;

        /// <summary>
        /// The movement coroutine.
        /// </summary>
        private IDisposable movementCoroutine;

        /// <summary>
        /// The last hit.
        /// </summary>
        private RaycastHit lastHit;

        /// <summary>
        /// The starting camera distance.
        /// </summary>
        private float startingCameraDistance;

        private float startingYPosition;

        /// <summary>
        /// The initialize.
        /// </summary>
        public void Initialize() {
            this.startingCameraDistance = Mathf.Abs(
                this.componentSettings.Transform.position.z - Camera.main.transform.position.z);
            this.startingYPosition = this.componentSettings.Transform.position.y;
        }

        /// <summary>
        /// The fixed tick.
        /// </summary>
        public void FixedTick() {
            var dir = (this.lastHit.point - Camera.main.transform.position).normalized;
            Debug.DrawRay(
                Camera.main.transform.position,
                dir * Vector3.Distance(Camera.main.transform.position, this.lastHit.point),
                Color.red);
            if (!Input.GetMouseButtonDown(0)) {
                return;
            }
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out this.lastHit, 100.0f)) {
                return;
            }
            this.movementCoroutine?.Dispose();
            this.movementCoroutine = this.MoveTo(this.lastHit.point, 0.5f).ToObservable().Subscribe();
        }

        /// <summary>
        /// The move to.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="minimumDistance">
        /// The minimum distance.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        private IEnumerator MoveTo(Vector3 target, float minimumDistance) {
            while (Vector3.Distance(this.componentSettings.Transform.position, target) > minimumDistance) {
                var cameraPlayerDist = Mathf.Abs(
                    this.componentSettings.Transform.position.z - Camera.main.transform.position.z);
                var distanceScaleRatio = cameraPlayerDist / this.startingCameraDistance;
                this.componentSettings.Transform.localScale = new Vector3(
                    distanceScaleRatio,
                    distanceScaleRatio,
                    distanceScaleRatio);
                this.componentSettings.Transform.position = Vector3.MoveTowards(
                    this.componentSettings.Transform.position,
                    new Vector3(target.x, 0.01f, target.z),
                    this.movementSettings.Speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            yield break;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose() {
            this.movementCoroutine?.Dispose();
        }
    }
}
