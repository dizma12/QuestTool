using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaker.Domain.Steps
{
    public interface IRuntimeStep
    {
        public string ProgressText { get; }        // "Slay enemy X 5/9"
        public bool IsComplete { get; }
        public event Action Changed;               // UI repaints on this
        public void Start();                    // subscribe to game events
        public void Finish();                  // unsubscribe
    }
}
