/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.ScriptableEvents.BuiltinTypes;
using Lean.Common;
using Lean.Touch;
using UnityEditor.Rendering;
using UnityEngine;

namespace DoubTech.OpenPath.Controllers
{
    public class TouchPositionController : MonoBehaviour
    {
        [SerializeField] private float tapDistance = 10;
        [SerializeField] private Vector3GameEvent onTapEvent;

        private Vector3 tapPos;

        public void OnNothing()
        {
            onTapEvent.Invoke(tapPos);
        }

        public void Tap(Vector3 worldPos)
        {
            tapPos = worldPos;
        }
    }
}
