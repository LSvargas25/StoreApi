namespace StoreApi.ModelsDTO.Audtit
{
    public class AuditTrailDTO
    {
        public int AuditId { get; set; }

        public string TableName { get; set; } = null!;

        public int? RecordId { get; set; }

        public string? FieldName { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public int? UserId { get; set; }

        public DateTime ChangedAt { get; set; }

    }
}
