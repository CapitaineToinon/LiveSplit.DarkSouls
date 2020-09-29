using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsState;

namespace LiveSplit.DarkSouls {
    class LinkedConditions {
        private LinkedList<Condition> conditions;
        private LinkedListNode<Condition> current;
        public event EventHandler OnTreeCompleted;

        public LinkedConditions(LinkedList<Condition> conditions)
        {
            this.conditions = conditions;
            this.current = conditions.First;
        }

        public void Reset()
        {
            foreach (var c in conditions)
            {
                c.Reset();
                c.Stop();
            }

            this.current = this.conditions.First;
        }

        public void Start()
        {
            this.current.Value.OnCompleted += ConditionnalTree_OnCompleted;
            this.current.Value.Start();
        }

        public void Stop()
        {
            this.current.Value.OnCompleted -= ConditionnalTree_OnCompleted;
            this.current.Value.Stop();
        }

        public void Next()
        {
            this.current.Value.OnCompleted -= ConditionnalTree_OnCompleted;
            this.current.Value.Stop();

            if (this.current.Next != null)
            {
                this.current = this.current.Next;
                this.current.Value.OnCompleted += ConditionnalTree_OnCompleted;
                this.current.Value.Start();
            }
        }

        private void ConditionnalTree_OnCompleted(object sender, EventArgs e)
        {
            if (this.current.Next == null)
            {
                OnTreeCompleted?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Next();
            }
        }
    }
}
