using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsState;
using DarkSoulsData;

namespace LiveSplit.DarkSouls {
    class OnBossDefeated : Condition {
        private int id;
        private bool isDead;

        public override string Name => "On Boss Defeated";

        public OnBossDefeated(int id)
        {
            this.id = id;
            this.isDead = false;
        }

        public OnBossDefeated(BossFlag boss) : this(boss.FlagID)
        {
            // empty
        }

        public override void Start() 
        {
            GameState.OnBossDefeated += GameState_OnBossDefeated;
        }

        public override void Stop()
        {
            GameState.OnBossDefeated -= GameState_OnBossDefeated;
        }

        public override void Reset()
        {
            isDead = false;
        }

        private void GameState_OnBossDefeated(object sender, BossDefeatedEventArgs e)
        {   
            if (!isDead && e.BossID == id)
            {
                RaiseCompleted(EventArgs.Empty);
            }
        }
    }
}
