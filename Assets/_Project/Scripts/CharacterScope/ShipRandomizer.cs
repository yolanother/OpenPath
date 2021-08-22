using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DoubTech.OpenPath.Data.Equipment;
using DoubTech.OpenPath.Controllers;
using Random = UnityEngine.Random;
using DoubTech.OpenPath.Data.UniverseScope;

namespace DoubTech.OpenPath
{
    /// <summary>
    /// The ShipRandomizer can be attached to ship prefabs so that when
    /// it is instantiated in game the equipment available will be randomized.
    /// This ensures, for example, that NPC ships provide some variety.
    /// </summary>
    public class ShipRandomizer : MonoBehaviour
    {
        [Header("AI Captain")]
        [SerializeField, Tooltip("How aggressive is the captain of this ship. This will be randomized to +/- 25% of this value"), Range(0f, 1f)]
        float aggression = 0.5f;

        [Header("Weapons")]
        //TODO currently all weapons have an equal chance of being used, we should have weighted chances.
        [SerializeField, Tooltip("The weapons this ship may be equipped with.")]
        AbstractShipWeapon[] possibleWeapons;

        ShipController controller;

        private void Start()
        {
            controller = GetComponent<ShipController>();

            if (controller.isAI)
            {
                RandomizeCaptain();
            }
            RandomizeWeapons();
            Destroy(this);
        }

        /// <summary>
        /// Create the AI captains profile and missions.
        /// </summary>
        private void RandomizeCaptain()
        {
            AIShipController ai = controller as AIShipController;
            ai.aggression = Random.Range(Mathf.Clamp01(aggression * 0.75f), Mathf.Clamp01(aggression * 1.25f));
        }

        /// <summary>
        /// Select a weapon from the list of available weapons
        /// </summary>
        private void RandomizeWeapons()
        {
            ShipWeaponController[] controllers = GetComponents<ShipWeaponController>();
            for (int i = 0; i < controllers.Length; i++)
            {
                if (Random.value < aggression)
                {
                    controllers[i].defaultWeapon = possibleWeapons[Random.Range(0, possibleWeapons.Length)];
                }
            }
        }
    }
}
