/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.OpenPath.Events
{
    public class TriggerEvent : MonoBehaviour
    {
        [SerializeField] private string tag;

        [SerializeField] private UnityEvent<Collider> onTriggerEntered = new UnityEvent<Collider>();
        [SerializeField] private UnityEvent<Collider> onTriggerExited = new UnityEvent<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                if (other.CompareTag(tag))
                {
                    onTriggerEntered.Invoke(other);
                }
            }
            else
            {
                onTriggerEntered.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                if (other.CompareTag(tag))
                {
                    onTriggerExited.Invoke(other);
                }
            }
            else
            {
                onTriggerExited.Invoke(other);
            }
        }
    }
}
