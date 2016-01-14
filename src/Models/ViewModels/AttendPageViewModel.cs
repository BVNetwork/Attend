using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;

namespace BVNetwork.Attend.Models.ViewModels
{
    public class AttendPageViewModel<T> where T : PageData
    {
        public AttendPageViewModel(T currentPage)
        {
            CurrentPage = currentPage;
        }

        public T CurrentPage { get; private set; }
        public IContent Section { get; set; }
    }

    public static class AttendPageViewModel
    {
        /// <summary>
        /// Returns a PageViewModel of type <typeparam name="T"/>.
        /// </summary>
        /// <remarks>
        /// Convenience method for creating PageViewModels without having to specify the type as methods can use type inference while constructors cannot.
        /// </remarks>
        public static AttendPageViewModel<T> Create<T>(T page) where T : PageData
        {
            return new AttendPageViewModel<T>(page);
        }
    }
}