using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Главное_окно;


namespace WpfApp1
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Student> Students { get; set; } = new();

        private Student? _selectedStudent;
        public Student? SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public StudentViewModel()
        {
            LoadStudents();
        }

        public void LoadStudents()
        {
            using var context = new School12Context();
            var students = context.Students
                          .OrderBy(s => s.IdStudent) // сортировка по ID по умолчанию
                          .ToList();
            Students.Clear();
            foreach (var student in students)
                Students.Add(student);
        }

        public void DeleteSelectedStudent()
        {
            if (SelectedStudent != null)
            {
                using var context = new School12Context();
                context.Students.Remove(SelectedStudent);
                context.SaveChanges();
                Students.Remove(SelectedStudent);
            }
        }

        public void SaveEditedStudent()
        {
            if (SelectedStudent != null)
            {
                using var context = new School12Context();
                context.Students.Update(SelectedStudent);
                context.SaveChanges();
            }

            IsEditing = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    
    public partial class MainWindow : Window
    { 
        UserDatum curUser;
        public MainWindow()
        {
            InitializeComponent();
            OpenAuthWindow();

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

        //Запуск модального окна с авторизацией
        private void OpenAuthWindow()
        {
            Auth authWindow = new Auth();
            bool? res = authWindow.ShowDialog();
            
            if (res == true)
            {
                curUser = authWindow.authUser;
                test.Content = curUser.Login;
            }
            else
                Close();
        }

        //Интерфейс для работы с БД

        private StudentViewModel ViewModel => (StudentViewModel)DataContext;

        private void btnAddStudent(object sender, RoutedEventArgs e)
        {
            var newStudent = new Student { FirstName = "Имя", SecondName = "Фамилия", YearOfBirth = 2000 };
            ViewModel.Students.Add(newStudent);
            using var context = new School12Context();
            context.Students.Add(newStudent);
            context.SaveChanges();
        }

        private void btnRemoveStudent(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteSelectedStudent();
        }

        private void btnEditStudent(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedStudent != null)
            {
                // Сначала отключаем редактирование у всех
                foreach (var student in ViewModel.Students)
                    student.IsEditing = false;

                // Включаем только у выбранного
                ViewModel.SelectedStudent.IsEditing = true;
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
            }
        }

        //Интерфейс для работы с таблицей заболеваемости


    }
}
