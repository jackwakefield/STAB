using System.Reflection;
using HarmonyLib;
using MonoMod.Cil;

namespace STAB.RemoveCausticLandsHue
{
    [HarmonyPatch("ZX.GameSystems.ZXMapThemeVO", "#=zjkxJraUBDjcb")]
    public class CausticLandsHuePatch
    {
        public static void ILManipulator(ILContext il, MethodBase original, ILLabel retLabel)
        {
            var cursor = new ILCursor(il);

            // find the first ldarg.1 instruction, which loads the hue values
            cursor.GotoNext(MoveType.After, instr => instr.MatchLdarg1());

            // remove the existing hue values
            cursor.RemoveRange(3);

            // change the hue values to white
            cursor.EmitLdcI4(0xFF);
            cursor.EmitLdcI4(0xFF);
            cursor.EmitLdcI4(0xFF);
        }
    }
}
