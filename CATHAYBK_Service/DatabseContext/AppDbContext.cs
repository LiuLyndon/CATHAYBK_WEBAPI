using CATHAYBK_Model.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CATHAYBK_Service.DatabseContext
{
    public class AppDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        public DbSet<tblBitcoin> Bitcoins { get; set; }

        public DbSet<tblCurrency> Currencies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLoggerFactory(_loggerFactory) // 設定 LoggerFactory
                    .EnableSensitiveDataLogging() // 記錄參數值
                    .EnableDetailedErrors(); // 顯示詳細錯誤
            }
        }

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


            // 配置 tblCurrency 的表屬性
            modelBuilder.Entity<tblCurrency>(entity =>
            {
                entity.ToTable("tblCurrency");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Code)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(e => e.Name)
                      .HasMaxLength(10);

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()") // 默認值為當前時間
                      .ValueGeneratedOnAdd(); // 僅在新增時生成

                entity.Property(e => e.UpdatedAt)
                      .HasDefaultValueSql("GETDATE()") // 默認值為當前時間
                      .ValueGeneratedOnAddOrUpdate(); // 在新增和更新時生成
            });
        }
    }
}