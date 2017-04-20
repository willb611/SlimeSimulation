using System.Collections.Generic;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.Pathing
{
    public class AllPaths
    {
        private readonly Dictionary<FoodSourceNode, List<Path>> _paths = new Dictionary<FoodSourceNode, List<Path>>();
        private readonly List<Path> _allPathsAsList = new List<Path>();

        public AllPaths(SlimeNetwork slimeNetwork)
        {
            var pathFinder = new PathFinder();
            List<FoodSourceNode> foodSources = new List<FoodSourceNode>(slimeNetwork.FoodSources);
            int fsCount = foodSources.Count;
            for (int i = 0; i < fsCount - 1; i++)
            {
                for (int j = i + 1; j < fsCount; j++)
                {
                    var a = foodSources[i];
                    var b = foodSources[j];
                    var path = pathFinder.FindPath(slimeNetwork, new Route(a, b));
                    if (path != null)
                    {
                        _allPathsAsList.Add(path);
                        var fromA = new List<Path>();
                        if (_paths.ContainsKey(a))
                        {
                            fromA = _paths[a];
                        }
                        fromA.Add(path);
                        _paths[a] = fromA;
                    }
                }
            }
        }

        public Path PathBetween(FoodSourceNode a, FoodSourceNode b)
        {
            if (_paths.ContainsKey(a))
            {
                foreach (var path in _paths[a])
                {
                    if (path.IsPathBetween(a, b))
                    {
                        return path;
                    }
                }
            }
            return null;
        }

        public List<Path> AsList()
        {
            return new List<Path>(_allPathsAsList);
        }
    }
}
