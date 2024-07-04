using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class WishList
{
    public int WishListId { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual ICollection<WishListItem> WishListItems { get; set; } = new List<WishListItem>();
}
