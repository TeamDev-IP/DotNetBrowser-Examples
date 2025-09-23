using System;
using System.IO;
using Google.Protobuf;
using Com.Teamdev.Dotnetbrowser.Prefs;

namespace ReactExample.Desktop.Rpc
{
    public class Prefs
    {
        public Account Account { get; set; }
        public ProfilePicture ProfilePicture { get; set; }
        public General General { get; set; }
        public Appearance Appearance { get; set; }
        public Notifications Notifications { get; set; }
    }

    public static class PrefsFile
    {
        private static readonly string PreferencesFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ReactExample", "preferences.bin");

        public static bool Exists()
        {
            return File.Exists(PreferencesFile);
        }

        public static Prefs Read()
        {
            if (!Exists())
                throw new FileNotFoundException("Preferences file not found.");

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
            lock(PreferencesFile)
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
