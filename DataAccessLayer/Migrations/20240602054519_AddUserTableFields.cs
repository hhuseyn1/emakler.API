using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTableFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "building_type",
                columns: table => new
                {
                    id_building_type = table.Column<int>(type: "int", nullable: false),
                    building_type_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "document",
                columns: table => new
                {
                    id_document = table.Column<int>(type: "int", nullable: false),
                    document_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    keyword_01 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_02 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_03 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document", x => x.id_document);
                });

            migrationBuilder.CreateTable(
                name: "metro",
                columns: table => new
                {
                    id_metro = table.Column<int>(type: "int", nullable: false),
                    metro_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    fk_id_region = table.Column<int>(type: "int", nullable: true),
                    keyword_01 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_02 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_03 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_04 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_05 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metro", x => x.id_metro);
                });

            migrationBuilder.CreateTable(
                name: "operation_type",
                columns: table => new
                {
                    id_operation_type = table.Column<int>(type: "int", nullable: false),
                    operation_type_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operation_type", x => x.id_operation_type);
                });

            migrationBuilder.CreateTable(
                name: "owner_type",
                columns: table => new
                {
                    id_owner_type = table.Column<int>(type: "int", nullable: false),
                    owner_type_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_owner_type", x => x.id_owner_type);
                });

            migrationBuilder.CreateTable(
                name: "property",
                columns: table => new
                {
                    id_property = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fk_id_source = table.Column<int>(type: "int", nullable: true),
                    fk_id_link = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fk_id_property_type = table.Column<int>(type: "int", nullable: true),
                    fk_id_operation_type = table.Column<int>(type: "int", nullable: true),
                    fk_id_city = table.Column<int>(type: "int", nullable: true),
                    address = table.Column<string>(type: "ntext", nullable: true),
                    fk_id_document = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    fk_id_currency = table.Column<int>(type: "int", nullable: true),
                    data = table.Column<string>(type: "ntext", nullable: true),
                    area = table.Column<double>(type: "float", nullable: true),
                    general_area = table.Column<double>(type: "float", nullable: true),
                    floor = table.Column<int>(type: "int", nullable: true),
                    floor_of = table.Column<int>(type: "int", nullable: true),
                    fk_id_room = table.Column<int>(type: "int", nullable: true),
                    fk_id_building_type = table.Column<int>(type: "int", nullable: true),
                    unit_price = table.Column<double>(type: "float", nullable: true),
                    eX = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    eY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cp_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cp_phone_number_01 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cp_phone_number_02 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cp_phone_number_03 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fk_id_owner_type = table.Column<int>(type: "int", nullable: true),
                    images = table.Column<string>(type: "ntext", nullable: true),
                    insert_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    upload_status = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    upload_message = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    fk_id_metro = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    approvment_status = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    approvment_message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fk_id_repair = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    fk_id_target = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_property", x => x.id_property);
                });

            migrationBuilder.CreateTable(
                name: "property_type",
                columns: table => new
                {
                    id_property_type = table.Column<int>(type: "int", nullable: false),
                    property_type_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_property_type", x => x.id_property_type);
                });

            migrationBuilder.CreateTable(
                name: "region",
                columns: table => new
                {
                    id_region = table.Column<int>(type: "int", nullable: true),
                    region_code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    region_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    keyword_01 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_02 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_03 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "region_unit_01",
                columns: table => new
                {
                    id_region = table.Column<int>(type: "int", nullable: true),
                    region_code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    region_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    keyword_01 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_02 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_03 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "repair_rate",
                columns: table => new
                {
                    id_repair_rate = table.Column<int>(type: "int", nullable: false),
                    repair_rate_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repair_rate", x => x.id_repair_rate);
                });

            migrationBuilder.CreateTable(
                name: "room_count",
                columns: table => new
                {
                    id_room_count = table.Column<int>(type: "int", nullable: false),
                    room_count_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_count", x => x.id_room_count);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserMail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ContactNumber = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
                    OtpCode = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: true),
                    OtpCreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValidate = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4CDE2E4347", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_building_type",
                table: "building_type",
                column: "id_building_type");

            migrationBuilder.CreateIndex(
                name: "IX_fk_id_region",
                table: "metro",
                column: "fk_id_region");

            migrationBuilder.CreateIndex(
                name: "IX_property_approvment_status",
                table: "property",
                column: "approvment_status");

            migrationBuilder.CreateIndex(
                name: "IX_property_code",
                table: "property",
                column: "code",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_property_cp_phone_number_01",
                table: "property",
                column: "cp_phone_number_01");

            migrationBuilder.CreateIndex(
                name: "IX_property_cp_phone_number_02",
                table: "property",
                column: "cp_phone_number_02");

            migrationBuilder.CreateIndex(
                name: "IX_property_cp_phone_number_03",
                table: "property",
                column: "cp_phone_number_03");

            migrationBuilder.CreateIndex(
                name: "IX_property_fk_id_link",
                table: "property",
                column: "fk_id_link");

            migrationBuilder.CreateIndex(
                name: "IX_property_fk_id_owner_type",
                table: "property",
                column: "fk_id_owner_type");

            migrationBuilder.CreateIndex(
                name: "IX_property_fk_id_source",
                table: "property",
                column: "fk_id_source");

            migrationBuilder.CreateIndex(
                name: "IX_property_insert_date",
                table: "property",
                column: "insert_date",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_property_upload_status",
                table: "property",
                column: "upload_status");

            migrationBuilder.CreateIndex(
                name: "IX_id_region",
                table: "region",
                column: "id_region",
                unique: true,
                filter: "[id_region] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_region_code",
                table: "region",
                column: "region_code");

            migrationBuilder.CreateIndex(
                name: "IX_region_unit_01_id_region",
                table: "region_unit_01",
                column: "id_region",
                unique: true,
                filter: "[id_region] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_region_unit_01_region_code",
                table: "region_unit_01",
                column: "region_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "building_type");

            migrationBuilder.DropTable(
                name: "document");

            migrationBuilder.DropTable(
                name: "metro");

            migrationBuilder.DropTable(
                name: "operation_type");

            migrationBuilder.DropTable(
                name: "owner_type");

            migrationBuilder.DropTable(
                name: "property");

            migrationBuilder.DropTable(
                name: "property_type");

            migrationBuilder.DropTable(
                name: "region");

            migrationBuilder.DropTable(
                name: "region_unit_01");

            migrationBuilder.DropTable(
                name: "repair_rate");

            migrationBuilder.DropTable(
                name: "room_count");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
