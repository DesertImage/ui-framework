using System;
using System.Linq;
using UnityEngine;

namespace UIFramework
{
    public class DeviceSimulatorTools
    {
        public static bool IsInSimulatorMode()
        {
#if UNITY_EDITOR

            var simulatorAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(i => i.GetName().Name == "Unity.DeviceSimulator.Editor");

            var simulatorWindowType = simulatorAssembly?.GetType("UnityEditor.DeviceSimulation.SimulatorWindow");

            var simulatorViewWindow =
                simulatorAssembly != null ? Resources.FindObjectsOfTypeAll(simulatorWindowType) : null;

            return simulatorViewWindow != null && simulatorViewWindow.Length > 0;
#endif
            return false;
        }
    }
}