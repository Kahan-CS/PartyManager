using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PartyManager.Migrations
{
    /// <inheritdoc />
    public partial class seeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    PartyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.PartyId);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    InvitationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuestEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.InvitationId);
                    table.ForeignKey(
                        name: "FK_Invitations_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "PartyId", "Description", "EventDate", "Location" },
                values: new object[,]
                {
                    { 1, "New Year's Eve Blast!!", new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Time Square, NY" },
                    { 2, "Drinks at Moe's Bar", new DateTime(2025, 10, 30, 16, 43, 12, 0, DateTimeKind.Unspecified), "Moe's Bar, Springfield" },
                    { 3, "Thanksgiving Gathering", new DateTime(2025, 10, 20, 16, 43, 12, 0, DateTimeKind.Unspecified), "Springfield" }
                });

            migrationBuilder.InsertData(
                table: "Invitations",
                columns: new[] { "InvitationId", "GuestEmail", "GuestName", "PartyId", "Status" },
                values: new object[,]
                {
                    { 1, "pmadziak@conestogac.on.ca", "Bob Jones", 1, "InviteNotSent" },
                    { 2, "peter.madziak@gmail.com", "Sally Smith", 1, "InviteNotSent" },
                    { 3, "pmadziak@conestogac.on.ca", "Bob Jones", 2, "InviteNotSent" },
                    { 4, "peter.madziak@gmail.com", "Sally Smith", 2, "InviteNotSent" },
                    { 5, "pmadziak@conestogac.on.ca", "Bob Jones", 3, "InviteNotSent" },
                    { 6, "peter.madziak@gmail.com", "Sally Smith", 3, "InviteNotSent" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_PartyId",
                table: "Invitations",
                column: "PartyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "Parties");
        }
    }
}
