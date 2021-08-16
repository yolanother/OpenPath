/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Data.Resources;
using DoubTech.OpenPath.UniverseScope;
using UnityEngine;

namespace DoubTech.OpenPath.Data.SolarSystemScope
{
    [CreateAssetMenu(fileName = "StarConfig", menuName = "OpenPath/Config/Star Config")]
    public class StarConfig : ScriptableObject
    {
        [SerializeField] private StarAppearenceManager starPrefab;
        [SerializeField] public Color starColor;
        [SerializeField] public float hdrMultiplier = 10;
        [SerializeField] public ResourceModifier[] resourceModifiers;

        public StarAppearenceManager StarPrefab => starPrefab;
    }

    [Serializable]
    public class ResourceModifier
    {
        public ProductionResource source;
        [Tooltip("A modifier to the chance of this resource appearing on this planet. " +
            "The base chance is set in the ProductionResource scriptable object."),
         Range(-1f, 1f)]
        public float modificationPercent;
    }
}
