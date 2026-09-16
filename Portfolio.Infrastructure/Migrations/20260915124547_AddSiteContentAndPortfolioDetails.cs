using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteContentAndPortfolioDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "client",
                table: "portfolio_items",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "format",
                table: "portfolio_items",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "result",
                table: "portfolio_items",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "year",
                table: "portfolio_items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    brand_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    seo_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    hero_eyebrow = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    hero_title_line_1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    hero_title_line_2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    hero_title_line_3 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    hero_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    about_title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    about_description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    portfolio_title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    portfolio_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    services_title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ProcessTitle = table.Column<string>(type: "text", nullable: true),
                    contact_title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    contact_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    instagram_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    whatsapp_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    vimeo_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_settings", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "site_settings");

            migrationBuilder.DropColumn(
                name: "client",
                table: "portfolio_items");

            migrationBuilder.DropColumn(
                name: "format",
                table: "portfolio_items");

            migrationBuilder.DropColumn(
                name: "result",
                table: "portfolio_items");

            migrationBuilder.DropColumn(
                name: "year",
                table: "portfolio_items");
        }
    }
}
