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
    public partial class AuthorChart : Form
    {
        private Dictionary<Point, SizeF> existingLabels = new Dictionary<Point, SizeF>();

        public AuthorChart()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\Med Botan\\Desktop\\VisualProject.accdb");

        private void AuthorChart_Paint(object sender, PaintEventArgs e)
        {
            DrawPieChart(e.Graphics);
        }
        private Random random = new Random(); // Instantiate Random class once

        private void DrawPieChart(Graphics g)
        {
            // Fetch data from the database
            List<AuthorBookCount> authorBookCounts = GetAuthorBookCounts();

            // Set up variables for drawing
            int totalBooks = authorBookCounts.Sum(item => item.BookCount);
            int radius = Math.Min(ClientSize.Width, ClientSize.Height) / 2 - 20;
            Point center = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
            float startAngle = 0;

            // Draw pie slices and labels with different colors for each author
            foreach (var authorCount in authorBookCounts)
            {
                float sweepAngle = 360f * authorCount.BookCount / totalBooks;

                // Draw pie slice with a unique color for each author
                using (Brush brush = new SolidBrush(GetRandomColor()))
                {
                    g.FillPie(brush, center.X - radius, center.Y - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                }

                // Draw label
                double labelAngle = (startAngle + sweepAngle / 2) * Math.PI / 180;
                float labelRadius = radius + 30; // Adjust the label distance from the center

                // Calculate label position to avoid overlap
                Point labelPosition = CalculateLabelPosition(center, labelRadius, labelAngle, g, authorCount.Author);

                using (Font font = new Font(FontFamily.GenericSansSerif, 10))
                {
                    g.DrawString($"{authorCount.Author} ({((double)authorCount.BookCount / totalBooks * 100):F1}%)",
                        font, Brushes.Black, labelPosition);
                }

                startAngle += sweepAngle;
            }
        }

        private Point CalculateLabelPosition(Point center, float labelRadius, double labelAngle, Graphics g, string label)
        {
            // Calculate the initial label position
            Point labelPosition = new Point(
                center.X + (int)(labelRadius * Math.Cos(labelAngle)),
                center.Y + (int)(labelRadius * Math.Sin(labelAngle))
            );

            // Check for label overlap and adjust position if necessary
            var labelSize = g.MeasureString(label, new Font(FontFamily.GenericSansSerif, 10));
            var overlapThreshold = 5; // Adjust as needed

            while (IsLabelOverlap(labelPosition, labelSize, g))
            {
                labelRadius += overlapThreshold; // Increase the radius to avoid overlap
                labelPosition = new Point(
                    center.X + (int)(labelRadius * Math.Cos(labelAngle)),
                    center.Y + (int)(labelRadius * Math.Sin(labelAngle))
                );
            }

            return labelPosition;
        }

        private bool IsLabelOverlap(Point labelPosition, SizeF labelSize, Graphics g)
        {
            // Check if the label rectangle overlaps with any existing labels
            var labelRectangle = new RectangleF(labelPosition, labelSize);

            foreach (var existingLabel in existingLabels)
            {
                var existingLabelRectangle = new RectangleF(existingLabel.Key, existingLabel.Value);
                if (labelRectangle.IntersectsWith(existingLabelRectangle))
                {
                    return true; // Overlapping labels
                }
            }

            // Add the current label position and size to the existing labels
            existingLabels.Add(labelPosition, labelSize);
            return false; // No overlap
        }


        private Color GetRandomColor()
        {
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }
        private List<AuthorBookCount> GetAuthorBookCounts()
        {
            List<AuthorBookCount> result = new List<AuthorBookCount>();

            conn.Open();

            string query = "SELECT Author, COUNT(*) AS BookCount FROM Book GROUP BY Author";
            using (OleDbCommand command = new OleDbCommand(query, conn))
            using (OleDbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string author = reader["Author"].ToString();
                    int bookCount = Convert.ToInt32(reader["BookCount"]);
                    result.Add(new AuthorBookCount(author, bookCount));
                }
            }
            conn.Close();
            return result;
        }
    }
}


public class AuthorBookCount
{
    public string Author { get; }
    public int BookCount { get; }

    public AuthorBookCount(string author, int bookCount)
    {
        Author = author;
        BookCount = bookCount;
    }
}
