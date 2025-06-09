using System.Configuration;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Главное_окно
{

    public partial class Auth : Window
    {
        public UserDatum? authUser { get; private set; }
        public Auth()
        {
            InitializeComponent();

            //БД users
            using (var db = new School12Context())
            {
                List<UserDatum> users = db.UserData.ToList();
            }

        }

        //Зашифровать Если останется время!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {

            using (var db = new School12Context())
            {
                authUser = db.UserData.ToList().Find(x => x.Login == $"{login.Text}" && x.Password == $"{password.Password}");
            }
            
            if (authUser != null)
                this.DialogResult = true;
            else
                errorAuth.Visibility = Visibility.Visible;
        }

        private void loginTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            login.Focus();
        }

        //Отладка, ПОТОМ УДАЛИТЬ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void Autorun()
        {
            login.Text = "user2";
            password.Password = "123";
            //login.Text = "admin";
            //password.Password = "admin";
            Thread.Sleep(1000);
            btnAuth_Click(btnAuth, new RoutedEventArgs());
        }
        
        private void AfterLoadBtn(object sender, RoutedEventArgs e)
        {
            Autorun();
        }
    }
}