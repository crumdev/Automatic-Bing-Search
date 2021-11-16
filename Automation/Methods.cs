using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Automation.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace Automation
{
    class Methods
    {
        const string emailInputId = "i0116";
        const string nextButtonId = "idSIButton9";
        const string passwordInputId = "i0118";
        const string noButtonId = "idBtn_Back";
        const string searchBarId = "sb_form_q";
        private readonly AutomationOptions _options;
        public Methods(IOptions<AutomationOptions> options) => _options = options.Value;

        public void SearchWeb()
        {
            foreach (BingUser user in _options.BingUsers)
            {
                IWebDriver driver = new ChromeDriver();
                //IWebDriver driver = new FirefoxDriver();
                LaunchBrowserToSignInPage(_options.SignInUrl, driver);
                EnterEmail(user.Email, driver);
                EnterPassword(user.Password, driver);
                DenyStayingSignedIn(driver);

                var wordsToSearch = GetRandomWords(user.SearchCount);

                foreach (string word in wordsToSearch)
                {
                    ClickIntoSearchBar(driver);
                    PassWordIntoSearchBar(word, driver);
                    ExecuteSearch(driver);
                    ClearSearchBar(driver);
                }
                CloseBrowser(driver);

                if (UserIsLastInList(user, _options.BingUsers))
                {
                    CleanUp(driver);
                }
            }
        }

        void LaunchBrowserToSignInPage(string URL, IWebDriver driver)
        {
            driver.Navigate().GoToUrl(URL);
        }

        void EnterEmail(string email, IWebDriver driver)
        {
            driver.FindElement(By.Id(emailInputId)).SendKeys(email);
            driver.FindElement(By.Id(nextButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void EnterPassword(string password, IWebDriver driver)
        {
            driver.FindElement(By.Id(passwordInputId)).SendKeys(password);
            driver.FindElement(By.Id(nextButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void DenyStayingSignedIn(IWebDriver driver)
        {
            driver.FindElement(By.Id(noButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        static List<string> GetRandomWords(int numberOfWords)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://random-word-api.herokuapp.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("word?number=" + numberOfWords.ToString()).Result;
            List<string> randomWords = JsonConvert.DeserializeObject<List<string>>(response.Content.ReadAsStringAsync().Result);
            return randomWords;
        }

        void ClickIntoSearchBar(IWebDriver driver)
        {
            driver.FindElement(By.Id(searchBarId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void PassWordIntoSearchBar(string word, IWebDriver driver)
        {
            driver.FindElement(By.Id(searchBarId)).SendKeys(word);
            Thread.Sleep(_options.SleepTime);
        }

        void ExecuteSearch(IWebDriver driver)
        {
            driver.FindElement(By.Id(searchBarId)).SendKeys(Keys.Enter);
            Thread.Sleep(_options.SleepTime);
        }

        void ClearSearchBar(IWebDriver driver)
        {
            driver.FindElement(By.Id(searchBarId)).Clear();
        }

        void CloseBrowser(IWebDriver driver)
        {
            driver.Close();
        }

        bool UserIsLastInList(BingUser currentUser, List<BingUser> users)
        {
            return users.IndexOf(currentUser) == users.Count - 1;
        }

        void CleanUp(IWebDriver driver)
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
