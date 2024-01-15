# BACKUP-SYSTEM

See the specifications of the system in the text file named NET BACKUP SYSTEM.pdf.

## PROJECT CONFIGURATION 
To configure the system you should follow the next steps: 
### 1. API config: Fill the API appsettings.json to complete the fields betwen [] (SqlServer, DbName, SercretKey, Issuer, Audience)
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DbConexion": "Server=[You sql server]; Database=[YouDbName]; TrustServerCertificate=true; Trusted_Connection=true; MultipleActiveResultSets=true"
  },
  "APISettings": {
    "JwtAuthFields": {
      "SecretKey": "[SecretKey]",
      "Issuer": "[Issuer]",
      "Audience": "[Audience]"
    }
  }
}

### 2. Start API: 
1. Register atleast one user as Admin and one ase Agent.
2. LogIn with the Admin user.
3. Use the given token to authentificate and authorize the use of the API Enpoints with Bearer (Ex: Bearer exToken)
5. Add atleast one Agent. (Save the agent key for next step)

### 3. Agent config:  Fill the API appsettings.json to complete the fields betwen [] (SqlServer, DbName, AgentKey (From previous step), ApiURL, UserName (From previous step), Password (From previous step))
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  },
  "ConnectionStrings": {
    "DbConexion": "Server=[SqlServer]; Database=[YourDbName]; TrustServerCertificate=true; Trusted_Connection=true; MultipleActiveResultSets=true"
  },
  "AppSettings": {
    "AgentConnectionKey": "[AgentKey]",
    "AgentManagerApiUrl": "[ApiURL]",
    "DefaultDateFormat": "yyyyMMdd-HHmmss",
    "BackUpSettings": {
      "SqlSeverName": "[SqlServer]"
    },
    "LoggingCredentials": {
      "UserName": "[UserName]",
      "Password": "[Password]"
    }
  }
}

### 4. Start Agent:
1. Once the agent start (you can start as many as you want with differtent agentKeys if previouslly created in the API) it should log in the API using the UserName and Password, get authorization to start using the AgentKey and get the back up configuration (null at the moment).
2. If any problems, check the previous configuration steps following the logs to know in which of the steps is the error.

### 5. Add a BackUpConfiguration from the API
1. Create a BackUpConfiguration using the AgentKey that you started.
2. The API should send the back up configuration to the Agent and the agent should start the preriodic BackUpConfiguration foll9owing the desired behaviour. 

Ex: {"id": 1,                                                                                                                
    "configurationName": "[Name of the configuration (This will also give the name of the folder where the back ups will be stored)]",
    "sourceDbName": "[Source DB name (The back up will be aplied to this back up. Make sure it exists in your DbServer)]",
    "tarjetDbName": "[Name that the backups will have. It will be followed by the date of the back up (Ex: Example_yyyyMMdd-HHmmss)]",
    "periodicity": "[Periodicity is a number and can have the following values (Daily = 0, Weekly = 1, Biweekly = 2, Monthly = 3)]",
    "createCloudBackUp": [Boolean value (true or false) to store the back ups in the cloud or not],
    "lastNBackUpsToStore": [Number of old back ups you want to keep],
    "excludedTablesJsonList": [null as default if no tables want to be escluded. String array with the names of the tables of the source DB you want to exclude],
    "agentId": "[Id of the agent that has to execute the back ups]" }
    
3. If you delete the configuration the agent will be notified and will stop the periodic back ups.
4. If you update the configuration the agent will be notified and will update the parameters.
5. If you delete the agent, the agent wil stop itself and all the configurations related to it will be eliminated too.


### 6. Check Back Up History
For each back up the different agents will store a record with some interesting data about the result of the proccess. You can check this info from the API too.

##  Initial Tasks Breakdown
### API Related Tasks
1. Create an API project
2. Include Authorization control using .NET Identity
3. Create a Hub to ue SignalR to comunicate with the agent and check if is connected
4. Create BD models for the Users, Agents, BackUpConfigurations and Backup history to follow a code first aproach. Automate migrations.
5. Create Repository and UnitOfWork to acess the data base. 
6. Create Controlers for each entity and the required endpoints.

### Console Aplication Related Tasks
1. Create Console project
2. Creat a signalR hub to connect with the API. Receive backup configuration and confirm the process is alive.
3. Create data base to store the backups history
4. Store backup configuration in Json or DB format.
5. Implement local and online backups.
6. Manage scheduled tasks for the periodic buckups.

### React Web API UI
1. Create a basic react web API in order to consume the API.
2. Create login and registration views.
3. Create view to consult, set and update back up configurations .
4. Create view to see the back up history  
