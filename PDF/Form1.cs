using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace PDFViewerApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeWebView();
        }

        // Initialize WebView2 control
        private async void InitializeWebView()
        {
            await webView2PDF.EnsureCoreWebView2Async(null);
        }

        // Open PDF file and display it in WebView2
        private void btnOpenPDF_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string pdfPath = openFileDialog.FileName;
                    webView2PDF.CoreWebView2.Navigate($"file:///{pdfPath}");
                }
            }
        }
    }
}
