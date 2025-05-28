using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace School12
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //Подключеник к БД

        //Интерфейс для авторизации
        private void btnLogin(object sender, RoutedEventArgs e)
        {

        }
        
        private bool authUser()
        {
            return true;
        }

        

        //Интерфейс для работы с БД
        private void bntRevertStudent(object sender, RoutedEventArgs e)
        {
            
        }
        private void btnAddStudent(object sender, RoutedEventArgs e)
        {

        }
        private void btnRemoveStudent(object sender, RoutedEventArgs e)
        {

        }
        private void btnEditStudent(object sender, RoutedEventArgs e)
        {

        }
        private void btnConfirmStudent(object sender, RoutedEventArgs e)
        {

        }

        //Интерфейс для работы с таблицей заболеваемости
        
        
    }
}
