namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reestablacerestado_idtablaIncritosyCrearForeignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inscritos", "estado_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inscritos", "estado_id");
            AddForeignKey("dbo.Inscritos", "estado_id", "dbo.Estado", "id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inscritos", "estado_id", "dbo.Estado");
            DropIndex("dbo.Inscritos", new[] { "estado_id" });
            DropColumn("dbo.Inscritos", "estado_id");
        }
    }
}
