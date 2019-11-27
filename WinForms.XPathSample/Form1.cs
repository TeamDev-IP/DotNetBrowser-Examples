using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WinForms.XPathSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();  

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ManualResetEvent waitEvent = new ManualResetEvent(false);
            browserView.Browser.FinishLoadingFrameEvent += delegate (object sender1, FinishLoadingEventArgs e1)
            {
                // Wait until main document of the web page is loaded completely.
                if (e1.IsMainFrame)
                {
                    waitEvent.Set();
                }
            };
            browserView.Browser.LoadURL("teamdev.com");
            waitEvent.WaitOne();



            label1.Text = "Press button 'XPath' to view an info about <div> on this page";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            button1.Text = "XPath";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DOMDocument document = browserView.Browser.GetDocument();

            //XPath expressions
            XPathResult resultNum = document.Evaluate("count(//div)");
            XPathResult resultNumSpec = document.Evaluate("count(//div[@class='col-sm-4 prod-thumb'])");
            XPathResult result = document.Evaluate("string( / html / body / div[2] / div[2] / div[1] / div[2] / div / div[1] / div[2] / h3 / a)");
            XPathResult resultNodes = document.Evaluate("/ html / body / div[2] / div[2] / div[1] / div[2] / div / *");

            var pagecontent = document.GetElementById("contact");

            //XPath expressions
            XPathResult resultСontactNum = pagecontent.Evaluate("count(.//div)");
            XPathResult resultСontactNumSpec = pagecontent.Evaluate("count(.//div[@class='form-group'])");
            XPathResult resultСontact = pagecontent.Evaluate("string(./form/div/h5)");
            XPathResult resultСontactNodes = pagecontent.Evaluate("./form/fieldset/div[4]/*");


            //Check if XPath result type is correct
            if (resultNum.IsNumber && resultNumSpec.IsNumber)
            {
                label1.Text = "Quantity of all <div> = " + resultNum.Number;
                label2.Text = "Quantity of <div> with class 'col-sm-4 prod-thumb' = " + resultNumSpec.Number;
            }
            else
            {
                label1.Text = "Error";
                label2.Text = "Error";
            }

            //Check if XPath result type is correct
            if (resultСontactNum.IsNumber && resultСontactNumSpec.IsNumber)
            {
                label6.Text = "Quantity of all <div> in <div id='contact'> = " + resultСontactNum.Number;
                label7.Text = "Quantity of <div> with class 'form-group'  in <div id='contact'> = " + resultСontactNumSpec.Number;
            }
            else
            {
                label6.Text = "Error";
                label7.Text = "Error";
            }


            //Check if XPath result type is correct
            if (result.IsString)
            {
                label3.Text = "Text from selected element: " + result.String;
            }
            else
            {
                label3.Text = "Error";
            }

            //Check if XPath result type is correct
            if (resultСontact.IsString)
            {
                label8.Text = "Text from selected element: " + resultСontact.String;
            }
            else
            {
                label8.Text = "Error";
            }


            //Check if XPath result type is correct
            if (resultNodes.IsIterator)
                label4.Text = resultNodes.ResultType.ToString();
            else
                label4.Text = "Mistake";

            //Check if XPath result type is correct
            if (resultСontactNodes.IsIterator)
                label9.Text = resultСontactNodes.ResultType.ToString();
            else
                label9.Text = "Mistake";

            //Get list with DOMNodes
            var tmp = resultNodes.Iterator.Select(item => item.Node.Children);

            //Get list with DOMNodes
            var tmpСontact = resultСontactNodes.Iterator.Select(item => item.Node.Children);
            try
            {
                //Extract list with DOMNodes          
                foreach (var item in tmp)
                {                    
                    foreach (var item1 in item)
                    {
                        //Extract DOMNode from the list
                        label4.Text = "Node type: " + item1.NodeType;
                        label5.Text = "Node name: " + item1.NodeName;
                    }
                }
                //Extract list with DOMNodes          
                foreach (var item in tmpСontact)
                {
                    foreach (var item1 in item)
                    {
                        //Extract DOMNode from the list
                        label9.Text = "Node type: " + item1.NodeType;
                        label10.Text = "Node name: " + item1.NodeName;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!browserView.IsDisposed)
            {
                browserView.Browser.Dispose();
                browserView.Dispose();
            }
        }
    }
}
