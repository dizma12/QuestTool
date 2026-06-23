using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaker.Runtime.Events
{
    internal class PlayerEventBus : CustomEventBus
    {
        public event Action PlayerLevelChanged;

        public void FireOnPlayerLevelChanged() => PlayerLevelChanged?.Invoke();
    }
}
