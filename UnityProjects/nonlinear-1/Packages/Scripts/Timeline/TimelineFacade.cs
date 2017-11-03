/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Memes are Dreams Studios. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.SectoredCylinder {
    using UnityEngine;

    using Zenject;

    /// <summary>
    /// The sectored cylinder facade.
    /// </summary>
    public class TimelineFacade : MonoBehaviour {
        /// <summary>
        /// The model.
        /// </summary>
        private Timeline model;

        /// <summary>
        /// The construct.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        [Inject]
        public void Construct(Timeline model) {
            this.model = model;
        }
    }
}
