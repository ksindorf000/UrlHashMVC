namespace Sonnetly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFavoritesModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Favorites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookmarkId = c.Int(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookmarks", t => t.BookmarkId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.BookmarkId)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Favorites", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Favorites", "BookmarkId", "dbo.Bookmarks");
            DropIndex("dbo.Favorites", new[] { "OwnerId" });
            DropIndex("dbo.Favorites", new[] { "BookmarkId" });
            DropTable("dbo.Favorites");
        }
    }
}
