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
                StartCoroutine(StopFiring());
            }
        }

        private IEnumerator StopFiring()
        {
            yield return new WaitForSeconds(.25f);
            if (isFiring)
            {
                fxController.Stop();
                isFiring = false;
            }
        }
    }
}
