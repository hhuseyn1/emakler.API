using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "building",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    metro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    village = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    district = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    room_count = table.Column<int>(type: "int", nullable: false),
                    area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ad_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    seller_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_building", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "building_type",
                columns: table => new
                {
                    id_building_type = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    building_type_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_building_type", x => x.id_building_type);
                });

            migrationBuilder.CreateTable(
                name: "document",
                columns: table => new
                {
                    id_document = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    id_metro = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    metro_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    fk_id_region = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    id_operation_type = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    id_owner_type = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    id_property = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkIdSource = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FkIdLink = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fk_id_property_type = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fk_id_operation_type = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fk_id_city = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    address = table.Column<string>(type: "ntext", nullable: true),
                    fk_id_document = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    fk_id_currency = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    data = table.Column<string>(type: "ntext", nullable: true),
                    area = table.Column<double>(type: "float", nullable: true),
                    general_area = table.Column<double>(type: "float", nullable: true),
                    floor = table.Column<int>(type: "int", nullable: true),
                    floor_of = table.Column<int>(type: "int", nullable: true),
                    FkIdRoom = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fk_id_building_type = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    unit_price = table.Column<double>(type: "float", nullable: true),
                    ex = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cp_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cp_phone_number_01 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    cp_phone_number_02 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    cp_phone_number_03 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fk_id_owner_type = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    images = table.Column<string>(type: "ntext", nullable: true),
                    insert_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    upload_status = table.Column<int>(type: "int", nullable: true),
                    upload_message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fk_id_metro = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    approvment_status = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    approvment_message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fk_id_repair = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fk_id_target = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_property", x => x.id_property);
                });

            migrationBuilder.CreateTable(
                name: "property_type",
                columns: table => new
                {
                    id_property_type = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    id_region = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    region_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    region_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_01 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_02 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_03 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_region", x => x.id_region);
                });

            migrationBuilder.CreateTable(
                name: "region_unit_01",
                columns: table => new
                {
                    id_region = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    region_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    region_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_01 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_02 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword_03 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_region_unit_01", x => x.id_region);
                });

            migrationBuilder.CreateTable(
                name: "repair_rate",
                columns: table => new
                {
                    id_repair_rate = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    repair_rate_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repair_rate", x => x.id_repair_rate);
                });

            migrationBuilder.CreateTable(
                name: "room_count",
                columns: table => new
                {
                    id_room_count = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    room_count_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyword = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_count", x => x.id_room_count);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_mail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    contact_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    otp_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    otp_created_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_validate = table.Column<bool>(type: "bit", nullable: false),
                    user_password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    password_salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "building_post",
                columns: table => new
                {
                    building_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_building_post", x => x.building_id);
                    table.ForeignKey(
                        name: "FK_building_post_building_building_id",
                        column: x => x.building_id,
                        principalTable: "building",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                column: "FkIdLink");

            migrationBuilder.CreateIndex(
                name: "IX_property_fk_id_owner_type",
                table: "property",
                column: "fk_id_owner_type");

            migrationBuilder.CreateIndex(
                name: "IX_property_fk_id_source",
                table: "property",
                column: "FkIdSource");

            migrationBuilder.CreateIndex(
                name: "IX_property_insert_date",
                table: "property",
                column: "insert_date",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_property_upload_status",
                table: "property",
                column: "upload_status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "building_post");

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
                name: "user");

            migrationBuilder.DropTable(
                name: "building");
        }
    }
}
