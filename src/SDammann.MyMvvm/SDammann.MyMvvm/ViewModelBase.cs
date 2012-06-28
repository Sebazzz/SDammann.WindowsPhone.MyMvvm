namespace SDammann.MyMvvm {
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;


    /// <summary>
    ///   Represents the base class of all view models, for both pages or classes
    /// </summary>
    public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged {
        private readonly bool isInDesignMode;
        private static bool? IsInDesignModePrivate;

        /// <summary>
        ///   Gets if we are in design mode
        /// </summary>
        public static bool DesignMode {
            [DebuggerStepThrough]
            get {
                bool designMode;
                if (IsInDesignModePrivate == null) {
                    IsInDesignModePrivate = designMode = DesignerProperties.IsInDesignTool;
                } else {
                    designMode = IsInDesignModePrivate.Value;
                }

                return designMode;
            }
        }

        /// <summary>
        ///   Gets if we are in design mode
        /// </summary>
        protected bool IsInDesignMode {
            [DebuggerStepThrough]
            get { return this.isInDesignMode; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ViewModelBase" /> class. Make sure to always call this constructor.
        /// </summary>
        protected ViewModelBase() {
            this.isInDesignMode = DesignerProperties.GetIsInDesignMode(this);
            IsInDesignModePrivate = this.isInDesignMode;
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
        protected virtual void OnPropertyChanged (string propertyName) {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
#pragma warning restore 0067

        #endregion
    }
}