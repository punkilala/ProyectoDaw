namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Borrarestado_idTablaInscritosparaRepararError : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inscritos", "Estado_id", "dbo.Estado");
            DropIndex("dbo.Inscritos", new[] { "Estado_id" });
            DropColumn("dbo.Inscritos", "estado_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inscritos", "estado_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inscritos", "Estado_id");
            AddForeignKey("dbo.Inscritos", "Estado_id", "dbo.Estado", "id");
        }
    }
}
