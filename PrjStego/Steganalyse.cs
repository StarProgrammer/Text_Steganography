using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrjStego
{
    class Steganalyse
    {
        private string BinMsg, Msg, StegoKey;
        int NumOfMsg;
        private char[] script = {'آ', 'ا', 'ب', 'پ', 'ت', 'ث', 'ج', 'چ', 'ح', 'خ', 'د',
        'ذ', 'ر', 'ز', 'ژ', 'س', 'ش', 'ص', 'ض', 'ط', 'ظ', 'ع', 'غ', 'ف', 'ق', 'ک', 'گ', 'ل', 'م', 'ن', 'و', 'ه', 'ی',' '};
        int pointchar;
        private int[] zz = { 1570, 1575, 1583, 1584, 1585, 1586, 1688, 1608, 32, 8204 };//آ ا د ذ ر ز ژ و فاصله دیس جوین 
        private int[] zo = { 1705, 1711, 1604, 1605, 1606, 1607, 1740, 1587, 1600 };// ک گ ل م ن ه ی س کشیدا
        private int[] oz = { 1662, 1578, 1579, 1580, 1670, 1593, 1594, 1601, 1602 };//پ ت ث ج چ ع غ ف ق
        private int[] oo = { 1576, 1588, 1581, 1582, 1589, 1590, 1591, 1592, 8205 };//ب ش ح خ ص ض ط ظ جوین
        //این تابع برای بدست آوردن رمز باینری است که کلید را میگیرد و متن را
        public string GiveBinMsg(string key, string stego)
        {
            int n = 0;
            int index = 0;
            BinMsg = "";
            bool found = false;//برای پیدا کردن اینست که حرف پیدا شده مربوط به کدام مجموعه است
            bool point;//برای پیدا کردن نشانه در متن است
          
          
            NumOfMsg = Convert.ToInt32(key);//از روی کلید طول پیام باینری را تشخیص میدهد کلید طول رمز باینری تقسیم بر 2 است
            while (n < NumOfMsg)
            {
                found = false;
                point = false;
                for (int i = index; i < stego.Length; ++i)//این حلقه برای پیدا کردن نشانه است
                    if ((stego[i].ToString() == "\u200c" && stego[i + 1].ToString() == "\u200c") || (stego[i].ToString() == "\u200d" && stego[i + 1].ToString() == "\u200d"))
                    {//نشانه را در متن پیدا میکند
                        pointchar = (int)stego[i - 1];//حرفی که رمز در آن مخفی شده
                        point = true;//یعنی نشانه پیدا شده است
                        index = i+1;//در روی متن اصلی یک واحد جلو برو
                        break;
                    }
               
               //حرف نشانه را تعیین میکند که متعلق به کدام مجموعه است

                while (true)
                {
                    foreach (int k in zz)
                        if (pointchar == k)
                        {
                            found = true;
                            BinMsg += "00";
                            ++n;
                            break;
                        }


                    if (found)
                        break;
                    
                        foreach (int k in zo)
                            if (pointchar == k)
                            {
                                found = true;
                                BinMsg += "01";
                                ++n;
                                break;
                            }

                    
                    if (found)
                        break;

                    foreach (int k in oz)
                        if (pointchar == k)
                        {
                            found = true;
                            BinMsg += "10";
                            ++n;
                            break;
                        }


                    if (found)
                        break;

                    foreach (int k in oo)
                        if (pointchar == k)
                        {
                            found = true;
                            BinMsg += "11";
                            ++n;
                            break;
                        }


                    if (found)
                        break;
                }
            }
           
            return BinMsg;

        }
        //این تابع برای بدست آوردن رمز واقعی از معادل باینری آن است
        public string GiveMsg(string bmsg)
        {//6رقم 6 رقم جدا میکند و کد حرف را بدست می آورد
           
            int i = 0;
            int num;
            Msg = "";
            while (i < bmsg.Length)
            {  
                 num=(int)Convert.ToInt32(bmsg.Substring(i, 6),2);//عدد مربوط به حرف را بدست می آورد
                 for (int j = 0; j < script.Length;j++ )
                 {
                     if (num == j)
                     {
                         Msg += script[j];
                         break;
                     }
                 }
                i=i+6;

            }
            return Msg;

        }
    }
}
