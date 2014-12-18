using System;
using System.Diagnostics;
using System.IO;

namespace FlauLib.Tools
{
    public abstract class AutoUpdateBase
    {
        public void CheckAndUpdate()
        {
            // Wait until other instances of this process are closed
            var currentProcess = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);
            // Loop through processes with the same name 
            foreach (var process in processes)
            {
                // Ignore the current process
                if (process.Id == currentProcess.Id) continue;
                // Close or kill other instances
                process.Close();
                process.WaitForExit(1000);
                // Ignore closed processes
                if (process.HasExited) continue;
                process.Kill();
                process.WaitForExit(1000);
            }

            // Update routine
            const string updateDirName = "AutoUpdateTemp";
            string exeName = AppDomain.CurrentDomain.FriendlyName;
            string currDir = Directory.GetCurrentDirectory();
            if (currDir.EndsWith(updateDirName))
            {
                // We're running in temp dir
                var mainDir = Directory.GetParent(currDir).FullName;
                var tempDir = currDir;
                // Copy to main
                foreach (var file in new DirectoryInfo(tempDir).GetFiles())
                {
                    file.CopyTo(Path.Combine(mainDir, file.Name), true);
                }
                // Run from main
                var startInfo = new ProcessStartInfo(Path.Combine(mainDir, exeName));
                startInfo.WorkingDirectory = mainDir;
                Process.Start(startInfo);
                Terminate();
            }
            else
            {
                // We're running in main directory
                var tempDir = Path.Combine(currDir, updateDirName);
                // Delete possibly existing temp
                if (Directory.Exists(tempDir)) { Directory.Delete(tempDir, true); }

                // Get versions
                var currentVersion = GetCurrentVersion();
                var newestVersion = GetNewestVersion();

                // Compare versions
                if (currentVersion != newestVersion)
                {
                    // Version changed, confirm the update
                    if (UpdateConfirmation(currentVersion, newestVersion))
                    {
                        // Prepare new version
                        PrepareNewVersion(tempDir);
                        // Run from temp
                        var startInfo = new ProcessStartInfo(Path.Combine(tempDir, exeName));
                        startInfo.WorkingDirectory = tempDir;
                        Process.Start(startInfo);
                        Terminate();
                    }
                }
            }
        }

        /// <summary>
        /// Get the current version
        /// </summary>
        /// <returns>Current version-string</returns>
        public abstract string GetCurrentVersion();

        /// <summary>
        /// Get the newest version
        /// </summary>
        /// <returns>Newest version-string</returns>
        public abstract string GetNewestVersion();

        /// <summary>
        /// Terminate the application
        /// </summary>
        public abstract void Terminate();

        /// <summary>
        /// Prepare the new version of the application in the given directory
        /// </summary>
        /// <param name="preparationDirectoryPath">The directory where the new version must be placed</param>
        public abstract void PrepareNewVersion(string preparationDirectoryPath);

        /// <summary>
        /// Optional method to confirm the update
        /// </summary>
        /// <param name="currentVersion">The current version-string</param>
        /// <param name="newestVersion">The newest version-string</param>
        /// <returns>true if it should update, false otherwise</returns>
        public virtual bool UpdateConfirmation(string currentVersion, string newestVersion)
        {
            return true;
        }
    }
}
