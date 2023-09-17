namespace ItsCheck.Domain
{
    public class Checklist : BaseEntity
    {
        public required string Name { get; set; }
        public virtual List<ChecklistItem> ChecklistItems { get; set; } = new List<ChecklistItem>();
    }
}
