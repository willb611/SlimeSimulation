using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Generation
{
    public interface IGraphWithFoodSourcesGenerator
    {
        GraphWithFoodSources Generate();
    }
}
