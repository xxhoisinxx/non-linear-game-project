namespace Scenes.Props {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using log4net;

    using UniRx;
    using UniRx.Triggers;

    using UnityEngine;

    /// <summary>
    ///     Represents a prop
    /// </summary>
    public class Prop : MonoBehaviour, IDisposable {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     The collider.
        /// </summary>
        [SerializeField]
        private new Collider collider;

        /// <summary>
        ///     The observers.
        /// </summary>
        private LinkedList<IDisposable> observers;

        /// <summary>
        ///     The sprite renderer.
        /// </summary>
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        /// <summary>
        ///     Finalizes an instance of the <see cref="Prop"/> class.
        /// </summary>
        ~Prop() {
            this.Dispose(false);
        }

        /// <inheritdoc />
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Initializes the <see cref="Prop"/> class.
        /// </summary>
        protected void Awake() {
            this.observers = new LinkedList<IDisposable>();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing,
        ///     releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">
        ///     Specifies whether this is being called by the
        ///      <see cref="Dispose"/> method.
        /// </param>
        protected virtual void Dispose(bool isDisposing) {
            this.ReleaseUnmanagedResources();
        }

        /// <summary>
        ///     Subscribers mouse enter and exit observers to toggle
        ///      a sprite renderer.
        /// </summary>
        protected void Start() {
            this.observers.AddLast(
                this.collider.OnMouseEnterAsObservable().Subscribe(
                    _ => {
                        this.spriteRenderer.enabled = true;
                        Log.InfoFormat(
                            "Sprite renderer activated\nGameobject: {0}",
                            this.spriteRenderer.name);
                    }));
            this.observers.AddLast(
                this.collider.OnMouseExitAsObservable().Subscribe(
                    _ => {
                        this.spriteRenderer.enabled = false;
                        Log.InfoFormat(
                            "Sprite renderer deactivated\nGameobject: {0}",
                            this.spriteRenderer.name);
                    }));
        }

        /// <summary>
        ///     Releases unmanaged resources.
        /// </summary>
        private void ReleaseUnmanagedResources() {
            while (this.observers.Any()) {
                this.observers.Last.Value.Dispose();
                this.observers.RemoveLast();
            }

            this.observers = null;
            this.spriteRenderer = null;
            this.collider = null;
        }
    }
}