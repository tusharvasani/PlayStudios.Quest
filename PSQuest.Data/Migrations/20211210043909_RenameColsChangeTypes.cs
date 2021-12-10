using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PSQuest.Data.Migrations
{
    public partial class RenameColsChangeTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_playerQuestStates",
                table: "playerQuestStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_playerQuestProgresses",
                table: "playerQuestProgresses");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Player");

            migrationBuilder.RenameTable(
                name: "playerQuestStates",
                newName: "PlayerQuestState");

            migrationBuilder.RenameTable(
                name: "playerQuestProgresses",
                newName: "PlayerQuestProgress");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalQuestPercentCompleted",
                table: "PlayerQuestState",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId",
                table: "PlayerQuestState",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId",
                table: "PlayerQuestProgress",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                table: "Player",
                column: "PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerQuestState",
                table: "PlayerQuestState",
                columns: new[] { "PlayerId", "QuestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerQuestProgress",
                table: "PlayerQuestProgress",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerQuestState",
                table: "PlayerQuestState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerQuestProgress",
                table: "PlayerQuestProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                table: "Player");

            migrationBuilder.RenameTable(
                name: "PlayerQuestState",
                newName: "playerQuestStates");

            migrationBuilder.RenameTable(
                name: "PlayerQuestProgress",
                newName: "playerQuestProgresses");

            migrationBuilder.RenameTable(
                name: "Player",
                newName: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "TotalQuestPercentCompleted",
                table: "playerQuestStates",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "playerQuestStates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "playerQuestProgresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_playerQuestStates",
                table: "playerQuestStates",
                columns: new[] { "PlayerId", "QuestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_playerQuestProgresses",
                table: "playerQuestProgresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "PlayerId");
        }
    }
}
