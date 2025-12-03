using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Setting
{
    public int SettingId { get; set; }

    public string KeyName { get; set; } = null!;

    public string? Value { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
