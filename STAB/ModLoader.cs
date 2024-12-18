using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using HarmonyLib;

namespace STAB
{
    public class ModLoader
    {
        /// <summary>
        /// Initialize the mod loader by loading mod assemblies and patching the game with Harmony.
        /// </summary>
        /// <param name="_">Unused, but required by ExecuteInDefaultAppDomain.</param>
        /// <returns>Unused, but required by ExecuteInDefaultAppDomain.</returns>
        public static int Initialize(string _)
        {
            GetModAssemblies().ToList().ForEach(assembly =>
            {
                try
                {
                    Harmony.CreateAndPatchAll(assembly);
                }
                catch (Exception e)
                {
                    var errorMessage = $"Failed to load mod assembly: {assembly.FullName}\n\n{e}";

                    MessageBox.Show(errorMessage, "STAB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            return 0;
        }

        /// <summary>
        /// Get assembly instances for all STAB mods in the Mods directory.
        /// </summary>
        /// <returns>Enumerable of mod assemblies.</returns>
        protected static IEnumerable<Assembly> GetModAssemblies()
        {
            var modDirectory = Path.Combine(Environment.CurrentDirectory, "Mods");

            return Directory.GetFiles(modDirectory, "STAB.*.dll")
                .Select(dll => Assembly.LoadFile(dll));
        }
    }
}
