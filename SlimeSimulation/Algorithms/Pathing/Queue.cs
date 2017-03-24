using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeSimulation.Algorithms.Pathing
{
    public class Queue
    {
        private readonly SortedDictionary<int, List<int>> _backingDictionary;

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
            else
            {
                var list = new List<int>();
                list.Add(value);
                _backingDictionary.Add(key, list);
            }
        }

        public bool Any()
        {
            return _backingDictionary.Count > 0;
        }

        public int MinKey()
        {
            return _backingDictionary.Keys.First();
        }

        public int Get(int key)
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

        public void Remove(int key, int value)
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
