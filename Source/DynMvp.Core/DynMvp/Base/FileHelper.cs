using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Base
{
    public class FileHelper
    {
        public static void SafeSave(string srcFileName, string bakFileName, string destFileName)
        {
            File.Delete(bakFileName);
            if (File.Exists(destFileName) == true)
            {
                File.Move(destFileName, bakFileName);
            }

            if (File.Exists(srcFileName) == true)
            {
                File.Move(srcFileName, destFileName);
            }
        }

        public static void Move(string srcFileName, string destFileName)
        {
            if (File.Exists(destFileName) == true)
            {
                File.Delete(destFileName);
            }

            if (File.Exists(srcFileName) == true)
            {
                File.Move(srcFileName, destFileName);
            }
        }

        public static void ClearFolder(string folderName, params string[] excludeFileNames)
        {
            if (Directory.Exists(folderName) == false)
            {
                return;
            }

            var dir = new DirectoryInfo(folderName);

            FileInfo[] fileInfos = dir.GetFiles();
            foreach (FileInfo fi in fileInfos)
            {
                bool exist = excludeFileNames != null && Array.Exists(excludeFileNames, f => f == fi.Name);
                if (exist == false)
                {
                    fi.IsReadOnly = false;
                    fi.Delete();
                }
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName, excludeFileNames);
                if (di.GetFiles().Count() == 0)
                {
                    di.Delete();
                }
            }
        }

        public static void CopyDirectory(string srcDirectory, string destDirectory)
        {
            string[] srcFilePaths = Directory.GetFiles(srcDirectory);

            foreach (string srcFilePath in srcFilePaths)
            {
                string fileName = Path.GetFileName(srcFilePath);
                string destFilePath = Path.Combine(destDirectory, fileName);

                File.Copy(srcFilePath, destFilePath);
            }
        }

        public static string GetSafeFilePath(string filePath, string mainFileName, string bakFileName)
        {
            string safeFilePath = Path.Combine(filePath, mainFileName);

            if (File.Exists(safeFilePath) == false)
            {
                safeFilePath = Path.Combine(filePath, bakFileName);
            }

            if (File.Exists(safeFilePath) == true)
            {
                return safeFilePath;
            }

            return "";
        }

        public static bool CopyFile(string srcCommonFile, string dstCommonFile, bool overWrite)
        {
            if (File.Exists(srcCommonFile) == false)
            {
                return false;
            }

            try
            {
                File.Copy(srcCommonFile, dstCommonFile, overWrite);
                return true;
            }
            catch (IOException)
            { return false; }
        }


        public static void CompressZip(DirectoryInfo path, FileInfo zip, CancellationTokenSource cancellationTokenSource)
        {
            string zipFileName = zip.FullName;
            using (var fileStream = new FileStream(zipFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, false))
                {
                    CompressZip(zipArchive, "", path, cancellationTokenSource?.Token);
                }
            }
        }

        private static void CompressZip(ZipArchive zipArchive, string entryName, DirectoryInfo path, CancellationToken? cancellationToken)
        {
            FileInfo[] fileInfos = path.GetFiles();
            Array.ForEach(fileInfos, f =>
            {
                cancellationToken?.ThrowIfCancellationRequested();
                zipArchive.CreateEntryFromFile(f.FullName, Path.Combine(entryName, f.Name), CompressionLevel.Optimal);
            });

            DirectoryInfo[] dInfos = path.GetDirectories();
            Array.ForEach(dInfos, f => CompressZip(zipArchive, Path.Combine(entryName, f.Name), f, cancellationToken));
        }

        public static void DecompressZip(FileInfo zip, DirectoryInfo path)
        {

        }
    }
}
