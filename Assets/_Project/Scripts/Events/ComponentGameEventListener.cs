/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using DoubTech.ScriptableEvents;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.OpenPath.Events
{
    [Serializable]
    public class
        ComponentGameEventListener<T> : GameEventListener<T,
            GameEvent<T>, ComponentUnityEvent<T>> where T : Component
    {
        [SerializeField] private GameEvent<T> gameEvent;

        [SerializeField] private ComponentUnityEvent<T> onEvent = new ComponentUnityEvent<T>();

        [SerializeField] private ComponentToTransformUnityEvent onTransformEvent = new
            ComponentToTransformUnityEvent();

        public override GameEvent<T> GameEvent => gameEvent;
        public override ComponentUnityEvent<T> OnEvent => onEvent;

        public override void OnEventRaised(T t)
        {
            base.OnEventRaised(t);
            onTransformEvent.Invoke(t?.transform);
        }
    }

    [Serializable]
    public class ComponentUnityEvent<T> : UnityEvent<T> where T : Component
    {
    }

    [Serializable]
    public class ComponentToTransformUnityEvent : UnityEvent<Transform>
    {
    }
}
