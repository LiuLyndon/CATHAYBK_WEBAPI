using CATHAYBK_Model.Database;
using Microsoft.EntityFrameworkCore;

namespace CATHAYBK_Service.DatabseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<tblBitcoin> Bitcoins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置 tblBitcoin 的表屬性
            modelBuilder.Entity<tblBitcoin>(entity =>
            {
                entity.ToTable("tblBitcoin");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Code)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(e => e.Symbol)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(e => e.Description)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Rate)
                      .HasColumnType("decimal(15, 6)")
                      .IsRequired();

                entity.Property(e => e.RateFloat)
                      .HasColumnType("decimal(15, 6)")
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()") // 默認值為當前時間
                      .ValueGeneratedOnAdd(); // 指定此欄位僅在新增時生成
            });
        }
    }
}