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
    public partial class EditForm : Form
    {
        public EditForm()
        {
            InitializeComponent();
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\Med Botan\\Desktop\\VisualProject.accdb");

        private void EditProfile()
        {
            conn.Open();

            try
            {
                string sqlText = "INSERT INTO [ClientProfile] ([TC], [ClientName]) VALUES (?, ?)";

                // Initialize AccessCommand within the button1_Click method
                OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn);

                AccessCommand.Parameters.AddWithValue("@TC", textBox1.Text.ToString());
                AccessCommand.Parameters.AddWithValue("@Name", textBox2.Text.ToString());



                AccessCommand.ExecuteNonQuery();

                MessageBox.Show("You edited");
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



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide(); // Close the current form (EditForm)
            ClientForm clientForm = new ClientForm();
            clientForm.Show(); // Show the ClientForm
        }

        
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            EditProfile();
        }
    }
}
