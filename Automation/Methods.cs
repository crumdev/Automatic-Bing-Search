using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Automation.Models;
using Microsoft.Extensions.Options;

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

                //getting email textbox & passing your@email
                driver.FindElement(By.Id("i0116")).SendKeys(user.Email);
                driver.FindElement(By.Id("idSIButton9")).Click();
                Thread.Sleep(_options.SleepTime);

                //getting password textbox & passing your@email
                driver.FindElement(By.Id("i0118")).SendKeys(user.Password);
                driver.FindElement(By.Id("idSIButton9")).Click();
                Thread.Sleep(_options.SleepTime);

                int x = 0;
                var words = _options.SearchTerms;
                while (x < words.Count)
                {
                    Random random = new Random();
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
    }
}
