using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Algorithms.Pathing
{
    class Queue
    {
        private SortedDictionary<int, List<int>> _backingDictionary;
        public Queue()
        {
            _backingDictionary = new SortedDictionary<int, List<int>>();
        }

        public void Add(int key, int value)
        {
            if (_backingDictionary.ContainsKey(key))
            {
                var list = _backingDictionary[key];
                list.Add(value);
            }
        }

        public bool Any()
        {
            return _backingDictionary.Count > 0;
        }

        internal int MinKey()
        {
            return _backingDictionary.Keys.First();
        }

        internal int Get(int key)
        {
            var list = _backingDictionary[key];
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                throw new ApplicationException("List was empty.");
            }
        }

        internal void Remove(int key, int value)
        {
            var list = _backingDictionary[key];
            if (list.Count > 1)
            {
                list.Remove(value);
            }
            else
            {
                _backingDictionary.Remove(key);
            }
        }
    }
}
