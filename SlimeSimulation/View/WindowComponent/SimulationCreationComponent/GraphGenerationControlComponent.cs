using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.View.WindowComponent.SimulationCreationComponent
{
    public class GraphGenerationControlComponent : VBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly LatticeGenerationControlComponent _latticeGenerationControlComponent;
        private readonly ComboBox _generatorInputComboBox;
        protected bool Disposed;

        public GraphGenerationControlComponent(GraphWithFoodSourceGenerationConfig defaultConfig)
        {
            _latticeGenerationControlComponent = new LatticeGenerationControlComponent(defaultConfig.ConfigForGenerator);
            _generatorInputComboBox = new ComboBox(GraphGeneratorFactory.Descriptions);

            Add(_latticeGenerationControlComponent);
            Add(_generatorInputComboBox);
        }

        public GraphWithFoodSourceGenerationConfig ReadGenerationConfig()
        {
            var latticeGeneratorConfig = _latticeGenerationControlComponent?.ReadGenerationConfig();
            int generatorType = GraphGeneratorFactory.GetValueForDescription(_generatorInputComboBox.ActiveText);
            if (latticeGeneratorConfig != null)
            {
                return new GraphWithFoodSourceGenerationConfig(latticeGeneratorConfig, generatorType);
            }
            else
            {
                Logger.Warn("[ReadGenerationConfig] Unable to create config due to missing values from {0}", nameof(_latticeGenerationControlComponent));
                return null;
            }
        }

        public List<string> Errors()
        {
            return _latticeGenerationControlComponent.Errors();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Logger.Debug("[Dispose] Overriden method called from within " + this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                base.Dispose();
                _latticeGenerationControlComponent.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
        ~GraphGenerationControlComponent()
        {
            Dispose(false);
        }
    }
}
