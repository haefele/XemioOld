using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xemio.Server.Infrastructure.Database.Migrations
{
    public partial class CreateFolders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ETag = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ParentFolderId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_UserId",
                table: "Folders",
                column: "UserId");

            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[TRI_Folders_InsteadOfDelete] ON [dbo].[Folders]
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    WITH foldersToDelete AS (
        SELECT  Id, CAST(1 AS INT) AS FolderLevel
        FROM    dbo.Folders
        WHERE   Id IN (SELECT Id FROM deleted)
        UNION ALL
        SELECT  f.Id, ftd.FolderLevel + 1
        FROM    dbo.Folders f
                JOIN foldersToDelete ftd ON f.ParentFolderId = ftd.Id AND f.ParentFolderId != f.Id
    )
    SELECT Id, FolderLevel
    INTO #foldersToDelete
    FROM foldersToDelete;

    DECLARE folderCursor CURSOR FOR SELECT Id FROM #foldersToDelete ORDER BY FolderLevel DESC;
    DECLARE @folder_id UNIQUEIDENTIFIER;

    OPEN folderCursor;

    FETCH NEXT FROM folderCursor INTO @folder_id
    WHILE @@FETCH_STATUS = 0 BEGIN
        DELETE FROM dbo.Folders
        WHERE Id = @folder_id;

        FETCH NEXT FROM folderCursor INTO @folder_id;
    END;

    CLOSE folderCursor;
END;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Folders");
        }
    }
}
