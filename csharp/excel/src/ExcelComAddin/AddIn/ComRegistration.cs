using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using ExcelComAddin.Interop;

namespace ExcelComAddin.AddIn
{
    /// <summary>
    /// Writes and removes the registry keys that tell Excel where to find the add-in.
    /// Called automatically by <c>regasm.exe</c> via <see cref="Connect.Register"/> and
    /// <see cref="Connect.Unregister"/>.
    /// </summary>
    public static class ComRegistration
    {
        /// <summary>Returns the <c>HKCU</c> registry path for this add-in's Excel entry.</summary>
        public static string GetExcelAddInRegistryPath()
        {
            return @"Software\Microsoft\Office\Excel\Addins\" + ComConstants.ProgId;
        }

        /// <summary>Returns the registry values required by the Excel add-in manager.</summary>
        public static IReadOnlyDictionary<string, object> GetExcelAddInRegistryValues()
        {
            return new Dictionary<string, object>
            {
                { "FriendlyName", "Sales Lead Add-in" },
                { "Description", "Excel add-in for newsletter/SMS integration" },
                { "LoadBehavior", 3 },
                { "ProgId", ComConstants.ProgId },
                { "CommandLineSafe", 0 }
            };
        }

        /// <summary>Creates the add-in registry key and writes all required values under <c>HKCU</c>.</summary>
        public static void RegisterExcelAddInKeys()
        {
            using (var key = Registry.CurrentUser.CreateSubKey(GetExcelAddInRegistryPath()))
            {
                if (key == null)
                {
                    throw new InvalidOperationException("Unable to create Excel add-in registry key.");
                }

                foreach (var entry in GetExcelAddInRegistryValues())
                {
                    key.SetValue(entry.Key, entry.Value);
                }
            }
        }

        /// <summary>Deletes the add-in registry key tree from <c>HKCU</c>.</summary>
        public static void UnregisterExcelAddInKeys()
        {
            Registry.CurrentUser.DeleteSubKeyTree(GetExcelAddInRegistryPath(), false);
        }
    }
}