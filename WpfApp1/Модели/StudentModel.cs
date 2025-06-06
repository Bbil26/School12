using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1;

namespace Главное_окно.StudentModel
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

        public void AddStudent()
        {
            var newStudent = new Student { FirstName = "Имя", SecondName = "Фамилия", StudentIdClass = MainWindow.curTeacher.TeacherIdClass };
            Students.Add(newStudent);
            using var context = new School12Context();
            context.Students.Add(newStudent);
            context.SaveChanges();
        }

        public void LoadStudents()
        {
            using var context = new School12Context();

            //!!!!!!!!!!!!!! Если останется время - сделать уведомление об отсутсвии учителя
            if (MainWindow.curTeacher != null) // Учитель
            {

                var students = context.Students
                    .Where(s => s.StudentIdClass == MainWindow.curTeacher.TeacherIdClass)
                    .OrderBy(s => s.IdStudent) // сортировка по ID по умолчанию
                    .ToList();
                Students.Clear();
                foreach (var student in students)
                    Students.Add(student);
            }
            //mmm govnokod
            else // Админ
            {
                var students = context.Students
                    .OrderBy(s => s.IdStudent) // сортировка по ID по умолчанию
                    .ToList();
                Students.Clear();
                foreach (var student in students)
                    Students.Add(student);
            } 
        }

        public void DeleteSelectedStudent()
        {
            if (SelectedStudent != null)
            {
                if (MessageBox.Show("Удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using var context = new School12Context();
                    context.Students.Remove(SelectedStudent);
                    context.SaveChanges();
                    Students.Remove(SelectedStudent);
                }
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

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}


