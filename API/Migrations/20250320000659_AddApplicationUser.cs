using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "JobDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resumes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchingResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResumeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    JobDescriptionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatchPercentage = table.Column<decimal>(type: "TEXT", nullable: false),
                    MatchedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchingResults_JobDescriptions_JobDescriptionId",
                        column: x => x.JobDescriptionId,
                        principalTable: "JobDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchingResults_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchingResults_JobDescriptionId",
                table: "MatchingResults",
                column: "JobDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingResults_ResumeId",
                table: "MatchingResults",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_UserId",
                table: "Resumes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchingResults");

            migrationBuilder.DropTable(
                name: "JobDescriptions");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");
        }
    }
}
