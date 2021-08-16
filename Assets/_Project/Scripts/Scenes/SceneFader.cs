/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Text;
using DoubTech.OpenPath.UI;
using UnityEngine;

namespace DoubTech.OpenPath.Scenes
{
    public class SceneFader : MonoBehaviour
    {
        [Header("Scene")]
        [SerializeField] private SceneConfiguration.Scenes nextScene;

        [Header("UI")]
        [SerializeField] private SceneConfiguration sceneConfiguration;
        [SerializeField] private Fader fader;

        private void OnEnable()
        {
            fader.InstantlySetVisibility(true);
            fader.Visible = false;
        }

        public void LoadStarSystem(Vector2 coords)
        {
            fader.OnVisible.AddListener(() => SceneConfiguration.LoadStarSystem(coords));
            fader.Visible = true;
        }

        public void LoadGalaxy()
        {
            fader.OnVisible.AddListener(SceneConfiguration.LoadGalaxy);
            fader.Visible = true;
        }
    }
}
