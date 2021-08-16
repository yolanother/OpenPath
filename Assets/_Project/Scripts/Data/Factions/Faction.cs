/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using Sirenix.OdinInspector;
using UnityEngine;

namespace DoubTech.OpenPath.Data.Factions
{
    [CreateAssetMenu(fileName = "Faction", menuName = "OpenPath/Factions/Faction")]
    public class Faction : ScriptableObject
    {
        [SerializeField] public Color factionColor;
        [PreviewField(200, ObjectFieldAlignment.Right)]
        [SerializeField] public Texture2D factionEmblem;
    }
}
