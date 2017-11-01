namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampoWebyInfoGeneralTablaUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Web", c => c.String());
            AddColumn("dbo.Usuario", "InfoGeneral", c => c.String(unicode: false, storeType: "text"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "InfoGeneral");
            DropColumn("dbo.Usuario", "Web");
        }
    }
}
