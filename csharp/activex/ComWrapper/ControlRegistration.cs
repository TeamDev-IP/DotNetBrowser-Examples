#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ComWrapper.WinForms
{
    internal static class ControlRegistration
    {
        /// <summary>
        ///     When the container resizes the space allocated to displaying one of the object's presentations,
        ///     the object wants to recompose the presentation. This means that on resize, the object wants
        ///     to do more than scale its picture.
        /// </summary>
        private const int OLEMISC_RECOMPOSEONRESIZE = 1;

        /// <summary>
        ///     This object cannot be the link source that when bound to activates (runs) the object.
        ///     If the object is selected and copied to the clipboard, the object's container can offer a link
        ///     in a clipboard data transfer that, when bound, must connect to the outside of the object.
        ///     The user would see the object selected in its container, not open for editing.
        ///     Rather than doing this, the container can simply refuse to offer a link source
        ///     when transferring objects with this bit set.
        /// </summary>
        private const int OLEMISC_CANTLINKINSIDE = 16;

        /// <summary>
        ///     This object is capable of activating in-place, without requiring installation of menus
        ///     and toolbars to run. Several such objects can be active concurrently. Some containers,
        ///     such as forms, may choose to activate such objects automatically.
        /// </summary>
        private const int OLEMISC_INSIDEOUT = 128;

        /// <summary>
        ///     This bit is set only when OLEMISC_INSIDEOUT is set, and indicates that this object
        ///     prefers to be activated whenever it is visible. Some containers may always ignore this hint.
        /// </summary>
        private const int OLEMISC_ACTIVATEWHENVISIBLE = 256;

        /// <summary>
        ///     This value is used with controls. It indicates that the control wants to use IOleObject::SetClientSite
        ///     as its initialization function, even before a call such as IPersistStreamInit::InitNew or
        ///     IPersistStorage::InitNew. This allows the control to access a container's ambient properties
        ///     before loading information from persistent storage. Note that the current implementations of OleCreate,
        ///     OleCreateFromData, OleCreateFromFile, OleLoad, and the default handler do not understand this value.
        ///     Control containers that wish to honor this value must currently implement their own versions of
        ///     these functions in order to establish the correct initialization sequence for the control.
        /// </summary>
        private const int OLEMISC_SETCLIENTSITEFIRST = 131072;

        /// <summary>
        ///     Current process bitness.
        /// </summary>
        private static readonly string Bitness = Environment.Is64BitProcess ? "64-bit" : "32-bit";

        public static void RegisterControl(Type t, int iconResourceIndex = 101)
        {
            try
            {
                if (t == null)
                {
                    throw new ArgumentNullException(nameof(t));
                }

                if (!typeof(Control).IsAssignableFrom(t))
                {
                    throw new ArgumentException("Type argument must be a Windows Forms control.");
                }


                string key = $"CLSID\\{t.GUID:B}";

                using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, true))
                {
                    // InprocServer32

                    using (RegistryKey inprocServerKey = registryKey?.OpenSubKey("InprocServer32", true))
                    {
                        //Override the default value - to make sure that the applications will be able to locate the DLL
                        inprocServerKey?.SetValue(null, $@"{Environment.SystemDirectory}\mscoree.dll");
                        // Create the CodeBase entry - needed if not string named and GACced.
                        inprocServerKey?.SetValue("CodeBase", t.Assembly.CodeBase);
                    }

                    // Control
                    // Create the 'Control' key - this allows our control to show up in 
                    // the ActiveX control container 
                    RegistryKey controlKey = registryKey?.CreateSubKey("Control");
                    controlKey?.Close();

                    // MiscStatus
                    using (RegistryKey miscKey = registryKey?.CreateSubKey("MiscStatus"))
                    {
                        const int miscStatusValue = OLEMISC_RECOMPOSEONRESIZE
                                                    + OLEMISC_CANTLINKINSIDE
                                                    + OLEMISC_INSIDEOUT
                                                    + OLEMISC_ACTIVATEWHENVISIBLE
                                                    + OLEMISC_SETCLIENTSITEFIRST;

                        miscKey?.SetValue("", miscStatusValue.ToString(), RegistryValueKind.String);
                    }

                    // ToolBoxBitmap32
                    using (RegistryKey bitmapKey = registryKey?.CreateSubKey("ToolboxBitmap32"))
                    {
                        bitmapKey?.SetValue("", $"{t.Assembly.Location}, {iconResourceIndex}",
                                            RegistryValueKind.String);
                    }

                    // TypeLib
                    using (RegistryKey typeLibKey = registryKey?.CreateSubKey("TypeLib"))
                    {
                        Guid libId = Marshal.GetTypeLibGuidForAssembly(t.Assembly);
                        typeLibKey?.SetValue("", libId.ToString("B"), RegistryValueKind.String);
                    }

                    // Version
                    using (RegistryKey versionKey = registryKey?.CreateSubKey("Version"))
                    {
                        int major, minor;
                        Marshal.GetTypeLibVersionForAssembly(t.Assembly, out major, out minor);
                        versionKey?.SetValue("", $"{major}.{minor}");
                    }
                }

                EventLogWrapper.Log($"Control registered for {Bitness} applications: {t.FullName}, {key}",
                                    EventLogEntryType.Information, 200);
            }
            catch (Exception ex)
            {
                EventLogWrapper.Log($"Control was not registered: {t.FullName}\n{ex}", EventLogEntryType.Error, 500);
            }
        }

        public static void UnregisterControl(Type t)
        {
            try
            {
                if (t == null)
                {
                    throw new ArgumentNullException(nameof(t));
                }

                if (!typeof(Control).IsAssignableFrom(t))
                {
                    throw new ArgumentException("Type argument must be a Windows Forms control.");
                }

                // CLSID
                string key = $"CLSID\\{t.GUID:B}";
                Registry.ClassesRoot.DeleteSubKeyTree(key);
                EventLogWrapper.Log($"Control unregistered for {Bitness} applications: {t.FullName}, {key}",
                                    EventLogEntryType.Information, 200);
            }
            catch (Exception ex)
            {
                EventLogWrapper.Log($"Control was not unregistered: {t.FullName}\n{ex}", EventLogEntryType.Error, 500);
            }
        }
    }
}
