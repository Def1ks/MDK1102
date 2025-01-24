using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Новый комментарий
namespace БазаДанныхМДК1102
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string userRole;
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string connectionString = ConfigurationManager.ConnectionStrings["Model_DB"].ConnectionString;
            string query = "SELECT role FROM users WHERE login = @login AND password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@login", username);
                command.Parameters.AddWithValue("@password", password);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        userRole = result.ToString();
                        label3.Text = $"Вход выполнен ({userRole})";
                        OpenForm2(userRole); 
                    }
                    else
                    {
                        label3.Text = "Неверный логин или пароль";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void OpenForm2(string role)
        {
            Form2 form2 = new Form2(role);
            this.Hide();
            form2.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            this.Hide();
            form4.ShowDialog();
            this.Show();
        }
    }
}
