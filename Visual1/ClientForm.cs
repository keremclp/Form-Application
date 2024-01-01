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
    public partial class ClientForm : Form
    {
        private string username;
        private string password;
        public ClientForm(string username, string password)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\ThinkPad\\Documents\\VisualProject.accdb");

        private void searchBookName(string searchTerm)
        {
            using (OleDbCommand command = new OleDbCommand())
            {
                command.Connection = conn;

                // Use parameterized query to avoid SQL injection
                command.CommandText = "SELECT * FROM Book WHERE BookName LIKE @searchTerm";
                command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataTable.Columns.Remove("BookID");

                    // Update the DataGridView with the search results
                    dataGridView1.DataSource = dataTable;
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text;
            searchBookName(searchTerm);
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            searchBookName("");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BorrowForm borrowForm = new BorrowForm();
            borrowForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditForm editForm = new EditForm(); 
            editForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AuthorChart authorChart = new AuthorChart();
            authorChart.Show();
        }
    }
}
