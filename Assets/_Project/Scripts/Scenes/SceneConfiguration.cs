/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace DoubTech.OpenPath.Scenes
{
    [CreateAssetMenu(fileName = "SceneConfiguration", menuName = "OpenPath/Scene Configuration")]
    public class SceneConfiguration : ScriptableObject
    {
        public enum Scenes
        {
            Loading = 0,
            Galaxy = 1,
            Solarsystem = 2
        }


        public static Vector2 currentCoordinates;

        private void OnEnable()
        {
            currentCoordinates = new Vector2(
                PlayerPrefs.GetFloat("coord::x", 0),
                PlayerPrefs.GetFloat("coord::y", 0));

        }

        public static Scenes currentScene = Scenes.Loading;
        public static Scenes nextScene = Scenes.Solarsystem;

        public static void LoadStarSystem(Vector2 coords)
        {
            currentCoordinates = coords;
            PlayerPrefs.SetFloat("coord::x", coords.x);
            PlayerPrefs.SetFloat("coord::y", coords.y);

            LoadScene(Scenes.Solarsystem);
        }

        public static void LoadGalaxy()
        {
            LoadScene(Scenes.Galaxy);
        }

        public static void LoadScene(Scenes scene)
        {
            nextScene = scene;
            UnityEngine.SceneManagement.SceneManager.LoadScene((int) Scenes.Loading);
        }
    }
}
