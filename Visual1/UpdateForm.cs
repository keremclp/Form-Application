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
        private string _shelfName;
        private int _shelfNumber;
        public UpdateForm(string bookID, string bookName, string author, string type, DateTime publicationYear, int pageNumber, bool status, string shelfName, int shelfNumber)
        {
            InitializeComponent();

            _bookID = bookID;
            _bookName = bookName;
            _author = author;
            _type = type;
            _publicationYear = publicationYear;
            _pageNumber = pageNumber;
            _status = status;
            _shelfName = shelfName;
            _shelfNumber = shelfNumber;

            // Populate the text boxes with the book information
            textBox1.Text = _bookID;
            textBox2.Text = _bookName;
            textBox3.Text = _author;
            textBox4.Text = _type;
            dateTimePicker1.Value = _publicationYear;
            textBox6.Text = _pageNumber.ToString();
            checkBox1.Checked = _status;
            textBox5.Text = _shelfName;
            textBox7.Text = _shelfNumber.ToString();
        }
        public UpdateForm()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\Med Botan\\Desktop\\VisualProject.accdb");
        private void UpdateBook()
        {
            conn.Open();

            try
            {
                string sqlText = "UPDATE [Book] SET [BookName] = ?, [Author] = ?, [Type] = ?, [PublicationYear] = ?, [PageNumber] = ?, [Status] = ?, [ShelfName] = ?, [ShelfNumber] = ? WHERE [ID] = ?";

                using (OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn))
                {
                    AccessCommand.Parameters.AddWithValue("@BookName", textBox2.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@Author", textBox3.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@Type", textBox4.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@PublicationYear", dateTimePicker1.Value.ToString("dd-MM-yyyy"));
                    AccessCommand.Parameters.AddWithValue("@PageNumber", int.Parse(textBox6.Text));
                    AccessCommand.Parameters.AddWithValue("@Status", checkBox1.Checked);
                    AccessCommand.Parameters.AddWithValue("@ShelfName", textBox5.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@ShelfNumber", int.Parse(textBox7.Text));
                    AccessCommand.Parameters.AddWithValue("@BookID", this._bookID);
                    

                    int rowsAffected = AccessCommand.ExecuteNonQuery();
                    MessageBox.Show(this._bookID);
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!!!");
                    }
                    else
                    {
                        MessageBox.Show("No matching record found for ID ");
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            UpdateBook();
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the current form (UpdateForm)
            ManagerForm managerForm = new ManagerForm();
            managerForm.Show(); // Show the ManagerForm
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
