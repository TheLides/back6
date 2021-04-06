using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class ChangeForum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forums_ForumCategories_ForumCategoryId",
                table: "Forums");

            migrationBuilder.AlterColumn<Guid>(
                name: "ForumCategoryId",
                table: "Forums",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Forums_ForumCategories_ForumCategoryId",
                table: "Forums",
                column: "ForumCategoryId",
                principalTable: "ForumCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forums_ForumCategories_ForumCategoryId",
                table: "Forums");

            migrationBuilder.AlterColumn<Guid>(
                name: "ForumCategoryId",
                table: "Forums",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Forums_ForumCategories_ForumCategoryId",
                table: "Forums",
                column: "ForumCategoryId",
                principalTable: "ForumCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
