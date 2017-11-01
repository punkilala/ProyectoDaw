namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoAbiertaenOfertaEmpleo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfertaEmpleo", "Abierta", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OfertaEmpleo", "Abierta");
        }
    }
}
