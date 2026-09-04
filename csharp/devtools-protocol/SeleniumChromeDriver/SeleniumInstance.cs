#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.IO;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumChromeDriver
{
    public class SeleniumInstance
    {
        private string RemoteDebuggingAddress { get; }

        /// <summary>
        ///     The page shipped alongside the application, so that the
        ///     scenario does not depend on an external web site.
        /// </summary>
        private static string StartPage =>
            new Uri(Path.Combine(Directory.GetCurrentDirectory(), "home.html"))
               .AbsoluteUri;

        public event Action Connected;

        public SeleniumInstance(int debuggingPort)
        {
            RemoteDebuggingAddress = $"localhost:{debuggingPort}";
        }

        public async Task ConnectAndRun()
        {
            await Task.Run(async () =>
            {
                IWebDriver webDriver = await Connect();

                //Time for displaying the loaded page
                await Task.Delay(3000);

                await RunScenario(webDriver);

                webDriver.Quit();
            });
        }

        protected virtual void OnConnected()
            => Connected?.Invoke();

        private async Task<IWebDriver> Connect()
        {
            return await Task.Run(() =>
            {
                // #docfragment "Selenium.Connect"
                ChromeOptions options = new ChromeOptions
                {
                    DebuggerAddress = RemoteDebuggingAddress
                };

                IWebDriver webDriver = new ChromeDriver(options)
                {
                    Url = StartPage
                };

                // Give FindElement time to wait for the page to load.
                webDriver.Manage().Timeouts().ImplicitWait =
                    TimeSpan.FromSeconds(10);
                // #enddocfragment "Selenium.Connect"

                OnConnected();

                return webDriver;
            });
        }

        private async Task RunScenario(IWebDriver webDriver)
        {
            await Task.Run(() =>
            {
                IWebElement evaluateLink = webDriver.FindElement(By.Id("evaluate"));
                evaluateLink.Click();

                IWebElement nameTextbox = webDriver.FindElement(By.Id("name"));
                nameTextbox.SendKeys("John Doe");

                IWebElement emailTextbox = webDriver.FindElement(By.Id("email"));
                emailTextbox.SendKeys("sales@teamdev.com");
            });
        }
    }
}
