namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelacionEstadoInscritos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inscritos", "Estado_id", c => c.Int());
            CreateIndex("dbo.Inscritos", "Estado_id");
            AddForeignKey("dbo.Inscritos", "Estado_id", "dbo.Estado", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inscritos", "Estado_id", "dbo.Estado");
            DropIndex("dbo.Inscritos", new[] { "Estado_id" });
            AlterColumn("dbo.Inscritos", "Estado_id", c => c.Int(nullable: false));
        }
    }
}
