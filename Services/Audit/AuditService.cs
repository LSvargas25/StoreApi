using Microsoft.Data.SqlClient;
using StoreApi.Interface.Audit;
using System.Data;

namespace StoreApi.Services.Audit
{
    public class AuditService : IAuditService
    {
        private readonly string _connectionString;
        public AuditService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("StoreDb");
        }

        public async Task InsertLogAsync(int? userId, string action, string? tableName, int? recordId, string? desc)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Log.sp_InsertLog", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@UserID", (object?)userId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@TableName", (object?)tableName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RecordID", (object?)recordId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", (object?)desc ?? DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task InsertAuditAsync(string tableName, int? recordId, string? fieldName, string? oldValue, string? newValue, int? userId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Audit.sp_InsertAudit", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@TableName", tableName);
            cmd.Parameters.AddWithValue("@RecordID", (object?)recordId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FieldName", (object?)fieldName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OldValue", (object?)oldValue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NewValue", (object?)newValue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UserID", (object?)userId ?? DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }

}

