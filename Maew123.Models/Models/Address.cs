using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? addressName {  get; set; } //บ้านเลขที่

    public string? Street { get; set; } //ถนน

    public string? City { get; set; } //อำเภอ

    public string? State { get; set; } //ตำบล

    public string? Zip { get; set; } //ไปรษณีย์

    public string? Country { get; set; } //จังหวัด

    public string? Phone { get; set; }

    public bool? IsDefault { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}
