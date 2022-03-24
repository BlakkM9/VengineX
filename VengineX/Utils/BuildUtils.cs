using System.Security.Cryptography;

namespace VengineX.Utils
{
    /// <summary>
    /// Provides utilities for building the game.
    /// </summary>
    public static class BuildUtils
    {
        public static void UpdateDataFolders(string engineDir, string gameDir, string outputDir)
        {
            string fullEngineDir = Path.GetFullPath(engineDir);
            string fullGameDir = Path.GetFullPath(gameDir);
            string fullOutputDir = Path.GetFullPath(outputDir);

            Console.WriteLine("Updating data folders for game");
            Console.WriteLine("EngineDir: " + fullEngineDir);
            Console.WriteLine("GameDir:   " + fullGameDir);
            Console.WriteLine("OutputDir: " + fullOutputDir);

            // Update folders from engine output dir, starting with underscore (underscore removed in game output dir)
            Console.WriteLine("\nFrom Engine:");
            UpdateFromDir(fullEngineDir, fullOutputDir);

            // Update folders from game project dir, starting with underscore (underscore removed in game output dir)
            Console.WriteLine("\nFrom Game:");
            UpdateFromDir(fullGameDir, fullOutputDir);

            Console.WriteLine();
        }


        private static void UpdateFromDir(string sourceDir, string targetDir)
        {
            foreach (string folder in Directory.EnumerateDirectories(sourceDir))
            {
                string folderName = Path.GetFileName(folder);

                if (folderName.StartsWith("_"))
                {
                    UpdateFolder(folder, targetDir);
                }
            }
        }


        private static void UpdateFolder(string sourceDir, string targetDir)
        {
            string rootSource = Directory.GetParent(sourceDir).FullName;

            foreach (string sourceFile in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(rootSource, sourceFile);
                string targetFile = Path.Combine(targetDir, relativePath[1..]);

                string fileChange;

                if (File.Exists(targetFile))
                {
                    fileChange = FileUpdated(sourceFile, targetFile) ? "MODIFIED" : "UP-TO-DATE";
                }
                else
                {
                    fileChange = "ADDED";
                }



                // Update file in ouput dir
                if (fileChange != "UP-TO-DATE")
                {
                    // Create folder if not existing
                    if (fileChange == "ADDED")
                    {
                        Directory.CreateDirectory(Directory.GetParent(targetFile).FullName);
                    }

                    File.Copy(sourceFile, targetFile, true);
                }

                Console.WriteLine($"checked file [{fileChange}] {sourceFile} -> {targetFile}");
            }
        }


        public static bool FileUpdated(string newFile, string oldFile)
        {
            return CalculateMD5(oldFile) != CalculateMD5(newFile);
        }


        public static string CalculateMD5(string filename)
        {
            using MD5 md5 = MD5.Create();
            using (var stream = File.OpenRead(filename))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
