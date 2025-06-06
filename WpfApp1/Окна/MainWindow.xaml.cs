using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Главное_окно;
using Главное_окно.StudentModel;


namespace WpfApp1
{
    public partial class MainWindow : Window
    { 
        public static UserDatum? curUser;
        public static Teacher? curTeacher;
        public MainWindow()
        {
            InitializeComponent();
            OpenAuthWindow();
            OpenDiseaseWindow();
        }
        
        //
        // Запуск модального окна с авторизацией
        //
        private void OpenAuthWindow()
        {
            Auth authWindow = new Auth();
            bool? res = authWindow.ShowDialog();
            
            if (res == true)
            {
                curUser = authWindow.authUser;
                test.Content = curUser.Login; //ОТЛАДКА!@!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                using var context = new School12Context();
                curTeacher = context.Teachers.FirstOrDefault(t => t.TeacherIdUser == curUser.IdUser);

                ((StudentViewModel)this.DataContext).LoadStudents();
            }
            else
                Close();
        }

        //
        // Таблица "Students"
        //
        private StudentViewModel ViewModel => (StudentViewModel)DataContext;

        private void btnAddStudent(object sender, RoutedEventArgs e)
        {
            ViewModel.AddStudent();
        }

        private void btnRemoveStudent(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteSelectedStudent();
        }

        private void btnEditStudent(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedStudent != null)
            {   
                // Включаем только у выбранного
                ViewModel.SelectedStudent.IsEditing = true;
                if (curUser != null && curUser.UserIdRole == 1)
                    ViewModel.SelectedStudent.IsEditingOnlyAdmin = true;
            }
        }

        private void btnConfirmStudent(object sender, RoutedEventArgs e)
        {
            StudentsGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            StudentsGrid.CommitEdit(DataGridEditingUnit.Row, true);

            if (ViewModel.SelectedStudent != null)
            {
                ViewModel.SaveEditedStudent();
                ViewModel.SelectedStudent.IsEditing = false;
                if (curUser != null && curUser.UserIdRole == 1)
                    ViewModel.SelectedStudent.IsEditingOnlyAdmin = false;
            }
        }

        //Интерфейс для работы с таблицей заболеваемости

        private void OpenDiseaseWindow()
        {
            DiseaseWindow diseaseWindow = new DiseaseWindow("Сюда id studenta");
            bool? res = diseaseWindow.ShowDialog();

            if (res == true)
            {

            }
            else Close();
            
        }

        private void btnAddDisease(object sender, RoutedEventArgs e)
        {

        }
    }
}
