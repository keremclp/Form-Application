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
        public Form1()
        {
            InitializeComponent(); 
        }
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\ThinkPad\\Documents\\VisualProject.accdb");
        private void button1_Click_1(object sender, EventArgs e)
        {
            conn.Open();

            try
            {
                string sqlText = "INSERT INTO [Users] ([Username], [Password]) VALUES (?, ?)";

                using (OleDbCommand AccessCommand = new OleDbCommand(sqlText, conn))
                {
                    AccessCommand.Parameters.AddWithValue("@Username", textBox1.Text);
                    AccessCommand.Parameters.AddWithValue("@Password", textBox2.Text);

                    AccessCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Record inserted successfully!!!");
                MessageBox.Show("Record inserted successfully!!!");
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



    }
}
