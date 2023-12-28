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
    public partial class ManagerForm : Form
    {
        public ManagerForm()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\ThinkPad\\Documents\\VisualProject.accdb");

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            int rowsAffected = 0;
            try
            {
                string sqlText = "INSERT INTO [Book] ([BookName], [Author], [Type], [PublicationYear], [PageNumber], [Status]) VALUES (?, ?, ?, ?, ?, ?)";

                using (OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn))
                {
                    AccessCommand.Parameters.AddWithValue("@BookName", textBox1.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@Author", textBox2.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@Type", textBox3.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@PublicationYear", dateTimePicker1.Value.ToString("dd-MM-yyyy"));
                    AccessCommand.Parameters.AddWithValue("@PageNumber", int.Parse(textBox5.Text));
                    AccessCommand.Parameters.AddWithValue("@Status", false);

                    rowsAffected = AccessCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Record inserted successfully!!!");
                MessageBox.Show("Record inserted successfully!!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            if (rowsAffected > 0)
            {
                MessageBox.Show("Signup successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Hide the current form (signup form)
                this.Hide();

                // Show the sign-in form

                tabPage4.Show(); // Use Show() instead if you want a non-modal window
            }
            else
            {
                MessageBox.Show("Signup failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
