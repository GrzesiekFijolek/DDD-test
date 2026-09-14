using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Users_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    ModificationInfo_ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModificationInfo_ModifiedById = table.Column<long>(type: "bigint", nullable: true),
                    ModificationInfo_ModifiedByName = table.Column<string>(type: "text", nullable: true),
                    DeletionInfo_DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletionInfo_DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletionInfo_DeletedByName = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "app",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "app",
                table: "Users",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "app");
        }
    }
}
