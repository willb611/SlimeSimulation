using System.Collections.Generic;
using NLog;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.Pathing
{
    public class Path
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly List<Node> _nodesInPath;
        private readonly Route _route;

        public Path(List<Node> pathThroughNodes, Route route)
        {
            _nodesInPath = pathThroughNodes;
            _route = route;
            if (Logger.IsDebugEnabled)
            {
                foreach (var node in pathThroughNodes)
                {
                    Logger.Debug("Found node: " + node);
                }
            }
        }

        public int IntermediateNodesInPathCount()
        {
            return _nodesInPath.Count - 2;
        }

        public int NodesInPathCount()
        {
            return _nodesInPath.Count;
        }

        public int IntermediateFoodSourcesInPathCount()
        {
            int count = 0;
            foreach (var node in _nodesInPath)
            {
                var food = node as FoodSourceNode;
                if (food != null && !food.Equals(_route.Source)
                    && !food.Equals(_route.Sink))
                {
                    count++;
                }
            }
            return count;
        }

        public int Distance()
        {
            return _nodesInPath.Count - 1;
        }

        public bool IsPathBetween(Node a, Node b)
        {
            return (_route.Sink.Equals(a) && _route.Source.Equals(b))
                   || (_route.Sink.Equals(b) && _route.Source.Equals(a));
        }
    }
}
