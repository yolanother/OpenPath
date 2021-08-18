/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System.Collections;
using FORGE3D;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DoubTech.OpenPath.Combat
{
    public class LaserWeaponControl : MonoBehaviour
    {
        public F3DFXController fxController;
        private bool isFiring;
        [SerializeField] private Transform socket;

        [Button]
        public void Fire(int weaponType = -1)
        {
            if (!isFiring)
            {
                if (weaponType >= 0)
                {
                    fxController.DefaultFXType = (F3DFXType) weaponType;
                }

                fxController.Fire();
                isFiring = true;
                StartCoroutine(StopFiringCO());
            }
        }

        [Button]
        public void StopFiring()
        {
            StartCoroutine(StopFiringCO());
        }

        private IEnumerator StopFiringCO()
        {
            yield return new WaitForSeconds(.25f);
            if (isFiring)
            {
                fxController.Stop();
                isFiring = false;
                for (int i = socket.childCount - 1; i >= 0; i--)
                {
                    Destroy(socket.GetChild(i).gameObject);
                }
            }
        }
    }
}
