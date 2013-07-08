using System;
using System.Collections.ObjectModel;
using Application.Driver;
using Application.Helper.Constants;
using Application.Helper.UserActions;
using OpenQA.Selenium;
using Tour = Application.Helper.Entities.Tour;
using Bookmark = Application.Helper.Entities.Bookmark;

namespace Application.Helper.Helpers
{
    public class TourHelper : DependentActions
    {
        private const string MayanHistoryTourName = "Mayan History";

        public void AddTour(Tour tour)
        {
            Logger.Log("<- " + tour);
            
            InitTourCreationMode();
            SetTourName(tour.Name);
            SetTourDescription(tour.Description);
            SetTourBookmarks(tour.Bookmarks);
            CreateTour();
            WaitAjaxComplete(60);
        }


        public void OpenToursListWindow()
        {
            Logger.Log("<-");
            Click(By.Id("tours_index"));
            Logger.Log("->");
        }

        public void SelectMayanHistoryTour()
        {
            Logger.Log("<-");
            SelectTour(MayanHistoryTourName);
            WaitForElementIsDisplayed(By.XPath("//*[@id='breadcrumbs-table']//*[text()='Mayan History']"));
            Logger.Log("->");
        }

        public void PauseTour()
        {
            Logger.Log("<-");
            Click(By.Id("tour_playpause"));
            Logger.Log("->");
        }

        public void ResumeTour()
        {
            Logger.Log("<-");
            PauseTour();
            Logger.Log("->");
        }

        private void SelectTour(string tour)
        {
            string xpath = String.Format("//*[@id='toursList']//*[text()='{0}']/following-sibling::div[1]", tour);
            Click(By.XPath(xpath));
        }

        private void CreateTour()
        {
            Click(By.XPath("//*[@id='auth-edit-tours-form']/div[2]/button[text()='create tour']"));
        }

        private void InitTourCreationMode()
        {
            Logger.Log("<-");
            MoveToElementAndClick(By.XPath("//*[@title='Create Your Events']"));
            MoveToElementAndClick(By.XPath("//*[@id='header-edit-form']/div[2]/button[3]"));
            Logger.Log("->");
        }

        private void SetTourBookmarks(Collection<Chronozoom.Entities.Bookmark> bookmarks)
        {
            foreach (var bookmark in bookmarks)
            {
                Logger.Log("Add bookmark: " + bookmark.Name);
                Click(By.XPath("//*[@id='auth-edit-tours-form']/div[2]/button[text()='add new stop']"));
                WaitForElementIsDisplayed(By.Id("message-window"));
                GoToTimeline(bookmark.Id.ToString());
                Click(By.Id("vc-container"));
                Sleep(3);
            }
        }

        public Tour CreateCustomTour()
        {
            const string script = Javascripts.LastCanvasElement;
            var timelineTitle = GetJavaScriptExecutionResult(script + ".title");
            var timelineId = GetJavaScriptExecutionResult(script + ".id");

            var tour = new Tour() { Name = "WebDriverTour", Description = "WebDriverTourDescription" };
            tour.Bookmarks = new Collection<Chronozoom.Entities.Bookmark> { new Bookmark { Name = timelineId } };
            AddTour(tour);

            return new Tour();
        }


        private void GoToTimeline(string guid)
        {
            ExecuteJavaScript("CZ.Search.goToSearchResult('" + guid + "', 'timeline')");
        }

        private void SetTourDescription(string tourDescription)
        {
            Logger.Log("<- tour description: " + tourDescription);
            TypeText(By.XPath("//*[@id='auth-edit-tours-form']/div[2]/textarea"), tourDescription);
            Logger.Log("->");
        }

        private void SetTourName(string tourName)
        {
            Logger.Log("<- tour name: " + tourName);
            TypeText(By.XPath("//*[@id='auth-edit-tours-form']/div[2]/input"), tourName);
            Logger.Log("->");
        }
    }
}