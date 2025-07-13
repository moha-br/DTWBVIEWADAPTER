using System;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

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
            // Load data from SQLite
            string dbPath = @"C:\Users\vm1\Desktop\dt_sql.db";
            string query = "SELECT * FROM data_table_EncsTask";
            var dbHelper = new SQLiteHelper(dbPath);
            data = dbHelper.ExecuteSelectQuery(query);
            dataGridView1.DataSource = data;

            // Generate HTML from data
            string html = CLS_DatatableToHTML.GenerateHTMLFromDataTable(data, "Card Viewer");
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.html");
            File.WriteAllText(outputPath, html);

            // Initialize WebView2
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.WebMessageReceived += WebMessageReceived;

            // Load the generated HTML
            string uri = $"file:///{outputPath.Replace("\\", "/")}";
            webView21.CoreWebView2.Navigate(uri);
        }

        private void WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                string json = e.TryGetWebMessageAsString();
               
                RootObject obj = JsonSerializer.Deserialize<RootObject>(json);
                if (obj?.type != "card_selected") return;
                var selectedCard = obj?.carddata; //JsonSerializer.Deserialize<carddata>(json);
                if (selectedCard == null) return;

                DataTable selectedTable = new DataTable();
                selectedTable.Columns.Add("id");
                selectedTable.Columns.Add("enc");
                selectedTable.Columns.Add("linen");
                selectedTable.Columns.Add("prodids");
                selectedTable.Columns.Add("sector");

                DataRow row = selectedTable.NewRow();
                row["id"] = selectedCard.id;
                row["enc"] = selectedCard.enc;
                row["linen"] = selectedCard.linen;
                row["prodids"] = selectedCard.prodids;
                row["sector"] = selectedCard.sector;
                selectedTable.Rows.Add(row);

                // Show in secondary DataGridView or message
                dataGridView2.DataSource = selectedTable;
                //MessageBox.Show($"Selected Card:\nID: {selectedCard.id}\nENC: {selectedCard.enc}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving message: " + ex.Message);
            }
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("No data loaded.");
                return;
            }

            string html = CLS_DatatableToHTML.GenerateHTMLFromDataTable(data, "Card Viewer");
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.html");
            File.WriteAllText(outputPath, html);

            await webView21.EnsureCoreWebView2Async(null);
            string uri = $"file:///{outputPath.Replace("\\", "/")}";
            webView21.CoreWebView2.Navigate(uri);
        }
    }

    public class carddata
    {
        public string id { get; set; }
        public string enc { get; set; }
        public string linen { get; set; }
        public string prodids { get; set; }
        public string sector { get; set; }
    }
    public class RootObject
    {
        public string type { get; set; }
        public carddata carddata { get; set; }
    }

}
