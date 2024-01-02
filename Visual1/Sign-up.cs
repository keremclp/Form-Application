using System;
using System.Collections;
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
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\\Users\\Med Botan\\Desktop\\VisualProject.accdb");

        private ArrayList managerCodes = new ArrayList() { "Manager1", "Manager2", "Manager3", "Manager4", "Manager5" };

        private OleDbCommand AccessCommand;
        private TextBox textBoxManagerCode;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Add ManagerCode parameter only if the role is Manager
            if (comboBox1.SelectedItem.ToString() == "Manager")
            {
                textBoxManagerCode = new TextBox();
                textBoxManagerCode.Name = "textBoxManagerCode";
                textBoxManagerCode.Location = new System.Drawing.Point(222, 359);
                textBoxManagerCode.Size = new System.Drawing.Size(150, 50);
                // textBoxManagerCode.PasswordChar = '*'; // Optionally, make it a password field

                this.Controls.Add(textBoxManagerCode);

                Label codeLabel = new Label();
                codeLabel.AutoSize = true;
                codeLabel.BackColor = System.Drawing.Color.Transparent;
                codeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                codeLabel.ForeColor = System.Drawing.Color.Transparent;
                codeLabel.Text = "ENTER CODE:";
                codeLabel.Location = new System.Drawing.Point(93, 359);
                codeLabel.Size = new System.Drawing.Size(110, 20);
                codeLabel.Name = "labelCode";
                this.Controls.Add(codeLabel);
            }
            else
            {
                // No need to remove textBoxManagerCode here
                // Remove the dynamically created TextBox and Label if Client is selected
                Control textBoxManagerCode = this.Controls["textBoxManagerCode"];
                Control labelCode = this.Controls["labelCode"];

                if (textBoxManagerCode != null)
                    this.Controls.Remove(textBoxManagerCode);
                if (labelCode != null)
                    this.Controls.Remove(labelCode);
            }

        }

      

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            conn.Open();

            try
            {
                string sqlText = "INSERT INTO [Users] ([Username], [Password], [Roles], [ManagerCode]) VALUES (?, ?, ?, ?)";

                // Initialize AccessCommand within the button1_Click method
                AccessCommand = new OleDbCommand(sqlText, conn);

                AccessCommand.Parameters.AddWithValue("@Username", textBox1.Text.ToString());
                AccessCommand.Parameters.AddWithValue("@Password", textBox2.Text.ToString());
                AccessCommand.Parameters.AddWithValue("@Roles", comboBox1.SelectedItem.ToString());

                if (comboBox1.SelectedItem.ToString() == "Manager")
                {
                    if (!managerCodes.Contains(textBoxManagerCode.Text))
                    {
                        MessageBox.Show("Invalid Manager Code. Please enter a valid code.");
                        AccessCommand.Parameters.AddWithValue("@ManagerCode", DBNull.Value);
                        return; // Stop further processing
                    }
                    else
                    {
                        AccessCommand.Parameters.AddWithValue("@ManagerCode", textBoxManagerCode.Text.ToString());
                    }
                }
                else
                {
                    AccessCommand.Parameters.AddWithValue("@ManagerCode", DBNull.Value);
                }

                AccessCommand.ExecuteNonQuery();

                MessageBox.Show("You signed up, going Sign-in");
                this.Hide();
                signInFormInstance.Show();
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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }
    }
}
