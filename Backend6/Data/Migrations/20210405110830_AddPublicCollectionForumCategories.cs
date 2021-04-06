using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class AddPublicCollectionForumCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ForumCategoryId",
                table: "Forums",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forums_ForumCategoryId",
                table: "Forums",
                column: "ForumCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forums_ForumCategories_ForumCategoryId",
                table: "Forums",
                column: "ForumCategoryId",
                principalTable: "ForumCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forums_ForumCategories_ForumCategoryId",
                table: "Forums");

            migrationBuilder.DropIndex(
                name: "IX_Forums_ForumCategoryId",
                table: "Forums");

            migrationBuilder.DropColumn(
                name: "ForumCategoryId",
                table: "Forums");
        }
    }
}
