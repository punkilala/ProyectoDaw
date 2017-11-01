namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modficaTablaOfertaEmpleo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfertaEmpleo", "Pago", c => c.String());
            AddColumn("dbo.OfertaEmpleo", "ModoPago", c => c.String());
            AlterColumn("dbo.OfertaEmpleo", "Salario", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OfertaEmpleo", "Salario", c => c.Decimal(storeType: "smallmoney"));
            DropColumn("dbo.OfertaEmpleo", "ModoPago");
            DropColumn("dbo.OfertaEmpleo", "Pago");
        }
    }
}
