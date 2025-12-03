using System;
using System.Collections.Generic;

namespace StoreApi.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Barcode { get; set; }

    public string? Brand { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Height { get; set; }

    public decimal? Width { get; set; }

    public decimal? Length { get; set; }

    public bool IsActive { get; set; }

    public int? ItemCategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AttributeDetail> AttributeDetails { get; set; } = new List<AttributeDetail>();

    public virtual ItemCategory? ItemCategory { get; set; }

    public virtual ICollection<ItemImage> ItemImages { get; set; } = new List<ItemImage>();

    public virtual ICollection<ItemVariant> ItemVariants { get; set; } = new List<ItemVariant>();

    public virtual ICollection<ItemVersion> ItemVersions { get; set; } = new List<ItemVersion>();

    public virtual ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
}
