namespace ItsCheck.Domain
{
    public class Ambulance : BaseEntity
    {
        public int Number { get; set; }
        public virtual Checklist Checklist { get; set; }
    }
}
