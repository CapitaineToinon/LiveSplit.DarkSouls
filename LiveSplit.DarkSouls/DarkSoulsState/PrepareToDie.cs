using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyHook;

namespace DarkSoulsState {
    class PrepareToDie : DarkSouls {
        /// <summary>
        /// Constants
        /// </summary>
        private const string CHAR_CLASS_BASE_AOB = "8B 0D ? ? ? ? 8B 7E 1C 8B 49 08 8B 46 20 81 C1 B8 01 00 00 57 51 32 DB";
        private const string FLAGS_AOB = "56 8B F1 8B 46 1C 50 A1 ? ? ? ? 32 C9";
        private const string CHR_DATA_AOB = "83 EC 14 A1 ? ? ? ? 8B 48 04 8B 40 08 53 55 56 57 89 4C 24 1C 89 44 24 20 3B C8";

        public PrepareToDie(PHook pHook) : base(pHook)
        {
            pCharClassBase = pHook.RegisterAbsoluteAOB(CHAR_CLASS_BASE_AOB, 2, 0);
            pFlags = pHook.RegisterAbsoluteAOB(FLAGS_AOB, 8, 0, 0);
            pLoaded = pHook.RegisterAbsoluteAOB(CHR_DATA_AOB, 4, 0, 0x4, 0x0);
            pHook.RescanAOB();
        }

        /// <summary>
        /// Returns the raw IGT from Memory
        /// </summary>
        /// <returns></returns>
        public override int MemoryIGT
        {
            get => pCharClassBase.ReadInt32(0x68);
        }
    }
}
