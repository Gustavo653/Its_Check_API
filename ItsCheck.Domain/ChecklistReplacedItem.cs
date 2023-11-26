namespace ItsCheck.Domain
{
    public class ChecklistReplacedItem : BaseEntity
    {
        public required int AmountReplaced { get; set; }
        public required virtual ChecklistReview ChecklistReview { get; set; }
        public required virtual ChecklistItem ChecklistItem { get; set; }
    }
}
