namespace Sonnetly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClicksLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClicksLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookmarkId = c.Int(nullable: false),
                        UserName = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClicksLogs");
        }
    }
}
