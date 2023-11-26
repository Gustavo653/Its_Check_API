namespace ItsCheck.Domain
{
    public class Checklist : BaseEntity
    {
        public required string Name { get; set; }
        public required virtual List<ChecklistItem> ChecklistItems { get; set; }
    }
}
