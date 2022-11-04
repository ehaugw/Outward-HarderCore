using BepInEx;

namespace HarderCore
{
    using UnityEngine;
    using HarmonyLib;
    using NodeCanvas.Tasks.Conditions;

    [BepInPlugin(GUID, NAME, VERSION)]
    public class HarderCore : BaseUnityPlugin
    {
        public const string GUID = "com.ehaugw.hardercore";
        public const string VERSION = "2.0.0";
        public const string NAME = "Harder Core";


        internal void Awake()
        {
            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }

        [HarmonyLib.HarmonyPatch(typeof(DefeatScenariosManager), "ActivateDefeatScenario")]
        public class Character_SendPerformAttackTrivial
        {
            [HarmonyPrefix]
            public static bool Prefix(DefeatScenariosManager __instance, ref DefeatScenario _scenario)
            {
                __instance.LastActivationNetworkTime = (float)PhotonNetwork.time;
                if (!_scenario)
                {
                    return true;//call the original function
                }
                if (CharacterManager.Instance.HardcoreMode && _scenario.SupportHardcore)
                {
                    __instance.photonView.RPC("DefeatHardcoreDeath", PhotonTargets.All, new object[0]);
                    return false;//dont call the original function
                }
                return true;//call the original function

            }
        }
    }
}