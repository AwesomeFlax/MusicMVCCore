using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MusicAssistant_CoreMVC.Data.Migrations
{
    public partial class extendModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SongText",
                table: "Songs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 900,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Albums",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 400,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SongText",
                table: "Songs",
                maxLength: 900,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Albums",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
