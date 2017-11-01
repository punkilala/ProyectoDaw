namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoFecha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inscritos", "Fecha", c => c.DateTime(nullable: false));
            DropColumn("dbo.Inscritos", "relacion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inscritos", "relacion", c => c.String(nullable: false, maxLength: 50, unicode: false));
            DropColumn("dbo.Inscritos", "Fecha");
        }
    }
}
