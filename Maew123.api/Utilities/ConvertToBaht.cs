namespace Maew123.Api.Utilities
{
    public static class ConvertToBaht
    {
        public static string ConvertToThaiBaht(decimal amount)
        {
            string[] strDigits = { "", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };
            string[] strPlaces = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
            string strBaht, strSatang, strWord = "", strEnd = "";

            long amount_int = (long)Math.Floor(amount);
            int amount_decimal = (int)((amount - amount_int) * 100);

            // Convert Baht
            strBaht = amount_int.ToString();
            int intLength = strBaht.Length;

            for (int i = 0; i < intLength; i++)
            {
                int intCurrentDigit = Convert.ToInt32(strBaht[i].ToString());
                int intCurrentPosition = intLength - i - 1;
                if (intCurrentDigit != 0)
                {
                    if (intCurrentPosition == 0 && intCurrentDigit == 1 && intLength > 1)
                    {
                        strWord += "เอ็ด";
                    }
                    else if (intCurrentPosition == 1 && intCurrentDigit == 2)
                    {
                        strWord += "ยี่";
                    }
                    else if (intCurrentPosition == 1 && intCurrentDigit == 1)
                    {
                        strWord += "";
                    }
                    else
                    {
                        strWord += strDigits[intCurrentDigit];
                    }

                    strWord += strPlaces[intCurrentPosition];
                }
            }

            if (strWord.Length > 0)
            {
                strEnd = "บาท";
            }

            // Convert Satang
            if (amount_decimal > 0)
            {
                strWord += "บาท";
                strEnd = "";
                strSatang = amount_decimal.ToString();
                intLength = strSatang.Length;

                for (int i = 0; i < intLength; i++)
                {
                    int intCurrentDigit = Convert.ToInt32(strSatang[i].ToString());
                    int intCurrentPosition = intLength - i - 1;
                    if (intCurrentDigit != 0)
                    {
                        if (intCurrentPosition == 0 && intCurrentDigit == 1 && intLength > 1)
                        {
                            strWord += "เอ็ด";
                        }
                        else if (intCurrentPosition == 1 && intCurrentDigit == 2)
                        {
                            strWord += "ยี่";
                        }
                        else if (intCurrentPosition == 1 && intCurrentDigit == 1)
                        {
                            strWord += "";
                        }
                        else
                        {
                            strWord += strDigits[intCurrentDigit];
                        }

                        strWord += strPlaces[intCurrentPosition];
                    }
                }

                if (strWord.Length > 0)
                {
                    strEnd += "สตางค์";
                }
            }
            else
            {
                if (strWord.Length > 0)
                {
                    strEnd += "ถ้วน";
                }
            }

            return strWord + strEnd;
        }
    }
}
