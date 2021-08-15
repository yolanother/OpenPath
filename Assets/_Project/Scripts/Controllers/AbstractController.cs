using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DoubTech.OpenPath.Controllers
{
    public abstract class AbstractController : MonoBehaviour
    {
        /// <summary>
        /// A string describing the current status of this controller.
        /// </summary>
        /// <returns>A string describing the current status of this controller.</returns>
        public abstract string StatusAsString();
    }
}
