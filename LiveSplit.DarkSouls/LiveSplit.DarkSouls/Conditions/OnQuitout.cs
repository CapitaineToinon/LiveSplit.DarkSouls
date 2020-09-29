using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls {
    class OnQuitout : Condition {
        int target;
        int count;

        public override string Name => "On Quitout";

        public OnQuitout(int target)
        {
            if (target <= 0)
                throw new ArgumentOutOfRangeException("target needs to be a positive number.");

            this.target = target;
            this.count = 0;
        }

        public override void Start()
        {
            GameState.OnQuitout += GameState_OnQuitout;
        }

        public override void Stop()
        {
            GameState.OnQuitout -= GameState_OnQuitout;
        }

        public override void Reset()
        {
            this.count = 0;
        }

        private void GameState_OnQuitout(object sender, EventArgs e)
        {
            this.count++;
            
            if (this.count == this.target)
            {
                RaiseCompleted(EventArgs.Empty);
            }
        }
    }
}
