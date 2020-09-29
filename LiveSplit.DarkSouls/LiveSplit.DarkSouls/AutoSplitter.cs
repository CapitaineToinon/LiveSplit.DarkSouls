using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls {
    class AutoSplitter {
        private LinkedList<LinkedConditions> LLConditions;
        private LinkedListNode<LinkedConditions> currentLConditions;
        public event EventHandler OnSplitRequest;

        public AutoSplitter(LinkedList<LinkedConditions> LLConditions)
        {
            this.LLConditions = LLConditions;
        }

        public void Dispose()
        {
            this.Stop();
        }

        public void Start()
        {
            this.currentLConditions.Value.OnTreeCompleted += Current_OnTreeCompleted;
            this.currentLConditions.Value.Start();
        }

        public void Stop()
        {
            this.currentLConditions.Value.Stop();
            this.currentLConditions.Value.OnTreeCompleted -= Current_OnTreeCompleted;
        }

        public void Reset()
        {
            foreach (var c in this.LLConditions)
            {
                c.Reset();
            }

            this.currentLConditions = LLConditions.First;
        }

        public void Next()
        {
            this.currentLConditions.Value.Stop();
            this.currentLConditions.Value.OnTreeCompleted -= Current_OnTreeCompleted;

            if (this.currentLConditions.Next != null)
            {
                this.currentLConditions = this.currentLConditions.Next;
                this.currentLConditions.Value.Start();
                this.currentLConditions.Value.OnTreeCompleted += Current_OnTreeCompleted;
            }
        }

        public void Previous()
        {
            this.currentLConditions.Value.Stop();
            this.currentLConditions.Value.OnTreeCompleted -= Current_OnTreeCompleted;

            if (this.currentLConditions.Previous != null)
            {
                this.currentLConditions = this.currentLConditions.Previous;
                this.currentLConditions.Value.Start();
                this.currentLConditions.Value.OnTreeCompleted += Current_OnTreeCompleted;
            }
        }

        private void Current_OnTreeCompleted(object sender, EventArgs e)
        {
            OnSplitRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
