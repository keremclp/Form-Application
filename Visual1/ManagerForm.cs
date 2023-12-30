using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Visual1
{
    public partial class ManagerForm : Form
    {
        public ManagerForm()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\ThinkPad\\Documents\\VisualProject.accdb");
        private void ShowBookList()
        {
            listView1.Items.Clear();
            conn.Open();

            OleDbCommand AccessCommand = new OleDbCommand();
            AccessCommand.Connection = conn;
            AccessCommand.CommandText = ("Select * from Book");
            OleDbDataReader read = AccessCommand.ExecuteReader();


            while (read.Read())
            {
                ListViewItem addNew = new ListViewItem();
                addNew.SubItems.Add(read["Book ID"].ToString());
                addNew.SubItems.Add(read["BookName"].ToString());
                addNew.SubItems.Add(read["Author"].ToString());
                addNew.SubItems.Add(read["Type"].ToString());
                addNew.SubItems.Add(read["PublicationYear"].ToString());
                addNew.SubItems.Add(read["PageNumber"].ToString());
                addNew.SubItems.Add(read["Status"].ToString());
                addNew.SubItems.Add(read["ShelfName"].ToString());
                addNew.SubItems.Add(read["ShelfNumber"].ToString());

                listView1.Items.Add(addNew);
            }
            conn.Close();

        }
        private void AddBook() 
        {
            conn.Open();

            try
            {
                string sqlText = "INSERT INTO [Book] ([Book ID], [BookName], [Author], [Type], [PublicationYear], [PageNumber], [Status], [ShelfName], [ShelfNumber]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

                using (OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn))
                {
                    AccessCommand.Parameters.AddWithValue("@BookID", textBox4.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@BookName", textBox1.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@Author", textBox2.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@Type", textBox3.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@PublicationYear", dateTimePicker1.Value.ToString("dd-MM-yyyy"));
                    AccessCommand.Parameters.AddWithValue("@PageNumber", int.Parse(textBox5.Text));
                    AccessCommand.Parameters.AddWithValue("@Status", false);
                    AccessCommand.Parameters.AddWithValue("@ShelfName", textBox8.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@ShelfNumber", textBox9.Text.ToString());


                    AccessCommand.ExecuteNonQuery();

                }

                MessageBox.Show("Record inserted successfully!!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Show the book list 
                tabControl1.SelectedTab = tabPage4; // Use Show() instead if you want a non-modal window
                conn.Close();
            }

        }

        private void DeleteBook()
        {
            conn.Open();

            try
            {
                string bookID = textBox6.Text.Trim();

                if (string.IsNullOrEmpty(bookID))
                {
                    MessageBox.Show("Please enter a BookID to delete.");
                    return;
                }

                string sqlText = "DELETE FROM [Book] WHERE [Book ID] = @BookID";

                using (OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn))
                {
                    AccessCommand.Parameters.AddWithValue("@BookID", bookID);
                    int rowsAffected = AccessCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified BookID.");
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
                tabControl1.SelectedTab = tabPage4; // Use Show() instead if you want a non-modal window
                conn.Close();
            }
        }


        private void UpdateForm()
        {
            conn.Open();
            string bookID = textBox7.Text.Trim();

            if (string.IsNullOrEmpty(bookID))
            {
                MessageBox.Show("Please enter a BookID to delete.");
                return;
            }

            string fetchedBookName = "";
            string fetchedAuthor = "";
            string fetchedType = "";
            DateTime fetchedPublicationYear = DateTime.MinValue;
            int fetchedPageNumber = 0;
            bool fetchedStatus = false;
            string fetchedShelfName = "";
            int fetchedShelfNumber = 0;


            string selectQuery = "SELECT [BookName], [Author], [Type], [PublicationYear], [PageNumber], [Status], [ShelfName], [ShelfNumber] FROM [Book] WHERE [Book ID] = @BookID";
            using (OleDbCommand selectCommand = new OleDbCommand(selectQuery, conn))
            {
                selectCommand.Parameters.AddWithValue("@BookID", bookID);
                // fetching requering data and sending UpdateForm
                using (OleDbDataReader selectReader = selectCommand.ExecuteReader())
                {
                    if (selectReader.Read())
                    {
                        fetchedBookName = selectReader["BookName"].ToString();
                        fetchedAuthor = selectReader["Author"].ToString();
                        fetchedType = selectReader["Type"].ToString();
                        fetchedPublicationYear = DateTime.Parse(selectReader["PublicationYear"].ToString());
                        fetchedPageNumber = int.Parse(selectReader["PageNumber"].ToString());
                        fetchedStatus = bool.Parse(selectReader["Status"].ToString());
                        fetchedShelfName = selectReader["ShelfName"].ToString();
                        fetchedShelfNumber = int.Parse(selectReader["ShelfNumber"].ToString());
                    }
                    else
                    {
                        // Handle the case where the book information is not found
                        MessageBox.Show("Book information not found for Book ID " + bookID);
                        return;
                    }
                }
            }
            UpdateForm updateForm = new UpdateForm(bookID, fetchedBookName, fetchedAuthor, fetchedType, fetchedPublicationYear, fetchedPageNumber, fetchedStatus, fetchedShelfName, fetchedShelfNumber);
            this.Hide();
            updateForm.ShowDialog();
        }

        private void ShowTakenBooks()
        {
            listView1.Items.Clear();
            conn.Open();

            OleDbCommand AccessCommand = new OleDbCommand();
            AccessCommand.Connection = conn;

            // Select only the books where the Status is true
            AccessCommand.CommandText = "SELECT * FROM Book WHERE Status = true";

            OleDbDataReader read = AccessCommand.ExecuteReader();

            while (read.Read())
            {
                

                ListViewItem addNew = new ListViewItem();
                addNew.SubItems.Add(read["Book ID"].ToString());
                addNew.SubItems.Add(read["BookName"].ToString());
                addNew.SubItems.Add(read["Author"].ToString());
                addNew.SubItems.Add(read["Type"].ToString());
                addNew.SubItems.Add(read["PublicationYear"].ToString());
                addNew.SubItems.Add(read["PageNumber"].ToString());
                addNew.SubItems.Add(read["Status"].ToString());
                addNew.SubItems.Add(read["ShelfName"].ToString());
                addNew.SubItems.Add(read["ShelfNumber"].ToString());

                listView1.Items.Add(addNew);
            }

            conn.Close();

        }



        private void button1_Click(object sender, EventArgs e)
        {
            AddBook();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowBookList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteBook();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            ShowTakenBooks();

        }
        
    }
}
