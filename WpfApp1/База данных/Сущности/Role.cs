using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Главное_окно;

public partial class Role
{
    public long IdRole { get; set; }

    public string? NameRole { get; set; }

    public virtual ICollection<UserDatum> UserData { get; set; } = new List<UserDatum>();
}
