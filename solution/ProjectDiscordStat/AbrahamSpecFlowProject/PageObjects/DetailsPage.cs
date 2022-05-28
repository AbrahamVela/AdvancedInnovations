using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace AbrahamSpecFlowProject.PageObjects
{
    public class DetailsPage : Page
    {
        private IWebElement Title => _browserInteractions.WaitAndReturnElement(By.Id("displayMessage"));
        private IWebElement VoiceGraph => _browserInteractions.WaitAndReturnElement(By.Id("usersVoiceHourlyAllTimeChart"));
        private IWebElement MessageGraph => _browserInteractions.WaitAndReturnElement(By.Id("usersHourlyAllTimeChart"));
        private IWebElement WelcomeText => _browserInteractions.WaitAndReturnElement(By.Id("loggedin-welcome"));
        private IWebElement MessagesTable => _browserInteractions.WaitAndReturnElement(By.Id("serverTable"));
        private IWebElement DetailsContainer => _browserInteractions.WaitAndReturnElement(By.Id("DetailsContainer"));
        private IWebElement GamesButton => _browserInteractions.WaitAndReturnElement(By.Id("ViewGamesButton"));
        private IWebElement GameGraph => _browserInteractions.WaitAndReturnElement(By.Id("presencesHourlyAllTimeChart"));
        private IWebElement ViewServerGrowthButton => _browserInteractions.WaitAndReturnElement(By.Id("ViewServerGrowthButton"));

        private IWebElement ExportJsonVoiceChannelButton => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonVoiceChannel"));
        private IWebElement ExporCsvVoiceChannelButton => _browserInteractions.WaitAndReturnElement(By.Id("exporCsvVoiceChannel"));

        private IWebElement ExportJsonMessagingButton => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonMessaging"));
        private IWebElement ExportCsvMessagingButton => _browserInteractions.WaitAndReturnElement(By.Id("exportCsvMessaging"));

        private IWebElement ExportJsonActiveGamingButton => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonActiveGaming"));
        private IWebElement ExportCsvActiveGamingButton => _browserInteractions.WaitAndReturnElement(By.Id("exportCsvActiveGaming"));

        private IWebElement ExportJsonHoursPerGameButton => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonHoursPerGame"));
        private IWebElement ExportCsvHoursPerGameButton => _browserInteractions.WaitAndReturnElement(By.Id("exportCsvHoursPerGame"));

        private IWebElement ExportJsonStatusesButton => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonStatuses"));
        private IWebElement ExportCsvStatusesButton => _browserInteractions.WaitAndReturnElement(By.Id("exportCsvStatuses"));

        public DetailsPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }

        public bool GetGameGraph => GameGraph.Displayed;
        public string GetDetailsContainer => DetailsContainer.Text;
        public bool GetVoiceGraph => VoiceGraph.Displayed;
        public bool GetMessageGraph => MessageGraph.Displayed;
        public string GetTitle => Title.Text;
        public string GetWelcomeText => WelcomeText.Text;
        public bool GetMessagingTable => MessagesTable.Displayed;

        public void ClickViewServerGrowthButton()
        {
            ViewServerGrowthButton.ClickWithRetry(30);
        }

        public void ClickGames()
        {
            GamesButton.ClickWithRetry(30);
        }

        public void ClickExportJsonVoiceChannelButton()
        {
            ExportJsonVoiceChannelButton.ClickWithRetry(30);
        }

        public void ClickExporCsvVoiceChannelButton()
        {
            ExporCsvVoiceChannelButton.ClickWithRetry(30);
        }

        public void ClickExportJsonMessagingButton()
        {
            ExportJsonMessagingButton.ClickWithRetry(30);
        }

        public void ClickExportCsvMessagingButton()
        {
            ExportCsvMessagingButton.ClickWithRetry(30);
        }

        public void ClickExportJsonActiveGamingButton()
        {
            ExportJsonActiveGamingButton.ClickWithRetry(30);
        }

        public void ClickExportCsvActiveGamingButton()
        {
            ExportCsvActiveGamingButton.ClickWithRetry(30);
        }

        public void ClickExportJsonHoursPerGameButton()
        {
            ExportJsonHoursPerGameButton.ClickWithRetry(30);
        }

        public void ClickExportCsvHoursPerGameButton()
        {
            ExportCsvHoursPerGameButton.ClickWithRetry(30);
        }

        public void ClickExportJsonStatusesButton()
        {
            ExportJsonStatusesButton.ClickWithRetry(30);
        }

        public void ClickExportCsvStatusesButton()
        {
            ExportCsvStatusesButton.ClickWithRetry(30);
        }

        public void VerifyFileJsonActiveVoiceChannelTimeExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\ActiveVoiceChannelTime.json";

            ClickExportJsonVoiceChannelButton();
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

        public void VerifyFileCsvActiveVoiceChannelTimeExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\ActiveVoiceChannelTime.csv";

            ClickExporCsvVoiceChannelButton();
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

        public void VerifyFileJsonActiveMessagingExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\ActiveMessagingTime.json";

            ClickExportJsonMessagingButton();
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

        public void VerifyFileCsvActiveMessagingTimeExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\ActiveMessagingTime.csv";

            ClickExportCsvMessagingButton();
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

        public void VerifyFileJsonActiveGamingTimeExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\ActiveGamingTime.json";

            ClickExportJsonActiveGamingButton();
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

        public void VerifyFileCsvActiveGamingTimeExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\ActiveGamingTime.csv";

            ClickExportCsvActiveGamingButton();
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

        public void VerifyFileJsonHoursPerGameExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\HoursPerGame.json";

            ClickExportJsonHoursPerGameButton();
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

        public void VerifyFileCsvHoursPerGameExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\HoursPerGame.csv";

            ClickExportCsvHoursPerGameButton();
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

        public void VerifyFileJsonStatusesExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\Statuses.json";

            ClickExportJsonStatusesButton();
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

        public void VerifyFileCsvStatusesExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\Statuses.csv";

            ClickExportCsvStatusesButton();
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

        public void SaveAllCookies()
        {
            ReadOnlyCollection<Cookie> cookies = _browserInteractions.GetCookies();
            FileUtils.SerializeCookiesToFile(Common.CookieFile, cookies);
        }

        public void LoadAllCookies()
        {
            List<Cookie> cookies = FileUtils.DeserializeCookiesFromFile(Common.CookieFile);
            foreach (Cookie cookie in cookies)
            {
                _browserInteractions.AddCookie(cookie);
            }
            _browserInteractions.RefreshPage();
        }

        public void DeleteCookies()
        {
            _browserInteractions.DeleteAllCookies();
        }

    }
}