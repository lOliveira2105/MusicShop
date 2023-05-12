namespace MusicShop.ProductApi.Migrations;

/// <inheritdoc />
public partial class SeedProducts : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder mb)
    {
        mb.Sql("Insert into Products(Name, Price, Description, Stock, ImageUrl, CategoryId)" + "Values('Blonde',9.99,'Blonde is the seconde studio album by American singer Frank Ocean', 10, 'blonde.png', 1)");       
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder mb)
    {
        mb.Sql("delete from Products");
    }
}
