namespace WebApplication156456.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreData1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "apellidos", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "apellido");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "apellidos", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "apellidos");
        }
    }
}
