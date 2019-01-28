namespace ogaMadamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmployeeAddObjective : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Objective", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Objective");
        }
    }
}
