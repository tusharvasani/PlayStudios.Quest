using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PSQuest.Data.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "playerQuestProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerLevel = table.Column<int>(type: "int", nullable: false),
                    ChipAmountBet = table.Column<int>(type: "int", nullable: false),
                    InsertDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playerQuestProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "playerQuestStates",
                columns: table => new
                {
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalQuestPercentCompleted = table.Column<int>(type: "int", nullable: false),
                    LastMilestoneIndexCompleted = table.Column<int>(type: "int", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playerQuestStates", x => new { x.PlayerId, x.QuestId });
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "playerQuestProgresses");

            migrationBuilder.DropTable(
                name: "playerQuestStates");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
