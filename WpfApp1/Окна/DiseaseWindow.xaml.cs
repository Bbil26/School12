using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Главное_окно.DiseaseModel;

namespace Главное_окно
{

    public partial class DiseaseWindow : Window
    {
        public string _fname { get; set; }
        public string _lname { get; set; } 
        public string _sname { get; set; }
        public long _idStudent { get; set; }
        private long _idDisease;
        private string _descriptionDisease;
        private bool? _hardDisease;
        private DateTime _dateDisease;
        private bool _isEditing = false;

        private DiseaseViewModel ViewModel = new DiseaseViewModel();
        
        public DiseaseWindow(string fname = "[Нет фамилии]", string lname = "[Нет имени]", string sname = "[Нет отчества]", long id = 0)
        {
            _fname = fname;
            _lname = lname;
            _sname = sname;
            _idStudent = id;

            DataContext = this;
            InitializeComponent();
            idBox.Text = _idStudent.ToString(); 
        }

        public DiseaseWindow(Disease selectedDisesase, string fname = "[Нет фамилии]", string lname = "[Нет имени]", string sname = "[Нет отчества]")
        {
            _fname = fname;
            _lname = lname;
            _sname = sname;
            _idStudent = selectedDisesase.DiseaseIdStudent;

            _idDisease = selectedDisesase.IdDisease;
            _descriptionDisease = selectedDisesase.DescriptionDisease;
            _hardDisease = selectedDisesase.HardDisease;
            _dateDisease = selectedDisesase.DateDisease;
            _isEditing = true;

            DataContext = this;
            InitializeComponent();

            idBox.Text = _idStudent.ToString();
            idBox.IsReadOnly = false;
            description.Text = _descriptionDisease.ToString();
            hardDisease.IsChecked = _hardDisease;
            dateDisease.SelectedDate = _dateDisease;

            btnDis.Content = "Изменить запись";
            titleWindow.Text = "Изменить данные заболевания для записи";

        }

        // Кнопка
        private void BtnDisease(object sender, RoutedEventArgs e)
        {
            if (_isEditing == false)
                AddDisease();
            else
            {  
                _idStudent = long.Parse(idBox.Text);
                _descriptionDisease = description.Text;
                _hardDisease = hardDisease.IsChecked;
                _dateDisease = DateTime.SpecifyKind(dateDisease.SelectedDate ?? DateTime.Today, DateTimeKind.Utc);

                ViewModel.UpdateDisease(_idDisease, _dateDisease, _descriptionDisease, _idStudent, _hardDisease);
                DialogResult = true;
            }    
                
        }

        private void AddDisease()
        {
            _descriptionDisease = description.Text;
            _hardDisease = hardDisease.IsChecked;
            _dateDisease = DateTime.SpecifyKind(dateDisease.SelectedDate ?? DateTime.Today, DateTimeKind.Utc);

            DialogResult = true;

            try
            {
                ViewModel.AddDisease(_descriptionDisease, _dateDisease, _idStudent, _hardDisease);
            }
            catch
            {
                DialogResult = false;
            }

        }

    }
}
