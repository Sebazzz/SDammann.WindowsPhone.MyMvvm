namespace SDammann.MyMvvm.Phone {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using Utils.Collections.Generic;
    using Utils.ServiceLocator;


    /// <summary>
    ///   Base class for all view models that are directly used for pages
    /// </summary>
    public abstract class PageViewModelBase : ViewModelBase {
        private readonly RelayCommand<CancelEventArgs> backKeyPressCommand;
        private readonly Lazy<INavigationService> navigationService;
        private readonly string stateKey;
        private readonly ShellTray shellTray;
        private bool isContentLoaded;

        /// <summary>
        /// Gets the shell tray.
        /// </summary>
        public ShellTray ShellTray {
            [DebuggerStepThrough]
            get { return this.shellTray; }
        }

        /// <summary>
        /// Gets the back key press command.
        /// </summary>
        public RelayCommand<CancelEventArgs> BackKeyPressCommand {
            [DebuggerStepThrough]
            get { return this.backKeyPressCommand; }
        }

        /// <summary>
        ///   Gets the state key used for serializing and deserializing objects to a state dictionary.
        /// </summary>
        /// <remarks>
        ///   By default the value of this property returns the name of the class. This can optionally be customized by derived view models.
        /// </remarks>
        protected virtual string StateKey {
            [DebuggerStepThrough]
            get { return this.stateKey; }
        }


        /// <summary>
        ///   Gets the navigation service.
        /// </summary>
        public INavigationService NavigationService {
            [DebuggerStepThrough]
            get { return this.navigationService.Value; }
        }

        /// <summary>
        ///   Initializes the <see cref="PageViewModelBase" /> class.
        /// </summary>
        static PageViewModelBase() {
            // only navigation service for now
            ServiceResolver.AddMapping<INavigationService>(() => new NavigationService());
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PageViewModelBase" /> class. Make sure to always call this constructor.
        /// </summary>
        protected PageViewModelBase() {
            this.stateKey = this.GetType().FullName;
            this.navigationService = new Lazy<INavigationService>(ServiceResolver.GetService<INavigationService>);

            this.backKeyPressCommand = new RelayCommand<CancelEventArgs>(this.OnBackKeyPress);
            this.shellTray = new ShellTray();
        }

        /// <summary>
        ///  Occurs when the back key is pressed to navigate away from the page
        /// </summary>
        /// <param name="obj">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnBackKeyPress (CancelEventArgs obj) {
            // no default implementation
        }

        /// <summary>
        ///   Saves the state for this view model to the state dictionary
        /// </summary>
        /// <param name="state"> </param>
        protected abstract void OnSaveState (IDictionary<string, object> state);

        /// <summary>
        ///   Restores the state to this view model from the specified state dictionary
        /// </summary>
        /// <param name="state"> The state dictionary. </param>
        protected abstract void OnRestoreState (IDictionary<string, object> state);

        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="queryString">The query string of the page.</param>
        protected virtual void OnPageLoaded(IDictionary<string,string> queryString) {
            // no default implementation
        }

        /// <summary>
        /// Called when navigated back to the page by the user. Implementors can use this to refresh an edited list item.
        /// </summary>
        protected virtual void OnPageRefresh() {
            // no default implementation
        }

        /// <summary>
        /// Called after dependency injection has been performed on the view model
        /// </summary>
        public virtual void OnRuntimeInitialize() {
            // no default implementation
        }

        /// <summary>
        /// Called when navigated back to the page by the user.
        /// </summary>
        internal void PageRefresh() {
            this.OnPageRefresh();
        }

        /// <summary>
        /// Called when the page is loaded
        /// </summary>
        /// <param name="queryString">The query string of the page.</param>
        internal void PageLoaded(IDictionary<string,string> queryString) {
            if (this.isContentLoaded) {
                return;
            }

            this.isContentLoaded = true;
            this.OnPageLoaded(queryString);
        }

        /// <summary>
        ///   Saves the state for this view model to the state dictionary
        /// </summary>
        /// <param name="stateDictionary"> </param>
        internal virtual void SaveState (IDictionary<string, object> stateDictionary) {
            this.OnSaveState(new PrefixDictionary<object>(stateDictionary, this.StateKey));
        }

        /// <summary>
        ///   Restores the state to this view model from the specified state dictionary
        /// </summary>
        /// <param name="stateDictionary"> The state dictionary. </param>
        internal virtual void RestoreState (IDictionary<string, object> stateDictionary) {
            this.OnRestoreState(new PrefixDictionary<object>(stateDictionary, this.StateKey));
        }
    }
}