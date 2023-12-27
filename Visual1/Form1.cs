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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Visual1
{
    public partial class Form1 : Form
    {
        private static Sign_in signInFormInstance;
        public Form1()
        {
            InitializeComponent();
            if (signInFormInstance == null)
            {
                signInFormInstance = new Sign_in();
            }
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\ThinkPad\\Documents\\VisualProject.accdb");
        private void button1_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            int rowsAffected = 0;
            try
            {
                string sqlText = "INSERT INTO [Users] ([Username], [Password], [Roles]) VALUES (?, ?, ?)";

                using (OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn))
                {
                    AccessCommand.Parameters.AddWithValue("@Username", textBox1.Text);
                    AccessCommand.Parameters.AddWithValue("@Password", textBox2.Text);
                    AccessCommand.Parameters.AddWithValue("@Roles", comboBox1.SelectedItem.ToString());

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

                signInFormInstance.ShowDialog(); // Use Show() instead if you want a non-modal window
            }
            else
            {
                MessageBox.Show("Signup failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            signInFormInstance.Show();
        }
    }
}
