namespace ClothingStoreAPICore.Model
{
    public partial class Order
    {
        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? OrderAddress { get; set; }

/*        public string? OrderSize { get; set; }
*/
        public int? OrderQuantity { get; set; }

        public double? OrderPrice { get; set; }

        public int? OrderStatus { get; set; }
    }
}
