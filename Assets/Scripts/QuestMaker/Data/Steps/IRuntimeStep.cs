using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaker.Data.Steps
{
    public interface IRuntimeStep
    {
        public string ProgressText { get; }        // "Slay enemy X 5/9"
        public bool IsComplete { get; }
        public event Action Changed;               // UI repaints on this
        public void Activate();                    // subscribe to game events
        public void Deactivate();                  // unsubscribe
    }
}
