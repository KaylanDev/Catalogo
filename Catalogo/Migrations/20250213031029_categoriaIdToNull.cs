using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalogo.Migrations
{
    /// <inheritdoc />
    public partial class categoriaIdToNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categoria_CategoriaId",
                table: "Produtos");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Produtos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Produtos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categoria_CategoriaId",
                table: "Produtos",
                column: "CategoriaId",
                principalTable: "Categoria",
                principalColumn: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categoria_CategoriaId",
                table: "Produtos");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Produtos",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categoria_CategoriaId",
                table: "Produtos",
                column: "CategoriaId",
                principalTable: "Categoria",
                principalColumn: "CategoriaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
