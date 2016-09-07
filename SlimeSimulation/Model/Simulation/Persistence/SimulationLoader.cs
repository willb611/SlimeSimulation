using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace SlimeSimulation.Model.Simulation.Persistence
{
    public class SimulationLoader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public SimulationLoader()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public SimulationSave LoadSimulationFromFile(string filepath)
        {
            try
            {
                string fileAsText = File.ReadAllText(filepath);
                SimulationSave simulationSave = JsonConvert.DeserializeObject<SimulationSave>(fileAsText, _jsonSerializerSettings);
                Logger.Info("[LoadSimulationFromFile] Succesfully loaded in simulation from file {0}", filepath);
                return simulationSave;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }
    }
}
