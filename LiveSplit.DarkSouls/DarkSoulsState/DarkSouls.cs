using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyHook;

namespace DarkSoulsState {
    internal static class Flags {
        public static Dictionary<string, int> Groups = new Dictionary<string, int>()
        {
            {"0", 0x00000},
            {"1", 0x00500},
            {"5", 0x05F00},
            {"6", 0x0B900},
            {"7", 0x11300},
        };

        public static Dictionary<string, int> Areas = new Dictionary<string, int>()
        {
            {"000", 00},
            {"100", 01},
            {"101", 02},
            {"102", 03},
            {"110", 04},
            {"120", 05},
            {"121", 06},
            {"130", 07},
            {"131", 08},
            {"132", 09},
            {"140", 10},
            {"141", 11},
            {"150", 12},
            {"151", 13},
            {"160", 14},
            {"170", 15},
            {"180", 16},
            {"181", 17},
        };
    }

    abstract class DarkSouls {
        /// <summary>
        /// The Game. Either Dark Souls or Remastered
        /// </summary>
        public PHook pHook { get; set; }

        /// <summary>
        /// Pointers to CharClassBase
        /// </summary>
        public PHPointer pCharClassBase { get; set; }

        /// <summary>
        /// Pointers to flags
        /// </summary>
        public PHPointer pFlags { get; set; }

        /// <summary>
        /// Loaded pointer
        /// </summary>
        public PHPointer pLoaded { get; set; }

        /// <summary>
        /// Returns if the player's game is current loaded (aka in game)
        /// </summary>
        public bool Loaded
        {
            get
            {
                return pLoaded.Resolve() != IntPtr.Zero;
            }
        }

        public DarkSouls(PHook pHook)
        {
            this.pHook = pHook;
        }

        /// <summary>
        /// Flag method that calcuates the offset according to the flag ID
        /// This is literally magic 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        private int getEventFlagOffset(int ID, out uint mask)
        {
            string idString = ID.ToString("D8");
            if (idString.Length == 8)
            {
                string group = idString.Substring(0, 1);
                string area = idString.Substring(1, 3);
                int section = Int32.Parse(idString.Substring(4, 1));
                int number = Int32.Parse(idString.Substring(5, 3));

                if (Flags.Groups.ContainsKey(group) && Flags.Areas.ContainsKey(area))
                {
                    int offset = Flags.Groups[group];
                    offset += Flags.Areas[area] * 0x500;
                    offset += section * 128;
                    offset += (number - (number % 32)) / 8;

                    mask = 0x80000000 >> (number % 32);
                    return offset;
                }
            }
            throw new ArgumentException("Unknown event flag ID: " + ID);
        }

        /// <summary>
        /// ReadEventFlag method for PTDE and Remastered
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool ReadEventFlag(int ID)
        {
            int offset = getEventFlagOffset(ID, out uint mask);
            return pFlags.ReadFlag32(offset, mask);
        }

        /// <summary>
        /// Returns the raw IGT from Memory
        /// </summary>
        /// <returns></returns>
        public abstract int MemoryIGT
        {
            get;
        }
    }
}
