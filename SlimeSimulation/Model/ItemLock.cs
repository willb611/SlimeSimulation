using System.Threading;
using NLog;

namespace SlimeSimulation.StdLibHelpers
{
    public class ItemLock<T>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private T _item;
        private int _currentAccessCount = 0;

        public T Get()
        {
            Logger.Trace("[Get] Locking..");
            Lock();
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace($"[Get] Returning item {_item}");
            }
            try
            {
                return _item;
            }
            finally
            {
                ClearLock();
            }
        }

        public void SetAndClearLock(T item)
        {
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace($"[SetAndClearLock] Setting {item}");
            }
            _item = item;
            ClearLock();
        }

        public void ClearLock()
        {
            Logger.Trace("[ClearLock] Unlocking..");
            var value = Interlocked.Exchange(ref _currentAccessCount, 0);
            if (value != 1)
            {
                Logger.Warn("[ClearLock] Attempted to clear something which was unlocked");
            }
        }

        public T Lock()
        {
            while (Interlocked.CompareExchange(ref _currentAccessCount, 1, 0) == 1)
            {
            }
            return _item;
        }
    }
}
