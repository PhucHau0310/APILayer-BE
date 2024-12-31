using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APILayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentIdToBigInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Payment DROP CONSTRAINT PK_Payment;");
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Payment",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
           migrationBuilder.Sql("ALTER TABLE Payment ADD CONSTRAINT PK_Payment PRIMARY KEY (Id);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Payment",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
