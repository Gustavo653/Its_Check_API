namespace ItsCheck.Domain
{
    public class ChecklistReplacedItem : TenantBaseEntity
    {
        public required int AmountReplaced { get; set; }
        public required virtual ChecklistReview ChecklistReview { get; set; }
        public required virtual ChecklistItem ChecklistItem { get; set; }
    }
}
