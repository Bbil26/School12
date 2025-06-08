using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WpfApp1;

namespace Главное_окно.TeacherModel
{
    class TeacherViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Teacher> Teachers { get; set; } = new();

        private Teacher? _selectedTeacher;
        public Teacher? SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                OnPropertyChanged(nameof(SelectedTeacher));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public void AddTeacher()
        {
            var newTeacher = new Teacher { FirstName = "Новый", SecondName = "Учитель" };
            Teachers.Add(newTeacher);
            using var context = new School12Context();
            context.Teachers.Add(newTeacher);
            context.SaveChanges();
        }

        public void LoadTeacher()
        {
            using var context = new School12Context();
            
            var teachers = context.Teachers
                .OrderBy(t => t.IdTeacher) // сортировка по ID по умолчанию
                .ToList();
            Teachers.Clear();
            foreach (var teacher in teachers)
                Teachers.Add(teacher);
        }

        public void DeleteSelectedTeacher()
        {
            if (SelectedTeacher != null)
            {
                if (MessageBox.Show("Удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // КАК ЖЕ Я ЛЮБЛЮ КОСТЫЛИ!!!!
                    using var context = new School12Context();
                    context.Teachers.Remove(SelectedTeacher);
                    context.SaveChanges();
                    Teachers.Remove(SelectedTeacher);
                }
            }
        }

        public void SaveEditedTeacher()
        {
            if (SelectedTeacher != null)
            {
                using var context = new School12Context();
                context.Teachers.Update(SelectedTeacher);
                context.SaveChanges();
            }
        }
        
        // Для комбобоксов Классов
        public ObservableCollection<SchoolClass> AllClasses { get; set; } = new();
        public ObservableCollection<UserDatum> AllUsers { get; set; } = new();
        public void LoadClasses()
        {
            using var context = new School12Context();
            AllClasses.Clear();
            AllClasses.Add(null);
            foreach (var cls in context.SchoolClasses.OrderBy(c => c.IdClass))
                AllClasses.Add(cls);
        }

        public void LoadUsers()
        {
            using var context = new School12Context();
            AllUsers.Clear();
            AllUsers.Add(null);
            foreach (var cls in context.UserData.OrderBy(u => u.IdUser))
                AllUsers.Add(cls);
        }
    }
}
