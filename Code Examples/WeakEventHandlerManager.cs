// <copyright company="Roche Diagnostics AG">
// Copyright (c) Roche Diagnostics AG. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace Roche.LabCore.UI {
    /// <summary>
    /// Handles management and dispatching of EventHandlers in a weak way.
    /// </summary>
    internal static class WeakEventHandlerManager {
        /// <summary>
        /// Invokes the handlers.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="handlers">
        /// The handlers.
        /// </param>
        public static void CallWeakReferenceHandlers(object sender, List<WeakReference> handlers) {
            if (handlers != null) {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.
                EventHandler[] callees = new EventHandler[handlers.Count];
                int count = 0;

                // Clean up handlers
                count = CleanupOldHandlers(handlers, callees, count);

                // Call the handlers that we snapshotted
                for (int i = 0; i < count; i++) {
                    CallHandler(sender, callees[i]);
                }
            }
        }

        /// <summary>
        /// Calls the handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventHandler">
        /// The event handler.
        /// </param>
        private static void CallHandler(object sender, EventHandler eventHandler) {
            DispatcherProxy dispatcher = DispatcherProxy.CreateDispatcher();

            if (eventHandler != null) {
                if (dispatcher != null && !dispatcher.CheckAccess()) {
                    dispatcher.BeginInvoke((Action<object, EventHandler>)CallHandler, sender, eventHandler);
                }
                else {
                    eventHandler(sender, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Hides the dispatcher mis-match between Silverlight and .Net, largely so code reads a bit easier.
        /// </summary>
        private class DispatcherProxy {
            private Dispatcher innerDispatcher;

            /// <summary>
            /// Initializes a new instance of the <see cref="DispatcherProxy"/> class.
            /// </summary>
            /// <param name="dispatcher">The dispatcher.</param>
            private DispatcherProxy(Dispatcher dispatcher) {
                this.innerDispatcher = dispatcher;
            }

            /// <summary>
            /// Creates the dispatcher.
            /// </summary>
            /// <returns>
            /// A new dispatcher.
            /// </returns>
            public static DispatcherProxy CreateDispatcher() {
                return new DispatcherProxy(Deployment.Current.Dispatcher);
            }

            /// <summary>
            /// Checks the access.
            /// </summary>
            /// <returns>
            /// If there is access.
            /// </returns>
            public bool CheckAccess() {
                return this.innerDispatcher.CheckAccess();
            }

            /// <summary>
            /// Begins the invoke.
            /// </summary>
            /// <param name="method">
            /// The method to invoke.
            /// </param>
            /// <param name="args">
            /// The arguments.
            /// </param>
            /// <returns>
            /// Dispatcher operation.
            /// </returns>
            public DispatcherOperation BeginInvoke(Delegate method, params object[] args) {
                return this.innerDispatcher.BeginInvoke(method, args);
            }
        }

        /// <summary>
        /// Cleanups the old handlers.
        /// </summary>
        /// <param name="handlers">
        /// The handlers.
        /// </param>
        /// <param name="callees">
        /// The callees.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// Count of old handlers.
        /// </returns>
        private static int CleanupOldHandlers(List<WeakReference> handlers, EventHandler[] callees, int count) {
            for (int i = handlers.Count - 1; i >= 0; i--) {
                WeakReference reference = handlers[i];
                EventHandler handler = reference.Target as EventHandler;
                if (handler == null) {
                    // Clean up old handlers that have been collected
                    handlers.RemoveAt(i);
                }
                else {
                    callees[count] = handler;
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Adds a handler to the supplied list in a weak way.
        /// </summary>
        /// <param name="handlers">
        /// Existing handler list.  It will be created if null.
        /// </param>
        /// <param name="handler">
        /// Handler to add.
        /// </param>
        /// <param name="defaultListSize">
        /// Default list size.
        /// </param>
        public static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize) {
            if (handlers == null) {
                handlers = defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>();
            }

            handlers.Add(new WeakReference(handler));
        }

        /// <summary>
        /// Removes an event handler from the reference list.
        /// </summary>
        /// <param name="handlers">
        /// Handler list to remove reference from.
        /// </param>
        /// <param name="handler">
        /// Handler to remove.
        /// </param>
        public static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler) {
            if (handlers != null) {
                for (int i = handlers.Count - 1; i >= 0; i--) {
                    WeakReference reference = handlers[i];
                    EventHandler existingHandler = reference.Target as EventHandler;
                    if ((existingHandler == null) || (existingHandler == handler)) {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        handlers.RemoveAt(i);
                    }
                }
            }
        }
    }
}