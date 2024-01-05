using BackUpAgent.Common.Enums;
using BackUpAgent.Common.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Services.Utils
{
    public class Utils : IUtils
    {
        public int GetAmountOfDaysFromPeriodicity(Periodicity periodicity)
        {
            int days;

            switch (periodicity)
            {
                case Periodicity.Daily:
                    days = 1;
                    break;

                case Periodicity.Weekly:
                    days = 7;
                    break;

                case Periodicity.TwoWeeks:
                    days = 15;
                    break;

                case Periodicity.Monthly:
                    days = 30;
                    break;

                default:
                    days = 1;
                    break;
            }

            return days;
        }

        public double CalculateFileSizeInMB(string backUpPath)
        {
            double size = 0;
            if (File.Exists(backUpPath))
            {
                FileInfo fileInfo = new FileInfo(backUpPath);
                size = (double)fileInfo.Length / (1024 * 1024);
            }

            return size;
        }

        public void DeleteOldFilesKeepingN(string path, string fileName, string regexPattern, int filesToKeep)
        {
            DirectoryInfo backupDirInfo = new DirectoryInfo(path);
            FileInfo[] backupFiles = backupDirInfo.GetFiles()
                                                  .Where(f => Regex.IsMatch(f.Name, regexPattern))
                                                  .OrderByDescending(f => f.LastWriteTime)
                                                  .ToArray();
            
            if (backupFiles.Length > filesToKeep)
            {
                for (int i = filesToKeep; i < backupFiles.Length; i++)
                {
                    backupFiles[i].Delete();
                }
            }
        }
    }
}
