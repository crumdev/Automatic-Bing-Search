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
        //private readonly IWebDriver driver = new FirefoxDriver();
        private IWebDriver driver = new ChromeDriver();
        public Methods(IOptions<AutomationOptions> options) => _options = options.Value;

        public void SearchWeb()
        {
            foreach (BingUser user in _options.BingUsers)
            {
                LaunchBrowserToSignInPage(_options.SignInUrl);
                EnterEmail(user.Email);
                EnterPassword(user.Password);
                DenyStayingSignedIn();

                var wordsToSearch = GetRandomWords(user.SearchCount);

                foreach (string word in wordsToSearch)
                {
                    ClickIntoSearchBar();
                    PassWordIntoSearchBar(word);
                    ExecuteSearch();
                    ClearSearchBar();
                }
                CloseBrowser();
            }
        }

        void CloseBrowser()
        {
            driver.Quit();
            //Thread.Sleep(_options.SleepTime);
        }

        void ClearSearchBar()
        {
            driver.FindElement(By.Id(searchBarId)).Clear();
        }

        void ExecuteSearch()
        {
            driver.FindElement(By.Id(searchBarId)).SendKeys(Keys.Enter);
            Thread.Sleep(_options.SleepTime);
        }

        void PassWordIntoSearchBar(string word)
        {
            driver.FindElement(By.Id(searchBarId)).SendKeys(word);
            Thread.Sleep(_options.SleepTime);
        }

        void ClickIntoSearchBar()
        {
            driver.FindElement(By.Id(searchBarId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void DenyStayingSignedIn()
        {
            driver.FindElement(By.Id(noButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void EnterPassword(string password)
        {
            driver.FindElement(By.Id(passwordInputId)).SendKeys(password);
            driver.FindElement(By.Id(nextButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void EnterEmail(string email)
        {
            driver.FindElement(By.Id(emailInputId)).SendKeys(email);
            driver.FindElement(By.Id(nextButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void LaunchBrowserToSignInPage(string URL)
        {
            driver.Navigate().GoToUrl(URL);
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
    }
}
