namespace TeamChallenge.Models.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public List<Cosmetic> Cosmetics { get; set; }
        public DeliveryState DeliveryState { get; set; }
    }
}
