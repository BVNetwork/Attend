using System.Collections.Generic;
using BVNetwork.Attend.Models.ViewModels;
using EPiServer.Core;

namespace BVNetwork.Attend.Models.ViewModels
{
    public class AttendPreviewModel : AttendPageViewModel<PageData>
    {
        public AttendPreviewModel(PageData currentPage, IContent previewContent)
            : base(currentPage)
        {
            PreviewContent = previewContent;
            Areas = new List<PreviewArea>();
        }

        public IContent PreviewContent { get; set; }
        public List<PreviewArea> Areas { get; set; } 

        public class PreviewArea
        {
            public bool Supported { get; set; }
            public string AreaName { get; set; }
            public string AreaTag { get; set; }
            public ContentArea ContentArea { get; set; }
        }
    }
}
