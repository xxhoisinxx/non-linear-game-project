/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Memes are Dreams Studios. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Scripts.SectoredCylinder {
    using System;

    using Scripts.TimelinePiece;

    using UnityEngine;

    using Zenject;

    using _3DPrimitives;

    /// <summary>
    /// The sectored cylinder installer.
    /// </summary>
    public class TimelineInstaller : MonoInstaller {
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
            this.Container.BindMemoryPool<TimelinePiece, TimelinePiece.Pool>().WithInitialSize(this.dimensionsSettings.NumberOfSectors)
                .FromSubContainerResolve().ByNewPrefab(this.componentsSettings.TimelineGameObject);
        }

        /// <summary>
        /// The on validate.
        /// </summary>
        protected void OnValidate() {
            this.dimensionsSettings.NumberOfSectors = Mathf.Clamp(
                this.dimensionsSettings.NumberOfSectors,
                3,
                int.MaxValue);
            this.dimensionsSettings.UniformSectorDepth = Mathf.Clamp(
                this.dimensionsSettings.UniformSectorDepth,
                0,
                float.MaxValue);
            this.dimensionsSettings.UniformSectorHeight = Mathf.Clamp(
                this.dimensionsSettings.UniformSectorHeight,
                0,
                float.MaxValue);
            this.radiusSettings.InitialRadius = Mathf.Clamp(
                this.radiusSettings.InitialRadius,
                this.radiusSettings.MinRadius,
                this.radiusSettings.MaxRadius);
            if (this.Container == null || this.Container.IsInstalling || this.Container.IsValidating) {
                return;
            }
            this.Container.Resolve<Timeline>().UniformSectorHeight =
                this.dimensionsSettings.UniformSectorHeight;
            this.Container.Resolve<Timeline>().UniformSectorDepth =
                this.dimensionsSettings.UniformSectorDepth;
            this.Container.Resolve<Timeline>().NumSectors =
                this.dimensionsSettings.NumberOfSectors;
        }
    }
}