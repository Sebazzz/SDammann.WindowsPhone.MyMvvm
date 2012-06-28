namespace SDammann.MyMvvm.Phone {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;


    /// <summary>
    ///   Silverlight / WP7 runtime implementation of <see cref="INavigationService" />
    /// </summary>
    [DebuggerStepThrough]
    public sealed class NavigationService : INavigationService {
        private PhoneApplicationFrame mainFrame;


        #region INavigationService Members

        public event NavigatingCancelEventHandler Navigating;

        public void NavigateTo(Uri pageUri) {
            if (this.EnsureMainFrame()) {
                this.mainFrame.Navigate(pageUri);
            }
        }

        public void GoBack() {
            if (this.EnsureMainFrame()
                && this.mainFrame.CanGoBack) {
                this.mainFrame.GoBack();
            }
        }

        public bool CanGoBack {
            get { return this.EnsureMainFrame() && this.mainFrame.CanGoBack; }
        }

        public void RemoveBackEntry() {
            if (this.EnsureMainFrame()) {
                this.mainFrame.RemoveBackEntry();
            }
        }

        public IEnumerable<JournalEntry> BackStack {
            get {
                if (!this.EnsureMainFrame()) {
                    return Enumerable.Empty<JournalEntry>();
                }

                return this.mainFrame.BackStack;
            }
        }

        #endregion




        private bool EnsureMainFrame() {
            if (this.mainFrame != null) {
                return true;
            }

            this.mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;

            if (this.mainFrame != null) {
                // Could be null if the app runs inside a design tool
                this.mainFrame.Navigating += (s, e) => {
                    if (this.Navigating != null) {
                        this.Navigating(s, e);
                    }
                };

                return true;
            }

            return false;
        }
    }
}