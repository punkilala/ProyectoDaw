namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambioNombreEstadoIdTablaInscritosHistorial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InscritosHistorial", "Estado_id", "dbo.Estado");
            DropIndex("dbo.Inscritos", new[] { "Estado_id" });
            DropIndex("dbo.InscritosHistorial", new[] { "Estado_id" });
            RenameColumn(table: "dbo.InscritosHistorial", name: "Estado_id", newName: "EstadoId");
            AlterColumn("dbo.Inscritos", "Estado_id", c => c.Int());
            AlterColumn("dbo.Inscritos", "estado_id", c => c.Int(nullable: false));
            AlterColumn("dbo.InscritosHistorial", "EstadoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Inscritos", "Estado_id");
            CreateIndex("dbo.InscritosHistorial", "EstadoId");
            AddForeignKey("dbo.InscritosHistorial", "EstadoId", "dbo.Estado", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InscritosHistorial", "EstadoId", "dbo.Estado");
            DropIndex("dbo.InscritosHistorial", new[] { "EstadoId" });
            DropIndex("dbo.Inscritos", new[] { "Estado_id" });
            AlterColumn("dbo.InscritosHistorial", "EstadoId", c => c.Int());
            AlterColumn("dbo.Inscritos", "estado_id", c => c.Int());
            AlterColumn("dbo.Inscritos", "Estado_id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.InscritosHistorial", name: "EstadoId", newName: "Estado_id");
            CreateIndex("dbo.InscritosHistorial", "Estado_id");
            CreateIndex("dbo.Inscritos", "Estado_id");
            AddForeignKey("dbo.InscritosHistorial", "Estado_id", "dbo.Estado", "id");
        }
    }
}
