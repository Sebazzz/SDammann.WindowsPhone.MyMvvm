namespace SDammann.MyMvvm.Phone {
    using System;
    using System.Diagnostics;


    /// <summary>
    /// Extensions for <see cref="INavigationService"/>
    /// </summary>
    [DebuggerStepThrough]
    [DebuggerNonUserCode]
    public static class NavigationServiceExtensions {

        /// <summary>
        /// Navigates to the specified url of a local page
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="url">The URL.</param>
        public static void NavigateTo(this INavigationService navigationService, string url) {
            navigationService.NavigateTo(new Uri(url, UriKind.Relative));
        }
    }
}