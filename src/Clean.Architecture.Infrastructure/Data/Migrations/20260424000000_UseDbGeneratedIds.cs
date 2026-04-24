using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Architecture.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class UseDbGeneratedIds : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
        {
            // SQL Server requires drop/recreate to add IDENTITY to an existing column.
            // Rename the old table, recreate with IDENTITY, copy data, then drop old.
            migrationBuilder.Sql("EXEC sp_rename N'[Contributors]', N'[Contributors_old]'");
            migrationBuilder.Sql("EXEC sp_rename N'[Contributors_old].[PK_Contributors]', N'PK_Contributors_old', N'INDEX'");

            migrationBuilder.CreateTable(
                name: "Contributors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber_CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber_Extension = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributors", x => x.Id);
                });

            migrationBuilder.Sql(
                "SET IDENTITY_INSERT [Contributors] ON;" +
                "INSERT INTO [Contributors] ([Id],[Name],[Status],[PhoneNumber_CountryCode],[PhoneNumber_Number],[PhoneNumber_Extension]) " +
                "SELECT [Id],[Name],[Status],[PhoneNumber_CountryCode],[PhoneNumber_Number],[PhoneNumber_Extension] FROM [Contributors_old];" +
                "SET IDENTITY_INSERT [Contributors] OFF;");

            migrationBuilder.DropTable("Contributors_old");
        }
        // SQLite already has AUTOINCREMENT from the initial migration — no change needed.
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
        {
            migrationBuilder.Sql("EXEC sp_rename N'[Contributors]', N'[Contributors_old]'");
            migrationBuilder.Sql("EXEC sp_rename N'[Contributors_old].[PK_Contributors]', N'PK_Contributors_old', N'INDEX'");

            migrationBuilder.CreateTable(
                name: "Contributors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber_CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber_Extension = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributors", x => x.Id);
                });

            migrationBuilder.Sql(
                "INSERT INTO [Contributors] ([Id],[Name],[Status],[PhoneNumber_CountryCode],[PhoneNumber_Number],[PhoneNumber_Extension]) " +
                "SELECT [Id],[Name],[Status],[PhoneNumber_CountryCode],[PhoneNumber_Number],[PhoneNumber_Extension] FROM [Contributors_old];");

            migrationBuilder.DropTable("Contributors_old");
        }
    }
}
