namespace iSTLDevEx.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperatorIDUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "OperatorID", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "OperatorID");
        }
    }
}
