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
    public partial class FormAuth : Form
    {

        public FormAuth()
        {
            InitializeComponent();
            btnLogin.BackColor = Color.MediumSpringGreen;
            btnGuest.BackColor = Color.MediumSpringGreen;
            this.Icon = Icon.FromHandle(new Bitmap(@"C:\Users\rusol\Desktop\БУ\Модуль 1\Прил_2_ОЗ_КОД 09.02.07-2-2026-М1\import\Icon.png").GetHicon());
        }



        private string GetUserRole(string login, string password)
        {
            using(SqlConnection connection = new SqlConnection("Data Source = Nikita; Initial Catalog = Обувь2; Integrated Security = True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Роль_сотрудника FROM user_import WHERE Логин = @login AND Пароль = @password", connection);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                object result = command.ExecuteScalar();
                if (result == null)
                    return "";
                return (string)result;
            }
        }




        private string GetUserFio(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection("Data Source = Nikita; Initial Catalog = Обувь2; Integrated Security = True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT ФИО FROM user_import WHERE Логин = @login AND Пароль = @password", connection);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);
                object result = command.ExecuteScalar();
                if (result == null)
                    return "";
                return (string)result;
            }
        }




        private void btnLogin_Click(object sender, EventArgs e)
        {
            string isValid = GetUserRole(txtLogin.Text, txtPassword.Text);
            if (isValid == "")
            {
                MessageBox.Show("Введите корректный логин и пароль");
            }
            string fio = GetUserFio(txtLogin.Text, txtPassword.Text);
            if (isValid == "Администратор")
            {
                MessageBox.Show("Вы вошли как Администратор");
                FormProduct formProduct = new FormProduct(isValid, fio);
                formProduct.FormClosed += (s, ex) =>
                    Application.Exit();
                formProduct.Show();
                this.Hide();
            }
            else if (isValid == "Менеджер")
            {
                MessageBox.Show("Вы вошли как Менеджер");
                FormProduct formProduct = new FormProduct(isValid, fio);
                formProduct.FormClosed += (s, ex) =>
                    Application.Exit();
                formProduct.Show();
                this.Hide();
            }
            else if (isValid == "Авторизированный клиент")
            {
                MessageBox.Show("Вы вошли как Авторизированный клиент");
                FormProduct formProduct = new FormProduct(isValid, fio);
                formProduct.FormClosed += (s, ex) =>
                    Application.Exit();
                formProduct.Show();
                this.Hide();
            }
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Вы вошли как Гость");
            FormProduct formProduct = new FormProduct(null, null);
            formProduct.FormClosed += (s, ex) =>
                Application.Exit();
            formProduct.Show();
            this.Hide();
        }
    }
}
