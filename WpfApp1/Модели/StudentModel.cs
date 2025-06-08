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
            else if (MainWindow.curUser.UserIdRole == 1)// Админ
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
                    // КАК ЖЕ Я ЛЮБЛЮ КОСТЫЛИ!!!!
                    using var context = new School12Context();
                    var diseasesToRemove = context.Diseases.
                        Where(d => d.DiseaseIdStudent == SelectedStudent.IdStudent)
                        .ToList();
                    if (diseasesToRemove.Any())
                    {
                        context.Diseases.RemoveRange(diseasesToRemove);
                        context.SaveChanges();
                    }
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
        
        // Для комбобоксов Классов
        public ObservableCollection<SchoolClass> AllClasses { get; set; } = new();

        public void LoadClasses()
        {
            using var context = new School12Context();
            AllClasses.Clear();
            foreach (var cls in context.SchoolClasses)
                AllClasses.Add(cls);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
