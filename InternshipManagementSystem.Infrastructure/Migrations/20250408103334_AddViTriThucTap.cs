using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddViTriThucTap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViTriThucTaps",
                columns: table => new
                {
                    VtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenViTri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongTuyen = table.Column<int>(type: "int", nullable: false),
                    YeuCau = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViTriThucTaps", x => x.VtId);
                    table.ForeignKey(
                        name: "FK_ViTriThucTaps_DoanhNghieps_DnId",
                        column: x => x.DnId,
                        principalTable: "DoanhNghieps",
                        principalColumn: "DnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViTriThucTaps_DnId",
                table: "ViTriThucTaps",
                column: "DnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViTriThucTaps");
        }
    }
}
