using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.View.WindowComponent.SimulationConfigurationComponent
{
    public class SlimeNetworkAdaptionCalculatorComponent : VBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected bool Disposed;

        private readonly FeedbackParameterControlComponent _feedbackParameterControlComponent;
        private readonly TimePerSimulationStepControlComponent _timePerSimulationStepControlComponent;

        public SlimeNetworkAdaptionCalculatorComponent(SlimeNetworkAdaptionCalculatorConfig defaultConfiguration)
        {
            _feedbackParameterControlComponent = new FeedbackParameterControlComponent(defaultConfiguration.FeedbackParam);
            _timePerSimulationStepControlComponent = new TimePerSimulationStepControlComponent(defaultConfiguration.TimePerSimulationStep);

            Add(_feedbackParameterControlComponent);
            Add(_timePerSimulationStepControlComponent);
        }

        public List<string> Errors()
        {
            var errors = new List<string>();
            errors.AddRange(_feedbackParameterControlComponent.Errors());
            errors.AddRange(_timePerSimulationStepControlComponent.Errors());
            return errors;
        }

        public SlimeNetworkAdaptionCalculatorConfig ReadConfiguration()
        {
            var feedbackParameter = _feedbackParameterControlComponent.ReadFeedbackParameter();
            var timePerSimulationStep = _timePerSimulationStepControlComponent.ReadFeedbackParameter();
            if (feedbackParameter.HasValue && timePerSimulationStep.HasValue)
            {
                return new SlimeNetworkAdaptionCalculatorConfig(feedbackParameter.Value, timePerSimulationStep.Value);
            }
            else
            {
                Logger.Warn("[ReadConfiguration] Missing values.");
                return null;
            }
        }

        public sealed override void Dispose()
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
                _feedbackParameterControlComponent.Dispose();
                _timePerSimulationStepControlComponent.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }

        ~SlimeNetworkAdaptionCalculatorComponent()
        {
            Dispose(false);
        }
    }
}
