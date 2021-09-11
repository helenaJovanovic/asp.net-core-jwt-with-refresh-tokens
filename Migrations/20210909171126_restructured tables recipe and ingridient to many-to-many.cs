using Microsoft.EntityFrameworkCore.Migrations;

namespace mycookingrecepies.Migrations
{
    public partial class restructuredtablesrecipeandingridienttomanytomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngridientId",
                table: "Recipes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_IngridientId",
                table: "Recipes",
                column: "IngridientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Ingridients_IngridientId",
                table: "Recipes",
                column: "IngridientId",
                principalTable: "Ingridients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Ingridients_IngridientId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_IngridientId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "IngridientId",
                table: "Recipes");
        }
    }
}
