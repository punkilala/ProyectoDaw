namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiarNombreEstadoTablaInscritos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inscritos", "estado_id", c => c.Int(nullable: false));
            DropColumn("dbo.Inscritos", "estado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inscritos", "estado", c => c.Int(nullable: false));
            DropColumn("dbo.Inscritos", "estado_id");
        }
    }
}
