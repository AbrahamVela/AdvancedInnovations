using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace AbrahamSpecFlowProject.PageObjects
{
    public class GameDetailsPage : Page
    {
        private IWebElement GameDetialsDownloadArea => _browserInteractions.WaitAndReturnElement(By.Id("gameDetialsDownloadArea"));

        private IWebElement ExportJsonMostPopularPlayTime => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonMostPopularPlayTime"));

        private IWebElement ExportCsvMostPopularPlayTime => _browserInteractions.WaitAndReturnElement(By.Id("exportCsvMostPopularPlayTime"));


        public GameDetailsPage(IBrowserInteractions browserInteractions)
          : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }

        public bool GetGameDetialsDownloadArea => GameDetialsDownloadArea.Displayed;

        public void ClickExportJsonMostPopularPlayTimeButton()
        {
            ExportJsonMostPopularPlayTime.ClickWithRetry(30);
        }

        public void ClickExportCsvMostPopularPlayTimeButton()
        {
            ExportCsvMostPopularPlayTime.ClickWithRetry(30);
        }

        public void VerifyFileJsonMostPopularPlayTimeExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\MostPopularPlayTime.json";

            ClickExportJsonMostPopularPlayTimeButton();
            var splitExpectedFilePath = expectedFilePath.Split('.');
            if (File.Exists(expectedFilePath) == true && splitExpectedFilePath[1] == "json")
            {
                return;
            }
            else
            {
                NUnit.Framework.Assert.Fail("File path does not exist or wrong format file type");
            }

        }

        public void VerifyFileCsvMostPopularPlayTimeExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\MostPopularPlayTime.csv";

            ClickExportCsvMostPopularPlayTimeButton();
            var splitExpectedFilePath = expectedFilePath.Split('.');
            if (File.Exists(expectedFilePath) == true && splitExpectedFilePath[1] == "csv")
            {
                return;
            }
            else
            {
                NUnit.Framework.Assert.Fail("File path does not exist or wrong format file type");
            }

        }
    }
}
