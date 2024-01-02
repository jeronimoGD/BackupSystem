using BackupSystem.Common.Interfaces.Hubs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackupSystem.Common.Hubs
{
    public class TimerCallbackParams
    {
        public Guid clientKey { get; set; }
        public string connectionId{ get; set; }
    }

    public class CheckAliveTimeoutsManager : ICheckAliveTimeoutsManager
    {
        private readonly Dictionary<Guid, Timer> _checkAliveTimers = new Dictionary<Guid, Timer>();
        private readonly object _lock = new object();

        public void AddTimer(Guid clientKey, Action<object> callback, object state, TimeSpan dueTime, TimeSpan period)
        {
            var timer = new Timer(new TimerCallback(callback), state, dueTime, period);

            lock (_lock)
            {
                _checkAliveTimers.Add(clientKey, timer);
            }
        }

        public Timer GetTimer(Guid timerId)
        {
            lock (_lock)
            {
                return _checkAliveTimers.TryGetValue(timerId, out var timer) ? timer : null;
            }
        }

        public void CancelTimer(Guid timerId)
        {
            lock (_lock)
            {
                if (_checkAliveTimers.TryGetValue(timerId, out var timer))
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                    timer.Dispose();
                    _checkAliveTimers.Remove(timerId);
                }
            }
        }

        public void ResetTimer(Guid timerId, TimeSpan dueTime, TimeSpan period)
        {
            lock (_lock)
            {
                if (_checkAliveTimers.TryGetValue(timerId, out var timer))
                {
                    timer.Change(dueTime, period);
                }
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var timer in _checkAliveTimers.Values)
                {
                    timer.Dispose();
                }
                _checkAliveTimers.Clear();
            }
        }
    }
}
