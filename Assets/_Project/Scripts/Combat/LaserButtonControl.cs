/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Controllers;
using DoubTech.ScriptableEvents.BuiltinTypes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DoubTech.OpenPath.Combat
{
    public class LaserButtonControl : MonoBehaviour
    {
        [SerializeField] private IntGameEvent weaponControl;
        [SerializeField] private Transform raycastOrigin;

        [SerializeField] private bool debugWeaponType = true;
        [SerializeField] private int debugWeaponIndex = 0;

        private bool buttonDown;

        private void Start()
        {
            #if !UNITY_EDITOR
            debugWeaponType = false;
            #endif
        }

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
            var weapon = PlayerShip.Instance.shipController.WeaponController.weapon;
            var impact = Physics.OverlapSphere(position, 1);
            if (impact.Length > 0)
            {
                var ship = impact[0].GetComponentInParent<ShipController>();
                Debug.Log("Hit: " + ship.name);
                PlayerShip.Instance.shipController.WeaponController.weapon.Fire(ship.transform);
            }

            if(buttonDown)
                weapon.fireWeaponEvent.Invoke(debugWeaponType
                    ? debugWeaponIndex
                    : weapon.weaponEffectIndex);
        }

        [Button]
        public void Fire()
        {
            var weapon = PlayerShip.Instance.shipController.WeaponController.weapon;
            if(Physics.Raycast(raycastOrigin.position, PlayerShip.Transform.forward, out var hit, 1000))
            {
                var ship = hit.transform.GetComponentInParent<ShipController>();
                if (ship)
                {
                    Debug.Log("Hit: " + ship.name);
                    weapon.Fire(ship.transform);
                }
            }

            weapon.fireWeaponEvent.Invoke(debugWeaponType ? debugWeaponIndex : weapon.weaponEffectIndex);
        }
    }
}
