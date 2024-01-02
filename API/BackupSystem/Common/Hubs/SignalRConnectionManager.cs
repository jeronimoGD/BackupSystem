using BackupSystem.Common.Interfaces.Hubs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackupSystem.Common.Hubs
{
    public class SignalRConnectionManager : ISignalRConnectionManager
    {

        private readonly Dictionary<Guid, HashSet<string>> _connections = new Dictionary<Guid, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(Guid key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    connections.Add(connectionId);
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(Guid key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public Guid GetConnectionKey(string connecionId)
        {
            var connectionKey = _connections.FirstOrDefault(x => x.Value.Contains(connecionId)).Key;
            return connectionKey;
        }

        public void Remove(Guid key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (_connections.TryGetValue(key, out connections))
                {
                    lock (connections)
                    {
                        connections.Remove(connectionId);

                        if (connections.Count == 0)
                        {
                            _connections.Remove(key);
                        }
                    }
                }
            }
        }

        public void RemoveHoleKey(Guid key)
        {
            lock (_connections)
            {
                _connections.Remove(key);
            }
        }
    }
}
