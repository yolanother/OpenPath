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
            Data.Equipment.AbstractShipWeapon weapon = GameManager.Instance.player.WeaponController.weapon;
            //OPTIMIZATION use layers for ships and planets so we aren't grabbing other things in the scene
            Collider[] impact = Physics.OverlapSphere(position, 1);
            if (impact.Length > 0)
            {
                ShipController ship = impact[0].GetComponentInParent<ShipController>();
                Debug.Log("Hit: " + impact[0].transform.name); // sometimes we are hitting something that is not a ship, not sure what though. This will uncover it
                GameManager.Instance.player.WeaponController.weapon.Fire(ship.transform);
            }

            if(buttonDown)
                weapon.fireWeaponEvent.Invoke(debugWeaponType
                    ? debugWeaponIndex
                    : weapon.weaponEffectIndex);
        }

        [Button]
        public void Fire()
        {
            //TODO currently we can only fire a single weapon in battle, need to be able to fire the most appropriate
            var weapon = GameManager.Instance.player.WeaponController.weapon;
            if(Physics.Raycast(raycastOrigin.position, GameManager.Instance.player.transform.forward, out var hit, 1000))
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
