using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WpfApp1;

namespace Главное_окно.UserModel
{
    class UserViewModel : INotifyPropertyChanged
    {
        // Данные Супер-Юзера для отмены выдачи его среди пользователей
        protected string superUserLogin = "admin";
        protected string superUserPassword = "admin";
        public ObservableCollection<UserDatum> UserData { get; set; } = new();

        private UserDatum? _selectedUser;
        public UserDatum? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public void AddUser()
        {
            var newUser = new UserDatum { Login = "login", Password = "password", UserIdRole = 2 };
            UserData.Add(newUser);
            using var context = new School12Context();
            context.UserData.Add(newUser);
            context.SaveChanges();
        }

        public void LoadUsers()
        {
            using var context = new School12Context();
            var users = context.UserData
                .Where(u => u.Login != superUserLogin && u.Password != superUserPassword) // Супер-пользователь
                .OrderBy(s => s.IdUser) // сортировка по ID по умолчанию
                .ToList();
            UserData.Clear();
            foreach (var user in users)
                UserData.Add(user);
        }

        public void DeleteSelectedUser()
        {
            if (SelectedUser != null)
            {
                if (MessageBox.Show("Удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using var context = new School12Context();
                    context.UserData.Remove(SelectedUser);
                    context.SaveChanges();
                    UserData.Remove(SelectedUser);
                }
            }
        }

        public void SaveEditedUser()
        {
            if (SelectedUser != null)
            {
                using var context = new School12Context();
                context.UserData.Update(SelectedUser);
                context.SaveChanges();
            }
        }

        // Для комбобоксов Ролей
        public ObservableCollection<Role> Roles { get; set; } = new();

        public void LoadRoles()
        {
            using var context = new School12Context();
            Roles.Clear();
            foreach (var role in context.Roles)
                Roles.Add(role);
        }

    }
}
