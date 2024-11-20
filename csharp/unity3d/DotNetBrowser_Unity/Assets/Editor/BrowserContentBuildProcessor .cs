using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Assets.DnbSimple.Scripts
{
    internal class BrowserContentBuildProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            string outputPath = Path.GetDirectoryName(report.summary.outputPath);
            const string sourceLicenseFilePath = "Assets/Editor/dotnetbrowser.license";

            if (File.Exists(sourceLicenseFilePath))
            {
                File.Copy(sourceLicenseFilePath, Path.Combine(outputPath, "dotnetbrowser.license"), true);
            }

            CreateDirectories(outputPath);
            CopyDirectory("Assets/DnbSimple/Html/Menu", Path.Combine(outputPath, "DnbSimple/Html/Menu"));
            CopyDirectory("Assets/DnbSimple/Html/Chat", Path.Combine(outputPath, "DnbSimple/Html/Chat"));
            CopyDirectory("Assets/DnbFps/Html/Menu", Path.Combine(outputPath, "DnbFps/Html/Menu"));
            CopyDirectory("Assets/DnbFps/Html/Menu/Images", Path.Combine(outputPath, "DnbFps/Html/Menu/Images"));
            CopyDirectory("Assets/DnbFps/Html/Chat", Path.Combine(outputPath, "DnbFps/Html/Chat"));
        }

        private void CreateDirectory(string outputPath, string dirName)
        {
            string path = Path.Combine(outputPath, dirName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void CreateDirectories(string outputPath)
        {
            CreateDirectory(outputPath, "DnbSimple");
            CreateDirectory(outputPath, "DnbSimple/Html");
            CreateDirectory(outputPath, "DnbSimple/Html/Menu");
            CreateDirectory(outputPath, "DnbSimple/Html/Chat");

            CreateDirectory(outputPath, "DnbFps");
            CreateDirectory(outputPath, "DnbFps/Html");
            CreateDirectory(outputPath, "DnbFps/Html/Menu");
            CreateDirectory(outputPath, "DnbFps/Html/Menu/Images");
            CreateDirectory(outputPath, "DnbFps/Html/Chat");
        }

        private void CopyDirectory(string source, string target)
        {
            string[] files = Directory.GetFiles(source);
            foreach (string file in files)
            {
                if (!file.EndsWith(".meta"))
                {
                    string fileName = Path.GetFileName(file);
                    File.Copy(file, Path.Combine(target, fileName), true);
                }
            }
        }
    }
}
