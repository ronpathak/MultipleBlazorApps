using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace MultipleBlazorApps.Server.Migrations
{
    public partial class NewTenancies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "Tenancies",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
                Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AttachmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndDate = table.Column<string>(type: "datetime2", nullable: false),
                RentAmount = table.Column<double>(type: "float", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tenancies", x => x.Id);
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Tenancies");
        }
    }
}
