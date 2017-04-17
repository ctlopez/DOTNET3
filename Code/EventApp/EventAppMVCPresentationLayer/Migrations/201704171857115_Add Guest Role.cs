namespace EventAppMVCPresentationLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuestRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventWithEmployees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EventID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 300),
                        Date = c.String(nullable: false),
                        Time = c.String(nullable: false),
                        Location = c.String(nullable: false, maxLength: 300),
                        MaxSeats = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AddedBy = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        EmployeeCreater_EmployeeID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employees", t => t.EmployeeCreater_EmployeeID)
                .Index(t => t.EmployeeCreater_EmployeeID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        UserName = c.String(),
                        Active = c.Boolean(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        User_EmployeeID = c.Int(),
                    })
                .PrimaryKey(t => t.RoleID)
                .ForeignKey("dbo.Employees", t => t.User_EmployeeID)
                .Index(t => t.User_EmployeeID);
            
            CreateTable(
                "dbo.PurchaseModels",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 300),
                        Date = c.String(nullable: false),
                        Time = c.String(nullable: false),
                        Location = c.String(nullable: false, maxLength: 300),
                        MaxSeats = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AddedBy = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Quantity = c.Int(nullable: false),
                        EmployeeCreater_EmployeeID = c.Int(),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.Employees", t => t.EmployeeCreater_EmployeeID)
                .Index(t => t.EmployeeCreater_EmployeeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseModels", "EmployeeCreater_EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EventWithEmployees", "EmployeeCreater_EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Roles", "User_EmployeeID", "dbo.Employees");
            DropIndex("dbo.PurchaseModels", new[] { "EmployeeCreater_EmployeeID" });
            DropIndex("dbo.Roles", new[] { "User_EmployeeID" });
            DropIndex("dbo.EventWithEmployees", new[] { "EmployeeCreater_EmployeeID" });
            DropTable("dbo.PurchaseModels");
            DropTable("dbo.Roles");
            DropTable("dbo.Employees");
            DropTable("dbo.EventWithEmployees");
        }
    }
}
