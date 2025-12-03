using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Log
{
    public int LogId { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? TableName { get; set; }

    public int? RecordId { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual UserAccount? User { get; set; }
}
