using System;
using System.Collections.Generic;
using NLog;
using NLog.Fluent;
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
            foreach (var node in pathThroughNodes)
            {
                Logger.Info("Found node: " + node);
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
    }
}
