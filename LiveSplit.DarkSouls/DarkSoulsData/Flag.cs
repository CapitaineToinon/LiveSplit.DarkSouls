using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSoulsData {
    public enum FlagTypes {
        Boss
    }

    public class Flag {
        public FlagTypes Type { get; private set; }
        public int FlagID { get; private set; }

        public Flag(FlagTypes type, int flagID)
        {
            this.Type = type;
            this.FlagID = flagID;
        }
    }

    public class BossFlag: Flag {
        public string Name { get; private set; }

        public BossFlag(string name, int flagID): base(FlagTypes.Boss, flagID)
        {
            this.Name = name;
        }
    }
}
