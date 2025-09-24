#region Copyright

// Copyright © 2025, TeamDev. All rights reserved.
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
using System.IO;
using Com.Teamdev.Dotnetbrowser.Prefs;
using Google.Protobuf;

namespace ReactExample.Desktop.Rpc
{
    public class Prefs
    {
        public Account Account { get; set; }
        public Appearance Appearance { get; set; }
        public General General { get; set; }
        public Notifications Notifications { get; set; }
        public ProfilePicture ProfilePicture { get; set; }
    }

    public static class PrefsFile
    {
        private static readonly string PreferencesFile = Path.Combine(
         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
         "ReactExample", "preferences.bin");

        public static bool Exists() => File.Exists(PreferencesFile);

        public static Prefs Read()
        {
            if (!Exists())
            {
                throw new FileNotFoundException("Preferences file not found.");
            }

            using var stream = File.OpenRead(PreferencesFile);
            var prefs = new Prefs();

            // Read each message from the file (simple binary serialization)
            prefs.Account = Account.Parser.ParseDelimitedFrom(stream);
            prefs.ProfilePicture = ProfilePicture.Parser.ParseDelimitedFrom(stream);
            prefs.General = General.Parser.ParseDelimitedFrom(stream);
            prefs.Appearance = Appearance.Parser.ParseDelimitedFrom(stream);
            prefs.Notifications = Notifications.Parser.ParseDelimitedFrom(stream);

            return prefs;
        }

        public static void Write(Prefs prefs)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PreferencesFile)!);
            lock (PreferencesFile)
            {
                using var stream = File.Create(PreferencesFile);

                prefs.Account.WriteDelimitedTo(stream);
                prefs.ProfilePicture.WriteDelimitedTo(stream);
                prefs.General.WriteDelimitedTo(stream);
                prefs.Appearance.WriteDelimitedTo(stream);
                prefs.Notifications.WriteDelimitedTo(stream);
            }
        }
    }
}
