using System;
using DoubTech.OpenPath.Controllers;
using DoubTech.OpenPath.SolarSystemScope;
using DoubTech.ScriptableEvents;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.OpenPath.Events
{
    [Serializable]
    public class ShipOrbitGameEventListener : GameEventListener<ShipController, PlanetInstance,
        ShipOrbitGameEvent, ShipOrbitUnityEvent>
    {
        [SerializeField] private ShipOrbitGameEvent gameEvent;
        [SerializeField] private ShipOrbitUnityEvent onEvent = new ShipOrbitUnityEvent();

        public override ShipOrbitGameEvent GameEvent => gameEvent;
        public override ShipOrbitUnityEvent OnEvent => onEvent;
    }

    [Serializable]
    public class ShipOrbitUnityEvent : UnityEvent<ShipController, PlanetInstance>
    {
    }
}
