namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCamposSociales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Facebook", c => c.String());
            AddColumn("dbo.Usuario", "Twitter", c => c.String());
            AddColumn("dbo.Usuario", "Linkedin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Linkedin");
            DropColumn("dbo.Usuario", "Twitter");
            DropColumn("dbo.Usuario", "Facebook");
        }
    }
}
