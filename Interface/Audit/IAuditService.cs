namespace StoreApi.Interface.Audit
{
    public interface IAuditService
    {
        Task InsertLogAsync(int? userId, string action, string? tableName, int? recordId, string? desc);
        Task InsertAuditAsync(string tableName, int? recordId, string? fieldName, string? oldValue, string? newValue, int? userId);

    }
}
