using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnketSistemi.API.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserResponses_AspNetUsers_AppUserId",
                table: "UserResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponses_PollQuestions_PollQuestionId",
                table: "UserResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponses_Polls_PollId",
                table: "UserResponses");

            migrationBuilder.AlterColumn<string>(
                name: "Detail",
                table: "Polls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalViews",
                table: "Polls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PollFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Score = table.Column<byte>(type: "tinyint", nullable: false),
                    UserComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollFeedbacks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PollFeedbacks_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PollFeedbacks_PollId",
                table: "PollFeedbacks",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_PollFeedbacks_UserId",
                table: "PollFeedbacks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponses_AspNetUsers_AppUserId",
                table: "UserResponses",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponses_PollQuestions_PollQuestionId",
                table: "UserResponses",
                column: "PollQuestionId",
                principalTable: "PollQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponses_Polls_PollId",
                table: "UserResponses",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserResponses_AspNetUsers_AppUserId",
                table: "UserResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponses_PollQuestions_PollQuestionId",
                table: "UserResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponses_Polls_PollId",
                table: "UserResponses");

            migrationBuilder.DropTable(
                name: "PollFeedbacks");

            migrationBuilder.DropColumn(
                name: "TotalViews",
                table: "Polls");

            migrationBuilder.AlterColumn<string>(
                name: "Detail",
                table: "Polls",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponses_AspNetUsers_AppUserId",
                table: "UserResponses",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponses_PollQuestions_PollQuestionId",
                table: "UserResponses",
                column: "PollQuestionId",
                principalTable: "PollQuestions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponses_Polls_PollId",
                table: "UserResponses",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id");
        }
    }
}
