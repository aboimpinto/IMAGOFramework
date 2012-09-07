// <copyright company="Roche Diagnostics AG">
// Copyright (c) Roche Diagnostics AG. All rights reserved.
// </copyright>
using System;
using System.Windows.Input;

namespace Roche.LabCore.UI
{
    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="ICommand.Execute"/> and <see cref="ICommand.CanExecute"/>.
    /// </summary>
    public class DelegateCommand : DelegateCommand<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        public DelegateCommand(Action executeMethod)
            : base(o => executeMethod())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        /// <param name="canExecuteMethod">
        /// The can execute method.
        /// </param>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(o => executeMethod(), o => canExecuteMethod())
        {
        }
    }

    /// <summary>
    /// An <see cref="ICommand"/> that relays its actual functionality to delegates.
    /// </summary>
    /// <typeparam name="T">
    /// Parameter type.
    /// </typeparam>
    public partial class DelegateCommand<T> : CommandBase
    {
        private readonly Action<T> executeMethod;
        private readonly Func<T, bool> canExecuteMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class. 
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="executeMethod">
        /// Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.
        /// </param>
        /// <remarks>
        /// <c>CanExecute</c> will always return true.
        /// </remarks>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class. 
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="executeMethod">
        /// Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.
        /// </param>
        /// <param name="canExecuteMethod">
        /// Delegate to execute when CanExecute is called on the command.  This can be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// When both <paramref name="executeMethod"/> and <paramref name="canExecuteMethod"/> ar <see langword="null"/>.
        /// </exception>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null && canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod", "Two null delegates don't make sense.");
            }

            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>
        /// True if this command can be executed; otherwise, false.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null"/>.
        /// </param>
        /// <returns>
        /// Returns <see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanExecute(T parameter)
        {
            if (this.canExecuteMethod == null)
            {
                return true;
            }
            return this.canExecuteMethod(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public override void Execute(object parameter)
        {
            this.Execute((T)parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null"/>.
        /// </param>
        public void Execute(T parameter)
        {
            if (this.executeMethod == null)
            {
                return;
            }
            this.executeMethod(parameter);
        }
    }
}