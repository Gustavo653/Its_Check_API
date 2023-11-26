namespace ItsCheck.Domain
{
    public class ChecklistItem : BaseEntity
    {
        public required int AmountRequired { get; set; }
        public required virtual Item Item { get; set; }
        public required virtual Category Category { get; set; }
        public required virtual Checklist Checklist { get; set; }
        public virtual List<ChecklistReplacedItem>? ChecklistReplacedItems { get; set; }
    }
}
