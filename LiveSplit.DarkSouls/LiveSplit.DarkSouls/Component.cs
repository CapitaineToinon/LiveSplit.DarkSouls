using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DarkSoulsState;

namespace LiveSplit.DarkSouls
{
    public class Component : LogicComponent {
        public override string ComponentName
        {
            get
            {
                return "Dark Souls & Dark Souls: Remastered";
            }
        }

        private LiveSplitState state;
        private TimerModel model;
        private AutoSplitter autosplitter;

        public Component(LiveSplitState state)
        {
            this.state = state;
            this.state.OnStart += State_OnStart;
            this.state.OnReset += State_OnReset;
            this.state.OnSkipSplit += State_OnSkipSplit;
            this.state.OnSplit += State_OnSplit;

            this.model = new TimerModel()
            {
                CurrentState = this.state
            };

            // todo, create settings forcing the player to 
            // chose one condition per split. Could be manual though
            foreach(var segment in this.state.Run)
                Console.WriteLine(segment.Name); // condition name is here

            // Example of building conditions
            // This is a list of all the conditions. Is should be the same
            // length as this.state.Run.Count since it's one condition
            // per split.
            var LLConditions = new LinkedList<LinkedConditions>();

            // then create a linkedlist of conditions. one per split
            var LConditions = new LinkedList<Condition>();

            // Then add how many conditions you wish to the list
            LConditions.AddLast(new OnBossDefeated(15));
            LConditions.AddLast(new OnQuitout(1));

            // Finally add the conditions to the linkedlist for the current split
            LLConditions.AddLast(new LinkedConditions(LConditions));

            // Create the autosplitter and hook to a simple event for
            // when the autosplitter requests for a programatic split
            this.autosplitter = new AutoSplitter(LLConditions);
            this.autosplitter.OnSplitRequest += Autosplitter_OnSplitRequest;
        }

        private void Autosplitter_OnSplitRequest(object sender, EventArgs e)
        {
            // Split was requested which means the current segment's
            // condition was completed
            this.model.Split();
        }

        private void State_OnSplit(object sender, EventArgs e)
        {
            // The autosplitter doesn't automatically switch to the next segment's
            // condition because the user could also have split manually
            this.autosplitter.Next();
        }

        private void State_OnStart(object sender, EventArgs e)
        {
            this.state.IsGameTimePaused = true; // For IGT
            this.autosplitter.Reset();
            this.autosplitter.Start();
        }

        private void State_OnReset(object sender, TimerPhase value)
        {
            this.autosplitter.Reset();
            GameState.Instance.Reset();
        }

        private void State_OnSkipSplit(object sender, EventArgs e)
        {
            this.autosplitter.Next();
        }

        public override void Dispose()
        {
            this.autosplitter.Dispose();
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            return null;
        }

        public override System.Windows.Forms.Control GetSettingsControl(LayoutMode mode)
        {
            return null;
        }

        public override void SetSettings(XmlNode settings)
        {
            
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (this.state.CurrentPhase == TimerPhase.Running)
            {
                GameState.Instance.Update();
                state.SetGameTime(new TimeSpan(0, 0, 0, 0, 2000));
            }
        }
    }
}
