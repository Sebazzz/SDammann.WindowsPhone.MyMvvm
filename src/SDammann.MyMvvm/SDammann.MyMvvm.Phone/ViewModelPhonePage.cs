namespace SDammann.MyMvvm.Phone {
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Data;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;
    using TombstoneHelper;
    using Utils.ServiceLocator;


    /// <summary>
    /// Base class for all phone application pages using a view model. 
    /// </summary>
    /// <remarks>
    /// The parameterless constructor is used for design time instantiation of the view model. The other constructor is 
    /// used at runtime and resolved by the dependency injection you plug in via the <see cref="ServiceResolver"/>
    /// </remarks>
    public class ViewModelPhonePage : PhoneApplicationPage {
        /// <summary>
        /// Gets the current progress indicator
        /// </summary>
        protected readonly ProgressIndicator ProgressIndicator;
        private bool _isConstructorCalled;

        /// <summary>
        /// Gets or sets whether or not to use the windows phone thombstone helper
        /// </summary>
        public bool UseThombStoneHelper { get; set; }

        /// <summary>
        /// Gets the view model class. 
        /// </summary>
        protected PageViewModelBase ViewModelBase {
            [DebuggerStepThrough]
            get { return (PageViewModelBase)this.DataContext; }
        }


        /// <summary>
        /// Called just before a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            if (e.NavigationMode != NavigationMode.Back) {
                this.State.Clear();

                if (this.UseThombStoneHelper) {
                    this.SaveState();
                }

                this.ViewModelBase.SaveState(this.State);
            }

            base.OnNavigatingFrom(e);
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (this.UseThombStoneHelper) {
                this.RestoreStateSafe();
            }

            // only inject and restore state in case we have been recreated as a page (after thombstoning)
            if (this._isConstructorCalled) {
                ServiceResolver.InjectInstance(this.ViewModelBase);
                this.ViewModelBase.OnRuntimeInitialize();
                this.ViewModelBase.RestoreState(this.State);
            }

            if (this.ShouldLoadViewModel(e)) {
                // important: we do _not_ use .Loaded here as that may fire twice or even more
                // see also: http://goo.gl/c446s 
                this.ViewModelBase.PageLoaded(this.NavigationContext.QueryString);
            } else {
                this.ViewModelBase.PageRefresh();
            }

            this._isConstructorCalled = false;
            base.OnNavigatedTo(e);
        }

        private bool RestoreStateSafe() {
            try {
                this.RestoreState();
                return true;
            } catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Called to check if we should load the view model
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual bool ShouldLoadViewModel(NavigationEventArgs e) {
            return e.NavigationMode == NavigationMode.New || (e.NavigationMode == NavigationMode.Back && !e.IsNavigationInitiator);
        }

        /// <summary>
        /// Called when the hardware Back button is pressed. Executes the view model <see cref="PageViewModelBase.BackKeyPressCommand"/>
        /// </summary>
        /// <param name="e">Set e.Cancel to true to indicate that the request was handled by the application.</param>
        protected override void OnBackKeyPress(CancelEventArgs e) {
            this.ViewModelBase.BackKeyPressCommand.Execute(e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelPhonePage"/> class.
        /// </summary>
        public ViewModelPhonePage() {
            this.UseThombStoneHelper = true;
            this._isConstructorCalled = true;

            ProgressIndicator = new ProgressIndicator();

            this.InitializeSystemTray();
        }

        private void InitializeSystemTray() {
            SystemTray.SetProgressIndicator(this, this.ProgressIndicator);

            this.SetBinding(SystemTray.IsVisibleProperty, new Binding("ShellTray.IsShellTrayVisible"));
            //this.SetBinding(SystemTray.ForegroundColorProperty, new Binding("ShellTray.ShellTrayForegroundColor"));
            //this.SetBinding(SystemTray.BackgroundColorProperty, new Binding("ShellTray.ShellTrayBackgroundColor"));
            
            BindingOperations.SetBinding(ProgressIndicator, ProgressIndicator.IsIndeterminateProperty, new Binding("ShellTray.IsProgressIndeterminate"));
            BindingOperations.SetBinding(ProgressIndicator, ProgressIndicator.IsVisibleProperty, new Binding("ShellTray.IsProgressVisible"));
            BindingOperations.SetBinding(ProgressIndicator, ProgressIndicator.TextProperty, new Binding("ShellTray.ProgressText"));
            BindingOperations.SetBinding(ProgressIndicator, ProgressIndicator.ValueProperty, new Binding("ShellTray.ProgressValue"));
        }
    }
}