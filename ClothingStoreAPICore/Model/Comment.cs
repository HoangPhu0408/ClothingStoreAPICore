namespace ClothingStoreAPICore.Model
{
    public class Comment
    {
        public int IdComment { get; set; }

        public int? UserId { get; set; }

        public int? ProductId { get; set; }

        public string? Content { get; set; }

        public DateTime? Date { get; set; }
    }
}
