namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablasIdiomaseIdioma : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Idioma",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Usuario_id = c.Int(nullable: false),
                        Idiomas_id = c.Int(nullable: false),
                        Hablado = c.String(nullable: false, maxLength: 20),
                        Escrito = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Idiomas", t => t.Idiomas_id, cascadeDelete: false)
                .ForeignKey("dbo.Usuario", t => t.Usuario_id, cascadeDelete: true)
                .Index(t => t.Usuario_id)
                .Index(t => t.Idiomas_id);
            
            CreateTable(
                "dbo.Idiomas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Idioma", "Usuario_id", "dbo.Usuario");
            DropForeignKey("dbo.Idioma", "Idiomas_id", "dbo.Idiomas");
            DropIndex("dbo.Idioma", new[] { "Idiomas_id" });
            DropIndex("dbo.Idioma", new[] { "Usuario_id" });
            DropTable("dbo.Idiomas");
            DropTable("dbo.Idioma");
        }
    }
}
