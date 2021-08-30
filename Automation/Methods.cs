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

namespace Automation
{
    class Methods
    {
        private readonly AutomationOptions _options;
        public Methods(IOptions<AutomationOptions> options) => _options = options.Value;
                
        public void SearchWeb()
        {
            foreach (BingUser user in _options.BingUsers)
            {
                IWebDriver driver = new FirefoxDriver();

                //enter URL for log in page
                driver.Navigate().GoToUrl(_options.SignInUrl);

                //getting email textbox & passing user.email
                driver.FindElement(By.Id("i0116")).SendKeys(user.Email);
                driver.FindElement(By.Id("idSIButton9")).Click();
                Thread.Sleep(_options.SleepTime);

                //getting password textbox & passing user.password
                driver.FindElement(By.Id("i0118")).SendKeys(user.Password);
                driver.FindElement(By.Id("idSIButton9")).Click();
                Thread.Sleep(_options.SleepTime);

                //clicking 'no' when asked to stay signed in
                driver.FindElement(By.Id("idBtn_Back")).Click();
                Thread.Sleep(_options.SleepTime);

                int x = 0;
                var words = GetRandomWords(user.SearchCount);



                while (x < user.SearchCount)
                {
                    Random random = new();
                    int index = random.Next(words.Count);
                    var searchItem = words[index];
                    //remove searched word from list (Only want to search each word once)
                    words.RemoveAt(index);

                    //clicking into search bar
                    driver.FindElement(By.Id("sb_form_q")).Click();
                    Thread.Sleep(_options.SleepTime);

                    //passing word from list into search bar
                    driver.FindElement(By.Id("sb_form_q")).SendKeys(searchItem);
                    Thread.Sleep(_options.SleepTime);

                    //execute search function
                    driver.FindElement(By.Id("sb_form_q")).SendKeys(Keys.Enter);
                    Thread.Sleep(_options.SleepTime);

                    //clear search bar
                    driver.FindElement(By.Id("sb_form_q")).Clear();
                    x++;
                }

            //Close the browser
            driver.Quit();
            Thread.Sleep(_options.SleepTime);
           }
        }


        public List<string> GetRandomWords(int numberOfWords)
        {
            List<string> randomWords = new();
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://random-word-api.herokuapp.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("word?number=" + numberOfWords.ToString()).Result;
            randomWords = JsonConvert.DeserializeObject<List<string>>(response.Content.ReadAsStringAsync().Result);
            return randomWords;
        }
    }
}
