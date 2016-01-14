using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace BVNetwork.Attend.Business.Core
{
    public class Folders
    {
        public static ContentFolder GetOrCreateEventFolder(ContentReference EventPageBase, string folderName)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var contentAssetHelper = ServiceLocator.Current.GetInstance<ContentAssetHelper>();

            ContentFolder folder = null;
            var assetsFolder = contentAssetHelper.GetOrCreateAssetFolder(EventPageBase);

            var folders = contentRepository.GetChildren<ContentFolder>(assetsFolder.ContentLink);

            foreach (ContentFolder cf in folders)
            {
                if (cf.Name == folderName)
                    folder = cf;
            }

            if (folder == null)
            {
                folder = contentRepository.GetDefault<ContentFolder>(assetsFolder.ContentLink);
                folder.Name = folderName;
                contentRepository.Save(folder, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            }

            return folder;

        }
    }
}