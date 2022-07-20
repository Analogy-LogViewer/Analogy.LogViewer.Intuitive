using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Template.IAnalogy;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveImages: AnalogyImages
    {
        public override Image GetLargeOpenFileImage(Guid analogyComponentId) => Resources.Intuitive32x32OpenFile;
        public override Image GetSmallOpenFileImage(Guid analogyComponentId) => Resources.Intuitive16x16OpenFile;
        public override Image GetLargeOpenFolderImage(Guid analogyComponentId) => Resources.Intuitive32x32OpenFolder;
        public override Image GetSmallOpenFolderImage(Guid analogyComponentId) => Resources.Intuitive16x16OpenFolder;

        public override Image GetLargeRecentFoldersImage(Guid analogyComponentId) => Resources.Intuitive32x32RecentFolders;
        public override Image GetSmallRecentFoldersImage(Guid analogyComponentId) => Resources.Intuitive16x16RecentFolders;
        
        public override Image GetLargeBookmarksImage(Guid analogyComponentId) => Resources.Intuitive32x32Bookmarks;
        public override Image GetSmallBookmarksImage(Guid analogyComponentId) => Resources.Intuitive16x16Bookmarks;
        public override Image GetLargeFilePoolingImage(Guid analogyComponentId) => Resources.Intuitive32x32Pooling;
        public override Image GetSmallFilePoolingImage(Guid analogyComponentId) => Resources.Intuitive16x16Pooling;

        public override Image GetLargeRecentFilesImage(Guid analogyComponentId) => Resources.Intuitive32x32Recents;

        public override Image GetSmallRecentFilesImage(Guid analogyComponentId) => Resources.Intuitive16x16Recents;

        public override Image GetLargeKnownLocationsImage(Guid analogyComponentId) => Resources.Intuitive32x32KnownLocations;

        public override Image GetSmallKnownLocationsImage(Guid analogyComponentId) => Resources.Intuitive16x16KnownLocations;

        //public override Image GetLargeSearchImage(Guid analogyComponentId) => Resources.Kama32x32SearchFiles;

        //public override Image GetSmallSearchImage(Guid analogyComponentId) => Resources.Kama16x16SearchFiles;

        //public override Image GetLargeCombineLogsImage(Guid analogyComponentId) => Resources.Kama32x32CombineFiles;

        //public override Image GetSmallCombineLogsImage(Guid analogyComponentId) => Resources.Kama16x16CombineFiles;

        //public override Image GetLargeCompareLogsImage(Guid analogyComponentId) => Resources.Kama32x32CompareFiles;

        //public override Image GetSmallCompareLogsImage(Guid analogyComponentId) => Resources.Kama16x16CompareFiles;

        //public override Image GetRealTimeConnectedLargeImage(Guid analogyComponentId) => Resources.Kama32x32connected;
        //public override Image GetRealTimeConnectedSmallImage(Guid analogyComponentId) => Resources.Kama16x16connected;
        //public override Image GetRealTimeDisconnectedLargeImage(Guid analogyComponentId) => Resources.Kama32x32disconnected;
        //public override Image GetRealTimeDisconnectedSmallImage(Guid analogyComponentId) => Resources.Kama16x16disconnected;
    }
}
