using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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

                    AccessCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Record inserted successfully!!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            // Show the book list 
            tabControl1.SelectedTab = tabPage4; // Use Show() instead if you want a non-modal window
        }

        private void show()
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

                listView1.Items.Add(addNew);
            }
            conn.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            show();
        }

        
    }
}
