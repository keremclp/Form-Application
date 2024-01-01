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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Visual1
{
    public partial class BorrowForm : Form
    {
        public BorrowForm()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\ThinkPad\\Documents\\VisualProject.accdb");
        private int GetBookIDById(string id)
        {
            int bookID = -1; // Default value indicating not found
            string sqlText = "SELECT [BookID] FROM [Book] WHERE [ID] = @ID";

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
            conn.Close();
            return bookID;
        }
        private int GetClientIDById(string tc)
        {
            int bookID = -1; // Default value indicating not found
            string sqlText = "SELECT [ClientID] FROM [ClientProfile] WHERE [TC] = @TC";

            using (OleDbCommand cmd = new OleDbCommand(sqlText, conn))
            {
                cmd.Parameters.AddWithValue("@TC", tc);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out bookID))
                {
                    // Successfully parsed as an integer
                }
            }
            conn.Close();
            return bookID;
        }


        private void BorrowBook(string id)
        {
            conn.Open();
            // Assuming your status field is of boolean type in the database
            string sqlText = "UPDATE [Book] SET [Status] = true WHERE [ID] = @ID";
            try
            {
                
                using (OleDbCommand cmd = new OleDbCommand(sqlText, conn))
                {
                    // Use parameters to avoid SQL injection
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
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

        
        private void button1_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text.ToString();
            string tc = textBox2.Text.ToString();   
            // BorrowBook(id);
            int bookID = GetBookIDById(id);
            int clientID = GetClientIDById(tc);

            


            MessageBox.Show("book:"+bookID.ToString());
            MessageBox.Show("client:"+clientID.ToString());


        }

    }
}
