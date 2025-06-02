using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdySyncProject.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseAttachmentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeLimitMinutes",
                table: "Assessments");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentUrl",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentName",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "AttachmentUrl",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "TimeLimitMinutes",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
