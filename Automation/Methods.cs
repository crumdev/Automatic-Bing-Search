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
        IWebDriver webDriver;
        const string emailInputId = "i0116";
        const string nextButtonId = "idSIButton9";
        const string passwordInputId = "i0118";
        const string noButtonId = "idBtn_Back";
        const string searchBarId = "sb_form_q";
        private readonly AutomationOptions _options;
        public Methods(IOptions<AutomationOptions> options) => _options = options.Value;

        public void SearchWebInMobileMode()
        {
            foreach (BingUser user in _options.BingUsers)
            {
                ChromeOptions chromeOptions = new();
                chromeOptions.EnableMobileEmulation("iPhone X");
                webDriver = new ChromeDriver(chromeOptions);
                StartSearchProcess(user, user.SearchCountMobile);
            }
        }

        public void SearchWebInDesktopMode()
        {
            foreach (BingUser user in _options.BingUsers)
            {
                webDriver = new ChromeDriver();
                StartSearchProcess(user, user.SearchCount);
            }
        }

        public void StartSearchProcess(BingUser user, int searchCount)
        {
                LaunchBrowserToSignInPage(_options.SignInUrl);
                EnterEmail(user.Email);
                EnterPassword(user.Password);
                DenyStayingSignedIn();

                var wordsToSearch = GetRandomWords(searchCount);

                foreach (string word in wordsToSearch)
                {
                    ClickIntoSearchBar();
                    PassWordIntoSearchBar(word);
                    ExecuteSearch();
                    ClearSearchBar();
                }
                CloseBrowser();
        }

        void LaunchBrowserToSignInPage(string URL)
        {
            webDriver.Navigate().GoToUrl(URL);
        }

        void EnterEmail(string email)
        {
            webDriver.FindElement(By.Id(emailInputId)).SendKeys(email);
            webDriver.FindElement(By.Id(nextButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void EnterPassword(string password)
        {
            webDriver.FindElement(By.Id(passwordInputId)).SendKeys(password);
            webDriver.FindElement(By.Id(nextButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void DenyStayingSignedIn()
        {
            webDriver.FindElement(By.Id(noButtonId)).Click();
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

        void ClickIntoSearchBar()
        {
            webDriver.FindElement(By.Id(searchBarId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void PassWordIntoSearchBar(string word)
        {
            webDriver.FindElement(By.Id(searchBarId)).SendKeys(word);
            Thread.Sleep(_options.SleepTime);
        }

        void ExecuteSearch()
        {
            webDriver.FindElement(By.Id(searchBarId)).SendKeys(Keys.Enter);
            Thread.Sleep(_options.SleepTime);
        }

        void ClearSearchBar()
        {
            webDriver.FindElement(By.Id(searchBarId)).Clear();
        }

        void CloseBrowser()
        {
            webDriver.Close();
        }

        //bool UserIsLastInList(BingUser currentUser, List<BingUser> users)
        //{
        //    return users.IndexOf(currentUser) == users.Count - 1;
        //}

        public void CleanUp()
        {
            webDriver.Quit();
            webDriver.Dispose();
        }
    }
}
