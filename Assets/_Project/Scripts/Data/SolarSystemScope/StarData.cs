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
    public class StarData
    {
        [SerializeField] private string starId;
        [SerializeField] private Vector2 coordinates;
        [SerializeField] private string name;

        /// <summary>
        /// Planet id is derived from solarsystem coordinate and planet index
        /// </summary>
        [PrimaryKey]
        public string StarId
        {
            get => starId;
            set => starId = value;
        }

        public float xCoord
        {
            get => coordinates.x;
            set => coordinates.x = value;
        }
        public float yCoord
        {
            get => coordinates.y;
            set => coordinates.y = value;
        }

        public Vector2 Coordinates
        {
            get => coordinates;
            set => coordinates = value;
        }

        public string Name
        {
            get;
            set;
        }

        public string DisplayName =>
            string.IsNullOrEmpty(Name) ? $"S{coordinates.x}.{coordinates.y}" : Name;
    }
}
