using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Главное_окно;

public partial class Teacher
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
}
