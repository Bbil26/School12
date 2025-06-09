namespace Главное_окно;

public partial class SchoolClass
{
    public long IdClass { get; set; }

    public string? NameClass { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
