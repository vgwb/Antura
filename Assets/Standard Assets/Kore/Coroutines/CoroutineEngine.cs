// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

using Kore.Utils;
using System.Collections;
using UnityEngine;

namespace Kore.Coroutines
{
    public class CoroutineEngine: ICoroutineEngine, ITickable
    {
        private Method method;

        public CoroutineEngine( Method method)
        {
            this.method = method;
        }

        internal class Node
        {
            public Node Next;
            public IEnumerator Enumerator;
        }

        Node first = null;
        Node current;
        Node previous;
        Node last;
        
        public void PushOverCurrent( IEnumerator nested)
        {
            var toBeRestored = current.Enumerator;
            current.Enumerator = WrapCoroutine( nested, toBeRestored);
        }

        private IEnumerator WrapCoroutine( IEnumerator nested, IEnumerator toBeRestored)
        {
            while ( nested.MoveNext())
                yield return nested.Current;

            current.Enumerator = toBeRestored;
            yield return null;
        }

        public void RegisterCustomYield( ICustomYield customYield)
        {
            var toBeRestored = current.Enumerator;
            current.Enumerator = CustomYieldCoroutine( customYield, toBeRestored);
        }

        private IEnumerator CustomYieldCoroutine( ICustomYield customYield, IEnumerator toBeRestored)
        {
            while (customYield.HasDone() == false)
            {
                yield return null;
                customYield.Update( method);
            }

            current.Enumerator = toBeRestored;
            yield return null;
        }

        public void ReplaceCurrentWith( IEnumerator nextState)
        {
            current.Enumerator = nextState;
        }

        private void RegisterLegacyCustomYield ( IEnumerator customYield)
        {
            var toBeRestored = current.Enumerator;
            current.Enumerator = LegacyCustomYieldCoroutine( customYield, toBeRestored);
        }

        private IEnumerator LegacyCustomYieldCoroutine( IEnumerator customYield, IEnumerator toBeRestored)
        {
            while (customYield.MoveNext())
                yield return null;

            current.Enumerator = toBeRestored;
            yield return null;
        }

        public void Run( IEnumerator enumerator)
        {
            var node = new Node();
            node.Enumerator = enumerator;

            if (first == null)
            {
                first = node;
                last = node;
                return; 
            }

            last.Next = node;
            last = node;
        }

        public void Tick()
        {
            current = first;
            previous = null;

            while(current != null)
            {
                var e = current.Enumerator;
                if (e.MoveNext())
                {
                    if (e.Current != null)
                    {
                        if( e.Current is IEnumerator)
                        {
                            // Keep compatibility with Custom yield instructions (because
                            // those are used in many frameworks, like DOTween)
                            RegisterLegacyCustomYield( (IEnumerator)e.Current);
                            if (e.Current is IYieldable)
                                Debug.LogWarning(
                                            "Warning: implemented both IYieldable and IEnumerator:"
                                           +"you cannot implement both (class: "+ e.GetType().Name+" )");
                        }
                        else
                        {
                            var y = (IYieldable)e.Current;
                            y.OnYield(this);
                        }
                    }
                    previous = current;
                    current = current.Next;
                }
                else
                    RemoveCurrentNode();
            }
        }

        // This one is bugged
        private void RemoveCurrentNode()
        {
            var removed = current;

            if (removed == first)
                first = removed.Next;

            if (removed == last)
                last = previous;

            current = removed.Next; // may be null

            if(previous != null)
                previous.Next = current;
        }
    }
}
