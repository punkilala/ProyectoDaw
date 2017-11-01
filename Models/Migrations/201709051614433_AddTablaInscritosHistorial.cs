namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablaInscritosHistorial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InscritosHistorial",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Usuario_id_D = c.Int(nullable: false),
                        Oferta_id = c.Int(nullable: false),
                        estado_id = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Inscritos", t => new { t.Usuario_id_D, t.Oferta_id }, cascadeDelete: true)
                .Index(t => t.Usuario_id_D)
                .Index(t => new { t.Usuario_id_D, t.Oferta_id })
                .Index(t => t.Oferta_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InscritosHistorial", new[] { "Usuario_id_D", "Oferta_id" }, "dbo.Inscritos");
            DropIndex("dbo.InscritosHistorial", new[] { "Oferta_id" });
            DropIndex("dbo.InscritosHistorial", new[] { "Usuario_id_D", "Oferta_id" });
            DropIndex("dbo.InscritosHistorial", new[] { "Usuario_id_D" });
            DropTable("dbo.InscritosHistorial");
        }
    }
}
