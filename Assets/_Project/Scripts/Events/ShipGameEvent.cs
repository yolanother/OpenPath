/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.ScriptableEvents;
using UnityEngine;

namespace DoubTech.OpenPath.Events
{
    [CreateAssetMenu(fileName = "ShipEvent",
        menuName = "OpenPath/Game Events/Ship Event")]
    [Serializable]
    public class ShipGameEvent : GameEvent<ShipController>
    {

    }
}
