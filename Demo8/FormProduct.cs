using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo8
{
    public partial class FormProduct : Form
    {
        string userFio;
        string userRole;
        int selectedPanelId = -1;
        Panel selectedPanel = null;
        public FormProduct(string role, string fio)
        {
            this.Icon = Icon.FromHandle(new Bitmap(@"C:\Users\rusol\Desktop\БУ\Модуль 1\Прил_2_ОЗ_КОД 09.02.07-2-2026-М1\import\Icon.png").GetHicon());
            userFio = fio;
            userRole = role;
            InitializeComponent();
            btnBack.BackColor = Color.MediumSpringGreen;
            btnReset.BackColor = Color.MediumSpringGreen;
            btnEdit.BackColor = Color.MediumSpringGreen;
            btnAdd.BackColor = Color.MediumSpringGreen;
            btnDelete.BackColor = Color.MediumSpringGreen;
            ShowFio();
            LoadProductCard();
            txtSearch.TextChanged += txtSearch_TextChanged;
            btnAdd.Visible = (userRole == "Администратор" || userRole == "Менеджер");
            btnEdit.Visible = (userRole == "Администратор" || userRole == "Менеджер");
            btnDelete.Visible = (userRole == "Администратор" || userRole == "Менеджер");
            btnReset.Visible = (userRole == "Администратор" || userRole == "Менеджер");
            txtSearch.Visible = (userRole == "Администратор" || userRole == "Менеджер");
            comboPost.Visible = (userRole == "Администратор" || userRole == "Менеджер");
            comboSort.Visible = (userRole == "Администратор" || userRole == "Менеджер");

        }



        private void LoadProductCard(string search = "", string sort = "", string post = "")
        {
            flowProduct.Controls.Clear();
            using (SqlConnection connection = new SqlConnection("Data Source = Nikita; Initial Catalog = Обувь2; Integrated Security = True"))
            {
                connection.Open();

                string orderBy = "";
                if (sort == "asc")
                    orderBy = "ORDER BY Кол_во_на_складе ASC";
                else if (sort == "desc")
                    orderBy = "ORDER BY Кол_во_на_складе DESC";
                string postWhere = "";
                if (post == "Kari")
                    postWhere = "AND Поставщик = 'Kari'";
                else if (post == "Обувь для вас")
                    postWhere = "AND Поставщик = 'Обувь для вас'";



                SqlCommand command = new SqlCommand($@"SELECT * FROM Tovar WHERE (Категория_товара LIKE @search OR Артикул LIKE @search OR Наименование_товара LIKE @search OR Поставщик LIKE @search OR Производитель LIKE @search OR Описание_товара LIKE @search ) {postWhere} {orderBy}", connection);
                command.Parameters.AddWithValue("@search", "%" + search + "%");
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Panel card = CreateProductCard(reader);
                    flowProduct.Controls.Add(card);
                }
            }
        }


        private Panel CreateProductCard(SqlDataReader reader)
        {
            Panel panel = new Panel()
            {
                Width = 950,
                Height = 250,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.Chartreuse
            };
            PictureBox pictureBox = new PictureBox()
            {
                Top = 20,
                Left = 20,
                Width = 200,
                Height = 200,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
            };

            string imagePath = Convert.ToString(reader["Фото"]);
            string noImagePath = @"C:\Users\rusol\Desktop\БУ\Модуль 1\Прил_2_ОЗ_КОД 09.02.07-2-2026-М1\import\picture.png";
            string finalPath = noImagePath;

            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                imagePath = imagePath.Trim('"');
                if (File.Exists(imagePath))
                {
                    finalPath = imagePath;
                }
            }
            pictureBox.Image = Image.FromFile(finalPath);





            Label lblCategory = new Label()
            {
                Text = $"{reader["Категория_товара"]}",
                Left = 240,
                Top = 20,
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                AutoSize = true,
            };
            Label lblName = new Label()
            {
                Text = $"| {reader["Наименование_товара"]}",
                Left = 340,
                Top = 20,
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                AutoSize = true,
            };

            Label lblOpisanie = new Label()
            {
                Text = $"Описание товара: {reader["Описание_товара"]}",
                Left = 240,
                Top = 40,
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
                AutoSize = true,
            };
            Label lblProizvoditel = new Label()
            {
                Text = $"Производитель: {reader["Производитель"]}",
                Left = 240,
                Top = 60,
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
                AutoSize = true,
            };
            Label lblPostavshik = new Label()
            {
                Text = $"Производитель: {reader["Производитель"]}",
                Left = 240,
                Top = 80,
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
                AutoSize = true,
            };


            int discaunt = Convert.ToInt32(reader["Действующая_скидка"]);
            decimal price = Convert.ToDecimal(reader["Цена"]);
            decimal newPrice = discaunt > 0
                ? price - (price * discaunt / 100)
                : price;


            Label lblNamePrice = new Label()
            {
                Text = $"Цена: ",
                Left = 240,
                Top = 100,
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
                AutoSize = true,
            };

            Label lblNewPrice = new Label()
            {
                Text = $"{newPrice:F2} P.",
                Left = 350,
                Top = 100,
                ForeColor = discaunt > 0 ? Color.Red : Color.Black,
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
                AutoSize = true,
                Visible = discaunt > 0,
            };
            Label lblOldPrice = new Label()
            {
                Text = $"{price:F2} P.",
                Left = 280,
                Top = 100,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 10, discaunt > 0 ? FontStyle.Strikeout : FontStyle.Regular),
                AutoSize = true,
            };

            Label lblDiscaunt = new Label()
            {
                Text = $"-{discaunt}%",
                Left = 800,
                Top = 85,
                ForeColor = Color.White,
                Padding = new Padding(30,50,30,50),
                BackColor = Color.Red,
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
                AutoSize = true,
                Visible = discaunt > 0,
                BorderStyle = BorderStyle.FixedSingle,
            };

            Label lblSh = new Label()
            {
                Text = $"Единица измерения: {reader["Единица_измерения"]}",
                Left = 240,
                Top = 120,
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
                AutoSize = true,
            };

            int count = Convert.ToInt32(reader["Кол_во_на_складе"]);

            Label lblCount = new Label()
            {
                Text = $"{count}",
                Left = 240,
                Top = 140,
                ForeColor = count == 0 ? Color.Blue : Color.Black,
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
                AutoSize = true,
            };

            int productId = Convert.ToInt32(reader["ID_товара"]);
            if (discaunt > 15)
                panel.BackColor = Color.SeaGreen;
            else
                panel.BackColor = Color.Chartreuse;
            panel.Tag = new object[] { productId, panel.BackColor };

            panel.Click += (s, e) =>
            {
                if(selectedPanel != null)
                {
                    var oldData = (object[])selectedPanel.Tag;
                    selectedPanel.BackColor = (Color)oldData[1];
                }
                selectedPanel = panel;
                var data = (object[])selectedPanel.Tag;
                selectedPanelId = (int)data[0];
                selectedPanel.BackColor = Color.AliceBlue;
            };







            panel.Controls.Add(pictureBox);
            panel.Controls.Add(lblCategory);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblOpisanie);
            panel.Controls.Add(lblProizvoditel);
            panel.Controls.Add(lblPostavshik);
            panel.Controls.Add(lblNamePrice);
            panel.Controls.Add(lblNewPrice);
            panel.Controls.Add(lblOldPrice);
            panel.Controls.Add(lblDiscaunt);

            return panel;
        }














        private void btnBack_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Вы вышли на страницу авторизации");
            FormAuth formAuth = new FormAuth();
            formAuth.FormClosed += (s, ex) =>
                Application.Exit();
            formAuth.Show();
            this.Hide();
        }

        private void ShowFio()
        {
            if (userFio == null)
                lblFio.Text = "Гость";
            else
                lblFio.Text = userFio;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string sort = "";
            if (comboSort.SelectedIndex == 1)
                sort = "asc";
            else if (comboSort.SelectedIndex == 2)
                sort = "desc";
            string post = "";
            if (comboPost.SelectedIndex == 1)
                post = "Kari";
            else if (comboPost.SelectedIndex == 2)
                post = "Обувь для вас";
            LoadProductCard(txtSearch.Text.Trim(), sort, post);
        }

        private void comboSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sort = "";
            if (comboSort.SelectedIndex == 1)
                sort = "asc";
            else if (comboSort.SelectedIndex == 2)
                sort = "desc";
            string post = "";
            if (comboPost.SelectedIndex == 1)
                post = "Kari";
            else if (comboPost.SelectedIndex == 2)
                post = "Обувь для вас";
            LoadProductCard(txtSearch.Text.Trim(), sort, post);
        }

        private void comboPost_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sort = "";
            if (comboSort.SelectedIndex == 1)
                sort = "asc";
            else if (comboSort.SelectedIndex == 2)
                sort = "desc";
            string post = "";
            if (comboPost.SelectedIndex == 1)
                post = "Kari";
            else if (comboPost.SelectedIndex == 2)
                post = "Обувь для вас";
            LoadProductCard(txtSearch.Text.Trim(), sort, post);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            comboPost.SelectedIndex = 0;
            comboSort.SelectedIndex = 0;
            LoadProductCard();
        }



        private void DeleteTovar(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source = Nikita; Initial Catalog = Обувь2; Integrated Security = True"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Tovar WHERE ID_товара = @id ", connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Товар успешно удален");
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedPanel == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }
            else if (MessageBox.Show("Удалить товар?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DeleteTovar(selectedPanelId);
                selectedPanel = null;
                selectedPanelId = -1;
                LoadProductCard();
            }
 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormAdd formAdd = new FormAdd();
            formAdd.ShowDialog();
            LoadProductCard();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedPanel == null)
            {
                MessageBox.Show("Для редактирования выберите товар");
            }
            else
            {
                FormAdd formAdd = new FormAdd(selectedPanelId);
                formAdd.ShowDialog();
                LoadProductCard();
            }
        }
    }
}
