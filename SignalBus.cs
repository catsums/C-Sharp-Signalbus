using System.Collections.Generic;
using System;

namespace SignalBusNS
{
    public class SignalBus{
        protected Dictionary< string, List< Action<object> > > handlers;
        protected Dictionary< string, Action<object> > signals;

        public SignalBus(){
            handlers = new Dictionary< string, List< Action<object> > >();
            signals = new Dictionary< string, Action<object> >();
        }

        protected void CreateSignal(string n){
            handlers[n] = new List<Action<object>>();
            signals[n] = null;
        }

        public void Subscribe(string n, Action<object> listener){
            if(!HasSignal(n)){
                CreateSignal(n);
            }

            handlers[n].Add(listener);
            signals[n] += listener;

        }
        public void Unsubscribe(string n, Action<object> listener){
            if(HasSignal(n)){
                signals[n] -= listener;
                handlers[n].Remove(listener);
            }
        }
        public bool IsSubscribed(string n, Action<object> listener){
            if(HasSignal(n)){
                return handlers[n].Contains(listener);
            }
            return false;
        }
        public bool HasSignal(string n){
            if(signals.ContainsKey(n))
                return true;
            return false;
        }

        public void Emit(string n){
            if(HasSignal(n)){
                signals[n]?.Invoke(null);
            }
        }
        public void Emit(string n, object data){
            if(HasSignal(n)){
                signals[n]?.Invoke(data);
            }
        }
    }
}