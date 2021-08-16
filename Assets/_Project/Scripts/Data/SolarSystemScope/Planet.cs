/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.OpenPath.Data.Config;
using SimpleSQL;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubTech.OpenPath.Data.SolarSystemScope
{
    /// <summary>
    /// Planet data holds all the runtime data about the planet.
    /// </summary>
    [Serializable]
    public class Planet
    {
        [SerializeField] private string playerId;
        [Tooltip("The current population of intelligent lifeforms in 10,000's.")]
        [SerializeField] private int population;
        [Tooltip("How likely is intelligent life to survive on this planet.")]
        [SerializeField] private float habitability;
        [Tooltip("Normalized Healthcare quality, the higher this number the better the healthcare. A Healthcare of 0 is roughly equivalent to automatic euthanasia upon any complaint, while 1 is a cure for almost every ill.")]
        [SerializeField, Range(0f, 1f)] private float healthcareQuality;

        internal float tickFrequency = 1f; // how often the planet should tick

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
            set
            {
                if (population == 0)
                {
                    healthcareQuality = Random.Range(0f, 0.5f);
                }
                population = value;
            }
        }

        /// <summary>
        /// How likely is intelligent life to survive on this planet.
        /// </summary>
        public float Habitability { get => habitability; set => habitability = value; }

        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(Name) ? PlanetId : Name;
            }
        }

        float births = 0;
        float deaths = 0;
        /// <summary>
        /// Called every tick to allow the planet to update its statistics.
        /// </summary>
        public void Tick()
        {
            if (population > 0)
            {
                births += (population * ((habitability / 100) * tickFrequency)) / 100;
                if (births > 1)
                {
                    population += (int)births;
                    births = 0;
                }

                // deaths from illness
                deaths += (population * (((1-healthcareQuality) / 100) * tickFrequency)) / 100;
                // deaths from age
                deaths += (population * ((habitability / 100) * tickFrequency)) / 300;
                if (deaths > 1)
                {
                    population -= (int)deaths;
                    deaths = 0;
                }

                tickFrequency = Mathf.Clamp(population / 1000000, 0.17f, 1);
            }

            Debug.LogFormat("{0} with a population of {1} has ticked at a frequency of {2}.", DisplayName, population, tickFrequency);
        }
    }
}
