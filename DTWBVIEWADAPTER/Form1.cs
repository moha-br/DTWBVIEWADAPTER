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
            //data = new DataTable();
            //data.Columns.Add("Enc");
            //data.Columns.Add("ProdIDs");
            //data.Columns.Add("ANuser");
            //data.Columns.Add("3Duser");
            //data.Columns.Add("2Duser");
            //data.Columns.Add("IMuser");
            //data.Columns.Add("VLuser");
            //data.Columns.Add("AN_desc");
            //data.Columns.Add("Ref_Client");
            //data.Columns.Add("Ref_Suplyer");
            //data.Columns.Add("DV_Branch");
            //data.Columns.Add("Sector");
            //data.Columns.Add("ArtCODE");

            //data.Rows.Add("1", "Alice", "alice@example.com");
            //data.Rows.Add("2", "Bob", "bob@example.com");

            //dataGridView1.DataSource = data;
            string dbPath = @"C:\Users\vm1\Desktop\dt_sql.db";
            string query = "SELECT * FROM data_table_EncsTask";

            var dbHelper = new SQLiteHelper(dbPath);
            data = dbHelper.ExecuteSelectQuery(query);
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



       
    }
}



