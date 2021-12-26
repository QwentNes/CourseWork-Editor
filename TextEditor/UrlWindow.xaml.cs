using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;    

namespace TextEditor
{
    public partial class UrlWindow : Window
    {
        public string Url { get; set; }
        public List<string> urlResource = new List<string>() { "http://", "https://", "другое" };
        public UrlWindow()
        {
            InitializeComponent();
            AccessUrlList.ItemsSource = this.urlResource;
            AccessUrlList.SelectedIndex = 0;
        }

        //start_state
        public void updateUrl()
        {
            this.Url = (this.urlResource.Count() - 1) != AccessUrlList.SelectedIndex ? AccessUrlList.SelectedItem + UrlField.Text : UrlField.Text;
        }
        public void updateChecker(bool status)
        {
            ResultCheckLink.Text = status ? "Доступно" : "Недоступно";
            ResultCheckLink.Foreground = status ? Brushes.Green : Brushes.Red;
        }
        public void AddressSelection(object sender, EventArgs e)
        {
            ResultCheckLink.Text = "";
        }

        //end_state

        public void CheckLinkClick(object sender, EventArgs e)
        {
            this.updateUrl();
            try
            {
                Uri bind = new Uri(this.Url);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(bind);
                request.Timeout = 3000;
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                updateChecker(response.StatusCode.ToString() == "OK");
            }
            catch (Exception)
            {
                updateChecker(false);
            }
            this.Url = "";
        }
        public void CancelLinkClick(object sender, EventArgs e)
        {
            this.Url = "";
            this.Close();
        }
        public void AttachLinkClick(object sender, EventArgs e)
        {
            this.updateUrl();
            this.Close();
        }

    }
}
