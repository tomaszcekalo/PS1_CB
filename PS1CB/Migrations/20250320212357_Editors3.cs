﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PS1CB.Migrations
{
    /// <inheritdoc />
    public partial class Editors3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Editors_Messages_MessageId",
                table: "Editors");

            migrationBuilder.DropIndex(
                name: "IX_Editors_MessageId",
                table: "Editors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Editors_MessageId",
                table: "Editors",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Editors_Messages_MessageId",
                table: "Editors",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
