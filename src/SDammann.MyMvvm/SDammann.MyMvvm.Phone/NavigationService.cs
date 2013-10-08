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
        private PhoneApplicationFrame _mainFrame;


        #region INavigationService Members

        /// <summary>
        /// Occurs when navigating
        /// </summary>
        public event NavigatingCancelEventHandler Navigating;


        /// <summary>
        /// Navigates to the specified uri
        /// </summary>
        /// <param name="pageUri"></param>
        public void NavigateTo(Uri pageUri) {
            if (this.EnsureMainFrame()) {
                this._mainFrame.Navigate(pageUri);
            }
        }

        /// <summary>
        /// Navigates back
        /// </summary>
        public void GoBack() {
            if (this.EnsureMainFrame()
                && this._mainFrame.CanGoBack) {
                this._mainFrame.GoBack();
            }
        }

        /// <summary>
        /// Gets a value indicating if there can be navigated back
        /// </summary>
        public bool CanGoBack {
            get { return this.EnsureMainFrame() && this._mainFrame.CanGoBack; }
        }

        /// <summary>
        /// Removes the last entry
        /// </summary>
        public void RemoveBackEntry() {
            if (this.EnsureMainFrame()) {
                this._mainFrame.RemoveBackEntry();
            }
        }

        /// <summary>
        /// Gets an IEnumerable that you use to enumerate the entries in back navigation history.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Collections.Generic.IEnumerable`1"/> List of entries in the back stack..
        /// </returns>
        public IEnumerable<JournalEntry> BackStack {
            get {
                if (!this.EnsureMainFrame()) {
                    return Enumerable.Empty<JournalEntry>();
                }

                return this._mainFrame.BackStack;
            }
        }

        #endregion

        private bool EnsureMainFrame() {
            if (this._mainFrame != null) {
                return true;
            }

            this._mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;

            if (this._mainFrame != null) {
                // Could be null if the app runs inside a design tool
                this._mainFrame.Navigating += (s, e) => {
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