using OfficeOpenXml;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Главное_окно.Модели
{
    public class ExcelSample
    {
        public void ExportDiseaseReport(long curClass, DateTime dateS, DateTime dateE)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить отчёт как...",
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                FileName = "Отчет_по_болезням.xlsx"
            };

            if (dialog.ShowDialog() != true)
                return;

            string filePath = dialog.FileName;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var context = new School12Context();

            var data = context.Diseases
                .Where(d =>
                    d.DiseaseIdStudentNavigation.StudentIdClassNavigation.IdClass == curClass &&
                    d.DateDisease >= dateS &&
                    d.DateDisease <= dateE)
                .Select(d => new
                {
                    d.DescriptionDisease,
                    d.DateDisease,
                    HardDisease = d.HardDisease == true ? "Да" : "Нет",
                    FullName = d.DiseaseIdStudentNavigation.SecondName + " " +
                               d.DiseaseIdStudentNavigation.FirstName + " " +
                               d.DiseaseIdStudentNavigation.Surname,
                    d.DiseaseIdStudentNavigation.YearOfBirth,
                    ClassName = d.DiseaseIdStudentNavigation.StudentIdClassNavigation.NameClass
                })
                .ToList();


            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Болезни");

            // Заголовки
            worksheet.Cells[1, 1].Value = "ФИО";
            worksheet.Cells[1, 2].Value = "Год рождения";
            worksheet.Cells[1, 3].Value = "Класс";
            worksheet.Cells[1, 4].Value = "Описание болезни";
            worksheet.Cells[1, 5].Value = "Тяжелая болезнь";
            worksheet.Cells[1, 6].Value = "Дата болезни";

            // Данные
            for (int i = 0; i < data.Count; i++)
            {
                var row = i + 2;
                worksheet.Cells[row, 1].Value = data[i].FullName;
                worksheet.Cells[row, 2].Value = data[i].YearOfBirth;
                worksheet.Cells[row, 3].Value = data[i].ClassName;
                worksheet.Cells[row, 4].Value = data[i].DescriptionDisease;
                worksheet.Cells[row, 5].Value = data[i].HardDisease;
                worksheet.Cells[row, 6].Value = data[i].DateDisease.ToShortDateString();
            }

            var totalRows = data.Count + 1; // +1 из-за заголовков
            var totalCols = 6;

            using (var range = worksheet.Cells[1, 1, totalRows, totalCols])
            {
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            try
            {
                package.SaveAs(new FileInfo(filePath));
                System.Windows.MessageBox.Show("Отчет успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
