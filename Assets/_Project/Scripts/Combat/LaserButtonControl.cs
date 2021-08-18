/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Controllers;
using DoubTech.ScriptableEvents.BuiltinTypes;
using UnityEngine;

namespace DoubTech.OpenPath.Combat
{
    public class LaserButtonControl : MonoBehaviour
    {
        [SerializeField] private IntGameEvent weaponControl;
        private bool buttonDown;

        public void OnLaserButtonDown()
        {
            buttonDown = true;
        }

        public void OnLaserButtonUp()
        {
            buttonDown = false;
        }

        public void WorldPositionFire(Vector3 position)
        {
            //PlayerShip.Instance.shipController.WeaponController.weapon.Fire(position);
            if(buttonDown) weaponControl.Invoke(0);
        }
    }
}
