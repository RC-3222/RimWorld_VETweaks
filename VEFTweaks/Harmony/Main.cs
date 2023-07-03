using Verse;

namespace VEFTweaks.Harmony;

[StaticConstructorOnStartup]
public static class Main
{
    static Main()
    {
        new HarmonyLib.Harmony("AmP.VEFTweaks").PatchAll();
    }
}