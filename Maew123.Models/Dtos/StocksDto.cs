using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class StocksDto
    {
        //ใส่ modal จบ  ใช้เป็นข้อมูลส่งกลับ
        public int ProductStockId { get; set; }
        public int? NumStock { get; set; }
        public bool? Subtract {  get; set; }
        [JsonIgnore]
        public int? ProductId { get; set; }
        [JsonIgnore]
        public string UpdateBy { get; set; } = string.Empty;
        [JsonIgnore]
        public DateTime? UpdateDate { get; set; }
        public string? DecreasedReason {  get; set; }
    }
}
