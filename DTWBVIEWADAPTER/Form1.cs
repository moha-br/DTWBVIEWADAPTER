using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DTWBVIEWADAPTER
{
    public partial class Form1 : Form
    {
        private DataTable data;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            data = new DataTable();
            data.Columns.Add("ID");
            data.Columns.Add("Name");
            data.Columns.Add("Email");

            data.Rows.Add("1", "Alice", "alice@example.com");
            data.Rows.Add("2", "Bob", "bob@example.com");

            dataGridView1.DataSource = data;
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.html");

            if (File.Exists(outputPath))
            {
                await webView21.EnsureCoreWebView2Async(null);
                webView21.CoreWebView2.Navigate($"file:///{outputPath.Replace("\\", "/")}");
            }
            else
            {
                MessageBox.Show("HTML file not found: " + outputPath);
            }
        }


        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            string html = CLS_DatatableToHTML.GenerateHTMLFromDataTable(data, "Card Viewer");
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.html");

            File.WriteAllText(outputPath, html);

            // Ensure WebView2 is ready
            await webView21.EnsureCoreWebView2Async(null);

            // Navigate using file:// protocol
            string uri = $"file:///{outputPath.Replace("\\", "/")}";
            webView21.CoreWebView2.Navigate(uri);
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "HTML files (*.html)|*.html",
                FileName = "cards.html"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string html = CLS_DatatableToHTML.GenerateHTMLFromDataTable(data, "Exported Cards");
                File.WriteAllText(sfd.FileName, html);
                MessageBox.Show("HTML exported successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

       
    }
}



