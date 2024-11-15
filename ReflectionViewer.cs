using System;
using System.Windows.Forms;

public partial class ReflectionViewer : Form
{
    public ReflectionViewer()
    {
        InitializeComponent();
        InitializeUI();
    }

    private void InitializeUI()
    {
        // Create button to upload PDF
        Button uploadButton = new Button();
        uploadButton.Text = "Upload PDF";
        uploadButton.Click += new EventHandler(UploadButton_Click);
        uploadButton.Dock = DockStyle.Top;
        this.Controls.Add(uploadButton);

        // Create WebBrowser to view PDF
        WebBrowser pdfViewer = new WebBrowser();
        pdfViewer.Name = "pdfViewer";
        pdfViewer.Dock = DockStyle.Fill;
        this.Controls.Add(pdfViewer);
    }

    private void UploadButton_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                DisplayPDF(filePath);
            }
        }
    }

    private void DisplayPDF(string filePath)
    {
        WebBrowser pdfViewer = this.Controls["pdfViewer"] as WebBrowser;
        if (pdfViewer != null)
        {
            // Load the PDF file in the WebBrowser control
            pdfViewer.Navigate(filePath);
        }
    }
}