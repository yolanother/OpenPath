/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using DoubTech.OpenPath.Data.SolarSystemScope;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoubTech.OpenPath.Data.Config
{
    [CreateAssetMenu(fileName = "PlanetConfig", menuName = "OpenPath/Config/Planet Config")]
    public class PlanetConfig : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] public int minSpawnDistanceFromSun;
        [SerializeField] public int maxSpawnDistanceFromSun;
        [SerializeField] public ResourceModifier[] resourceModifiers;

        public GameObject Prefab => prefab;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PlanetConfig))]
    public class PlanetConfigEditor : Editor
    {
        private PreviewRenderUtility previewRenderUtility;
        private Editor gameObjectEditor;
        public override bool HasPreviewGUI() => true;

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            var cfg = target as PlanetConfig;


            if (gameObjectEditor == null)
            {
                gameObjectEditor = Editor.CreateEditor(cfg.Prefab);
            }
            gameObjectEditor.OnPreviewGUI(r, background);
        }
    }
#endif
}
