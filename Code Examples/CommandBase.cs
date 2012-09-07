// <copyright company="Roche Diagnostics AG">
// Copyright (c) Roche Diagnostics AG. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace Roche.LabCore.UI
{
    /// <summary>
    /// Base class for LabUI commands.
    /// </summary>
    /// <remarks>
    /// Please read the remarks under <see cref="CanExecuteChanged"/> regarding weak events.
    /// </remarks>
    public abstract class CommandBase : ICommand
    {
        private List<WeakReference> canExecuteChangedHandlers;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute. You must keep a hard
        /// reference to the handler to avoid garbage collection and unexpected results. See remarks for more information.
        /// </summary>
        /// <remarks>
        /// When subscribing to the <see cref="ICommand.CanExecuteChanged"/> event using 
        /// code (not when binding using XAML) will need to keep a hard reference to the event handler. This is to prevent 
        /// garbage collection of the event handler because the command implements the Weak Event pattern so it does not have
        /// a hard reference to this handler. An example implementation can be seen in the CompositeCommand and CommandBehaviorBase
        /// classes. In most scenarios, there is no reason to sign up to the CanExecuteChanged event directly, but if you do, you
        /// are responsible for maintaining the reference.
        /// </remarks>
        /// <example>
        /// The following code holds a reference to the event handler. The myEventHandlerReference value should be stored
        /// in an instance member to avoid it from being garbage collected.
        /// <code>
        /// EventHandler myEventHandlerReference = new EventHandler(this.OnCanExecuteChanged);
        /// command.CanExecuteChanged += myEventHandlerReference;
        /// </code>.
        /// </example>
        public event EventHandler CanExecuteChanged
        {
            add { WeakEventHandlerManager.AddWeakReferenceHandler(ref this.canExecuteChangedHandlers, value, 2); }
            remove { WeakEventHandlerManager.RemoveWeakReferenceHandler(this.canExecuteChangedHandlers, value); }
        }

        /// <summary>
        /// Raises <see cref="ICommand.CanExecuteChanged"/> on the UI thread so every 
        /// command invoker can requery <see cref="ICommand.CanExecute"/> to check if the
        /// CompositeCommand can execute.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, this.canExecuteChangedHandlers);
        }

        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/> on the UI thread so every command invoker
        /// can requery to check if the command can execute.
        /// <remarks>
        /// Note that this will trigger the execution of <see cref="CanExecute"/> once for each invoker.
        /// </remarks>
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This method is needed to be compatible with Microsoft code.")]
        public void RaiseCanExecuteChanged()
        {
            this.OnCanExecuteChanged();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to null. 
        /// </param>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to null. 
        /// </param>
        public abstract void Execute(object parameter);
    }
}