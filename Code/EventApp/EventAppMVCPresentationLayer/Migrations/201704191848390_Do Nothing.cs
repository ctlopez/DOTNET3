namespace EventAppMVCPresentationLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoNothing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Roles", "User_EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EventWithEmployees", "EmployeeCreater_EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.PurchaseModels", "EmployeeCreater_EmployeeID", "dbo.Employees");
            DropIndex("dbo.EventWithEmployees", new[] { "EmployeeCreater_EmployeeID" });
            DropIndex("dbo.Roles", new[] { "User_EmployeeID" });
            DropIndex("dbo.PurchaseModels", new[] { "EmployeeCreater_EmployeeID" });
            DropTable("dbo.EventWithEmployees");
            DropTable("dbo.Employees");
            DropTable("dbo.Roles");
            DropTable("dbo.PurchaseModels");
        }
        
        public override void Down()
        {
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
                        CurrentAmount = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        RoomId = c.String(),
                        EmployeeCreater_EmployeeID = c.Int(),
                    })
                .PrimaryKey(t => t.EventID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        User_EmployeeID = c.Int(),
                    })
                .PrimaryKey(t => t.RoleID);
            
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
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.PurchaseModels", "EmployeeCreater_EmployeeID");
            CreateIndex("dbo.Roles", "User_EmployeeID");
            CreateIndex("dbo.EventWithEmployees", "EmployeeCreater_EmployeeID");
            AddForeignKey("dbo.PurchaseModels", "EmployeeCreater_EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.EventWithEmployees", "EmployeeCreater_EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.Roles", "User_EmployeeID", "dbo.Employees", "EmployeeID");
        }
    }
}
