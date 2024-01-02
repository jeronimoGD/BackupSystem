﻿namespace BackupSystem.Common.Interfaces.Hubs
{
    public interface ISignalRConnectionManager
    {
        int Count { get; }
        void Add(Guid key, string connectionId);
        IEnumerable<string> GetConnections(Guid key);
        Guid GetConnectionKey(string connecionId);
        void Remove(Guid key, string connectionId);
    }
}
