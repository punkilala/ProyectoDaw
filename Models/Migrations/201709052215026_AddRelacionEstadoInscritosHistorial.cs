namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelacionEstadoInscritosHistorial : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inscritos", new[] { "Estado_id" });
            AlterColumn("dbo.Inscritos", "Estado_id", c => c.Int());
            AlterColumn("dbo.Inscritos", "estado_id", c => c.Int(nullable: false));
            AlterColumn("dbo.InscritosHistorial", "Estado_id", c => c.Int());
            CreateIndex("dbo.Inscritos", "Estado_id");
            CreateIndex("dbo.InscritosHistorial", "Estado_id");
            AddForeignKey("dbo.InscritosHistorial", "Estado_id", "dbo.Estado", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InscritosHistorial", "Estado_id", "dbo.Estado");
            DropIndex("dbo.InscritosHistorial", new[] { "Estado_id" });
            DropIndex("dbo.Inscritos", new[] { "Estado_id" });
            AlterColumn("dbo.InscritosHistorial", "Estado_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inscritos", "estado_id", c => c.Int());
            AlterColumn("dbo.Inscritos", "Estado_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inscritos", "Estado_id");
        }
    }
}
