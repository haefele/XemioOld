using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xemio.Server.Infrastructure.Migrations
{
    public partial class RenameTimeStampToETag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Folders",
                newName: "ETag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ETag",
                table: "Folders",
                newName: "TimeStamp");
        }
    }
}
