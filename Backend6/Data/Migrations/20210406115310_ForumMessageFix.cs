using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class ForumMessageFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ForumTopicId",
                table: "ForumMessages",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ForumMessages_ForumTopicId",
                table: "ForumMessages",
                column: "ForumTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicId",
                table: "ForumMessages",
                column: "ForumTopicId",
                principalTable: "ForumTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicId",
                table: "ForumMessages");

            migrationBuilder.DropIndex(
                name: "IX_ForumMessages_ForumTopicId",
                table: "ForumMessages");

            migrationBuilder.DropColumn(
                name: "ForumTopicId",
                table: "ForumMessages");
        }
    }
}
