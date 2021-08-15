﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DoubTech.OpenPath.Data.Resources
{
    [CreateAssetMenu(fileName = "ProductionResource", menuName = "OpenPath/Config/Resource")]
    public class ProductionResource : ScriptableObject
    {
        [SerializeField, Tooltip("The base value of this resource in the Universe. This will be modified by local factors such as scarcity.")]
        float baseValue = 100;
    }
}