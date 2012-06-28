namespace SDammann.MyMvvm.Phone {
    using System;
    using System.Collections.Generic;
    using System.Windows.Navigation;


    /// <summary>
    ///   Helper interface that describes a service for navigating
    /// </summary>
    public interface INavigationService {
        /// <summary>
        ///   Occurs when navigating
        /// </summary>
        event NavigatingCancelEventHandler Navigating;

        /// <summary>
        /// Navigates to the specified uri
        /// </summary>
        /// <param name="pageUri"></param>
        void NavigateTo(Uri pageUri);

        /// <summary>
        /// Navigates back
        /// </summary>
        void GoBack();

        /// <summary>
        /// Gets a value indicating if there can be navigated back
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Removes the last entry
        /// </summary>
        void RemoveBackEntry();

        /// <summary>
        /// Gets an IEnumerable that you use to enumerate the entries in back navigation history.
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see cref="T:System.Collections.Generic.IEnumerable`1"/> List of entries in the back stack..
        /// </returns>
        IEnumerable<JournalEntry> BackStack { get; }
    }
}