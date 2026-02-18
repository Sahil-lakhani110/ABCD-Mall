using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lakhani.Migrations
{
    /// <inheritdoc />
    public partial class AddMoviesAndBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "Movies",
    columns: table => new
    {
        Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        MovieName = table.Column<string>(type: "nvarchar(max)", nullable: false),
        ShowTime = table.Column<TimeSpan>(type: "time", nullable: false),
        TotalSeats = table.Column<int>(type: "int", nullable: false),
        AvailableSeats = table.Column<int>(type: "int", nullable: false),
        Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Movies", x => x.Id);
    });

            migrationBuilder.CreateTable(
                name: "MovieBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatsBooked = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieBookings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieBookings_MovieId",
                table: "MovieBookings",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieBookings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ShowTime",
                table: "Movies",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<string>(
                name: "MovieName",
                table: "Movies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
