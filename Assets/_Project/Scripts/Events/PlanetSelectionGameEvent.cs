/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.ScriptableEvents;
using UnityEngine;

namespace DoubTech.OpenPath.Events
{
    [CreateAssetMenu(fileName = "PlanetSelectionEvent",
        menuName = "OpenPath/Game Events/Planet Selection Event")]
    [Serializable]
    public class PlanetSelectionGameEvent : GameEvent<PlanetInstance>
    {

    }
}
