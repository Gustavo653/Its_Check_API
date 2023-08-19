namespace ItsCheck.Domain
{
    public class ChecklistItem : BaseEntity
    {
        public int RequiredQuantity { get; set; }
        public virtual Item Item { get; set; }
        public virtual Category Category { get; set; }
        public virtual Checklist Checklist { get; set; }
    }
}
