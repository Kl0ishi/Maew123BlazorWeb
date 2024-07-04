using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public int? RoleId { get; set; }

    public string? UserTel { get; set; }

    public string? UserAddress { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    [JsonIgnore]
    public virtual ICollection<AddressSaleSnapshot> AddressSaleSnapshots { get; set; } = new List<AddressSaleSnapshot>();

    public virtual Role? Role { get; set; }

    [JsonIgnore]
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    [JsonIgnore]
    public virtual ICollection<WishList> WishLists { get; set; } = new List<WishList>();

    [JsonIgnore]
    public virtual ICollection<OtpEntity> Otps { get; set; } = new List<OtpEntity>();
}
