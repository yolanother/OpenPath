/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using SimpleSQL;
using UnityEngine;

namespace DoubTech.OpenPath.Data.SolarSystemScope
{
    [Serializable]
    public class Planet
    {
        [SerializeField] private string playerId;
        [Tooltip("The current population of intelligent lifeforms in 10, 000's.")]
        [SerializeField] private int population;

        /// <summary>
        /// Planet id is derived from solarsystem coordinate and planet index
        /// </summary>
        [PrimaryKey]
        public string PlanetId
        {
            get => playerId;
            set => playerId = value;
        }
        public string Name { get; set; }
        public float xCoord { get; set; }
        public float yCoord { get; set; }
        public int PlanetIndex { get; set; }

        /// <summary>
        /// The current population of intelligent lifeforms in 10,000's.
        /// </summary>
        public int Population
        {
            get => population;
            set => population = value;
        }

        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(Name) ? PlanetId : Name;
            }
        }
    }
}
