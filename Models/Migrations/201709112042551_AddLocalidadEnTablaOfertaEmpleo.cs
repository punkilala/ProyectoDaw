namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocalidadEnTablaOfertaEmpleo : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inscritos", new[] { "Estado_id" });
            AddColumn("dbo.OfertaEmpleo", "Localidad", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Inscritos", "Estado_id", c => c.Int());
            AlterColumn("dbo.Inscritos", "estado_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inscritos", "Estado_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inscritos", new[] { "Estado_id" });
            AlterColumn("dbo.Inscritos", "estado_id", c => c.Int());
            AlterColumn("dbo.Inscritos", "Estado_id", c => c.Int(nullable: false));
            DropColumn("dbo.OfertaEmpleo", "Localidad");
            CreateIndex("dbo.Inscritos", "Estado_id");
        }
    }
}
