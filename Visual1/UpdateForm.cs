using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace Visual1
{
    public partial class UpdateForm : Form
    {
        private string _bookID;
        private string _bookName;
        private string _author;
        private string _type;
        private DateTime _publicationYear;
        private int _pageNumber;
        private bool _status;
        public UpdateForm(string bookID, string bookName, string author, string type, DateTime publicationYear, int pageNumber, bool status)
        {
            InitializeComponent();

            _bookID = bookID;
            _bookName = bookName;
            _author = author;
            _type = type;
            _publicationYear = publicationYear;
            _pageNumber = pageNumber;
            _status = status;

            // Populate the text boxes with the book information
            textBox1.Text = _bookID;
            textBox2.Text = _bookName;
            textBox3.Text = _author;
            textBox4.Text = _type;
            dateTimePicker1.Value = _publicationYear;
            textBox6.Text = _pageNumber.ToString();
            checkBox1.Checked = _status;
        }
        public UpdateForm()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\ThinkPad\\Documents\\VisualProject.accdb");
        private void UpdateBook()
        {
            conn.Open();

            try
            {
                string sqlText = "UPDATE [Book] SET [BookName] = ?, [Author] = ?, [Type] = ?, [PublicationYear] = ?, [PageNumber] = ?, [Status] = ? WHERE [Book ID] = ?";

                using (OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn))
                {
                    AccessCommand.Parameters.AddWithValue("@BookName", textBox1.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@Author", textBox2.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@Type", textBox3.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@PublicationYear", dateTimePicker1.Value.ToString("dd-MM-yyyy"));
                    AccessCommand.Parameters.AddWithValue("@PageNumber", int.Parse(textBox6.Text));
                    AccessCommand.Parameters.AddWithValue("@Status", false);
                    AccessCommand.Parameters.AddWithValue("@BookID", textBox4.Text.ToString());

                    int rowsAffected = AccessCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!!!");
                    }
                    else
                    {
                        MessageBox.Show("No matching record found for Book ID " + textBox4.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Show the book list 
                //tabControl1.SelectedTab = tabPage4; // Use Show() instead if you want a non-modal window
                conn.Close();
            }
        }





    }
}
