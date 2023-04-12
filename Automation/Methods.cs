using System;
using System.Threading;
using OpenQA.Selenium;
using Automation.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using OpenQA.Selenium.Edge;
using Microsoft.Extensions.Logging;

namespace Automation
{

    public class Methods : IMethods
    {
        IWebDriver webDriver;
        const string emailInputId = "i0116";
        const string nextButtonId = "idSIButton9";
        const string passwordInputId = "i0118";
        const string noButtonId = "idBtn_Back";
        const string searchBarId = "sb_form_q";
        private readonly AutomationOptions _options;
        private readonly ILogger<Methods> _logger;

        public Methods(ILogger<Methods> logger, IOptions<AutomationOptions> options)
        {
            _options = options.Value;
            _logger = logger;
        }

        public void SearchWebInMobileMode()
        {
            foreach (BingUser user in _options.BingUsers)
            {
                _logger.LogInformation($"Starting Mobile Browser for: {user.Email}");
                EdgeOptions edgeOptions = new();
                
                edgeOptions.AddArguments(new List<string>(){
                    "headless",
                    "--profile-directory=Profile 5",
                    "--allow-top-navigation",
                    "disable-gpu"
                });


                
                edgeOptions.EnableMobileEmulation("iPhone X");
                webDriver = new EdgeDriver(edgeOptions);
                StartSearchProcess(user, user.SearchCountMobile);
            }
        }

        public void SearchWebInDesktopMode()
        {
            foreach (BingUser user in _options.BingUsers)
            {
                _logger.LogInformation($"Starting Desktop Browser for: {user.Email}");

                EdgeOptions edgeOptions = new();
                edgeOptions.AddArguments(new List<string>(){
                    "headless",
                    "--profile-directory=Profile 5",
                    "--allow-top-navigation",
                    "disable-gpu"
                });
                webDriver = new EdgeDriver(edgeOptions);
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
            _logger.LogInformation("Entering email");
            webDriver.FindElement(By.Id(emailInputId)).SendKeys(email);
            webDriver.FindElement(By.Id(nextButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void EnterPassword(string password)
        {
            _logger.LogInformation("Entering password");
            webDriver.FindElement(By.Id(passwordInputId)).SendKeys(password);
            webDriver.FindElement(By.Id(nextButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void DenyStayingSignedIn()
        {
            _logger.LogInformation("Deny staying logged in");
            webDriver.FindElement(By.Id(noButtonId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        public List<string> GetRandomWords(int numberOfWords)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://random-word.ryanrk.com/api/en/word/random/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(numberOfWords.ToString()).Result;
            List<string> randomWords = JsonConvert.DeserializeObject<List<string>>(response.Content.ReadAsStringAsync().Result);
            _logger.LogInformation($"Retrieved list of random words");
            return randomWords;
        }

        void ClickIntoSearchBar()
        {
            _logger.LogInformation("Selecting Search Bar");
            webDriver.FindElement(By.Id(searchBarId)).Click();
            Thread.Sleep(_options.SleepTime);
        }

        void PassWordIntoSearchBar(string word)
        {
            _logger.LogInformation($"Typing: {word}");
            webDriver.FindElement(By.Id(searchBarId)).SendKeys(word);
            Thread.Sleep(_options.SleepTime);
        }

        void ExecuteSearch()
        {
            _logger.LogInformation("Starting search");
            webDriver.FindElement(By.Id(searchBarId)).SendKeys(Keys.Enter);
            Thread.Sleep(_options.SleepTime);
        }

        void ClearSearchBar()
        {
            _logger.LogInformation("Clearing Search");
            webDriver.FindElement(By.Id(searchBarId)).Clear();
        }

        void CloseBrowser()
        {
            _logger.LogInformation("Closing Browser");
            webDriver.Close();
        }

        //bool UserIsLastInList(BingUser currentUser, List<BingUser> users)
        //{
        //    return users.IndexOf(currentUser) == users.Count - 1;
        //}

        public void CleanUp()
        {
            _logger.LogInformation("Cleaning Up");
            webDriver.Quit();
            webDriver.Dispose();
        }
    }
}
