using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DICOM_Router.Migrations
{
    /// <inheritdoc />
    public partial class AddedPrimarykeyIncoming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: true),
                    Device_Name = table.Column<string>(type: "TEXT", nullable: true),
                    IP_Address = table.Column<string>(type: "TEXT", nullable: true),
                    Calling_AE = table.Column<string>(type: "TEXT", nullable: true),
                    Called_AE = table.Column<string>(type: "TEXT", nullable: true),
                    Port = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Incoming Studies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Date_Rec = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_ID = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Modality_Type = table.Column<string>(type: "TEXT", nullable: true),
                    Image_Count = table.Column<int>(type: "INTEGER", nullable: true),
                    SUID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Outging_Studies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: true),
                    Date_Sent = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_ID = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Modality_Type = table.Column<string>(type: "TEXT", nullable: true),
                    Image_Count = table.Column<int>(type: "INTEGER", nullable: true),
                    Rule_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Device_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    SUID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Routing_Rules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Source_Tag = table.Column<string>(type: "TEXT", nullable: true),
                    Dest_Tag = table.Column<string>(type: "TEXT", nullable: true),
                    Replace_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Match_Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    AE_Title = table.Column<string>(type: "TEXT", nullable: true),
                    Port = table.Column<int>(type: "INTEGER", nullable: true),
                    Logging_Mode = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Incoming Studies");

            migrationBuilder.DropTable(
                name: "Outging_Studies");

            migrationBuilder.DropTable(
                name: "Routing_Rules");

            migrationBuilder.DropTable(
                name: "Server");
        }
    }
}
