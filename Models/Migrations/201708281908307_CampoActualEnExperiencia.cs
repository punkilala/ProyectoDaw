namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoActualEnExperiencia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Experiencia", "Actual", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Experiencia", "Actual");
        }
    }
}
