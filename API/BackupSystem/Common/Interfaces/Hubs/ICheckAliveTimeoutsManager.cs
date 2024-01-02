namespace BackupSystem.Common.Interfaces.Hubs
{
    public interface ICheckAliveTimeoutsManager
    {
        void AddTimer(Guid clientKey, Action<object> callback, object state, TimeSpan dueTime, TimeSpan period);
        Timer GetTimer(Guid timerId);
        void CancelTimer(Guid timerId);
        void ResetTimer(Guid timerId, TimeSpan dueTime, TimeSpan period);
    }
}
