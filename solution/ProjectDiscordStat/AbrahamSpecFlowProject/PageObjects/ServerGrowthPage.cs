using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;


namespace AbrahamSpecFlowProject.PageObjects
{
    public class ServerGrowthPage : Page
    {
        private IWebElement ExportJsonActiveMemberInServer => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonActiveMemberInServer"));

        private IWebElement ExportCsvActiveMemberInServer => _browserInteractions.WaitAndReturnElement(By.Id("exportCsvActiveMemberInServer"));


        public ServerGrowthPage(IBrowserInteractions browserInteractions)
         : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }

        public void ClickExportJsonActiveMemberInServerButton()
        {
            ExportJsonActiveMemberInServer.ClickWithRetry(30);
        }

        public void ClickExportCsvActiveMemberInServerButton()
        {
            ExportCsvActiveMemberInServer.ClickWithRetry(30);
        }

        public void VerifyFileJsonActiveMembersInServerExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\ActiveMembersInServer.json";

            ClickExportJsonActiveMemberInServerButton();
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

        public void VerifyFileCsvActiveMembersInServerExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\ActiveMembersInServer.csv";

            ClickExportCsvActiveMemberInServerButton();
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
