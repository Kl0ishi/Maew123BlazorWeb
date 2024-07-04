using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class WishListItem
{
    public int WishListItemId { get; set; }

    public int WishListId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? AddWhen { get; set; }

    public virtual Product? Product { get; set; }

    public virtual WishList? WishList { get; set; } = null!;
    [NotMapped]
    public decimal? ProPrice { get; set; }
}
