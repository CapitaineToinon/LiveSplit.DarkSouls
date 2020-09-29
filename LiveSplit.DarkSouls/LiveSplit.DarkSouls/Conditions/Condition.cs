using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsState;

namespace LiveSplit.DarkSouls {
    abstract class Condition {
        /// <summary>
        /// Name of the condition, used for the UI
        /// </summary>
        abstract public string Name { get; }

        /// <summary>
        /// All conditions have access to an internal GameState
        /// as a singleton. Conditions aren't responsable for 
        /// updating or reset the GameState though. Conditions
        /// only use the GameState's events to be notified of
        /// changes.
        /// </summary>
        internal GameState GameState => GameState.Instance;

        /// <summary>
        /// Event for when the condition is completed. Will only
        /// fire once. Needs to call Reset() if you want the 
        /// condition to happen again.
        /// </summary>
        public event EventHandler OnCompleted;

        /// <summary>
        /// Helper for child classes to raise the actual handler
        /// </summary>
        /// <param name="e"></param>
        internal void RaiseCompleted(EventArgs e)
        {
            OnCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Start listing to events to know when condition is completed
        /// </summary>
        abstract public void Start();

        /// <summary>
        /// Stops listing to events to know when condition is completed
        /// </summary>
        abstract public void Stop();

        /// <summary>
        /// Resets the requirements to default values
        /// </summary>
        abstract public void Reset();
    }
}
