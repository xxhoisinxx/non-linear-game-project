/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Memes are Dreams Studios. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Scripts.Timeline {
    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The sectored cylinder installer.
    /// </summary>
    public class TimelineFacadeInstaller : MonoInstaller {
        /// <summary>
        /// The components settings.
        /// </summary>
        [SerializeField]
        private Timeline.Settings.Components componentsSettings;

        /// <summary>
        /// The dimensions settings.
        /// </summary>
        [SerializeField]
        private Timeline.Settings.Dimensions dimensionsSettings;

        /// <summary>
        /// The radius settings.
        /// </summary>
        [SerializeField]
        private Timeline.Settings.Radius radiusSettings;

        /// <summary>
        /// The speed settings.
        /// </summary>
        [SerializeField]
        private Timeline.Settings.Speeds speedSettings;

        /// <summary>
        /// The install bindings.
        /// </summary>
        public override void InstallBindings() {
            this.Container.BindInterfacesAndSelfTo<Timeline>().AsSingle();
            this.Container.Bind<Timeline.Settings.Components>().FromInstance(this.componentsSettings);
            this.Container.Bind<Timeline.Settings.Dimensions>().FromInstance(this.dimensionsSettings);
            this.Container.Bind<Timeline.Settings.Radius>().FromInstance(this.radiusSettings);
            this.Container.Bind<Timeline.Settings.Speeds>().FromInstance(this.speedSettings);
            this.Container.BindMemoryPool<TimelinePiece.TimelinePiece, TimelinePiece.TimelinePiece.Pool>().WithInitialSize(
                this.dimensionsSettings.NumberOfTimelinePieces.Value)
                .FromSubContainerResolve().ByNewPrefab(this.componentsSettings.TimelineGameObject);

            this.Container.Bind<TimelinePiece.TimelinePieceNode>().AsSingle();
            this.Container.BindInterfacesAndSelfTo<PiecePickUpHandler>().AsSingle().WithArguments("Timeline_Piece");
            this.Container.BindInterfacesAndSelfTo<PieceDropHandler>().AsSingle().WithArguments("Timeline_Piece");
        }

        /// <summary>
        /// The on validate.
        /// </summary>
        protected void OnValidate() {
           /* this.dimensionsSettings.NumberOfTimelinePieces.Value = Mathf.Clamp(
                this.dimensionsSettings.NumberOfTimelinePieces.Value,
                3,
                int.MaxValue);
            this.dimensionsSettings.UniformSectorDepth.Value = Mathf.Clamp(
                this.dimensionsSettings.UniformSectorDepth.Value,
                0,
                float.MaxValue);
            this.dimensionsSettings.UniformSectorHeight.Value = Mathf.Clamp(
                this.dimensionsSettings.UniformSectorHeight.Value,
                0,
                float.MaxValue);
            this.radiusSettings.CurrentRadius.Value = Mathf.Clamp(
                this.radiusSettings.CurrentRadius.Value,
                this.radiusSettings.MinRadius.Value,
                this.radiusSettings.MaxRadius.Value);
            this.radiusSettings.MinRadius.Value = Mathf.Clamp(
                this.radiusSettings.MinRadius.Value,
                0,
                this.radiusSettings.MaxRadius.Value);
            this.radiusSettings.MaxRadius.Value = Mathf.Clamp(
                this.radiusSettings.MaxRadius.Value,
                this.radiusSettings.MinRadius.Value,
                float.MaxValue);
            if (this.Container == null || this.Container.IsInstalling || this.Container.IsValidating) {
                return;
            }
            this.Container.Resolve<Timeline>().UniformSectorHeight =
                this.dimensionsSettings.UniformSectorHeight.Value;
            this.Container.Resolve<Timeline>().UniformSectorDepth =
                this.dimensionsSettings.UniformSectorDepth.Value;*/
        }
    }
}