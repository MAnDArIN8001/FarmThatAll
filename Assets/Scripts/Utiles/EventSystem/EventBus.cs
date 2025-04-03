using System;
using System.Collections.Generic;

namespace Utiles.EventSystem
{
    public class EventBus : IDisposable
    {
        private Dictionary<Type, List<Delegate>> _events;

        public EventBus()
        {
            _events = new Dictionary<Type, List<Delegate>>();
        }

        public void Subscribe<T>(Action<T> action)
        {
            if (!_events.TryGetValue(typeof(T), out var events))
            {
                events = new List<Delegate>();
                _events[typeof(T)] = events;
            }
            
            events.Add(action);
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            if (_events.TryGetValue(typeof(T), out var events))
            {
                events.Remove(action);

                if (events.Count == 0)
                {
                    _events.Remove(typeof(T));
                }
            }
        }

        public void Publish<T>(T eventData)
        {
            if (_events.TryGetValue(typeof(T), out var list))
            {
                var listeners = new List<Delegate>(list);
                
                foreach (var listener in listeners)
                {
                    try
                    {
                        ((Action<T>)listener).Invoke(eventData);
                    }
                    catch (Exception exception)
                    {
                        UnityEngine.Debug.LogError($"EventBus: exception in event handler {typeof(T)}: {exception}");
                    }
                }
            }
        }

        public void Dispose()
        {
            _events.Clear();
        }
    }
}