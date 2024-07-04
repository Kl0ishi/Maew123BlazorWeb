using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.InputedValues
{
    public static class DropdownForSale
    {
        public static List<string> SaleStatuses = new List<string>
        {
            "รอการชำระเงิน", "อยู่ระหว่างการตรวจสอบการชำระเงิน", "เตรียมการจัดส่ง", "จัดส่งแล้ว", "ยกเลิกโดยลูกค้า", "ยกเลิกโดยAdmin", "อยู่ระหว่างแก้ไขข้อผิดพลาด"
        };

        public static List<DeliveryType> DeliveryTypes = new List<DeliveryType>
{
            new DeliveryType { Name = "ไปรษณีย์ไทย", Value = "001"},
            new DeliveryType { Name = "Kerry Express", Value = "002"},
            new DeliveryType { Name = "BEST EXPRESS", Value = "003"},
            new DeliveryType { Name = "Ninja Van", Value = "004"},
            new DeliveryType { Name = "J&T EXPRESS", Value = "005"},
            new DeliveryType { Name = "FLASH EXPRESS", Value = "006"},
            new DeliveryType { Name = "SCG EXPRESS", Value = "007"},
            new DeliveryType { Name = "DHL EXPRESS", Value = "008"},
            new DeliveryType { Name = "LALAMOVE", Value = "009"},
            new DeliveryType { Name = "Deliveree", Value = "010"}
        };

        public class DeliveryType
        {
            public string? Name { get; set; }
            public string? Value { get; set; }
        }

        public static string GetImageFileName(string parcelTypeNo)
        {
            Dictionary<string, string> imageFileMapping = new Dictionary<string, string>
        {
            { "001", "ThaiDelivery.png" },
            { "002", "KerryExpress.jpg" },
            { "003", "BestExpress.png" },
            { "004", "NinjaVan.jpg" },
            { "005", "JandTExpress.png" },
            { "006", "FlashExpress.jpg" },
            { "007", "SCGExpress.jpg" },
            { "008", "DHLEXPRESS.jpg" },
            { "009", "LALAMOVE.jpg" },
            { "010", "Deliveree.png" }
        };

            if (imageFileMapping.ContainsKey(parcelTypeNo))
            {
                return imageFileMapping[parcelTypeNo];
            }

            return "No_images.png";
        }
    }
}
