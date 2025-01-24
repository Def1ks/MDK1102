using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace БазаДанныхМДК1102
{
    public partial class Form2 : Form
    {
        public string UserRole { get; set; }
        private int startIndex = 0;

        public Form2(string role)
        {
            InitializeComponent();
            UserRole = role;
            AdjustControlsByRole();
            LoadProductsData();
        }

        private void AdjustControlsByRole()
        {
            if (UserRole == "admin")
            {
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                button1.Enabled = true;
            }
            else
            {
                buttonPrev.Enabled = true;
                buttonNext.Enabled = true;
                button1.Enabled = false;
            }
        }
        private void LoadProductsData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Model_DB"].ConnectionString;

            string query = $"SELECT title, articlenumber, mincostforagent, image, id FROM product ORDER BY id OFFSET {startIndex} ROWS FETCH NEXT 4 ROWS ONLY;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int productIndex = 1;
                            while (reader.Read())
                            {
                                SetProductLabels(productIndex, reader, connection);
                                productIndex++;
                            }
                            if (productIndex <= 1)
                            {
                                ClearLabels();
                                MessageBox.Show("Товары не найдены!");
                            }
                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка подключения или запроса: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        private void ClearLabels()
        {
            for (int i = 1; i <= 4; i++)
            {
                Label titleLabel = (Label)this.Controls.Find($"label{i}_2", true)[0];
                Label priceLabel = (Label)this.Controls.Find($"label{i}_3", true)[0];
                Label descriptionLabel = (Label)this.Controls.Find($"label{i}_4", true)[0];
                Label descriptionLabel5 = (Label)this.Controls.Find($"label{i}_5", true)[0];

                PictureBox pictureBox = (PictureBox)this.Controls.Find($"pictureBox{i}", true)[0];

                if (titleLabel != null && priceLabel != null && descriptionLabel != null && descriptionLabel5 != null)
                {
                    titleLabel.Text = "";
                    priceLabel.Text = "";
                    descriptionLabel.Text = "";
                    descriptionLabel5.Text = "";
                }

                if (pictureBox != null)
                {
                    pictureBox.Image = null;
                }
            }
        }
        private void SetProductLabels(int productIndex, SqlDataReader productReader, SqlConnection connection)
        {
            Label titleLabel = (Label)this.Controls.Find($"label{productIndex}_2", true)[0];
            Label priceLabel = (Label)this.Controls.Find($"label{productIndex}_3", true)[0];
            Label descriptionLabel = (Label)this.Controls.Find($"label{productIndex}_4", true)[0];
            Label descriptionLabel5 = (Label)this.Controls.Find($"label{productIndex}_5", true)[0];
            PictureBox pictureBox = (PictureBox)this.Controls.Find($"pictureBox{productIndex}", true)[0];

            if (titleLabel != null && priceLabel != null && descriptionLabel != null && descriptionLabel5 != null)
            {
                string materialDescription = GetMaterialDescription(Convert.ToInt32(productReader["id"]), connection);

                titleLabel.Text = $"Название: {productReader["title"].ToString()}";
                priceLabel.Text = $"Артикул: {productReader["articlenumber"].ToString()}";
                descriptionLabel.Text = $"Минимальная стоимость: {productReader["mincostforagent"].ToString()}";
                descriptionLabel5.Text = $"Материалы: {materialDescription}";
            }
            if (pictureBox != null)
            {
                LoadImage(pictureBox, productReader["image"]?.ToString());
            }

        }
        private void LoadImage(PictureBox pictureBox, string imagePath)
        {
            // Проверяем, является ли путь к изображению корректным
            if (string.IsNullOrEmpty(imagePath) ||
                 imagePath.Trim().ToLower() == "нет" ||
                 imagePath.Trim().ToLower() == "отсутствует" ||
                 imagePath.Trim().ToLower() == "не указано")
            {
                LoadDefaultImage(pictureBox); //Загружаем картинку по умолчанию
                return;
            }

            string basePath = Path.GetDirectoryName(Application.ExecutablePath); // Получаем путь до исполняемого файла
            string fullImagePath = Path.Combine(basePath, imagePath.TrimStart('\\')); // Получаем полный путь к картинке

            if (File.Exists(fullImagePath))
            {
                try
                {
                    pictureBox.Image?.Dispose(); //Очищаем старое изображение
                    pictureBox.Image = Image.FromFile(fullImagePath); // Загружаем новое изображение
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom; //Масштабируем изображение
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения {fullImagePath}: {ex.Message}");
                    LoadDefaultImage(pictureBox); // загружаем картинку по умолчанию если не получилось загрузить
                }
            }
            else
            {
                LoadDefaultImage(pictureBox); // загружаем картинку по умолчанию если не удалось найти
            }
        }

        private void LoadDefaultImage(PictureBox pictureBox)
        {
            string defaultImagePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "picture.png"); //путь к файлу по умолчанию
            if (File.Exists(defaultImagePath))
            {
                try
                {
                    pictureBox.Image?.Dispose();//Очищаем старое изображение
                    pictureBox.Image = Image.FromFile(defaultImagePath);
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки дефолтного изображения {defaultImagePath}: {ex.Message}");
                    pictureBox.Image = null; // удаляем картинку если не удалось загрузить
                }
            }
            else
            {
                MessageBox.Show("Дефолтное изображение не найдено.");
                pictureBox.Image = null;// удаляем картинку если не удалось загрузить
            }
        }

        private string GetMaterialDescription(int productId, SqlConnection connection)
        {
            string description = "";
            string query = $"SELECT description FROM material WHERE id = {productId}";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            description = reader["description"].ToString();
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка получения description из таблицы material: {ex.Message}");
                    description = "Описание не найдено";
                }
            }
            return description;
        }
        private void buttonPrev_Click_1(object sender, EventArgs e)
        {
            if (startIndex > 0)
            {
                startIndex -= 4;
                LoadProductsData();
            }
        }
        private void buttonNext_Click_1(object sender, EventArgs e)
        {
            startIndex += 4;
            LoadProductsData();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }
    }
}
