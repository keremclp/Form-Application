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
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\Med Botan\\Desktop\\VisualProject.accdb");
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
                addNew.SubItems.Add(read["ID"].ToString());
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

        private void ShowTakenBooks()
        {
            listView1.Items.Clear();
            try
            {

                conn.Open();

                OleDbCommand accessCommand = new OleDbCommand();
                accessCommand.Connection = conn;

                // Select borrowed books along with client details
                accessCommand.CommandText = "SELECT bb.*, b.BookName, c.ClientName, c.TC " +
                             "FROM (BorrowedBooks bb " +
                             "INNER JOIN Book b ON bb.BookID = b.BookID) " +
                             "INNER JOIN ClientProfile c ON bb.ClientID = c.ClientID";

                OleDbDataReader read = accessCommand.ExecuteReader();

                while (read.Read())
                {

                    ListViewItem addNew = new ListViewItem();
                    addNew.SubItems.Add(read["BookID"].ToString());
                    addNew.SubItems.Add(read["BookName"].ToString());
                    addNew.SubItems.Add(read["TC"].ToString());
                    addNew.SubItems.Add(read["ClientName"].ToString());
                    addNew.SubItems.Add(read["BorrowDate"].ToString());

                    // Add the rest of the columns as needed

                    listView2.Items.Add(addNew);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private int GetBookIDById(string id)
        {
            int bookID = -1; // Default value indicating not found
            string sqlText = "SELECT [BookID] FROM [Book] WHERE [ID] = @ID";
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(sqlText, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out bookID))
                    {
                        // Successfully parsed as an integer
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return bookID;
        }

        private int GetClientIDById(string tc)
        {
            int bookID = -1;
            string sqlText = "SELECT [ClientID] FROM [ClientProfile] WHERE [TC] = @TC";
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(sqlText, conn))
                {
                    cmd.Parameters.AddWithValue("@TC", tc);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out bookID))
                    {
                        // Successfully parsed as an integer
                    }
                    if(bookID  == -1)
                    {
                        MessageBox.Show("Hahooo aq");
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);

            }
            finally
            {
                conn.Close();
            }
            return bookID;
        }

        private void DeleteBorrowedBooks(string tc, int bookID, int clientID)
        {
            try
            {
                conn.Open();

                string sqlText = "DELETE FROM [BorrowedBooks] WHERE [ID] = @ID AND [BookID] = @BookID AND [ClientID] = @ClientID";

                using (OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn))
                {
                    AccessCommand.Parameters.AddWithValue("@ID", tc);
                    AccessCommand.Parameters.AddWithValue("@BookID", bookID);
                    AccessCommand.Parameters.AddWithValue("@ClientID", clientID);

                    AccessCommand.ExecuteNonQuery();

                    MessageBox.Show("Book deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void ReturnBook(string id)
        {
            // Update the status field
            try
            {
                conn.Open();
                string sqlText = "UPDATE [Book] SET [Status] = false WHERE [ID] = @ID";

                using (OleDbCommand cmd = new OleDbCommand(sqlText, conn))
                {
                    // Use parameters to avoid SQL injection
                    cmd.Parameters.AddWithValue("@ID", id);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book status updated successfully");
                    }
                    else
                    {
                        MessageBox.Show("Book not found or status update failed");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void AddBook() 
        {
            conn.Open();

            try
            {
                string sqlText = "INSERT INTO [Book] ([ID], [BookName], [Author], [Type], [PublicationYear], [PageNumber], [Status], [ShelfName], [ShelfNumber]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

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

                string sqlText = "DELETE FROM [Book] WHERE [ID] = @BookID";

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


            string selectQuery = "SELECT [BookName], [Author], [Type], [PublicationYear], [PageNumber], [Status], [ShelfName], [ShelfNumber] FROM [Book] WHERE [ID] = @ID";
            using (OleDbCommand selectCommand = new OleDbCommand(selectQuery, conn))
            {
                selectCommand.Parameters.AddWithValue("@ID", bookID);
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
                        MessageBox.Show("Book information not found for ID " + bookID);
                        return;
                    }
                }
            }
            UpdateForm updateForm = new UpdateForm(bookID, fetchedBookName, fetchedAuthor, fetchedType, fetchedPublicationYear, fetchedPageNumber, fetchedStatus, fetchedShelfName, fetchedShelfNumber);
            this.Hide();
            updateForm.ShowDialog();
        }

        



 
        private void button2_Click(object sender, EventArgs e)
        {
            ShowBookList();
        }

    
         private void pictureBox6_Click(object sender, EventArgs e)
        {
            AddBook();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DeleteBook();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowTakenBooks();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string id = textBox10.Text.ToString();
            string tc = textBox11.Text.ToString();

            int bookID = GetBookIDById(id);
            int clientID = GetClientIDById(tc);

            try
            {
                ReturnBook(id);
                DeleteBorrowedBooks(tc, bookID, clientID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TakenBookChart takenBookChart = new TakenBookChart();
            takenBookChart.Show();
        }
    }
}
