using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elearn.Migrations
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Sales = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Course_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseImages_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FullName", "ProfilePhoto", "SoftDelete" },
                values: new object[] { 1, "Mark Smith", "course_author_2.jpg", false });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FullName", "ProfilePhoto", "SoftDelete" },
                values: new object[] { 2, "Julia Williams", "course_author_3.jpg", false });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FullName", "ProfilePhoto", "SoftDelete" },
                values: new object[] { 3, "James S. Morrison", "featured_author.jpg", false });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "AuthorId", "Description", "Price", "Sales", "SoftDelete", "Title" },
                values: new object[] { 1, 1, "Maecenas rutrum viverra sapien sed ferm entum. Morbi tempor odio eget lacus tempus pulvinar.", 35m, 352, false, "Social Media Course" });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "AuthorId", "Description", "Price", "Sales", "SoftDelete", "Title" },
                values: new object[] { 2, 2, "Maecenas rutrum viverra sapien sed ferm entum. Morbi tempor odio eget lacus tempus pulvinar.", 35m, 352, false, "Marketng Course" });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "AuthorId", "Description", "Price", "Sales", "SoftDelete", "Title" },
                values: new object[] { 3, 3, "Maecenas rutrum viverra sapien sed ferm entum. Morbi tempor odio eget lacus tempus pulvinar.", 35m, 352, false, "Online Literature Course" });

            migrationBuilder.InsertData(
                table: "CourseImages",
                columns: new[] { "Id", "CourseId", "IsMain", "Name", "SoftDelete" },
                values: new object[,]
                {
                    { 1, 1, true, "course_2.jpg", false },
                    { 2, 1, false, "course_1.jpg", false },
                    { 3, 2, true, "course_3.jpg", false },
                    { 4, 2, false, "course_2.jpg", false },
                    { 5, 3, true, "course_1.jpg", false },
                    { 6, 3, false, "course_3.jpg", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_AuthorId",
                table: "Course",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseImages_CourseId",
                table: "CourseImages",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseImages");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
