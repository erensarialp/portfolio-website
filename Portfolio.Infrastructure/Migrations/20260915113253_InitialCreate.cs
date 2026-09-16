using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "portfolio_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "portfolio_media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    portfolio_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    aspect_ratio = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    storage_key = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    thumbnail_storage_key = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    teaser_storage_key = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    alt_text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_cover = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    display_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_portfolio_media_portfolio_items_portfolio_item_id",
                        column: x => x.portfolio_item_id,
                        principalTable: "portfolio_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_portfolio_items_slug",
                table: "portfolio_items",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_portfolio_media_portfolio_item_id",
                table: "portfolio_media",
                column: "portfolio_item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "portfolio_media");

            migrationBuilder.DropTable(
                name: "portfolio_items");
        }
    }
}
