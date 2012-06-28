namespace SDammann.MyMvvm.Phone {
    using System;
    using System.Windows.Input;


    /// <summary>
    ///   Implements a simple command that actually calls an <see cref="Action" />
    /// </summary>
    public sealed class RelayCommand : ICommand {
        private readonly Action target;

        /// <summary>
        ///   Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="target"> The target. </param>
        public RelayCommand (Action target) {
            this.target = target;
        }


        #region ICommand Members

        /// <summary>
        ///   Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter"> Data used by the command. If the command does not require data to be passed, this object can be set to null. </param>
        public void Execute (object parameter) {
            this.target.Invoke();
        }

        /// <summary>
        ///   Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter"> Data used by the command. If the command does not require data to be passed, this object can be set to null. </param>
        /// <returns> true if this command can be executed; otherwise, false. </returns>
        public bool CanExecute (object parameter) {
            return true;
        }

        event EventHandler ICommand.CanExecuteChanged {
            add { }
            remove { }
        }


        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Action"/> to <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="methodCall">The method call.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator RelayCommand(Action methodCall) {
            return new RelayCommand(methodCall);
        }

        #endregion
    }

    /// <summary>
    ///   Implements a simple command that actually calls an action
    /// </summary>
    /// <typeparam name="TParameter"> The type of the parameter. </typeparam>
    public sealed class RelayCommand<TParameter> : ICommand {
        private readonly Action<TParameter> target;

        /// <summary>
        ///   Initializes a new instance of the <see cref="RelayCommand&lt;TParameter&gt;" /> class.
        /// </summary>
        /// <param name="target"> The target. </param>
        public RelayCommand (Action<TParameter> target) {
            this.target = target;
        }


        #region ICommand Members

        /// <summary>
        ///   Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter"> Data used by the command. If the command does not require data to be passed, this object can be set to null. </param>
        public void Execute (object parameter) {
            try {
                this.target.Invoke((TParameter) parameter);
            } catch (InvalidCastException) {
                string targetType = (parameter ?? new object()).GetType().FullName;
                throw new ArgumentException(
                        String.Format("Argument 'parameter' is of type '{0}' but type '{1}' was required",
                                      targetType,
                                      typeof (TParameter).FullName),
                        "parameter");
            }
        }

        /// <summary>
        ///   Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter"> Data used by the command. If the command does not require data to be passed, this object can be set to null. </param>
        /// <returns> true if this command can be executed; otherwise, false. </returns>
        public bool CanExecute (object parameter) {
            return true;
        }

        /// <summary>
        ///   Occurs when changes occur that affect whether or not the command should execute. Not implemented.
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged {
            add { }
            remove { }
        }

        public static implicit operator RelayCommand<TParameter>(Action<TParameter> methodCall) {
            return new RelayCommand<TParameter>(methodCall);
        }

        #endregion
    }
}