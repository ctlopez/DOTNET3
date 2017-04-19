namespace EventAppMVCPresentationLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CatchUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseModels", "CurrentAmount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseModels", "CurrentAmount");
        }
    }
}
