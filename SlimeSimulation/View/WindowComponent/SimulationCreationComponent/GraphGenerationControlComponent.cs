using System;
using System.Collections.Generic;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.View.WindowComponent.SimulationCreationComponent
{
    public class GraphGenerationControlComponent : Table, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly LatticeGenerationControlComponent _latticeGenerationControlComponent;
        private readonly GeneratorShapeInputComponent _generatorShapeInputComponent;
        protected bool Disposed;

        public GraphGenerationControlComponent(GraphWithFoodSourceGenerationConfig defaultConfig) : base(5, 1, false)
        {
            _latticeGenerationControlComponent = new LatticeGenerationControlComponent(defaultConfig.ConfigForGenerator);
            _generatorShapeInputComponent = new GeneratorShapeInputComponent();

            Attach(_latticeGenerationControlComponent, 0, 1, 0, 3);
            Attach(_generatorShapeInputComponent, 0, 1, 3, 4);
        }

        public GraphWithFoodSourceGenerationConfig ReadGenerationConfig()
        {
            var latticeGeneratorConfig = _latticeGenerationControlComponent?.ReadGenerationConfig();
            int generatorType = _generatorShapeInputComponent.GetGeneratorTypeAsInt();
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


        public override void Dispose()
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
                _generatorShapeInputComponent.Dispose();
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
