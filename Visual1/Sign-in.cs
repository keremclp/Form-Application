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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Visual1
{
    public partial class Sign_in : Form
    {
        public Sign_in()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\ThinkPad\\Documents\\VisualProject.accdb");

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                string sqlText = "SELECT [Roles] FROM [Users] WHERE [Username] = ? AND [Password] = ?";
                using (OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn))
                {
                    AccessCommand.Parameters.AddWithValue("@Username", textBox1.Text.ToString());
                    AccessCommand.Parameters.AddWithValue("@Password", textBox2.Text.ToString());

                    object result = AccessCommand.ExecuteScalar();

                    if (result != null) // User found
                    {
                        string role = result.ToString();

                        MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Hide the current form (sign-in form)
                        this.Hide();

                        // Open the appropriate form based on the user's role
                        if (role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                        {
                            ManagerForm managerForm = new ManagerForm();
                            managerForm.ShowDialog(); // Use Show() instead if you want a non-modal window
                        }
                        else if (role.Equals("Client", StringComparison.OrdinalIgnoreCase))
                        {
                            ClientForm clientForm = new ClientForm();
                            clientForm.ShowDialog(); // Use Show() instead if you want a non-modal window
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 signup = new Form1();
            signup.ShowDialog();
        }
    }
}
