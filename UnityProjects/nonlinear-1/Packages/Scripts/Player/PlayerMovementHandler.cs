namespace Scripts.Player {
    using System;
    using System.Collections;

    using UniRx;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The player movement handler.
    /// </summary>
    public class PlayerMovementHandler : IInitializable, IFixedTickable {
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
        /// The initialize.
        /// </summary>
        public void Initialize() {
        }

        /// <summary>
        /// The fixed tick.
        /// </summary>
        public void FixedTick() {
            Debug.Log(Vector3.Distance(Camera.main.transform.position, this.lastHit.point));
            Debug.DrawRay(Camera.main.transform.position, (this.lastHit.point - Camera.main.transform.position).normalized * Vector3.Distance(Camera.main.transform.position, this.lastHit.point), Color.red);
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
                Debug.Log(Vector3.Distance(this.componentSettings.Transform.position, target));
                Debug.Log(minimumDistance);
                this.componentSettings.Transform.position = Vector3.MoveTowards(
                    this.componentSettings.Transform.position,
                    new Vector3(target.x, this.componentSettings.Transform.position.y, target.z),
                    this.movementSettings.Speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            yield break;
        }


    }
}
