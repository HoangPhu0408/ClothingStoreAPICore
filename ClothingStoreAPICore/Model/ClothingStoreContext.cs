using Microsoft.EntityFrameworkCore;

namespace ClothingStoreAPICore.Model
{
    public partial class ClothingStoreContext : DbContext
    {
        public ClothingStoreContext() { }
        public ClothingStoreContext(DbContextOptions<ClothingStoreContext> opt) : base(opt)
        {

        }
        #region DbSet
        public virtual DbSet<Admin> AdminAccounts { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<FavoriteProduct> FavoriteProducts { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetails> OrderDetails { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
     
        #endregion
        /*        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                    => optionsBuilder.UseSqlServer("Server=.;Database=MobileStoreDB;Trusted_Connection=True;TrustServerCertificate=True");*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.AdminId);

                entity.ToTable("Admin");

                entity.Property(e => e.AdminId);
                entity.Property(e => e.AdminUserName).HasMaxLength(50);
                entity.Property(e => e.AdminPassword).HasMaxLength(50);
                ;
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId);
                entity.Property(e => e.CategoryName).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            });

            /*modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ContentComment)
                    .HasMaxLength(250)
                    .HasColumnName("Content_Comment");
                entity.Property(e => e.DateComment)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Comment");
                entity.Property(e => e.ProductId).HasColumnName("Product_ID");
                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });*/

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId);
                entity.Property(e => e.Avatar);
                entity.Property(e => e.UserName).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20).IsFixedLength();
            });

            modelBuilder.Entity<FavoriteProduct>(entity =>
            {
                entity.HasKey(e => e.FavoriteId);

                entity.Property(e => e.FavoriteId);
                entity.Property(e => e.ProductId);
                entity.Property(e => e.UserId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId);
                entity.Property(e => e.OrderAddress).HasMaxLength(200);
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.OrderStatus);
                entity.Property(e => e.OrderPrice);
                entity.Property(e => e.OrderQuantity);
                entity.Property(e => e.UserId);
                //entity.Property(e => e.OrderSize).HasMaxLength(10);

            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => e.OrderDetailsId);

                entity.Property(e => e.OrderDetailsId);
                entity.Property(e => e.OrderId);
                entity.Property(e => e.ProductId);
                entity.Property(e => e.Quantity);
                entity.Property(e => e.Price);
                entity.Property(e => e.Size).HasMaxLength(10);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId);
                entity.Property(e => e.CategoryId);
                entity.Property(e => e.ProductName).HasMaxLength(100);
                entity.Property(e => e.InitialPrice);
                entity.Property(e => e.OfficialPrice);

                entity.Property(e => e.Size1).HasMaxLength(10);
                entity.Property(e => e.Size2).HasMaxLength(10);
                entity.Property(e => e.Size3).HasMaxLength(10);
                entity.Property(e => e.Amount1);
                entity.Property(e => e.Amount2);
                entity.Property(e => e.Amount3);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ImgPath1);
                entity.Property(e => e.ImgPath2);
                entity.Property(e => e.ImgPath3);
                entity.Property(e => e.Introduction).HasMaxLength(500);
                    
            });
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.IdComment);

                entity.Property(e => e.IdComment).HasColumnName("IdComment");
                entity.Property(e => e.Content)
                    .HasMaxLength(250)
                    .HasColumnName("Content");
                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("Date");
                entity.Property(e => e.ProductId).HasColumnName("ProductId");
                entity.Property(e => e.UserId).HasColumnName("UserId");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
