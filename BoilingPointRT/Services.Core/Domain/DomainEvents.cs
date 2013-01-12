using System;
using System.Collections.Generic;

using BoilingPointRT.Services.Domain;

namespace eVision.VisionControl.Interfaces.Shell.Core.Domain
{
    public static class DomainEvents
    {
        [ThreadStatic]
        private static List<Delegate> actions;

        [ThreadStatic]
        private static bool suspended;

        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
            {
                actions = new List<Delegate>();
            }

            actions.Add(callback);
        }

        public static void ClearCallbacks()
        {
            actions = null;
        }

        public static void Suspend()
        {
            suspended = true;
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (!suspended)
            {
//                if (Ioc.IsInitialized)
//                {
//                    foreach (var handler in Ioc.Global.FindAll<Handles<T>>())
//                    {
//                        handler.Handle(args);
//                    }
//                }

                if (actions != null)
                {
                    foreach (Delegate action in actions)
                    {
                        if (action is Action<T>)
                        {
                            ((Action<T>)action)(args);
                        }
                    }
                }
            }
        }
    }
}