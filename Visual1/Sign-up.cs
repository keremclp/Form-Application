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
    public partial class Sign_up : Form
    {
        private static Form1 signInFormInstance;
        public Sign_up()
        {
            InitializeComponent();
            signInFormInstance = new Form1();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\ThinkPad\\Documents\\VisualProject.accdb");
        private void button1_Click(object sender, EventArgs e)
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

                MessageBox.Show("You signed up, going Sign-in");
                this.Hide();    
                signInFormInstance.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            

        }

        
    }
}
