using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Visual1
{
    public partial class TakenBookChart : Form
    {
        private Dictionary<string, Color> clientColors = new Dictionary<string, Color>();
        private Random random = new Random();

        public TakenBookChart()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\Med Botan\\Desktop\\VisualProject.accdb");

        private void TakenBooksChart_Paint(object sender, PaintEventArgs e)
        {
            DrawPieChart(e.Graphics);
        }

        private void DrawPieChart(Graphics g)
        {
            // Fetch data from the database
            List<ClientBookCount> clientBookCounts = GetClientBookCounts();

            // Set up variables for drawing
            int totalBooks = clientBookCounts.Sum(item => item.BookCount);
            int radius = Math.Min(ClientSize.Width, ClientSize.Height) / 2 - 20;
            Point center = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
            float startAngle = 0;

            // Draw pie slices with different colors for each client
            foreach (var clientCount in clientBookCounts)
            {
                float sweepAngle = 360f * clientCount.BookCount / totalBooks;

                // Generate or retrieve color for the client
                Color clientColor = GetClientColor(clientCount.ClientName);

                // Draw pie slice with the color for the client
                using (Brush brush = new SolidBrush(clientColor))
                {
                    g.FillPie(brush, center.X - radius, center.Y - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                }

                startAngle += sweepAngle;
            }

            // Draw legend
            int legendX = center.X + radius + 10; // Adjust the legend position
            int legendY = center.Y - totalBooks * 10 / 2; // Adjust the legend position

            for (int i = 0; i < clientBookCounts.Count; i++)
            {
                Color legendColor = GetClientColor(clientBookCounts[i].ClientName);
                string legendText = $"{clientBookCounts[i].ClientName} ({clientBookCounts[i].BookCount})";

                using (Brush brush = new SolidBrush(legendColor))
                {
                    g.FillRectangle(brush, legendX, legendY + i * 20, 10, 10);
                }

                using (Font font = new Font(FontFamily.GenericSansSerif, 10))
                {
                    g.DrawString(legendText, font, Brushes.Black, legendX + 15, legendY + i * 20);
                }
            }
        }

        private List<ClientBookCount> GetClientBookCounts()
        {
            List<ClientBookCount> result = new List<ClientBookCount>();

            conn.Open();

            string query = "SELECT cp.ClientName AS ClientName, COUNT(*) AS BookCount " +
               "FROM (BorrowedBooks bb " +
               "INNER JOIN ClientProfile cp ON bb.ClientID = cp.ClientID) " +
               "INNER JOIN Book b ON bb.BookID = b.BookID " +
               "GROUP BY cp.ClientName";



            using (OleDbCommand command = new OleDbCommand(query, conn))
            using (OleDbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string clientName = reader["ClientName"].ToString();
                    int bookCount = Convert.ToInt32(reader["BookCount"]);
                    result.Add(new ClientBookCount(clientName, bookCount));
                }
            }

            return result;
        }


        private Color GetClientColor(string clientName)
        {
            if (!clientColors.ContainsKey(clientName))
            {
                // If the color for the client is not already assigned, generate a new color
                clientColors[clientName] = RandomColor();
            }

            return clientColors[clientName];
        }

        private Color RandomColor()
        {
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }

        
    }
}

public class ClientBookCount
{
    public string ClientName { get; }
    public int BookCount { get; }

    public ClientBookCount(string clientName, int bookCount)
    {
        ClientName = clientName;
        BookCount = bookCount;
    }
}