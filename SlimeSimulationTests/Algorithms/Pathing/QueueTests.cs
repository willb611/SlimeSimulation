using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Algorithms.Pathing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Algorithms.Pathing.Tests
{
    [TestClass()]
    public class QueueTests
    {
        [TestMethod()]
        public void GetTest()
        {
            var key = 6;

            var a = 2;
            var b = 3;
            Queue queue = new Queue();
            queue.Add(key, a);
            queue.Add(key, b);

            Assert.IsTrue(queue.Any());
            List<int> results = new List<int>();
            var v1 = queue.Get(key);
            results.Add(v1);
            queue.Remove(key, v1);
            var v2 = queue.Get(key);
            results.Add(v2);
            queue.Remove(key, v2);

            Assert.IsTrue(results.Contains(a));
            Assert.IsTrue(results.Contains(b));
            Assert.IsFalse(queue.Any());
        }
    }
}
