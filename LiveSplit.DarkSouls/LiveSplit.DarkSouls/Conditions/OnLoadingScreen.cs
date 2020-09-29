using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls {
    class OnLoadingScreen: Condition {
        int target;
        int count;

        public override string Name => "On Loading Screen";

        public OnLoadingScreen(int target)
        {
            if (target <= 0)
                throw new ArgumentOutOfRangeException("target needs to be a positive number.");

            this.target = target;
            this.count = 0;
        }

        public override void Start()
        {
            GameState.OnLoadingScreen += GameState_OnLoadingScreen;
        }

        public override void Stop()
        {
            GameState.OnLoadingScreen -= GameState_OnLoadingScreen;
        }

        public override void Reset()
        {
            this.count = 0;
        }

        private void GameState_OnLoadingScreen(object sender, EventArgs e)
        {
            this.count++;

            if (this.count == this.target)
            {
                RaiseCompleted(EventArgs.Empty);
            }
        }
    }
}
