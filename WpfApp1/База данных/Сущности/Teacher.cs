using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Главное_окно;

public partial class Teacher : INotifyPropertyChanged
{
    public long IdTeacher { get; set; }

    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public string? Surname { get; set; }

    public int? YearOfBirth { get; set; }

    public long? TeacherIdUser { get; set; }

    public long? TeacherIdClass { get; set; }

    public virtual SchoolClass? TeacherIdClassNavigation { get; set; }

    public virtual UserDatum? TeacherIdUserNavigation { get; set; }

    [NotMapped]
    private bool _isEditing;

    [NotMapped]
    public bool IsEditing
    {
        get => _isEditing;
        set
        {
            _isEditing = value;
            OnPropertyChanged(nameof(IsEditing));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
