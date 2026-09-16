using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;

namespace Portfolio.Infrastructure.Data
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
            : base(options)
        {
        }

        public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();

        public DbSet<PortfolioMedia> PortfolioMedia => Set<PortfolioMedia>();

        public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();

        public DbSet<Service> Services => Set<Service>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigurePortfolioItem(modelBuilder);
            ConfigurePortfolioMedia(modelBuilder);
            ConfigureSiteSetting(modelBuilder);
            ConfigureService(modelBuilder);
        }

        private static void ConfigurePortfolioItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PortfolioItem>(entity =>
            {
                entity.ToTable("portfolio_items");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id");

                entity.Property(x => x.Title)
                    .HasColumnName("title")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.Slug)
                    .HasColumnName("slug")
                    .HasMaxLength(220)
                    .IsRequired();

                entity.HasIndex(x => x.Slug)
                    .IsUnique();

                entity.Property(x => x.Description)
                    .HasColumnName("description")
                    .HasMaxLength(2000);

                entity.Property(x => x.Category)
                    .HasColumnName("category")
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.Client)
                    .HasColumnName("client")
                    .HasMaxLength(200);

                entity.Property(x => x.Year)
                    .HasColumnName("year");

                entity.Property(x => x.Format)
                    .HasColumnName("format")
                    .HasMaxLength(200);

                entity.Property(x => x.Result)
                    .HasColumnName("result")
                    .HasMaxLength(1000);

                entity.Property(x => x.DisplayOrder)
                    .HasColumnName("display_order")
                    .HasDefaultValue(0);

                entity.Property(x => x.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                entity.Property(x => x.CreatedAt)
                    .HasColumnName("created_at");

                entity.Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");

                entity.HasMany(x => x.Media)
                    .WithOne(x => x.PortfolioItem)
                    .HasForeignKey(x => x.PortfolioItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void ConfigurePortfolioMedia(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PortfolioMedia>(entity =>
            {
                entity.ToTable("portfolio_media");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id");

                entity.Property(x => x.PortfolioItemId)
                    .HasColumnName("portfolio_item_id");

                entity.Property(x => x.MediaType)
                    .HasColumnName("media_type")
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(x => x.AspectRatio)
                    .HasColumnName("aspect_ratio")
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(x => x.StorageKey)
                    .HasColumnName("storage_key")
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(x => x.ThumbnailStorageKey)
                    .HasColumnName("thumbnail_storage_key")
                    .HasMaxLength(1000);

                entity.Property(x => x.TeaserStorageKey)
                    .HasColumnName("teaser_storage_key")
                    .HasMaxLength(1000);

                entity.Property(x => x.AltText)
                    .HasColumnName("alt_text")
                    .HasMaxLength(500);

                entity.Property(x => x.IsCover)
                    .HasColumnName("is_cover")
                    .HasDefaultValue(false);

                entity.Property(x => x.DisplayOrder)
                    .HasColumnName("display_order")
                    .HasDefaultValue(0);

                entity.Property(x => x.CreatedAt)
                    .HasColumnName("created_at");
            });
        }

        private static void ConfigureSiteSetting(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteSetting>(entity =>
            {
                entity.ToTable("site_settings");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id");

                entity.Property(x => x.BrandName)
                    .HasColumnName("brand_name")
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.SeoDescription)
                    .HasColumnName("seo_description")
                    .HasMaxLength(500);

                entity.Property(x => x.Location)
                    .HasColumnName("location")
                    .HasMaxLength(200);

                entity.Property(x => x.Email)
                    .HasColumnName("email")
                    .HasMaxLength(250);

                entity.Property(x => x.HeroEyebrow)
                    .HasColumnName("hero_eyebrow")
                    .HasMaxLength(150);

                entity.Property(x => x.HeroTitleLine1)
                    .HasColumnName("hero_title_line_1")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.HeroTitleLine2)
                    .HasColumnName("hero_title_line_2")
                    .HasMaxLength(200);

                entity.Property(x => x.HeroTitleLine3)
                    .HasColumnName("hero_title_line_3")
                    .HasMaxLength(200);

                entity.Property(x => x.HeroDescription)
                    .HasColumnName("hero_description")
                    .HasMaxLength(1000);

                entity.Property(x => x.AboutTitle)
                    .HasColumnName("about_title")
                    .HasMaxLength(200);

                entity.Property(x => x.AboutDescription)
                    .HasColumnName("about_description")
                    .HasMaxLength(3000);

                entity.Property(x => x.PortfolioTitle)
                    .HasColumnName("portfolio_title")
                    .HasMaxLength(200);

                entity.Property(x => x.PortfolioDescription)
                    .HasColumnName("portfolio_description")
                    .HasMaxLength(1000);

                entity.Property(x => x.ServicesTitle)
                    .HasColumnName("services_title")
                    .HasMaxLength(200);

                entity.Property(x => x.ContactTitle)
                    .HasColumnName("contact_title")
                    .HasMaxLength(200);

                entity.Property(x => x.ContactDescription)
                    .HasColumnName("contact_description")
                    .HasMaxLength(1000);

                entity.Property(x => x.InstagramUrl)
                    .HasColumnName("instagram_url")
                    .HasMaxLength(1000);

                entity.Property(x => x.WhatsAppUrl)
                    .HasColumnName("whatsapp_url")
                    .HasMaxLength(1000);

                entity.Property(x => x.VimeoUrl)
                    .HasColumnName("vimeo_url")
                    .HasMaxLength(1000);

                entity.Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");
            });
        }

        private static void ConfigureService(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("services");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id");

                entity.Property(x => x.Title)
                    .HasColumnName("title")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1500);

                entity.Property(x => x.DisplayOrder)
                    .HasColumnName("display_order")
                    .HasDefaultValue(0);

                entity.Property(x => x.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                entity.Property(x => x.CreatedAt)
                    .HasColumnName("created_at");

                entity.Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");
            });
        }
    }
}