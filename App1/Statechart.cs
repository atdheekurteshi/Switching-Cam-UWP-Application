using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class State<T>
    {
        private Dictionary<T, State<T>> transitions = new Dictionary<T, State<T>>();
        public Func<Task> enter, exit;

        public bool HasState(T e)
        {
            return transitions.ContainsKey(e);
        }

        public State<T> this[T e]
        {
            get
            {
                return transitions[e];
            }
            set
            {
                transitions[e] = value;
            }
        }
    }

    public class Statechart<T>
    {
        private State<T> current;
        private bool inTransition = false;

        public Statechart(State<T> state)
        {
            current = state;
        }

        public bool IsInState(State<T> s)
        {
            return current == s;
        }

        public bool IsInTransition()
        {
            return inTransition;
        }

        public void Dispose()
        {
            current = null;
        }

        public async Task go(T t)
        {
            if (current == null)
                return;

            inTransition = true;

            if (current.exit != null)
                await current.exit();


            // need to check again, Dispose() might have been called inbetween
            if (current == null)
                return;

            if (current.HasState(t))
            {
                State<T> next = current[t];
                current = next;

                if (next.enter != null)
                    await next.enter();
            }
            else
            {
                Debug.WriteLine($"Warning: trying to reach non-existing state {t} from {current}");

            }
            inTransition = false;
        }
    }
}
