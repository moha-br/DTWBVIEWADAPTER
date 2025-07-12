using System;
using System.Data;
using System.IO;
using System.Text;

namespace DTWBVIEWADAPTER
{
    internal class CLS_DatatableToHTML
    {
        public static string GenerateHTMLFromDataTable(DataTable table, string pageTitle = "Card List")
        {
            if (table == null || table.Rows.Count == 0)
                return "<div class='alert alert-warning'>No data to display.</div>";

            string baseTemplate = LoadTemplate("Html/BaseTemplate.html");
            string cardTemplate = LoadTemplate("Html/CardTemplate.html");

            StringBuilder allCards = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {
                string currentCard = cardTemplate;
                StringBuilder fields = new StringBuilder();

                foreach (DataColumn col in table.Columns)
                {
                    string key = col.ColumnName;
                    string value = row[col]?.ToString() ?? "";
                    fields.AppendLine($"<p><strong>{key}:</strong> {System.Net.WebUtility.HtmlEncode(value)}</p>");
                }

                currentCard = currentCard.Replace("{{FIELDS}}", fields.ToString());
                allCards.AppendLine(currentCard);
            }

            baseTemplate = baseTemplate.Replace("{{TITLE}}", pageTitle);
            baseTemplate = baseTemplate.Replace("{{CARDS}}", allCards.ToString());

            return baseTemplate;
        }

        private static string LoadTemplate(string relativePath)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", relativePath);
            return File.ReadAllText(fullPath);
        }

        public static void ExportToHTMLFile(string html, string outputPath = "output.html")
        {
            File.WriteAllText(outputPath, html, Encoding.UTF8);
        }
    }
}
