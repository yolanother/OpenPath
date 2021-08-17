/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using UnityEngine;

namespace DoubTech.OpenPath.Controllers
{
    public class PlayerShip : MonoBehaviour
    {
        private static PlayerShip playerShip;

        public ShipController shipController;
        public static PlayerShip Instance => playerShip;

        private void OnEnable()
        {
            playerShip = this;
        }
    }
}