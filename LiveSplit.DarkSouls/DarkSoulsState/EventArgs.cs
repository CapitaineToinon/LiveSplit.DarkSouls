using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSoulsState {
    public delegate void BossDefeatedEventHandler(object sender, BossDefeatedEventArgs e);

    public class BossDefeatedEventArgs : EventArgs {
        public int BossID { get; set; }
    }
}
