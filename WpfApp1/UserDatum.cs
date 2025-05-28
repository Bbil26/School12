using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Главное_окно;

public partial class UserDatum
{
    public long IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long? UserIdRole { get; set; }

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public virtual Role? UserIdRoleNavigation { get; set; }
}
