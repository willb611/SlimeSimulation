using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SlimeSimulation.Model.Simulation;

namespace SlimeSimulation.StdLibHelpers
{
    public class ItemLock<T>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private T _item;
        private int _accessCount = 0;

        public T Get()
        {
            Lock();
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
            _item = item;
            ClearLock();
        }

        public void ClearLock()
        {
            var value = Interlocked.Exchange(ref _accessCount, 0);
            if (value != 1)
            {
                Logger.Warn("[ClearLock] Attempted to clear something which was unlocked");
            }
        }

        public T Lock()
        {
            while (Interlocked.CompareExchange(ref _accessCount, 1, 0) == 1)
            {
            }
            return _item;
        }
    }
}
