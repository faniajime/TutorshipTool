namespace WebApplication156456.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "nombre", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "apellido", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "descripcion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "descripcion");
            DropColumn("dbo.AspNetUsers", "apellido");
            DropColumn("dbo.AspNetUsers", "nombre");
        }
    }
}
