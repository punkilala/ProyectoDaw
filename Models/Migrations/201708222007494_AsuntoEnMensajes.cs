namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AsuntoEnMensajes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mensaje", "Asunto", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mensaje", "Asunto");
        }
    }
}
