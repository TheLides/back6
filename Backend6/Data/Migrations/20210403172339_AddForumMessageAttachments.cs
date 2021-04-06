using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class AddForumMessageAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics");

            migrationBuilder.CreateTable(
                name: "ForumMessageAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    FilePath = table.Column<string>(nullable: false),
                    ForumMessageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumMessageAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumMessageAttachments_ForumMessages_ForumMessageId",
                        column: x => x.ForumMessageId,
                        principalTable: "ForumMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumMessageAttachments_ForumMessageId",
                table: "ForumMessageAttachments",
                column: "ForumMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics");

            migrationBuilder.DropTable(
                name: "ForumMessageAttachments");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
