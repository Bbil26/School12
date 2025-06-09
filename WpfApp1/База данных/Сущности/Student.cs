using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Главное_окно;

public partial class Student : INotifyPropertyChanged
{
    public long IdStudent { get; set; }
    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public string? Surname { get; set; }

    public int? YearOfBirth { get; set; }

    public long? StudentIdClass { get; set; }

    public virtual ICollection<Disease> Diseases { get; set; } = new List<Disease>();
    
    public virtual SchoolClass? StudentIdClassNavigation { get; set; }

    [NotMapped]
    private bool _isEditing;

    [NotMapped]
    private bool _isEditingOnlyAdmin;

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

    [NotMapped]
    public bool IsEditingOnlyAdmin
    {
        get => _isEditingOnlyAdmin;
        set
        {
            _isEditingOnlyAdmin = value;
            OnPropertyChanged(nameof(IsEditingOnlyAdmin));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
