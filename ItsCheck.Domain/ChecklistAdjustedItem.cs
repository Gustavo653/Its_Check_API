namespace ItsCheck.Domain
{
    public class ChecklistAdjustedItem : BaseEntity
    {
        public required int Quantity { get; set; }
        public required virtual Checklist Checklist { get; set; }
        public required virtual Item Item { get; set; }
    }
}
