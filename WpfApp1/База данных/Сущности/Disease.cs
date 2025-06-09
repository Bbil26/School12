namespace Главное_окно;

public partial class Disease
{
    public long IdDisease { get; set; }

    public string? DescriptionDisease { get; set; }

    public DateTime DateDisease { get; set; }

    public long DiseaseIdStudent { get; set; }

    public bool? HardDisease { get; set; }

    public virtual Student DiseaseIdStudentNavigation { get; set; } = null!;
}
