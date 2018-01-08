using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Northwind.IDP.Migrations
{
    public partial class User_clasic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    SubjectId = table.Column<string>(maxLength: 50, nullable: false),
                    Documento = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    TipoDocumento = table.Column<int>(nullable: false),
                    Username = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    ClaimType = table.Column<string>(maxLength: 250, nullable: false),
                    ClaimValue = table.Column<string>(maxLength: 250, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Users_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumentos",
                columns: table => new
                {
                    DocumentoID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    UserSubjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumentos", x => x.DocumentoID);
                    table.ForeignKey(
                        name: "FK_TipoDocumentos_Users_UserSubjectId",
                        column: x => x.UserSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 250, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 250, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_SubjectId",
                table: "Claims",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumentos_UserSubjectId",
                table: "TipoDocumentos",
                column: "UserSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_SubjectId",
                table: "UserLogins",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "TipoDocumentos");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");
        }
    }
}
