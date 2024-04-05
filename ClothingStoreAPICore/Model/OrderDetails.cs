namespace ClothingStoreAPICore.Model
{
    public partial class OrderDetails
    {
        public int OrderDetailsId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }
        public string? Size { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }
    }
}
