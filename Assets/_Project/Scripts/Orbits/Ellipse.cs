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
    [Serializable]
    public class Ellipse
    {
        [SerializeField] public float radiusX;
        [SerializeField] public float radiusY;

        public Vector2 Evaluate(float t)
        {
            var angle = Mathf.Deg2Rad * 360 * t;
            var x = Mathf.Sin(angle) * radiusX;
            var y = Mathf.Cos(angle) * radiusY;
            return new Vector2(x, y);
        }
    }
}
