namespace ClothingStoreAPICore.Model
{
    public partial class FavoriteProduct
    {
        public int FavoriteId { get; set; }

        public int? ProductId { get; set; }

        public int? UserId { get; set; }
    }
}
