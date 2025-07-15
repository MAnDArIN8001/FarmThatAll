using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utiles.EventSystem
{
    public class EventBus : IDisposable
    {
        private Dictionary<Type, List<Delegate>> _events;

        private Dictionary<Type, int> _lastInvokeDictionary;
        
        public static EventBus Instance { get; private set; }
        
        public EventBus()
        {
            _events = new Dictionary<Type, List<Delegate>>();
            
            _lastInvokeDictionary = new Dictionary<Type, int>();
            
            Instance = this;
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
            if (_lastInvokeDictionary.TryGetValue(typeof(T), out var lastInvokeFrame))
            {
                _lastInvokeDictionary[typeof(T)] = Time.frameCount;
            }
            else
            {
                _lastInvokeDictionary.Add(typeof(T), Time.frameCount);
            }
            
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

        public bool WasInvokedThisFrame<T>()
        {
            if (!_lastInvokeDictionary.TryGetValue(typeof(T), out var lastInvokeFrame))
            {
                return false;
            }

            if (lastInvokeFrame != Time.frameCount)
            {
                return false;
            }
            
            return true;
        }
        
        public void Dispose()
        {
            _events.Clear();
        }
    }
}