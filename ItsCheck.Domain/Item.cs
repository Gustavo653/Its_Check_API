namespace ItsCheck.Domain
{
    public class Item : BaseEntity
    {
        public required string Name { get; set; }
        public virtual List<ChecklistAdjustedItem>? ChecklistAdjustedItems { get; set; }
    }
}
