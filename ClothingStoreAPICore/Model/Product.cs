namespace ClothingStoreAPICore.Model
{
    public partial class Product
    {
        public int ProductId { get; set; }

        public int? CategoryId { get; set; }

        public string? ProductName { get; set; }

        public int? InitialPrice { get; set; }

        public int? OfficialPrice { get; set; }
        public string? Size1 { get; set; }
        public string? Size2 { get; set; }
        public string? Size3 { get; set; }
        public int? Amount1 { get; set; }
        public int? Amount2 { get; set; }
        public int? Amount3 { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Introduction { get; set; }

        public string? ImgPath1 { get; set; }

        public string? ImgPath2 { get; set; }

        public string? ImgPath3 { get; set; }

      
    }
}
