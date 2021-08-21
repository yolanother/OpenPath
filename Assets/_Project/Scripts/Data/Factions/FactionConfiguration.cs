/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoubTech.OpenPath.Data.Factions
{
    [CreateAssetMenu(fileName = "AiFactions", menuName = "OpenPath/Factions/Faction Configuration")]
    public class FactionConfiguration : ScriptableObject
    {
        [SerializeField] public Sprite[] emblems;
        [SerializeField] public Faction[] aiFactions;
        [SerializeField, Tooltip("The density of faction occupation in this universe." +
            "This is a normalized value where 0 means no planets of interest will be owned by a faction through " +
            "1 which means all planets of interest will be owned by a faction.")]
        internal float factionDensity = 0.2f;

        /// <summary>
        /// Get a randomly selected faction.
        /// </summary>
        /// <returns>A randomly selected faction.</returns>
        internal Faction GetRandomFaction()
        {
            return aiFactions[UnityEngine.Random.Range(0, aiFactions.Length)];
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(FactionConfiguration))]
    public class AiFactionsEditor : Editor
    {
        private List<Texture2D> textures;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            FactionConfiguration factionConfig = target as FactionConfiguration;

            GUILayout.Space(64);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Create factions with emblem textures", EditorStyles.boldLabel);
            if (GUILayout.Button("Generate Factions"))
            {
                GenerateFactions(factionConfig);

                AssetDatabase.SaveAssets();
            }
        }

        private static void GenerateFactions(FactionConfiguration factionConfig)
        {
            List<Faction> factions = new List<Faction>();

            for (int i = 0; i < factionConfig.emblems.Length; i++)
            {
                Faction faction =
                    ScriptableObject.CreateInstance<Faction>();
                faction.factionEmblem = factionConfig.emblems[i];
                faction.factionColor =
                    Color.HSVToRGB(i / (float)factionConfig.emblems.Length, .75f, .75f);

                AssetDatabase.CreateAsset(faction, $"Assets/_Project/Data/Factions/AI/AI Faction {i}.asset");
                factions.Add(faction);
            }
        }
    }
    #endif
}
