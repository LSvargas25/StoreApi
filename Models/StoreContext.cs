using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StoreApi.Models;

public partial class StoreContext : DbContext
{
    public StoreContext()
    {
    }

    public StoreContext(DbContextOptions<StoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<AttributeDetail> AttributeDetails { get; set; }

    public virtual DbSet<AuditTrail> AuditTrails { get; set; }

    public virtual DbSet<CashRegister> CashRegisters { get; set; }

    public virtual DbSet<CashRegisterSession> CashRegisterSessions { get; set; }

    public virtual DbSet<CostingMethod> CostingMethods { get; set; }

    public virtual DbSet<CreditAccount> CreditAccounts { get; set; }

    public virtual DbSet<CreditPayment> CreditPayments { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerRole> CustomerRoles { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }

    public virtual DbSet<InventoryBatch> InventoryBatches { get; set; }

    public virtual DbSet<InventoryBatchMovement> InventoryBatchMovements { get; set; }

    public virtual DbSet<InventoryLedger> InventoryLedgers { get; set; }

    public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }

    public virtual DbSet<InventoryWarehouse> InventoryWarehouses { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<InvoiceDetailTax> InvoiceDetailTaxes { get; set; }

    public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }

    public virtual DbSet<InvoiceVersion> InvoiceVersions { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemCategory> ItemCategories { get; set; }

    public virtual DbSet<ItemCostHistory> ItemCostHistories { get; set; }

    public virtual DbSet<ItemImage> ItemImages { get; set; }

    public virtual DbSet<ItemVariant> ItemVariants { get; set; }

    public virtual DbSet<ItemVersion> ItemVersions { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }

    public virtual DbSet<PriceHistory> PriceHistories { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<PurchaseDetailTax> PurchaseDetailTaxes { get; set; }

    public virtual DbSet<PurchaseType> PurchaseTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierType> SupplierTypes { get; set; }

    public virtual DbSet<Tax> Taxes { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    public virtual DbSet<TransferDetail> TransferDetails { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<UnitConversion> UnitConversions { get; set; }

    public virtual DbSet<UnitRelation> UnitRelations { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<VwSalesSummary> VwSalesSummaries { get; set; }

    public virtual DbSet<VwStockSummary> VwStockSummaries { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WebhookEvent> WebhookEvents { get; set; }

    public virtual DbSet<WebhookSubscriber> WebhookSubscribers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=STEVEN\\SQLEXPRESS;Database=Store;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId).HasName("PK__Attribut__C189298A5D2875F5");

            entity.ToTable("Attribute");

            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<AttributeDetail>(entity =>
        {
            entity.HasKey(e => new { e.ItemId, e.AttributeId });

            entity.ToTable("AttributeDetail");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.Value).HasMaxLength(255);

            entity.HasOne(d => d.Attribute).WithMany(p => p.AttributeDetails)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttributeDetail_Attribute");

            entity.HasOne(d => d.Item).WithMany(p => p.AttributeDetails)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttributeDetail_Item");
        });

        modelBuilder.Entity<AuditTrail>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__AuditTra__A17F23B875DAE269");

            entity.ToTable("AuditTrail");

            entity.HasIndex(e => e.UserId, "IX_Audit_UserID");

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.FieldName).HasMaxLength(255);
            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.TableName).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.AuditTrails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Audit_User");
        });

        modelBuilder.Entity<CashRegister>(entity =>
        {
            entity.HasKey(e => e.CashRegisterId).HasName("PK__CashRegi__7B5CAEB4B94BB0CB");

            entity.ToTable("CashRegister");

            entity.Property(e => e.CashRegisterId).HasColumnName("CashRegisterID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.CashRegisters)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK_CashRegister_Warehouse");
        });

        modelBuilder.Entity<CashRegisterSession>(entity =>
        {
            entity.HasKey(e => e.CashRegisterSessionId).HasName("PK__CashRegi__B7B1EDE29A7873C0");

            entity.ToTable("CashRegisterSession");

            entity.Property(e => e.CashRegisterSessionId).HasColumnName("CashRegisterSessionID");
            entity.Property(e => e.CashRegisterId).HasColumnName("CashRegisterID");
            entity.Property(e => e.CloseAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OpenAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CashRegister).WithMany(p => p.CashRegisterSessions)
                .HasForeignKey(d => d.CashRegisterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CashRegisterSession_CashRegister");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.CashRegisterSessions)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_CashRegisterSession_User");
        });

        modelBuilder.Entity<CostingMethod>(entity =>
        {
            entity.HasKey(e => e.CostingMethodId).HasName("PK__CostingM__F3C02B168DF3066E");

            entity.ToTable("CostingMethod");

            entity.Property(e => e.CostingMethodId).HasColumnName("CostingMethodID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<CreditAccount>(entity =>
        {
            entity.HasKey(e => e.CreditId).HasName("PK__CreditAc__ED5ED09B787A22D8");

            entity.ToTable("CreditAccount");

            entity.Property(e => e.CreditId).HasColumnName("CreditID");
            entity.Property(e => e.CurrentBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TotalCredit).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.CreditAccounts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditAccount_Customer");
        });

        modelBuilder.Entity<CreditPayment>(entity =>
        {
            entity.HasKey(e => e.CreditPaymentId).HasName("PK__CreditPa__3C85B0B3C68A65DE");

            entity.ToTable("CreditPayment");

            entity.Property(e => e.CreditPaymentId).HasColumnName("CreditPaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CreditId).HasColumnName("CreditID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.CreditPayments)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_CreditPayment_User");

            entity.HasOne(d => d.Credit).WithMany(p => p.CreditPayments)
                .HasForeignKey(d => d.CreditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditPayment_Credit");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK__Currency__14470B105818295A");

            entity.ToTable("Currency");

            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B84E82BBB0");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "IX_Customer_Email");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CardId)
                .HasMaxLength(100)
                .HasColumnName("CardID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CustomerRoleId).HasColumnName("CustomerRoleID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.CustomerRole).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerRoleId)
                .HasConstraintName("FK_Customer_CustomerRole");
        });

        modelBuilder.Entity<CustomerRole>(entity =>
        {
            entity.HasKey(e => e.CustomerRoleId).HasName("PK__Customer__D6D3B149B401863C");

            entity.ToTable("CustomerRole");

            entity.Property(e => e.CustomerRoleId).HasColumnName("CustomerRoleID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__Discount__E43F6DF6EAA06D25");

            entity.ToTable("Discount");

            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<ExchangeRate>(entity =>
        {
            entity.HasKey(e => e.ExchangeRateId).HasName("PK__Exchange__B05604A94F2AE8E7");

            entity.ToTable("ExchangeRate");

            entity.Property(e => e.ExchangeRateId).HasColumnName("ExchangeRateID");
            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.Rate).HasColumnType("decimal(18, 6)");

            entity.HasOne(d => d.Currency).WithMany(p => p.ExchangeRates)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExchangeRate_Currency");
        });

        modelBuilder.Entity<InventoryBatch>(entity =>
        {
            entity.HasKey(e => e.InventoryBatchId).HasName("PK__Inventor__A5585209605A93BE");

            entity.ToTable("InventoryBatch");

            entity.Property(e => e.InventoryBatchId).HasColumnName("InventoryBatchID");
            entity.Property(e => e.BatchNumber).HasMaxLength(255);
            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.SupplierLotNumber).HasMaxLength(255);
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");

            entity.HasOne(d => d.ItemVariant).WithMany(p => p.InventoryBatches)
                .HasForeignKey(d => d.ItemVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryBatch_ItemVariant");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.InventoryBatches)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryBatch_Warehouse");
        });

        modelBuilder.Entity<InventoryBatchMovement>(entity =>
        {
            entity.HasKey(e => e.InventoryBatchMovementId).HasName("PK__Inventor__DFF1808FEA542100");

            entity.ToTable("InventoryBatchMovement");

            entity.Property(e => e.InventoryBatchMovementId).HasColumnName("InventoryBatchMovementID");
            entity.Property(e => e.InventoryBatchId).HasColumnName("InventoryBatchID");
            entity.Property(e => e.MovementType).HasMaxLength(50);
            entity.Property(e => e.Reference).HasMaxLength(255);

            entity.HasOne(d => d.InventoryBatch).WithMany(p => p.InventoryBatchMovements)
                .HasForeignKey(d => d.InventoryBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryBatchMovement_Batch");
        });

        modelBuilder.Entity<InventoryLedger>(entity =>
        {
            entity.HasKey(e => e.LedgerId).HasName("PK__Inventor__AE70E0AF8BDA5D31");

            entity.ToTable("InventoryLedger");

            entity.Property(e => e.LedgerId).HasColumnName("LedgerID");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.InventoryTransactionId).HasColumnName("InventoryTransactionID");
            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.RunningBalanceCost).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");

            entity.HasOne(d => d.InventoryTransaction).WithMany(p => p.InventoryLedgers)
                .HasForeignKey(d => d.InventoryTransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryLedger_InventoryTransaction");

            entity.HasOne(d => d.ItemVariant).WithMany(p => p.InventoryLedgers)
                .HasForeignKey(d => d.ItemVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryLedger_ItemVariant");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.InventoryLedgers)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryLedger_Warehouse");
        });

        modelBuilder.Entity<InventoryTransaction>(entity =>
        {
            entity.HasKey(e => e.InventoryTransactionId).HasName("PK__Inventor__0863F868A1C1B1D0");

            entity.ToTable("InventoryTransaction");

            entity.HasIndex(e => e.InventoryWarehouseId, "IX_InventoryTransaction_InventoryWarehouseID");

            entity.Property(e => e.InventoryTransactionId).HasColumnName("InventoryTransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.InventoryWarehouseId).HasColumnName("InventoryWarehouseID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionTypeID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_InventoryTransaction_User");

            entity.HasOne(d => d.InventoryWarehouse).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.InventoryWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryTransaction_InventoryWarehouse");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.TransactionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryTransaction_TransactionType");
        });

        modelBuilder.Entity<InventoryWarehouse>(entity =>
        {
            entity.HasKey(e => e.InventoryWarehouseId).HasName("PK__Inventor__5542D4D22E53B7C0");

            entity.ToTable("InventoryWarehouse");

            entity.HasIndex(e => e.ItemVariantId, "IX_InventoryWarehouse_ItemVariantID");

            entity.HasIndex(e => e.WarehouseId, "IX_InventoryWarehouse_WarehouseID");

            entity.Property(e => e.InventoryWarehouseId).HasColumnName("InventoryWarehouseID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");

            entity.HasOne(d => d.ItemVariant).WithMany(p => p.InventoryWarehouses)
                .HasForeignKey(d => d.ItemVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryWarehouse_ItemVariant");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.InventoryWarehouses)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryWarehouse_Warehouse");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoice__D796AAD5E3E44AB9");

            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CurrencyId)
                .HasDefaultValue(1)
                .HasColumnName("CurrencyID");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ExchangeRate)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.InvoiceTypeId).HasColumnName("InvoiceTypeID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Currency).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_Currency");

            entity.HasOne(d => d.InvoiceType).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.InvoiceTypeId)
                .HasConstraintName("FK_Invoice_Type");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.InvoiceDetailId).HasName("PK__InvoiceD__1F1578F17817DD3F");

            entity.ToTable("InvoiceDetail", tb => tb.HasTrigger("trg_AfterInsert_InvoiceDetail"));

            entity.HasIndex(e => e.InvoiceId, "IX_InvoiceDetail_InvoiceID");

            entity.HasIndex(e => e.ItemVariantId, "IX_InvoiceDetail_ItemVariantID");

            entity.Property(e => e.InvoiceDetailId).HasColumnName("InvoiceDetailID");
            entity.Property(e => e.CostAtMovement).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.InventoryBatchId).HasColumnName("InventoryBatchID");
            entity.Property(e => e.InventoryTransactionId).HasColumnName("InventoryTransactionID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxId).HasColumnName("TaxID");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalLine).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Discount).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_InvoiceDetail_Discount");

            entity.HasOne(d => d.InventoryBatch).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InventoryBatchId)
                .HasConstraintName("FK_InvoiceDetail_Batch");

            entity.HasOne(d => d.InventoryTransaction).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InventoryTransactionId)
                .HasConstraintName("FK_InvoiceDetail_InventoryTransaction");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetail_Invoice");

            entity.HasOne(d => d.ItemVariant).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.ItemVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetail_ItemVariant");

            entity.HasOne(d => d.Tax).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.TaxId)
                .HasConstraintName("FK_InvoiceDetail_Tax");
        });

        modelBuilder.Entity<InvoiceDetailTax>(entity =>
        {
            entity.HasKey(e => e.InvoiceDetailTaxId).HasName("PK__InvoiceD__4D38FAEF7D64CF3B");

            entity.ToTable("InvoiceDetailTax");

            entity.Property(e => e.InvoiceDetailTaxId).HasColumnName("InvoiceDetailTaxID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.InvoiceDetailId).HasColumnName("InvoiceDetailID");
            entity.Property(e => e.TaxId).HasColumnName("TaxID");

            entity.HasOne(d => d.InvoiceDetail).WithMany(p => p.InvoiceDetailTaxes)
                .HasForeignKey(d => d.InvoiceDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetailTax_InvoiceDetail");

            entity.HasOne(d => d.Tax).WithMany(p => p.InvoiceDetailTaxes)
                .HasForeignKey(d => d.TaxId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetailTax_Tax");
        });

        modelBuilder.Entity<InvoiceType>(entity =>
        {
            entity.HasKey(e => e.InvoiceTypeId).HasName("PK__InvoiceT__BFD3969CB472BEF4");

            entity.ToTable("InvoiceType");

            entity.Property(e => e.InvoiceTypeId).HasColumnName("InvoiceTypeID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<InvoiceVersion>(entity =>
        {
            entity.HasKey(e => e.InvoiceVersionId).HasName("PK__InvoiceV__42BFEE672EAA8809");

            entity.ToTable("InvoiceVersion");

            entity.Property(e => e.InvoiceVersionId).HasColumnName("InvoiceVersionID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceVersions)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceVersion_Invoice");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__727E83EBB9FC4E00");

            entity.ToTable("Item", tb =>
                {
                    tb.HasTrigger("trg_AfterUpdate_ItemVersion");
                    tb.HasTrigger("trg_BeforeUpdate_Item");
                });

            entity.HasIndex(e => e.Barcode, "IX_Item_Barcode");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Barcode).HasMaxLength(255);
            entity.Property(e => e.Brand).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Height).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ItemCategoryId).HasColumnName("ItemCategoryID");
            entity.Property(e => e.Length).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Weight).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Width).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.ItemCategory).WithMany(p => p.Items)
                .HasForeignKey(d => d.ItemCategoryId)
                .HasConstraintName("FK_Item_Category");
        });

        modelBuilder.Entity<ItemCategory>(entity =>
        {
            entity.HasKey(e => e.ItemCategoryId).HasName("PK__ItemCate__C24A290553D4BB5B");

            entity.ToTable("ItemCategory");

            entity.Property(e => e.ItemCategoryId).HasColumnName("ItemCategoryID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<ItemCostHistory>(entity =>
        {
            entity.HasKey(e => e.ItemCostHistoryId).HasName("PK__ItemCost__0D3364416348808A");

            entity.ToTable("ItemCostHistory");

            entity.Property(e => e.ItemCostHistoryId).HasColumnName("ItemCostHistoryID");
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ChangedByUserId).HasColumnName("ChangedByUserID");
            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.MethodId).HasColumnName("MethodID");
            entity.Property(e => e.NewCost).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.OldCost).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Reason).HasMaxLength(255);

            entity.HasOne(d => d.ItemVariant).WithMany(p => p.ItemCostHistories)
                .HasForeignKey(d => d.ItemVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemCostHistory_ItemVariant");

            entity.HasOne(d => d.Method).WithMany(p => p.ItemCostHistories)
                .HasForeignKey(d => d.MethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemCostHistory_Method");
        });

        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasKey(e => e.ItemImageId).HasName("PK__ItemImag__09AE32B7E737C05A");

            entity.ToTable("ItemImage");

            entity.Property(e => e.ItemImageId).HasColumnName("ItemImageID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Url).HasMaxLength(1000);

            entity.HasOne(d => d.Item).WithMany(p => p.ItemImages)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemImage_Item");
        });

        modelBuilder.Entity<ItemVariant>(entity =>
        {
            entity.HasKey(e => e.ItemVariantId).HasName("PK__ItemVari__5DBBD1498545EE95");

            entity.ToTable("ItemVariant");

            entity.HasIndex(e => e.Barcode, "IX_ItemVariant_Barcode");

            entity.HasIndex(e => e.ItemId, "IX_ItemVariant_ItemID");

            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.Barcode).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Sku)
                .HasMaxLength(255)
                .HasColumnName("SKU");
            entity.Property(e => e.StandardCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StandardPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemVariants)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemVariant_Item");
        });

        modelBuilder.Entity<ItemVersion>(entity =>
        {
            entity.HasKey(e => e.ItemVersionId).HasName("PK__ItemVers__07C8827D38A427AF");

            entity.ToTable("ItemVersion");

            entity.Property(e => e.ItemVersionId).HasColumnName("ItemVersionID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemVersions)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemVersion_Item");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Log__5E5499A8EE67BDFD");

            entity.ToTable("Log");

            entity.HasIndex(e => e.UserId, "IX_Log_UserID");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.TableName).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Log_User");
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(e => e.OutboxMessageId).HasName("PK__OutboxMe__10554DCBA895AC95");

            entity.ToTable("OutboxMessage");

            entity.Property(e => e.OutboxMessageId).HasColumnName("OutboxMessageID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.EventType).HasMaxLength(100);
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Invoice).WithMany(p => p.OutboxMessages)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_OutboxMessage_Invoice");

            entity.HasOne(d => d.Sale).WithMany(p => p.OutboxMessages)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK_OutboxMessage_Sale");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A5820815870");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.PurchaseId, "IX_Payment_PurchaseID");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Payment_User");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Payment_Customer");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Payments)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_Payment_Invoice");

            entity.HasOne(d => d.Purchase).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("FK_Payment_Purchase");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Payment_Supplier");
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.HasKey(e => e.PaymentDetailId).HasName("PK__PaymentD__7F4E342F55D24E1C");

            entity.ToTable("PaymentDetail");

            entity.HasIndex(e => e.PaymentId, "IX_PaymentDetail_PaymentID");

            entity.Property(e => e.PaymentDetailId).HasColumnName("PaymentDetailID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Reference).HasMaxLength(255);

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentDetails)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentDetail_Payment");
        });

        modelBuilder.Entity<PriceHistory>(entity =>
        {
            entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__A927CB2BABC51AED");

            entity.ToTable("PriceHistory");

            entity.Property(e => e.PriceHistoryId).HasColumnName("PriceHistoryID");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Item).WithMany(p => p.PriceHistories)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PriceHistory_Item");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__6B0A6BDECC0D011D");

            entity.ToTable("Purchase");

            entity.HasIndex(e => e.SupplierId, "IX_Purchase_SupplierID");

            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CurrencyId)
                .HasDefaultValue(1)
                .HasColumnName("CurrencyID");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ExchangeRate)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PurchaseTypeId).HasColumnName("PurchaseTypeID");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Purchase_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchase_Currency");

            entity.HasOne(d => d.PurchaseType).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.PurchaseTypeId)
                .HasConstraintName("FK_Purchase_PurchaseType");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Purchase_Supplier");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.PurchaseDetailId).HasName("PK__Purchase__88C328D5096E57D2");

            entity.ToTable("PurchaseDetail", tb =>
                {
                    tb.HasTrigger("trg_PurchaseDetail_AfterDelete");
                    tb.HasTrigger("trg_PurchaseDetail_AfterInsert");
                    tb.HasTrigger("trg_PurchaseDetail_AfterUpdate");
                });

            entity.Property(e => e.PurchaseDetailId).HasColumnName("PurchaseDetailID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TaxId).HasColumnName("TaxID");
            entity.Property(e => e.TotalLine).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.Discount).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_PurchaseDetail_Discount");

            entity.HasOne(d => d.ItemVariant).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.ItemVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetail_ItemVariant");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetail_Purchase");

            entity.HasOne(d => d.Tax).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.TaxId)
                .HasConstraintName("FK_PurchaseDetail_Tax");
        });

        modelBuilder.Entity<PurchaseDetailTax>(entity =>
        {
            entity.HasKey(e => e.PurchaseDetailTaxId).HasName("PK__Purchase__E3412523C40BB24A");

            entity.ToTable("PurchaseDetailTax");

            entity.Property(e => e.PurchaseDetailTaxId).HasColumnName("PurchaseDetailTaxID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PurchaseDetailId).HasColumnName("PurchaseDetailID");
            entity.Property(e => e.TaxId).HasColumnName("TaxID");

            entity.HasOne(d => d.PurchaseDetail).WithMany(p => p.PurchaseDetailTaxes)
                .HasForeignKey(d => d.PurchaseDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetailTax_PurchaseDetail");

            entity.HasOne(d => d.Tax).WithMany(p => p.PurchaseDetailTaxes)
                .HasForeignKey(d => d.TaxId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseDetailTax_Tax");
        });

        modelBuilder.Entity<PurchaseType>(entity =>
        {
            entity.HasKey(e => e.PurchaseTypeId).HasName("PK__Purchase__774F588DE8FF9851");

            entity.ToTable("PurchaseType");

            entity.Property(e => e.PurchaseTypeId).HasColumnName("PurchaseTypeID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3AB1A85A84");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sale__1EE3C41F9DDFF3D5");

            entity.ToTable("Sale");

            entity.HasIndex(e => e.CustomerId, "IX_Sale_CustomerID");

            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.CurrencyId)
                .HasDefaultValue(1)
                .HasColumnName("CurrencyID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Sale_User");

            entity.HasOne(d => d.Currency).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Currency");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Sale_Customer");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Sales)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_Sale_Invoice");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("PK__Settings__54372AFDEFD78D3E");

            entity.HasIndex(e => e.KeyName, "UQ__Settings__F0A2A337E0D3A4A2").IsUnique();

            entity.Property(e => e.SettingId).HasColumnName("SettingID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.KeyName).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Value).HasMaxLength(2000);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE6669454F72E71");

            entity.ToTable("Supplier");

            entity.HasIndex(e => e.Email, "IX_Supplier_Email");

            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.SupplierTypeId).HasColumnName("SupplierTypeID");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.SupplierType).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.SupplierTypeId)
                .HasConstraintName("FK_Supplier_Type");
        });

        modelBuilder.Entity<SupplierType>(entity =>
        {
            entity.HasKey(e => e.SupplierTypeId).HasName("PK__Supplier__27DA8AF3E46E60B2");

            entity.ToTable("SupplierType");

            entity.Property(e => e.SupplierTypeId).HasColumnName("SupplierTypeID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasKey(e => e.TaxId).HasName("PK__Tax__711BE08C628C653F");

            entity.ToTable("Tax");

            entity.Property(e => e.TaxId).HasColumnName("TaxID");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Percentage).HasColumnType("decimal(7, 4)");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeId).HasName("PK__Transact__20266CEB0D34EA94");

            entity.ToTable("TransactionType");

            entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionTypeID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.TransferId).HasName("PK__Transfer__9549017151B4CFFB");

            entity.ToTable("Transfer");

            entity.Property(e => e.TransferId).HasColumnName("TransferID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.FromWarehouseId).HasColumnName("FromWarehouseID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ToWarehouseId).HasColumnName("ToWarehouseID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Transfers)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Transfer_User");

            entity.HasOne(d => d.FromWarehouse).WithMany(p => p.TransferFromWarehouses)
                .HasForeignKey(d => d.FromWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfer_FromWarehouse");

            entity.HasOne(d => d.ToWarehouse).WithMany(p => p.TransferToWarehouses)
                .HasForeignKey(d => d.ToWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfer_ToWarehouse");
        });

        modelBuilder.Entity<TransferDetail>(entity =>
        {
            entity.HasKey(e => e.TransferDetailId).HasName("PK__Transfer__F9BF690F60114A30");

            entity.ToTable("TransferDetail");

            entity.HasIndex(e => e.TransferId, "IX_TransferDetail_TransferID");

            entity.Property(e => e.TransferDetailId).HasColumnName("TransferDetailID");
            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.TransferId).HasColumnName("TransferID");

            entity.HasOne(d => d.ItemVariant).WithMany(p => p.TransferDetails)
                .HasForeignKey(d => d.ItemVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransferDetail_ItemVariant");

            entity.HasOne(d => d.Transfer).WithMany(p => p.TransferDetails)
                .HasForeignKey(d => d.TransferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransferDetail_Transfer");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Unit__44F5EC954E299F1D");

            entity.ToTable("Unit");

            entity.Property(e => e.UnitId).HasColumnName("UnitID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<UnitConversion>(entity =>
        {
            entity.HasKey(e => e.UnitConversionId).HasName("PK__UnitConv__5D64C5DF87911C17");

            entity.ToTable("UnitConversion");

            entity.Property(e => e.UnitConversionId).HasColumnName("UnitConversionID");
            entity.Property(e => e.Factor).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.FromUnitId).HasColumnName("FromUnitID");
            entity.Property(e => e.ToUnitId).HasColumnName("ToUnitID");

            entity.HasOne(d => d.FromUnit).WithMany(p => p.UnitConversionFromUnits)
                .HasForeignKey(d => d.FromUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitConversion_From");

            entity.HasOne(d => d.ToUnit).WithMany(p => p.UnitConversionToUnits)
                .HasForeignKey(d => d.ToUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitConversion_To");
        });

        modelBuilder.Entity<UnitRelation>(entity =>
        {
            entity.HasKey(e => e.UnitRelationId).HasName("PK__UnitRela__E957B87890CEB198");

            entity.ToTable("UnitRelation");

            entity.HasIndex(e => e.ItemVariantId, "IX_UnitRelation_ItemVariant");

            entity.Property(e => e.UnitRelationId).HasColumnName("UnitRelationID");
            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.UnitId).HasColumnName("UnitID");
            entity.Property(e => e.Value).HasMaxLength(100);

            entity.HasOne(d => d.ItemVariant).WithMany(p => p.UnitRelations)
                .HasForeignKey(d => d.ItemVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitRelation_ItemVariant");

            entity.HasOne(d => d.Unit).WithMany(p => p.UnitRelations)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitRelation_Unit");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CCACA7B3B4C1");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.Email, "IX_UserAccount_Email");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CardId)
                .HasMaxLength(100)
                .HasColumnName("CardID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.PasswordSalt).HasMaxLength(128);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.UserName).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.UserAccounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<VwSalesSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_SalesSummary");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<VwStockSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_StockSummary");

            entity.Property(e => e.ItemVariantId).HasColumnName("ItemVariantID");
            entity.Property(e => e.VariantName).HasMaxLength(255);
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");
            entity.Property(e => e.WarehouseName).HasMaxLength(255);
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFD99CD7FBBB");

            entity.ToTable("Warehouse");

            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<WebhookEvent>(entity =>
        {
            entity.HasKey(e => e.WebhookEventId).HasName("PK__WebhookE__8883E89682CD2927");

            entity.ToTable("WebhookEvent");

            entity.Property(e => e.WebhookEventId).HasColumnName("WebhookEventID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.OutboxMessageId).HasColumnName("OutboxMessageID");
            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.SubscriberId).HasColumnName("SubscriberID");

            entity.HasOne(d => d.Invoice).WithMany(p => p.WebhookEvents)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_WebhookEvent_Invoice");

            entity.HasOne(d => d.OutboxMessage).WithMany(p => p.WebhookEvents)
                .HasForeignKey(d => d.OutboxMessageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WebhookEvent_Outbox");

            entity.HasOne(d => d.Sale).WithMany(p => p.WebhookEvents)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK_WebhookEvent_Sale");

            entity.HasOne(d => d.Subscriber).WithMany(p => p.WebhookEvents)
                .HasForeignKey(d => d.SubscriberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WebhookEvent_Subscriber");
        });

        modelBuilder.Entity<WebhookSubscriber>(entity =>
        {
            entity.HasKey(e => e.WebhookSubscriberId).HasName("PK__WebhookS__786423D0901B3B6F");

            entity.ToTable("WebhookSubscriber");

            entity.Property(e => e.WebhookSubscriberId).HasColumnName("WebhookSubscriberID");
            entity.Property(e => e.Events).HasMaxLength(1000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Url).HasMaxLength(2000);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
