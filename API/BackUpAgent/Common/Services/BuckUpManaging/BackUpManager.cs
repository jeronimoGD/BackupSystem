using BackUpAgent.Common.Interfaces.BackUpManaging;
using BackUpAgent.Common.Interfaces.NewFolder;
using BackUpAgent.Common.Interfaces.Utils;
using BackUpAgent.Data.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Services.BuckUpManaging
{
    public class BackUpManager : IBackUpManager
    {
        private readonly IUtils _utils;
        private readonly ILogger<BackUpManager> _logger;
        public BackUpManager(IUtils utils, ILogger<BackUpManager> logger)
        {
            _utils = utils;
            _logger = logger;
        }

        public async Task<BackUpHistory> DoBackUp(BackUpConfiguration conf)
        {
            string dateFormat = "dd-MM-yyyy_HH-mm-ss";
            string backUpName = $"{conf.TarjetDbName}_{DateTime.Now.ToString(dateFormat)}";
            string connectionString = $"Server=B258LSL; Database={conf.SourceDbName}; TrustServerCertificate=true; Trusted_Connection=true; MultipleActiveResultSets=true";
            string originDatabaseName = conf.SourceDbName;
            string excludedTablesSqlFormat = "";

            BackUpHistory backUpRecord = new BackUpHistory()
            {
                IsSuccessfull = false,
                BackUpSizeInMB = 0,
                BuckUpDate =  DateTime.UtcNow,
                BackUpName = backUpName,
                BackUpPath = "",
                AvailableToDownload = false
            };

            try
            {
                if (conf.ExcludedTablesJsonList != string.Empty && conf.ExcludedTablesJsonList != null)
                {
                    // TODO: Implement partial backups
                    // excludedTablesSqlFormat = string.Join(",", JsonConvert.DeserializeObject<List<string>>(conf.ExcludedTablesJsonList));
                }

                string backUpsFolder = "Backups";
                string targetDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, backUpsFolder, conf.ConfigurationName);

                if (!Directory.Exists(targetDirectoryPath))
                {
                    try
                    {
                        Directory.CreateDirectory(targetDirectoryPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error al crear la carpeta '{backUpsFolder}': {ex.Message}");
                        return backUpRecord;
                    }
                }

                string backupPath = Path.Combine(targetDirectoryPath, $"{backUpName}.bak");

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

                _logger.LogInformation($"Back up finished succesfully at: {backupPath}");
                backUpRecord.IsSuccessfull = true;
                backUpRecord.BackUpPath = backupPath;
                backUpRecord.BackUpSizeInMB = _utils.CalculateFileSizeInMB(backupPath);
                backUpRecord.Description = "Back up finished succesfully!";

                string regexPattern = $"{Regex.Escape(conf.TarjetDbName)}_\\.?";
                _utils.DeleteOldFilesKeepingN(targetDirectoryPath, backUpName, regexPattern, conf.LastNBackUpsToStore);
                // TODO: set if avaibale to download when online back ups are developed
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                backUpRecord.Description = e.Message;
            }

            return backUpRecord;
        }


       
    }
}
