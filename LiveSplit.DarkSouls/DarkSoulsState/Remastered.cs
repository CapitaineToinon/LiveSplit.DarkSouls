using PropertyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSoulsState {
    class Remastered : DarkSouls {
        /// <summary>
        /// Constants
        /// </summary>
        // TODO

        public Remastered(PHook pHook) : base(pHook)
        {
            // TODO
            pHook.RescanAOB();
        }

        /// <summary>
        /// Returns the raw IGT from Memory
        /// </summary>
        /// <returns></returns>
        public override int MemoryIGT
        {
            get => 0; // TODO
        }
    }
}
