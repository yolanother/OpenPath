/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using UnityEngine;

namespace DoubTech.OpenPath.Orbits
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 axis;
        [SerializeField] private float angularSpeed = .25f;

        private void Update()
        {
            transform.RotateAround(transform.position, axis, Time.deltaTime * angularSpeed);
        }
    }
}
