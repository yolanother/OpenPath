/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using UnityEngine;

namespace DoubTech.OpenPath.Data
{
    [CreateAssetMenu(fileName = "GalaxyConfig", menuName = "OpenPath/Config/Galaxy Config")]
    public class GalaxyConfig : ScriptableObject
    {
        [SerializeField] public int seed = 0;
        [SerializeField] public int galaxySize = (int) Mathf.Sqrt(int.MaxValue);
    }
}
