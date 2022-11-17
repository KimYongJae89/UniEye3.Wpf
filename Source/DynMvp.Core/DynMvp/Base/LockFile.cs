using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Base
{
    public static class SystemLockHandler
    {
        private static FileStream LockFile { get; set; } = null;
        private static string LockFilePath { get; set; }

        public static bool IsLocked => LockFile != null;

        public static void CreateLockFile(string lockFilePath, string debugLogPath)
        {
            LockFilePath = lockFilePath;

            if (File.Exists(lockFilePath) == true)
            {
                LogHelper.Warn(LoggerType.StartUp, "Abnormal program termination is detected.");
                LogHelper.Warn(LoggerType.StartUp, "Debug log will be backed up.");

                try
                {
                    File.Delete(lockFilePath);

                    if (File.Exists(debugLogPath) == true)
                    {
                        string bakFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Path.GetFileName(debugLogPath);
                        try
                        {
                            File.Copy(debugLogPath, Path.Combine(Path.GetDirectoryName(debugLogPath), bakFileName));
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(StringManager.GetString("Error on copy debug log.\n") + e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(StringManager.GetString("Error on delete lock file.\n") + e.Message);
                }
            }

            LockFile = File.Open(lockFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
        }

        public static void Dispose()
        {
            LockFile?.Close();
            if (File.Exists(LockFilePath))
            {
                File.Delete(LockFilePath);
            }
        }
    }
}
