using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace БазаДанныхМДК1102
{
    public partial class Form3 : Form
    {

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_ИСП_31DataSet3.Product". При необходимости она может быть перемещена или удалена.
            this.productTableAdapter.Fill(this._ИСП_31DataSet3.Product);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем ID продукта из textBox1
            int productId;

            // Проверяем, является ли введённый текст числом
            if (int.TryParse(textBox1.Text, out productId))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Model_DB"].ConnectionString;
                string query = "SELECT title, articlenumber, Description, ProductionWorkshopNumber, MinCostForAgent FROM product WHERE id = @id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", productId);

                    try
                    {
                        connection.Open(); // Открываем соединение
                        using (SqlDataReader reader = command.ExecuteReader()) // Выполняем запрос и получаем результаты
                        {
                            if (reader.Read()) // Если есть данные
                            {
                                // Получаем значения и выводим их в соответствующие текстовые поля
                                textBox2.Text = reader["title"].ToString();
                                textBox3.Text = reader["articlenumber"].ToString();
                                textBox4.Text = reader["Description"].ToString();
                                textBox5.Text = reader["ProductionWorkshopNumber"].ToString();
                                textBox6.Text = reader["MinCostForAgent"].ToString();
                            }
                            else
                            {
                                // Если продукт не найден, уведомляем пользователя
                                MessageBox.Show("Продукт не найден.");
                                ClearTextBoxes();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Обработка исключений и уведомление пользователя об ошибке
                        MessageBox.Show($"Произошла ошибка: {ex.Message}");
                    }
                }
            }
            else
            {
                // Уведомление о некорректном вводе ID
                MessageBox.Show("Введите корректный ID продукта.");
                ClearTextBoxes();
            }
        }

        // Метод для очистки текстовых полей
        private void ClearTextBoxes()
        {
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Получаем ID продукта из textBox1
            int productId;

            // Проверяем, является ли введённый текст числом
            if (int.TryParse(textBox1.Text, out productId))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Model_DB"].ConnectionString;
                string query = "UPDATE product SET title = @title, articlenumber = @articlenumber, Description = @Description, ProductionWorkshopNumber = @ProductionWorkshopNumber, MinCostForAgent = @MinCostForAgent WHERE id = @id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@title", textBox2.Text);
                    command.Parameters.AddWithValue("@articlenumber", textBox3.Text);
                    command.Parameters.AddWithValue("@Description", textBox4.Text);
                    command.Parameters.AddWithValue("@ProductionWorkshopNumber", textBox5.Text);
                    command.Parameters.AddWithValue("@MinCostForAgent", textBox6.Text);
                    command.Parameters.AddWithValue("@id", productId);

                    try
                    {
                        connection.Open(); // Открываем соединение
                        int rowsAffected = command.ExecuteNonQuery(); // Выполняем запрос и получаем количество затронутых строк

                        if (rowsAffected > 0)
                        {
                            // Если обновление прошло успешно
                            MessageBox.Show("Данные успешно обновлены.");
                        }
                        else
                        {
                            // Если не было обновлено ни одной строки (например, ID не существует)
                            MessageBox.Show("Продукт с указанным ID не найден.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Обработка исключений и уведомление пользователя об ошибке
                        MessageBox.Show($"Произошла ошибка: {ex.Message}");
                    }
                }
            }
            else
            {
                // Уведомление о некорректном вводе ID
                MessageBox.Show("Введите корректный ID продукта.");
            }
        }
    }
}