/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections;
using System.Numerics;
using DoubTech.OpenPath.Data;
using DoubTech.OpenPath.Data.SolarSystemScope;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace DoubTech.OpenPath.UniverseScope
{
    public class GalaxyInstance : MonoBehaviour
    {
        [SerializeField] private GalaxyConfig galaxyConfig;
        [SerializeField] private SolarSystemConfig solarSystemConfig;

        private void Start()
        {
            Generate();
        }

        [Button]
        public void Generate()
        {
            StartCoroutine(AsyncGenerate());
        }

        public IEnumerator AsyncGenerate()
        {
            float verticalSeen = Camera.main.orthographicSize * 2.0f;
            float horizontalSeen = verticalSeen * Screen.height / Screen.width;

            var dimension = Mathf.Max(verticalSeen, horizontalSeen);

            Debug.Log($"Screen world dimensions: {verticalSeen},{horizontalSeen}" );
#if UNITY_EDITOR
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
#endif

            for (int i = (int) (Camera.main.transform.position.x - dimension * 2); i <
                Camera.main.transform.position.x + dimension * 2; i++)
            {
                for (int j = (int) (Camera.main.transform.position.y - dimension);
                    j < Camera.main.transform.position.y + dimension; j++)
                {
                    var coord = new Vector2(i, j);
                    Random.InitState(solarSystemConfig.GetSeed(coord));
                    var value = Random.value;
                    if (Random.value < galaxyConfig.starDensity)
                    {
                        SpawnStar(coord);
                        yield return null;
                    }
                }
            }
        }

        private void SpawnStar(Vector2 coordinates)
        {
            var starConfig = solarSystemConfig.GetStarConfig(coordinates);

            StarInstance star;
            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                star = (StarInstance) PrefabUtility.InstantiatePrefab(starConfig.StarPrefab);
            }
            else
            {
                star = Instantiate(starConfig.StarPrefab);
            }
            #else
            star = Instantiate(starConfig.StarPrefab);
            #endif

            star.StarConfig = starConfig;
            star.transform.parent = transform;
            star.name = $"S{coordinates.x}.{coordinates.y}";
            star.transform.localPosition = new Vector3(coordinates.x, coordinates.y);
            star.transform.localScale = Vector3.one * .00125f;
            star.starData.Coordinates = coordinates;
        }
    }
}
