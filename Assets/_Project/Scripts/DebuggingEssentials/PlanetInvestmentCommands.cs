using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.Controllers;
using DebuggingEssentials;
using Sirenix.OdinInspector;

namespace DoubTech.OpenPath.Debugging
{
    [ConsoleAlias("test.investment")]
    public class PlanetInvestmentCommands : AbstractDebugCommands<PlanetInvestmentController>
    {
        [Button(), HideInEditorMode]
        [ConsoleCommand("healthcare", "Find the planet within sensor range with the lowest healthcare and invest credits there (default 1000 credits).")]
        public void InvestInHealthcare(float credits = 1000)
        {
            controller.InvestInHealthcare(1000);
        }
    }
}
