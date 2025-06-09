using System.Net.Mail;
using System.Windows;
using WpfApp1;
using Главное_окно.TeacherModel;
using Главное_окно.Модели;

namespace Главное_окно.Окна
{
    public partial class Report : Window
    {
        public bool IsEditingOnlyAdmin;

        private int curYear;
        private long curClass;
        private DateTime dateS;
        private DateTime dateE;
        public Report()
        {
            InitializeComponent();
            TBYear.Text = DateTime.Now.Year.ToString();
            LoadClasses();
            if 
                (MainWindow.curUser.UserIdRole == 1) CBClass.IsEnabled = true;
            else
                CBClass.SelectedValue = MainWindow.curTeacher.TeacherIdClass;
        }

        private void LoadClasses()
        {
            using var context = new School12Context();
            var classList = context.SchoolClasses.OrderBy(c => c.IdClass).ToList();

            CBClass.ItemsSource = classList;
            CBClass.DisplayMemberPath = "NameClass";
            CBClass.SelectedValuePath = "IdClass";
        }

        private void btnCreate(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TBYear.Text, out curYear))
            {
                MessageBox.Show("Пожалуйста, введите корректный год.");
                return;
            }

            if (CBClass.SelectedValue is long selectedId)
                curClass = selectedId;
            else
            {
                MessageBox.Show("Пожалуйста, выберите класс.");
                return;
            }

            if (RB1.IsChecked == true)
            {
                dateS = new DateTime(curYear, 1, 1);
                dateE = new DateTime(curYear, 5, 31);
            }
            else if (RB2.IsChecked == true)
            {
                dateS = new DateTime(curYear, 9, 1);
                dateE = new DateTime(curYear, 12, 31);
            }
            else if (RB3.IsChecked == true)
            {
                dateS = new DateTime(curYear, 1, 1);
                dateE = new DateTime(curYear, 12, 31);
            }
            else if (RB4.IsChecked == true)
            {
                if (DateStart.SelectedDate.HasValue && DateEnd.SelectedDate.HasValue)
                {
                    dateS = DateStart.SelectedDate.Value;
                    dateE = DateEnd.SelectedDate.Value;
                    if (dateS > dateE)
                    {
                        MessageBox.Show("Ошибка при вводе даты.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка при вводе даты.");
                    return;
                }
            }

            dateS = DateTime.SpecifyKind(dateS, DateTimeKind.Utc);
            dateE = DateTime.SpecifyKind(dateE, DateTimeKind.Utc);

            var excel = new ExcelSample();
            excel.ExportDiseaseReport(curClass, dateS, dateE);
        }

    }
}
