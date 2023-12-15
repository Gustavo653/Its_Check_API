namespace ItsCheck.Domain
{
    public class Checklist : TenantBaseEntity
    {
        public required string Name { get; set; }
        public virtual List<ChecklistItem> ChecklistItems { get; set; }
    }
}
