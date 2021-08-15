using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DoubTech.OpenPath.Debugging
{
    using DebuggingEssentials;
    using DoubTech.OpenPath.Controllers;
    using Sirenix.OdinInspector;

    /// <summary>
    /// Debugging Essentials commands for testing the cargo pod functionality of the game.
    /// </summary>
    [ConsoleAlias("test.cargo")]
    public class CargoDebugCommands : AbstractDebugCommands<CargoController>
    {
    }
}
