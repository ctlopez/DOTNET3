namespace EventAppMVCPresentationLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseModels", "RoomId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseModels", "RoomId");
        }
    }
}
