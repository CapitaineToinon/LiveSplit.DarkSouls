using System;
using System.Collections.Generic;
using System.Diagnostics;
using PropertyHook;

namespace DarkSoulsState {
    public class MemoryState {
        public bool? IsLoadingScreen;
        public bool? IsQuitout;
        public Dictionary<int, bool?> Bosses = new Dictionary<int, bool?>(DarkSoulsData.Flags.Bosses.Length);

        public MemoryState()
        {
            Reset();
        }

        public void Reset()
        {
            IsLoadingScreen = null;
            IsQuitout = null;

            foreach (var boss in DarkSoulsData.Flags.Bosses)
                Bosses[boss.FlagID] = null;
        }
    }

    public class GameState : PHook {
        /// <summary>
        /// Singleton
        /// </summary>
        private static readonly Lazy<GameState> lazy = new Lazy<GameState>(() => new GameState());
        public static GameState Instance => lazy.Value;

        /// <summary>
        /// Constants
        /// </summary>
        private const string PTDE_NAME = "DARKSOULS";
        private const string REMASTERED_NAME = "DARK SOULS™: REMASTERED";
        private const int REFRESH_INTERVAL = 5000;
        private const int MIN_LIFE_SPAN = 5000;

        /// <summary>
        /// Process Selector
        /// </summary>
        private static Func<Process, bool> PROCESS_SELECTOR = (p) =>
        {
            return (p.MainWindowTitle == REMASTERED_NAME) || (p.ProcessName == PTDE_NAME);
        };

        /// <summary>
        /// Local private variables
        /// </summary>
        private MemoryState memoryState { get; set; }

        /// <summary>
        /// Abstract DarkSouls class
        /// Could be null
        /// </summary>
        private DarkSouls DarkSouls { get; set; }

        /// <summary>
        /// Events
        /// </summary>
        public event BossDefeatedEventHandler OnBossDefeated;
        public event EventHandler OnQuitout;
        public event EventHandler OnLoadingScreen;

        private GameState() : base(REFRESH_INTERVAL, MIN_LIFE_SPAN, PROCESS_SELECTOR)
        {
            memoryState = new MemoryState();
            OnHooked += DarkSoulsState_OnHooked;
            OnUnhooked += DarkSoulsState_OnUnhooked;
            Start();
        }

        ~GameState()
        {
            Stop();
        }

        private void DarkSoulsState_OnHooked(object sender, PHEventArgs e)
        {
            if (Is64Bit)
            {
                DarkSouls = new Remastered(this);
            } else
            {
                DarkSouls = new PrepareToDie(this);
            }
        }

        private void DarkSoulsState_OnUnhooked(object sender, PHEventArgs e)
        {
            DarkSouls = null;
        }

        public void Update()
        {
            if (DarkSouls != null)
            {
                // Flags are all false on quitouts so don't update
                if (memoryState.IsQuitout == false)
                {
                    UpdateBosses();
                }

                DetectQuitoutOrLoadingScreen();
            }
        }

        /// <summary>
        /// Reset the game state
        /// </summary>
        public void Reset()
        {
            // reset the memory state to get all values
            // to null to avoid raising events on the 
            // initial state's values
            this.memoryState.Reset();
        }

        private void UpdateBosses()
        {
            foreach (int flag in new List<int>(memoryState.Bosses.Keys))
            {
                // if value is null
                if (!memoryState.Bosses[flag].HasValue)
                {
                    // Set the initial state without raising events and continue
                    // Avoid raising events the first time we read a value, even
                    // if the value is true
                    memoryState.Bosses[flag] = DarkSouls.ReadEventFlag(flag);
                    continue;
                }

                // if boss is already dead, skip it
                if (memoryState.Bosses[flag] == true)
                {
                    continue;
                }

                // if boss isn't dead yet, check the current memory state
                if (memoryState.Bosses[flag] == false && DarkSouls.ReadEventFlag(flag))
                {
                    // from alive to dead -> raise the event
                    memoryState.Bosses[flag] = true;
                    OnBossDefeated?.Invoke(this, new BossDefeatedEventArgs()
                    {
                        BossID = flag
                    });
                }
            }
        }

        /// <summary>
        /// Raise event on quitouts or loading screens
        /// </summary>
        private void DetectQuitoutOrLoadingScreen()
        {
            int _tmpIGT = DarkSouls.MemoryIGT;
            bool _isLoaded = DarkSouls.Loaded;

            if (_tmpIGT == 0 && !_isLoaded)
            {
                if (memoryState.IsQuitout == false)
                {
                    memoryState.IsQuitout = true;
                    OnQuitout?.Invoke(this, EventArgs.Empty);
                }
            }

            if (_tmpIGT > 0 && _isLoaded)
            {
                memoryState.IsQuitout = false;
            }

            // If it is not a quitout, then check if it's a simple loading screen instead
            if (_isLoaded == false && memoryState.IsQuitout == false)
            {
                if (memoryState.IsLoadingScreen == false)
                {
                    memoryState.IsLoadingScreen = true;
                    OnLoadingScreen?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                memoryState.IsLoadingScreen = false;
            }
        }
    }
}
