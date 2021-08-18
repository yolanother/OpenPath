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
            var impact = Physics.OverlapSphere(position, 1);
            if (impact.Length > 0)
            {
                Debug.Log("Hit: " + impact[0].name);
                PlayerShip.Instance.shipController.WeaponController.weapon.Fire(impact[0].transform);
            }

            if(buttonDown) weaponControl.Invoke(0);
        }

        public void Fire()
        {
            if(Physics.Raycast(PlayerShip.Transform.position, PlayerShip.Transform.forward, out var hit, 1000))
            {
                Debug.Log("Hit: " + hit.transform.name);
                PlayerShip.Instance.shipController.WeaponController.weapon
                    .Fire(hit.transform);
            }

            weaponControl.Invoke(0);
        }
    }
}
