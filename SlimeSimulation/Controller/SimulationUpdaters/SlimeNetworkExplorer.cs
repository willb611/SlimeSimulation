using System.Collections.Generic;
using NLog;
using SlimeSimulation.Model;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class SlimeNetworkExplorer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const double DefaultNewSlimeEdgeConnectivity = 1;
        private readonly double _connectivityOfNewSlimeEdges;

        public SlimeNetworkExplorer() : this(DefaultNewSlimeEdgeConnectivity)
        {
            
        }
        public SlimeNetworkExplorer(double connectivityOfNewSlimeEdges)
        {
            _connectivityOfNewSlimeEdges = connectivityOfNewSlimeEdges;
        }

        public SlimeNetwork ExpandSlimeInNetwork(SlimeNetwork slimeNetwork, GraphWithFoodSources graph)
        {
            Logger.Info("[ExpandSlimeInNetwork] Expanding.. ");
            var edgesConnectedToSlime = GetEdgesConnectedToSlimeInGraph(slimeNetwork, graph);
            var edgesToBeCoveredWithSlime = RemoveEdgesAlreadyCoveredBySlime(edgesConnectedToSlime, slimeNetwork);

            var slimeEdges = new HashSet<SlimeEdge>(slimeNetwork.SlimeEdges);
            foreach (var unslimedEdge in edgesToBeCoveredWithSlime)
            {
                slimeEdges.Add(new SlimeEdge(unslimedEdge, _connectivityOfNewSlimeEdges));
            }
            Logger.Info(
                $"[ExpandSlimeInNetwork] Expanded 1 step, slime now covers {(graph.EdgesInGraph.Count/(double) slimeEdges.Count)*100} percent");
            return new SlimeNetwork(slimeEdges);
        }

        private IEnumerable<Edge> RemoveEdgesAlreadyCoveredBySlime(HashSet<Edge> edgesConnectedToSlime, SlimeNetwork slimeNetwork)
        {
            foreach (var slimeEdge in slimeNetwork.SlimeEdges)
            {
                if (edgesConnectedToSlime.Contains(slimeEdge.Edge))
                {
                    edgesConnectedToSlime.Remove(slimeEdge.Edge);
                }
            }
            return edgesConnectedToSlime;
        }

        private HashSet<Edge> GetEdgesConnectedToSlimeInGraph(SlimeNetwork slimeNetwork, GraphWithFoodSources graph)
        {
            var edgesConnectedToSlime = new HashSet<Edge>();
            foreach (var slimeNode in slimeNetwork.NodesInGraph)
            {
                foreach (var edge in graph.EdgesConnectedToNode(slimeNode))
                {
                    edgesConnectedToSlime.Add(edge);
                }
            }
            return edgesConnectedToSlime;
        }
    }
}
