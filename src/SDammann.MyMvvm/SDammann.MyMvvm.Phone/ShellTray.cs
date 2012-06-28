namespace SDammann.MyMvvm.Phone {
    using System.ComponentModel;
    using System.Windows.Media;


    /// <summary>
    ///   Represents bindings for the shell tray
    /// </summary>
    public sealed class ShellTray : INotifyPropertyChanged {
        /// <summary>
        ///   Gets or sets the progress text to show in the progress tray
        /// </summary>
        public string ProgressText { get; set; }

        /// <summary>
        ///   Gets a value from 0 to 100 showing progress in the progress tray
        /// </summary>
        public int ProgressValue { get; set; }

        /// <summary>
        ///   Gets or sets whether or not to show an indeterminate progress
        /// </summary>
        public bool IsProgressIndeterminate { get; set; }

        /// <summary>
        ///   Gets or sets whether or not the progress bar and progress text are visible
        /// </summary>
        public bool IsProgressVisible { get; set; }

        /// <summary>
        ///   Gets or sets whether or not the progress tray is visible
        /// </summary>
        public bool IsShellTrayVisible { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the shell tray
        /// </summary>
        public Color ShellTrayForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color of the shell tray
        /// </summary>
        public Color ShellTrayBackgroundColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellTray"/> class.
        /// </summary>
        public ShellTray() {
            this.IsProgressVisible = false;
            this.IsProgressIndeterminate = true;

            this.IsShellTrayVisible = true;
        }


        #region INotifyPropertyChanged Members

#pragma warning disable 0067 //disable event is never used warning as we use de INotifyPropertyWeaver
        /// <summary>
        ///   Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///   Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName"> Name of the property. </param>
        private void OnPropertyChanged (string propertyName) {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
#pragma warning restore 0067

        #endregion
    }
}