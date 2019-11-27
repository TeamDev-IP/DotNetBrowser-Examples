using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinForms.Demo
{
    class ProxyConfigForm
    {
        private BrowserView browserView;
        private Browser browser;

        public ProxyConfigForm(BrowserView browserView)
        {
            this.browserView = browserView;
            this.browser = browserView.Browser;
        }

        public void Show()
        {
            Form proxyConfigForm = new Form();
            proxyConfigForm.ShowIcon = false;
            proxyConfigForm.Size = new Size(500, 300);
            proxyConfigForm.Text = "Proxy Settings";
            proxyConfigForm.MaximizeBox = false;
            proxyConfigForm.MinimizeBox = false;
            proxyConfigForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            proxyConfigForm.StartPosition = FormStartPosition.CenterParent;

            //Choose proxy config
            Label nameProxyList = new Label();
            nameProxyList.Text = "Choose proxy type:";
            nameProxyList.Location = new Point(20, 30);
            ComboBox proxyList = new ComboBox();
            proxyList.Location = new Point(200, 30);
            proxyList.Items.Add("AutoDetect");
            proxyList.Items.Add("Direct");
            proxyList.Items.Add("PAC File");
            proxyList.Items.Add("Custom");
            proxyList.SelectedIndex = 0;

            //Set proxy auto-config
            Label namePacFileUrl = new Label();
            namePacFileUrl.Location = new Point(20, 75);
            namePacFileUrl.Size = new Size(150, 50);
            namePacFileUrl.Text = "Set URL address of the proxy auto-config (PAC) file:";
            TextBox pacFileUrl = new TextBox();
            pacFileUrl.Location = new Point(200, 80);
            pacFileUrl.Size = new Size(250, 20);

            //Set proxy rules
            Label nameProxyRules = new Label();
            nameProxyRules.Location = new Point(20, 85);
            nameProxyRules.Text = "Set proxy rules:";
            TextBox proxyRules = new TextBox();
            proxyRules.Location = new Point(200, 80);
            proxyRules.Size = new Size(250, 20);
            ToolTip toolTipProxyRules = new ToolTip();
            toolTipProxyRules.SetToolTip(proxyRules, "Ex.: 'http=foopy:80;ftp=foopy:20'");

            //Set bypass rules
            Label nameExceptions = new Label();
            nameExceptions.Location = new Point(20, 125);
            nameExceptions.Text = "Set URLs that should bypass the proxy settings:";
            nameExceptions.Size = new Size(150, 50);
            TextBox exceptions = new TextBox();
            exceptions.Location = new Point(200, 130);
            exceptions.Size = new Size(250, 20);
            toolTipProxyRules.SetToolTip(exceptions, "Match local addresses.");

            //Buttons
            Button buttonOk = new Button();
            buttonOk.Text = "OK";
            buttonOk.Location = new Point(50, 210);
            Button buttonCancel = new Button();
            buttonCancel.Text = "Cancel";
            buttonCancel.Location = new Point(350, 210);

            namePacFileUrl.Visible = false;
            pacFileUrl.Visible = false;
            nameProxyRules.Visible = false;
            proxyRules.Visible = false;
            nameExceptions.Visible = false;
            exceptions.Visible = false;

            proxyList.SelectedIndexChanged += delegate
            {
                if (proxyList.SelectedIndex == 0)
                {
                    namePacFileUrl.Visible = false;
                    pacFileUrl.Visible = false;
                    nameProxyRules.Visible = false;
                    proxyRules.Visible = false;
                    nameExceptions.Visible = false;
                    exceptions.Visible = false;
                }
                else if (proxyList.SelectedIndex == 1)
                {
                    namePacFileUrl.Visible = false;
                    pacFileUrl.Visible = false;
                    nameProxyRules.Visible = false;
                    proxyRules.Visible = false;
                    nameExceptions.Visible = false;
                    exceptions.Visible = false;
                }
                else if (proxyList.SelectedIndex == 2)
                {
                    namePacFileUrl.Visible = true;
                    pacFileUrl.Visible = true;
                    nameProxyRules.Visible = false;
                    proxyRules.Visible = false;
                    nameExceptions.Visible = false;
                    exceptions.Visible = false;
                }
                else if (proxyList.SelectedIndex == 3)
                {
                    namePacFileUrl.Visible = false;
                    pacFileUrl.Visible = false;
                    nameProxyRules.Visible = true;
                    proxyRules.Visible = true;
                    nameExceptions.Visible = true;
                    exceptions.Visible = true;
                }
            };

            buttonOk.Click += delegate
            {
                if (proxyList.SelectedIndex == 0)
                {
                    browser.Context.ProxyConfig = new AutoDetectProxyConfig();
                }
                else if (proxyList.SelectedIndex == 1)
                {
                    browser.Context.ProxyConfig = new DirectProxyConfig();
                }
                else if (proxyList.SelectedIndex == 2)
                {
                    browser.Context.ProxyConfig = new URLProxyConfig(pacFileUrl.Text);
                }
                else if (proxyList.SelectedIndex == 3)
                {
                    browser.Context.ProxyConfig = new CustomProxyConfig(proxyRules.Text, exceptions.Text);
                }   

                InfoMessageBox.Show((Control)browserView, "Proxy Settings successfully applied", "Warning");
                proxyConfigForm.Close();
            };

            buttonCancel.Click += delegate
            {
                proxyConfigForm.Close();
            };

            //Added controls on the form
            proxyConfigForm.Controls.Add(nameProxyList);
            proxyConfigForm.Controls.Add(proxyList);
            proxyConfigForm.Controls.Add(buttonOk);
            proxyConfigForm.Controls.Add(buttonCancel);
            proxyConfigForm.Controls.Add(proxyRules);
            proxyConfigForm.Controls.Add(exceptions);
            proxyConfigForm.Controls.Add(nameProxyRules);
            proxyConfigForm.Controls.Add(nameExceptions);
            proxyConfigForm.Controls.Add(namePacFileUrl);
            proxyConfigForm.Controls.Add(pacFileUrl);

            proxyConfigForm.ShowDialog();
        }
    }
}
