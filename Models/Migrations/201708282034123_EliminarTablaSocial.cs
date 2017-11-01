namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminarTablaSocial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Social", "Usuario_id", "dbo.Usuario");
            DropIndex("dbo.Social", new[] { "Usuario_id" });
            DropTable("dbo.Social");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Social",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Usuario_id = c.Int(nullable: false),
                        nombre = c.String(nullable: false, maxLength: 50, unicode: false),
                        url = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateIndex("dbo.Social", "Usuario_id");
            AddForeignKey("dbo.Social", "Usuario_id", "dbo.Usuario", "id", cascadeDelete: true);
        }
    }
}
