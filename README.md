# BACKUP-SYSTEM

See the specifications of the system in the text file named NET BACKUP SYSTEM.pdf.

## Tasks
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
