using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartTodo.Data.Migrations
{
    public partial class Initital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimeCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateTimeCompleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "DateTimeCompleted", "DateTimeCreated", "IsCompleted", "Title" },
                values: new object[] { "e3649d5c-95e2-4fa1-bed3-976b4442095a", null, new DateTime(2021, 7, 30, 17, 53, 23, 695, DateTimeKind.Local).AddTicks(4777), false, "Wash dishes" });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "DateTimeCompleted", "DateTimeCreated", "IsCompleted", "Title" },
                values: new object[] { "524bda5a-3e8b-4814-a601-e56172f95370", new DateTime(2021, 7, 30, 15, 53, 23, 697, DateTimeKind.Local).AddTicks(4171), new DateTime(2021, 7, 29, 17, 53, 23, 697, DateTimeKind.Local).AddTicks(4139), true, "Go shopping" });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "DateTimeCompleted", "DateTimeCreated", "IsCompleted", "Title" },
                values: new object[] { "9072e8c2-e00a-4899-8e13-e210de105d8a", null, new DateTime(2021, 7, 30, 22, 53, 23, 697, DateTimeKind.Local).AddTicks(4478), false, "Meet friends" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems");
        }
    }
}
