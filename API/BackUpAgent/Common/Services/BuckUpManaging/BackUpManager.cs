using BackUpAgent.Common.Interfaces.BackUpManaging;
using BackUpAgent.Common.Interfaces.NewFolder;
using BackUpAgent.Data.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Services.BuckUpManaging
{
    public class BackUpManager : IBackUpManager
    {

        public async Task<BackUpHistory> DoBackUp(BackUpConfiguration conf)
        {

            string backUpName = $"{conf.TarjetDbName}_{DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}";
            string connectionString = $"Server=B258LSL; Database={conf.SourceDbName}; TrustServerCertificate=true; Trusted_Connection=true; MultipleActiveResultSets=true";
            string originDatabaseName = conf.SourceDbName;
            string excludedTablesSqlFormat = "";

            BackUpHistory backUpRecord = new BackUpHistory()
            {
                IsSuccessfull = false,
                BackUpSizeInMB = 0,
                BuckUpDate =  DateTime.UtcNow,
                BackUpName = backUpName,
                AvailableToDownload = false
            };

            try
            {
                if (conf.ExcludedTablesJsonList != string.Empty && conf.ExcludedTablesJsonList != null)
                {
                    excludedTablesSqlFormat = string.Join(",", JsonConvert.DeserializeObject<List<string>>(conf.ExcludedTablesJsonList));
                }

                string backUpsFolder = "Backups";
                string rutaDirectorioBackups = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, backUpsFolder);

                if (!Directory.Exists(rutaDirectorioBackups))
                {
                    try
                    {
                        Directory.CreateDirectory(rutaDirectorioBackups);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al crear la carpeta '{backUpsFolder}': {ex.Message}");
                        return backUpRecord;
                    }
                }

                string backupPath = Path.Combine(rutaDirectorioBackups, $"{backUpName}.bak");

                string backupCommandText = $@"
                BACKUP DATABASE @DatabaseName  TO DISK = @BackupPath 
                WITH NOFORMAT, NOINIT, NAME = N'@BackupName', 
                SKIP, NOREWIND, NOUNLOAD, STATS = 10,
                CHECKSUM, CONTINUE_AFTER_ERROR;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(backupCommandText, connection))
                    {
                        command.Parameters.AddWithValue("@DatabaseName", originDatabaseName);
                        command.Parameters.AddWithValue("@BackupPath", backupPath);
                        command.Parameters.AddWithValue("@BackupName", backUpName);

                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine($"Back up finished succesfully at: {backupPath}");
                backUpRecord.IsSuccessfull = true;
                backUpRecord.BackUpPath = backupPath;
                backUpRecord.BackUpSizeInMB = CalculateBackupSize(backupPath);
                backUpRecord.Description = "Back up finished succesfully!";

                // TODO: set if avaibale to download when online back ups are developed
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                backUpRecord.Description = e.Message;
            }

            return backUpRecord;
        }

        static double CalculateBackupSize(string backUpPath)
        {
            double size = 0;
            if (File.Exists(backUpPath))
            {
                FileInfo fileInfo = new FileInfo(backUpPath);
                size = (double)fileInfo.Length / (1024 * 1024);
            }

            return size;
        }
    }
}
