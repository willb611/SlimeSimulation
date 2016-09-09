using Gtk;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeSimulation.View.WindowComponent
{
    public class ErrorDisplayComponent : HBox, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private Label _errorLabel;
        private List<string> _errorBuffer;

        public bool Disposed { get; private set; }

        public ErrorDisplayComponent()
        {
            _errorBuffer = new List<string>();
            _errorLabel = new Label();
            Add(_errorLabel);
        }

        public void ClearBuffer()
        {
            _errorBuffer = new List<string>();
        }

        internal void AddToDisplayBuffer(string errorMsg)
        {
            _errorBuffer.Add(errorMsg);
        }

        internal void AddToDisplayBuffer(List<string> errors)
        {
            foreach (var errorMessage in errors)
            {
                AddToDisplayBuffer(errorMessage);
            }
        }

        internal void UpdateDisplayFromBuffer()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string error in _errorBuffer)
            {
                if (sb.Length > 0)
                {
                    sb.Append(Environment.NewLine);
                }
                sb.Append(error);
            }
            _errorLabel.Text = sb.ToString();
            ClearBuffer();
        }

        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                _errorLabel.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }

        ~ErrorDisplayComponent()
        {
            Dispose(false);
        }
    }
}
