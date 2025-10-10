using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calories.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddMealsAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpectedCaloriesPerDay",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    MealDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    MealTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    MealDescription = table.Column<string>(type: "TEXT", nullable: false),
                    MealCalories = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meals_UserId",
                table: "Meals",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropColumn(
                name: "ExpectedCaloriesPerDay",
                table: "AspNetUsers");
        }
    }
}
