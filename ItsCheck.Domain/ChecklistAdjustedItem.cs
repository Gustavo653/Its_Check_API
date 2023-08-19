namespace ItsCheck.Domain
{
    public class ChecklistAdjustedItem : BaseEntity
    {
        public int Quantity { get; set; }
        public virtual Checklist Checklist { get; set; }
        public virtual Item Item { get; set; }
    }
}
