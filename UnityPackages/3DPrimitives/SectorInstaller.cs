/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Memes are Dreams Studios. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DPrimitives {
    using JetBrains.Annotations;

    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The sector installer.
    /// </summary>
    public class SectorInstaller : MonoInstaller {
        /// <summary>
        /// The settings.
        /// </summary>
        [SerializeField]
        private Settings settings;

        /// <summary>
        /// The install bindings.
        /// </summary>
        public override void InstallBindings() {
            this.Container.Bind<MeshFilter>().FromInstance(this.settings.MeshFilter);
            this.Container.Bind<MeshCollider>().FromInstance(this.settings.MeshCollider);
            this.Container.Bind<Transform>().FromInstance(this.settings.Transform);
        }

        /// <summary>
        /// The settings.
        /// </summary>
        [Serializable]
        public class Settings {
            /// <summary>
            /// The mesh filter.
            /// </summary>
            [SerializeField]
            private MeshFilter meshFilter;

            /// <summary>
            /// The mesh collider.
            /// </summary>
            [SerializeField]
            private MeshCollider meshCollider;

            /// <summary>
            /// The transform.
            /// </summary>
            [SerializeField]
            private Transform transform;

            /// <summary>
            /// The mesh filter.
            /// </summary>
            public MeshFilter MeshFilter => this.meshFilter;

            /// <summary>
            /// The mesh collider.
            /// </summary>
            public MeshCollider MeshCollider => this.meshCollider;

            /// <summary>
            /// The transform.
            /// </summary>
            public Transform Transform => this.transform;
        }
    }
}
