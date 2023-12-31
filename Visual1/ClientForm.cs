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
        public ClientForm()
        {
            InitializeComponent();
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
    }
}
