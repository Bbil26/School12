using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Главное_окно;
using Главное_окно.DiseaseModel;
using Главное_окно.StudentModel;
using Главное_окно.TeacherModel;
using Главное_окно.UserModel;
using Главное_окно.Окна;


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

            // Прячем непотребства
            if (curUser != null && curUser.UserIdRole != 1)
            {
                UserTab.Visibility = Visibility.Collapsed;
                TeacherTab.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateTables(object sender, SelectionChangedEventArgs e)
        {
            ((StudentViewModel)StudentGrid.DataContext).LoadClasses();
            ((StudentViewModel)StudentGrid.DataContext).LoadStudents();
            ((DiseaseViewModel)DiseaseGrid.DataContext).LoadDiseases();
            ((UserViewModel)UserGrid.DataContext).LoadRoles();
            ((UserViewModel)UserGrid.DataContext).LoadUsers();
            ((TeacherViewModel)TeacherGrid.DataContext).LoadClasses();
            ((TeacherViewModel)TeacherGrid.DataContext).LoadUsers();
            ((TeacherViewModel)TeacherGrid.DataContext).LoadTeacher();
        }

        //
        // Запуск модального окна с авторизацией
        //
        private void OpenAuthWindow()
        {
            Auth authWindow = new Auth();

            if (authWindow.ShowDialog() == true)
            {
                curUser = authWindow.authUser;

                using var context = new School12Context();
                curTeacher = context.Teachers.FirstOrDefault(t => t.TeacherIdUser == curUser.IdUser);

                userInfo.Text = curUser.Login;

                UpdateTables(null, null);
            }
            else
                Close();
        }
        
        private StudentViewModel ViewModelStudent => (StudentViewModel)StudentGrid.DataContext;
        private DiseaseViewModel ViewModelDisease => (DiseaseViewModel)DiseaseGrid.DataContext;
        private UserViewModel ViewModelUser => (UserViewModel)UserGrid.DataContext;
        private TeacherViewModel ViewModelTeacher => (TeacherViewModel)TeacherGrid.DataContext;
        
        //
        // Таблица "Students"
        //
        private void btnAddStudent(object sender, RoutedEventArgs e)
        {
            ViewModelStudent.AddStudent();
        }

        private void btnRemoveStudent(object sender, RoutedEventArgs e)
        {
            ViewModelStudent.DeleteSelectedStudent();
            UpdateTables(null, null);
        }

        private void btnEditStudent(object sender, RoutedEventArgs e)
        {
            if (ViewModelStudent.SelectedStudent != null)
            {   
                // Включаем только у выбранного
                ViewModelStudent.SelectedStudent.IsEditing = true;
                if (curUser != null && curUser.UserIdRole == 1)
                    ViewModelStudent.SelectedStudent.IsEditingOnlyAdmin = true;
            }
        }

        private void btnConfirmStudent(object sender, RoutedEventArgs e)
        {
            StudentsGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            StudentsGrid.CommitEdit(DataGridEditingUnit.Row, true);

            if (ViewModelStudent.SelectedStudent != null)
            {
                ViewModelStudent.SaveEditedStudent();
                ViewModelStudent.SelectedStudent.IsEditing = false;
                if (curUser != null && curUser.UserIdRole == 1)
                    ViewModelStudent.SelectedStudent.IsEditingOnlyAdmin = false;
            }
        }

        // Модальное окно для добавления заболеваний //

        private void OpenDiseaseWindow(string fname, string lname, string sname, long id)
        {
            DiseaseWindow diseaseWindow = new DiseaseWindow(fname, lname, sname, id);
            bool? res = diseaseWindow.ShowDialog();

            if (res == false)
            {
                MessageBox.Show("Добавление болезни прошло неудачно!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        private void btnAddDisease(object sender, RoutedEventArgs e)
        {
            if (ViewModelStudent.SelectedStudent != null)
            {
                OpenDiseaseWindow(ViewModelStudent.SelectedStudent.FirstName,
                                  ViewModelStudent.SelectedStudent.SecondName,
                                  ViewModelStudent.SelectedStudent.Surname,
                                  ViewModelStudent.SelectedStudent.IdStudent);
            }
            UpdateTables(null, null);
        }

        //
        // Таблица с заболеваниями
        //

        private void btnEditDisease(object sender, RoutedEventArgs e)
        {
            if (ViewModelDisease.SelectedDisease == null) return;

            using var context = new School12Context();
            var enity = context.Students.First(s => s.IdStudent == ViewModelDisease.SelectedDisease.DiseaseIdStudent);

            DiseaseWindow disWin = new DiseaseWindow(ViewModelDisease.SelectedDisease, enity.FirstName, enity.SecondName, enity.Surname);
            bool? res = disWin.ShowDialog();
            if (res == false)
            {
                MessageBox.Show("Обновление данных прошло неудачно!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                UpdateTables(null, null);
            }
        }

        private void btnRemoveDisease(object sender, RoutedEventArgs e)
        {
            ViewModelDisease.DeleteSelectedDisease();
        }

        //
        // Таблица Пользователей
        //

        private void btnAddUser(object sender, RoutedEventArgs e)
        {
            ViewModelUser.AddUser();
        }

        private void btnRemoveUser(object sender, RoutedEventArgs e)
        {
            ViewModelUser.DeleteSelectedUser();
            UpdateTables(null, null);
        }

        private void btnEditUser(object sender, RoutedEventArgs e)
        {
            if (ViewModelUser.SelectedUser != null)
            {
                // Включаем только у выбранного
                ViewModelUser.SelectedUser.IsEditing = true;
            }
        }

        private void btnConfirmUser(object sender, RoutedEventArgs e)
        {
            UsersGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            UsersGrid.CommitEdit(DataGridEditingUnit.Row, true);

            if (ViewModelUser.SelectedUser != null)
            {
                ViewModelUser.SaveEditedUser();
                ViewModelUser.SelectedUser.IsEditing = false;
            }
        }


        //
        // Таблица Учителей
        //

        private void btnAddTeacher(object sender, RoutedEventArgs e)
        {
            ViewModelTeacher.AddTeacher();
        }

        private void btnRemoveTeacher(object sender, RoutedEventArgs e)
        {
            ViewModelTeacher.DeleteSelectedTeacher();
            UpdateTables(null, null);
        }

        private void btnEditTeacher(object sender, RoutedEventArgs e)
        {
            if (ViewModelTeacher.SelectedTeacher != null)
            {
                // Включаем только у выбранного
                ViewModelTeacher.SelectedTeacher.IsEditing = true;
            }
        }

        private void btnConfirmTeacher(object sender, RoutedEventArgs e)
        {
            TeachersGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            TeachersGrid.CommitEdit(DataGridEditingUnit.Row, true);

            if (ViewModelTeacher.SelectedTeacher != null)
            {
                ViewModelTeacher.SaveEditedTeacher();
                ViewModelTeacher.SelectedTeacher.IsEditing = false;
            }
        }

        //
        // Отчет
        //
        private void btnCreateReport(object sender, RoutedEventArgs e)
        {
            Report report = new Report();
            if (report.ShowDialog() == true)
            {

            }
            else
            {
                
            }

        }

    }

}
