using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.InputedValues
{
    public class DropdownForProduct
    {
        public static List<string> ProductStatuses = new List<string>
        {
            "Available", "Out of stock", "เลิกจำหน่าย"
        };

        public static List<string> ProductCondition = new List<string>
        {
            "สินค้ามือหนึ่ง", "สินค้ามือสอง"
        };

        public static List<string> DeliveryMethod = new List<string>
        {
            "Flash Delivery only", "maybe someday"
        };

    }
}
