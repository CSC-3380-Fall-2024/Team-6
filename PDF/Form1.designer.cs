namespace PDFViewerApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnOpenPDF;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2PDF;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnOpenPDF = new System.Windows.Forms.Button();
            this.webView2PDF = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.webView2PDF)).BeginInit();
            this.SuspendLayout();

            // btnOpenPDF
            this.btnOpenPDF.Location = new System.Drawing.Point(12, 12);
            this.btnOpenPDF.Name = "btnOpenPDF";
            this.btnOpenPDF.Size = new System.Drawing.Size(100, 30);
            this.btnOpenPDF.TabIndex = 0;
            this.btnOpenPDF.Text = "Open PDF";
            this.btnOpenPDF.UseVisualStyleBackColor = true;
            this.btnOpenPDF.Click += new System.EventHandler(this.btnOpenPDF_Click);

            // webView2PDF
            this.webView2PDF.Location = new System.Drawing.Point(12, 50);
            this.webView2PDF.Name = "webView2PDF";
            this.webView2PDF.Size = new System.Drawing.Size(760, 400);
            this.webView2PDF.TabIndex = 1;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.webView2PDF);
            this.Controls.Add(this.btnOpenPDF);
            this.Name = "Form1";
            this.Text = "PDF Viewer App";
            ((System.ComponentModel.ISupportInitialize)(this.webView2PDF)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
