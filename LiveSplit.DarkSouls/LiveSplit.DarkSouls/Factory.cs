using LiveSplit.DarkSouls;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

[assembly: ComponentFactory(typeof(Factory))]
namespace LiveSplit.DarkSouls {
    internal class Factory : IComponentFactory {
        public string ComponentName => "Dark Souls & Dark Souls: Remastered";
        public string Description => "Dark Souls & Dark Souls: Remastered autosplitter by CapitaineToinon";
        public string UpdateName => ComponentName;

        public ComponentCategory Category => ComponentCategory.Timer;
        public string UpdateURL => null;
        public string XMLURL => null;

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public IComponent Create(LiveSplitState state)
        {
            return new Component(state);
        }
    }
}
