using BackUpAgent.Common.Interfaces.BackUpManaging;
using BackUpAgent.Common.Interfaces.NewFolder;
using BackUpAgent.Common.Interfaces.Utils;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackUpAgent.Common.Services.BuckUpManaging
{
    public class BackUpManager : IBackUpManager
    {
        private readonly IUtils _utils;
        private readonly ILogger<BackUpManager> _logger;
        private readonly AppSettings _appSettings;

        public BackUpManager(IUtils utils, ILogger<BackUpManager> logger, IOptions<AppSettings> appSettings)
        {
            _utils = utils;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task<BackUpHistory> DoBackUp(BackUpConfiguration conf)
        {
            string targetBackUpName = $"{conf.TarjetDbName}_{DateTime.Now.ToString(_appSettings.DefaultDateFormat)}";
            string backUpConnectionString = $"Server={_appSettings.BackUpSettings.SqlSeverName}; Database={conf.SourceDbName}; TrustServerCertificate=true; Trusted_Connection=true; MultipleActiveResultSets=true";
            
            BackUpHistory backUpRecord = new BackUpHistory()
            {
                IsSuccessfull = false,
                BackUpSizeInMB = 0,
                BuckUpDate =  DateTime.UtcNow,
                BackUpName = targetBackUpName,
                BackUpPath = "",
                AvailableToDownload = false
            };

            try
            {
                string backUpsFolder = "Backups";
                string targetDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, backUpsFolder, conf.ConfigurationName);

                if (_utils.CreateDirectoryIfDoeNotExist(targetDirectoryPath))
                {
                    string targetbBackupPath = Path.Combine(targetDirectoryPath, $"{targetBackUpName}.bak");

                    if (conf.ExcludedTablesJsonList == string.Empty || conf.ExcludedTablesJsonList == null)
                    {
                        ExceuteBackUp(backUpConnectionString, conf.SourceDbName, targetbBackupPath, targetBackUpName);
                      
                    }
                    else
                    {
                        string tempDatabaseName = $"TempDatabase({DateTime.Now.ToString(_appSettings.DefaultDateFormat)})";
                        
                        string duplicateConnectionString = $"Data Source={_appSettings.BackUpSettings.SqlSeverName}; Database={conf.SourceDbName}; TrustServerCertificate=true; Trusted_Connection=true; MultipleActiveResultSets=true";
                        _logger.LogDebug("Duplicationg database to create temporal DB");
                        CopyDatabase(conf.SourceDbName, tempDatabaseName, _appSettings.BackUpSettings.SqlSeverName);

                        List<string> excludedTablesSqlFormat = JsonConvert.DeserializeObject<List<string>>(conf.ExcludedTablesJsonList);
                        string tempDatabaseConnectionString = $"Data Source={_appSettings.BackUpSettings.SqlSeverName}; Database={tempDatabaseName}; TrustServerCertificate=true; Trusted_Connection=true; MultipleActiveResultSets=true";
                        _logger.LogDebug($"Cleaning tables {conf.ExcludedTablesJsonList}");
                        DeleteTablesFromDatabase(tempDatabaseConnectionString, excludedTablesSqlFormat.ToArray());


                        _logger.LogDebug($"Executing back up on temporsal DB");
                        ExceuteBackUp(tempDatabaseConnectionString, tempDatabaseName, targetbBackupPath, targetBackUpName);

                        tempDatabaseConnectionString = $"Data Source={_appSettings.BackUpSettings.SqlSeverName}; Initial Catalog=master; TrustServerCertificate=true; Trusted_Connection=true; MultipleActiveResultSets=true";
                        _logger.LogDebug($"Droping temporal DB");
                        DropTempDB(tempDatabaseConnectionString, tempDatabaseName);
                    }

                    backUpRecord.IsSuccessfull = true;
                    backUpRecord.BackUpPath = targetbBackupPath;
                    backUpRecord.BackUpSizeInMB = _utils.CalculateFileSizeInMB(targetbBackupPath);
                    backUpRecord.Description = "Back up finished succesfully!";
                    // TODO: set if avaibale to download when online back ups are developed
                    string regexPattern = $"{Regex.Escape(conf.TarjetDbName)}_\\.?";
                    _utils.DeleteOldFilesKeepingN(targetDirectoryPath, targetBackUpName, regexPattern, conf.LastNBackUpsToStore);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                backUpRecord.Description = e.Message;
            }

            return backUpRecord;
        }

        private void DropTempDB(string masterConnectionString, string tempDdName)
        {
            using (SqlConnection masterConnection = new SqlConnection(masterConnectionString))
            {
                masterConnection.Open();

                string closeConnectionsQuery = $"ALTER DATABASE [{tempDdName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                using (SqlCommand command = new SqlCommand(closeConnectionsQuery, masterConnection))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand dropDatabaseCommand = new SqlCommand($"DROP DATABASE [{tempDdName}]", masterConnection))
                {
                    dropDatabaseCommand.ExecuteNonQuery();
                }

                masterConnection.Close();
            }
        }

        private void CopyDatabase(string sourceDatabaseName, string newDatabaseName, string serverName)
        {

            ServerConnection sourceConnection = new ServerConnection(serverName);
            Server sourceServer = new Server(sourceConnection);
            Database sourceDatabase = sourceServer.Databases[sourceDatabaseName];

            ServerConnection newConnection = new ServerConnection(serverName);
            Server newServer = new Server(newConnection);

            Database newDatabase = new Database(newServer, newDatabaseName);
            newDatabase.Create();

            Transfer transfer = new Transfer(sourceDatabase);
            transfer.CopyAllObjects = true;
            transfer.CopyData = true;
            transfer.DestinationDatabase = newDatabase.Name;
            transfer.DestinationServer = newDatabase.Parent.Name;

            transfer.TransferData();

            sourceConnection.Disconnect();
            newConnection.Disconnect();
            _logger.LogInformation($"La base de datos '{sourceDatabaseName}' se ha copiado como '{newDatabaseName}'.");
        }

        static void DeleteTablesFromDatabase(string connectionString, string[] tablesToDelete)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (string tableName in tablesToDelete)
                {
                    string tableExistsQuery = $"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL SELECT 1 ELSE SELECT 0";
                    using (SqlCommand existsCommand = new SqlCommand(tableExistsQuery, connection))
                    {
                        bool tableExists = Convert.ToBoolean(existsCommand.ExecuteScalar());

                        if (tableExists)
                        {
                            string deleteTableQuery = $"DELETE FROM {tableName}";
                            using (SqlCommand deleteCommand = new SqlCommand(deleteTableQuery, connection))
                            {
                                deleteCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                connection.Close();
            }
        }

        private void ExceuteBackUp(string connectionString, string originDatabaseName, string targetBackupPath, string targetBackUpName)
        {
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
                    command.Parameters.AddWithValue("@BackupPath", targetBackupPath);
                    command.Parameters.AddWithValue("@BackupName", targetBackUpName);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            _logger.LogInformation($"Back up finished succesfully at: {targetBackupPath}");
        }
    }
}
