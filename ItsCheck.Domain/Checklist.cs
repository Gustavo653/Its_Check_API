namespace ItsCheck.Domain
{
    public class Checklist : BaseEntity
    {
        public required string Name { get; set; }
        public virtual List<ChecklistItem> ChecklistItems { get; set; }
        public virtual List<Ambulance> Ambulances { get; set; }
    }
}
