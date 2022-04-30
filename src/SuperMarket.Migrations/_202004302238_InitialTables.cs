using FluentMigrator;

namespace SuperMarket.Migrations
{
    [Migration(202004302238)]
    public class _202004302238_InitialTables : Migration
    {
        public override void Up()
        {
            CreateCategory();
            CreateStuff();
            CreateInvoice();
            CreateVouchers();
        }

        public override void Down()
        {
            Delete.Table("Invoices");
            Delete.Table("Vouchers");
            Delete.Table("Stuffs");
            Delete.Table("Categories");
        }

        private void CreateVouchers()
        {
            Create.Table("Vouchers")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("Title").AsString(100).NotNullable()
                .WithColumn("Date").AsDate()
                .WithColumn("Quantity").AsInt32().NotNullable()
                .WithColumn("Price").AsDecimal().NotNullable()
                .ForeignKey("FK_Vouchers_Stuffs", "Stuffs", "Id")
                .OnDelete(System.Data.Rule.None);
        }

        private void CreateInvoice()
        {
            Create.Table("Invoices")
                            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                            .WithColumn("Title").AsString(100).NotNullable()
                            .WithColumn("Date").AsDate()
                            .WithColumn("Buyer").AsString(100)
                            .WithColumn("Quantity").AsInt32().NotNullable()
                            .WithColumn("Price").AsDecimal().NotNullable()
                            .ForeignKey("FK_Invoices_Stuffs", "Stuffs", "Id")
                            .OnDelete(System.Data.Rule.None);
        }

        private void CreateStuff()
        {
            Create.Table("Stuffs")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("Title").AsString(100).NotNullable()
                .WithColumn("Inventory").AsInt32()
                .WithColumn("Unit").AsString(50).NotNullable()
                .WithColumn("MinimumInventory").AsInt32().NotNullable()
                .WithColumn("MaximumInventory").AsInt32().NotNullable()
                .WithColumn("CategoryId").AsInt32().NotNullable()
                .ForeignKey("FK_Stuffs_Categories", "Categories", "Id")
                .OnDelete(System.Data.Rule.None);
        }

        private void CreateCategory()
        {
            Create.Table("Categories")
                            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                            .WithColumn("Title").AsString(100).NotNullable();
        }
    }
}
