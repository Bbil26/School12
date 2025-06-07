
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WpfApp1;

namespace Главное_окно.DiseaseModel
{
    class DiseaseViewModel : INotifyPropertyChanged
    {

        //
        // Древнее зло
        //
        public ObservableCollection<Disease> Diseases { get; set; } = new();

        private Disease? _selectedDisease;
        public Disease? SelectedDisease
        {
            get => _selectedDisease;
            set
            {
                _selectedDisease = value;
                OnPropertyChanged(nameof(SelectedDisease));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        
        //
        // Основные модули
        //
        public void AddDisease(string description, DateTime date, long idStud, bool? hardD)
        {
            var enity = new Disease
            {
                DateDisease = date,
                DiseaseIdStudent = idStud,
                HardDisease = hardD,
                DescriptionDisease = description
            };

            Diseases.Add(enity);
            using var context = new School12Context();
            context.Diseases.Add(enity);
            context.SaveChanges();
        }

        public void LoadDiseases()
        {
            using var context = new School12Context();

            //!!!!!!!!!!!!!! Если останется время - сделать уведомление об отсутсвии учителя
            if (MainWindow.curTeacher != null) //Учитель
            {
                var diseases = context.Diseases
                    .Where(d => 
                        (context.Students.First(s => s.IdStudent == d.DiseaseIdStudent)).StudentIdClass == MainWindow.curTeacher.TeacherIdClass)
                    .OrderBy(d => d.IdDisease) // сортировка по ID по умолчанию
                    .ToList();
                Diseases.Clear();
                foreach (var disease in diseases)
                    Diseases.Add(disease);
            }
        }

        public void DeleteSelectedDisease()
        {
            if (SelectedDisease != null)
            {
                if (MessageBox.Show("Удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using var context = new School12Context();
                    context.Diseases.Remove(SelectedDisease);
                    context.SaveChanges();
                    Diseases.Remove(SelectedDisease);
                }
            }
        }

        public void UpdateDisease(long _idDisease, DateTime _dateDisease, string _descriptionDisease, long _idStudent, bool? _hardDisease)
        {
            var enity = new Disease
            {
                IdDisease = _idDisease,
                DateDisease = _dateDisease,
                DescriptionDisease = _descriptionDisease,
                DiseaseIdStudent = _idStudent,
                HardDisease = _hardDisease
            };

            using var context = new School12Context();
            context.Diseases.Update(enity);
            context.SaveChanges();
            
        }

    }

}
