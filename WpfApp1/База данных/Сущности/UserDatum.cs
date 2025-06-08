using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Главное_окно;

public partial class UserDatum : INotifyPropertyChanged
{
    public long IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long? UserIdRole { get; set; }

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public virtual Role? UserIdRoleNavigation { get; set; }

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
