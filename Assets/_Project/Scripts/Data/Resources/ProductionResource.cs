using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace DoubTech.OpenPath.Data.Resources
{
    [CreateAssetMenu(fileName = "ProductionResource", menuName = "OpenPath/Config/Resource")]
    public class ProductionResource : ScriptableObject
    {
        [SerializeField, Tooltip("The base value of this resource in the Universe. This will be modified by local factors such as scarcity.")]
        internal float baseValue = 100;
        [SerializeField, Tooltip("The chance of this resource being found on any given planet. This may be modified on a per planet basis."),
         Range(0, 1f)]
        internal float generationChance = 0.2f;

        [PreviewField(200, ObjectFieldAlignment.Right)]
        [SerializeField] public Sprite icon;
    }
}
