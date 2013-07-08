using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Application.Helper.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class AuthoringTour : TestBase
    {
        #region Initialize and Cleanup
        public TestContext TestContext { get; set; }
        private static Timeline _newTimeline;
        private static Exhibit _newExhibit;
        private static Timeline _timeline;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            //BrowserStateManager.RefreshState();
            //HomePageHelper.OpenSandboxPage();
            //HomePageHelper.DeleteAllElementsLocally();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //if (_newExhibit != null && ExhibitHelper.IsExhibitFound(_newExhibit))
            //{
            //    ExhibitHelper.DeleteExhibitByJavascript(_newExhibit);
            //}
            //if (_newTimeline != null && TimelineHelper.IsTimelineFound(_newTimeline))
            //{
            //    TimelineHelper.DeleteTimelineByJavaScript(_newTimeline);
            //}
            //CreateScreenshotsIfTestFail(TestContext);
        }

        #endregion

        [TestMethod]
        public void tour_should_be_created()
        {
            _newTimeline = new Timeline
                {
                    //Timeline_ID = new Guid("bdc1ceff-76f8-4df4-ba72-96b353991314"),
                    FromYear = -2358409999,
                    ToYear = -7972900000,
                    Title = "WebDriverApiTitle"

                };
            Guid newTimelineId = ApiHelper.CreateTimelineByApi(_newTimeline);
            Bookmark bookmark = new Bookmark {Name = "name", Id = newTimelineId};
            Tour tour = new Tour();
            tour.Name = "webdriverTour";
            tour.Description = "webdriver description";
            tour.Bookmarks = new Collection<Chronozoom.Entities.Bookmark>() { bookmark };
            TourHelper.AddTour(tour);
        }
    }
}