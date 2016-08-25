using System.Collections.Generic;
using SlimeSimulation.Model;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class SlimeNetworkExplorer
    {
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
            var edgesConnectedToSlime = GetEdgesConnectedToSlimeInGraph(slimeNetwork, graph);
            var edgesToBeCoveredWithSlime = RemoveEdgesAlreadyCoveredBySlime(edgesConnectedToSlime, slimeNetwork);

            var slimeEdges = new HashSet<SlimeEdge>(slimeNetwork.Edges);
            foreach (var unslimedEdge in edgesToBeCoveredWithSlime)
            {
                slimeEdges.Add(new SlimeEdge(unslimedEdge, _connectivityOfNewSlimeEdges));
            }
            return new SlimeNetwork(slimeEdges);
        }

        private IEnumerable<Edge> RemoveEdgesAlreadyCoveredBySlime(HashSet<Edge> edgesConnectedToSlime, SlimeNetwork slimeNetwork)
        {
            foreach (var slimeEdge in slimeNetwork.Edges)
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
            foreach (var slimeNode in slimeNetwork.Nodes)
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
