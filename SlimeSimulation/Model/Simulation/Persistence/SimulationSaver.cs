using System;
using Newtonsoft.Json;
using NLog;

namespace SlimeSimulation.Model.Simulation.Persistence
{
    public class SimulationSaver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public Exception SaveSimulation(SimulationSave simulation, string filepath)
        {
            try
            {
                var simulationAsJson = JsonConvert.SerializeObject(simulation, SerializationSettings.JsonSerializerSettings);
                System.IO.File.WriteAllText(filepath, simulationAsJson);
                Logger.Info("[SaveSimulation] Saved simulation to {0}", filepath);
                return null;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return e;
            }
        }
    }
}
