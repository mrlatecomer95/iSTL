namespace iSTLDevEx.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduserProfileEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Email");
        }
    }
}
