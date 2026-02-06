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

namespace Demo8
{
    public partial class FormAdd : Form
    {
        int productId;
        public FormAdd()
        {
            this.Icon = Icon.FromHandle(new Bitmap(@"C:\Users\rusol\Desktop\БУ\Модуль 1\Прил_2_ОЗ_КОД 09.02.07-2-2026-М1\import\Icon.png").GetHicon());
            InitializeComponent();
        }

        public FormAdd(int id)
        {
            productId = id;
            this.Icon = Icon.FromHandle(new Bitmap(@"C:\Users\rusol\Desktop\БУ\Модуль 1\Прил_2_ОЗ_КОД 09.02.07-2-2026-М1\import\Icon.png").GetHicon());
            InitializeComponent();
            LoadProductForEdit();
        }



        private void LoadProductForEdit()
        {
            using (SqlConnection connection = new SqlConnection("Data Source = Nikita; Initial Catalog = Обувь2; Integrated Security = True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(@"SELECT Артикул, Наименование_товара, Единица_измерения, Цена, Поставщик, Производитель, Категория_товара, Действующая_скидка, Кол_во_на_складе, Описание_товара
                                                        FROM Tovar
                                                            WHERE ID_товара = @id", connection);
                command.Parameters.AddWithValue("@id", productId);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    txtArticl.Text = reader["Артикул"].ToString();
                    txtCount.Text = reader["Кол_во_на_складе"].ToString();
                    txtDiscount.Text = reader["Действующая_скидка"].ToString();
                    txtName.Text = reader["Наименование_товара"].ToString();
                    txtOpisanie.Text = reader["Описание_товара"].ToString();
                    txtPrice.Text = reader["Цена"].ToString();
                    comboCategory.Text = reader["Категория_товара"].ToString();
                    comboPostavshik.Text = reader["Поставщик"].ToString();
                    comboProizvoditel.Text = reader["Производитель"].ToString();
                    comboSh.Text = reader["Единица_измерения"].ToString();
                }
                btnSave.Text = "Сохранить изменения";
            }
        }










        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtArticl.Text == "" || txtCount.Text == "" || txtDiscount.Text == "" || txtName.Text == "" || txtOpisanie.Text == "" || txtPrice.Text == "" || comboCategory.Text == ""|| comboPostavshik.Text == "" || comboProizvoditel.Text == "" || comboSh.Text == "")
                {
                    MessageBox.Show("Заполните все поля для ввода");
                    return;
                }
                int discaunt = Convert.ToInt32(txtDiscount.Text);
                if (discaunt > 100)
                    discaunt = 100;
                using (SqlConnection connection = new SqlConnection("Data Source = Nikita; Initial Catalog = Обувь2; Integrated Security = True"))
                {
                    connection.Open();






                    SqlCommand command;


                    if (productId == -1)
                    {
                        command = new SqlCommand(@"INSERT INTO Tovar (Артикул, Наименование_товара, Единица_измерения, Цена, Поставщик, Производитель, Категория_товара, Действующая_скидка, Кол_во_на_складе, Описание_товара) 
                                                        VALUES (@arctic,@name,@sh, @price,@postavshik, @proizvoditel,@category, @discaunt, @count, @opisanie  )", connection);
                    }
                    else
                    {
                        command = new SqlCommand(@"UPDATE Tovar SET
                                                   Артикул =@arctic, Наименование_товара = @name, Единица_измерения = @sh, Цена = @price, Поставщик =@postavshik, Производитель =  @proizvoditel, Категория_товара = @category, Действующая_скидка =  @discaunt, Кол_во_на_складе = @count, Описание_товара =  @opisanie WHERE ID_товара = @id  ", connection);
                        command.Parameters.AddWithValue("@id", productId);
                    }



                    command.Parameters.AddWithValue("@arctic", txtArticl.Text);
                    command.Parameters.AddWithValue("@name", txtName.Text);
                    command.Parameters.AddWithValue("@sh", comboSh.Text);
                    command.Parameters.AddWithValue("@price", txtPrice.Text);
                    command.Parameters.AddWithValue("@postavshik", comboPostavshik.Text);
                    command.Parameters.AddWithValue("@proizvoditel", comboProizvoditel.Text);
                    command.Parameters.AddWithValue("@category", comboCategory.Text);
                    command.Parameters.AddWithValue("@discaunt", txtDiscount.Text);
                    command.Parameters.AddWithValue("@count", txtCount.Text);
                    command.Parameters.AddWithValue("@opisanie", txtOpisanie.Text);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show(productId == -1 ?"Товар добавлен" : "Товар изменен");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!(char.IsDigit(e.KeyChar)) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void txtCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
    }
}
