using System;
using Newtonsoft.Json;
using NLog;

namespace SlimeSimulation.Model.Simulation.Persistence
{
    public class SimulationSaver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public SimulationSaver()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            };
        }

        public Exception SaveSimulation(SimulationSave simulation, string filepath)
        {
            try
            {
                var simulationAsJson = JsonConvert.SerializeObject(simulation, _jsonSerializerSettings);
                System.IO.File.WriteAllText(filepath, simulationAsJson);
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
