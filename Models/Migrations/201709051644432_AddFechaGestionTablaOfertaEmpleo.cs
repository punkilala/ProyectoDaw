namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFechaGestionTablaOfertaEmpleo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfertaEmpleo", "FechaGestion", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OfertaEmpleo", "FechaGestion");
        }
    }
}
