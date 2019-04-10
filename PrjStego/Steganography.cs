using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrjStego
{
    class Steganography
    {
       #region Fields
		 private string  BinMsg;
        private string Join = "\u200d";
        private string DisJoin = "\u200c";
        private string key, collection;
         private int Msglen,Coverlen;
        //private int index, NumOfMsg;
        //private char LastIndex;
        //public string Cover, Stego;
        private string  Stego;
       
        private char[] script = {'آ', 'ا', 'ب', 'پ', 'ت', 'ث', 'ج', 'چ', 'ح', 'خ', 'د',
        'ذ', 'ر', 'ز', 'ژ', 'س', 'ش', 'ص', 'ض', 'ط', 'ظ', 'ع', 'غ', 'ف', 'ق', 'ک', 'گ', 'ل', 'م', 'ن', 'و', 'ه', 'ی',' '};
        

      
        // این آرایه ها کد دهدهی دسته حروف های الفبا هستند
        //آرایه آخری حروف جدا را شامل میشود 

        private int[] zz = { 1570, 1575, 1583, 1584, 1585, 1586, 1688, 1608,32,8204 };//آ ا د ذ ر ز ژ و فاصله دیس جوین 
        private int[] zo = { 1705, 1711, 1604, 1605, 1606, 1607, 1740, 1587, 1600 };// ک گ ل م ن ه ی س کشیدا
        private int[] oz = { 1662, 1578, 1579, 1580, 1670, 1593, 1594, 1601, 1602 };//پ ت ث ج چ ع غ ف ق
        private int[] oo = { 1576, 1588, 1581, 1582, 1589, 1590, 1591, 1592, 8205 };// ب ش ح خ ص ض ط ظ جوین
        public bool wrong;
	#endregion
  //این تابع برای تولید مقدار باینری رمز است و 
  //به این صورت عمل میکند که اندیس هر کاراکتر در ارایه تعریف شده را به مبنای 
        //دو میبرد و اگر طول رشته باینری از 6 کمتر بود به اول آن 0 اضافه میکند تا به 6 برسد
        public string ProduceBinCode(string m)
        { 
            BinMsg = "";
            string bin;
            for (int i = 0; i < m.Length; i++)//انتخاب یک حرف رمز
            {
                bin = "";
                for (int j = 0; j < script.Length;j++ )//جستجو در آرایه حروف جهت تطابق با حرف رمز
                {
                   
                    if (m[i] == script[j])
                    {
                        bin = Convert.ToString(j, 2);//اندیس حرف موردنظر را به مبنای 2 میبرد
                        while (bin.Length < 6)
                            bin = "0" + bin;

                        BinMsg += bin;
                        break;
                    }
                }
            }
           
            return (BinMsg);
        }
        public string SetKey()//رمز را دو رقم دو رقم مخفی میکند ولی هر کاراکتر معادل 6 رقم است به همین دلیل
            //بر 6 تقسیم میکنیم تا موقع استخراج حروف مشکلی پیش نیاید
        {
            if (Msglen % 6 != 0)
                Msglen=Msglen - (Msglen % 6);
            //طول پیام باینری مخفی شده
                key = (Msglen/2).ToString();
          
            return key;
        }
       
        //این تابع الگوریتم اصلی پنهان سازی است
        public string SetStego(string bmsg,string cover)
        {
            Stego = cover;
            Coverlen = cover.Length;
            int i = 0;//اندیسی که روی رمز باینری حرکت میکند    
            int j = 0;//اندیسی که روی حروف متن اصلی حرکت میکند
            string sub;
            bool point;
            wrong = false;
          
            while (i < bmsg.Length&&(!wrong))
            { //دوتا دوتا از رشته باینری جدا میکند و مجموعه ای که متعلق به آنست را مشخص میکند
                //چون هر دو رقم صفر ویک متعلق به یک مجموعه حرف است که باید در آنها پنهان شود



                //wrong = false;
                //if (j + 1 > Stego.Length - 1)//بررسی بیشتر از حد مجاز بودن متن
                //{
                //    wrong = true;
                //    break;
                //}


                point = false;//وقتی که حرف موردنظر در متن پیدا میشود مقدار صحیح میگیرد
                sub = bmsg.Substring(i, 2);
                switch (sub)
                { 
                case "00":collection = "zz"; break;
                case "01": collection = "zo"; break;
                case "10": collection = "oz"; break;
                case "11": collection = "oo"; break;
                }
                
               
                while (!point)
                {// در بین حروف متن اصلی به دنبال حرف مربوط به مجموعه مشخص شده میگردد 
                    if (j + 1 > Stego.Length - 1)
                    {//بررسی بیشتر از حد مجاز بودن متن
                        wrong = true;
                        break;
                    }
                    if (collection == "zz")
                    {
                        foreach (int k in zz)//یکی یکی حرف موردنظر از متن را با اعضای ارایه موردنظر مقایسه میکند
                        {
                            if ((int)Stego[j] == k )
                            {//چون این دسته از حروف جدا هستند از کاراکترهای دیس جوین استفاده شده است
                                
                                
                                Stego = Stego.Insert(j + 1, DisJoin + DisJoin);
                            point = true;
                            break;
                            }


                        }   
                        
                    }
                    else if (collection == "zo")
                    {//اگر حرف مربوط پیدا شد و کاراکتر بعدی آن فاصله و یا خط جدید نبود در متن مربوطه بعد از
                        //حرف به عنوان نشانه دوتا از کاراکترهای جوین را قرار بده
                        foreach (int k in zo)
                        {
                            if ((int)Stego[j] == k && (int)Stego[j+1]!=1600)//حرف بعدی کشیدا نباشد
                            {//حرف بعدی را درنظر میگیرد اگر در مجموعه حروف الفبا باشد از جوین استفاده میکند
                                if ((1570 < (int)Stego[j + 1] && (int)Stego[j + 1] < 1600) || (1600 < (int)Stego[j + 1] && (int)Stego[j + 1] < 1611) || (1653 < (int)Stego[j + 1] && (int)Stego[j + 1] < 1749) || (int)Stego[j + 1]==8205)
                                    Stego=   Stego.Insert(j+1, Join + Join);
                                else//در غیر اینصورت از دیس جوین استفاده میکند
                                    Stego = Stego.Insert(j + 1, DisJoin + DisJoin);
                                point = true;
                                break;
                            }


                        }
                      
                    }
                    else if (collection == "oz")
                    {
                        foreach (int k in oz)
                        {
                            if ((int)Stego[j] == k && (int)Stego[j + 1] != 1600)
                            {//حرف بعدی را درنظر میگیرد اگر در مجموعه حروف الفبا باشد از جوین استفاده میکند
                                if ((1570 < (int)Stego[j + 1] && (int)Stego[j + 1] < 1600) || (1600 < (int)Stego[j + 1] && (int)Stego[j + 1] < 1611) || (1653 < (int)Stego[j + 1] && (int)Stego[j + 1] < 1749) || (int)Stego[j + 1] == 8205)
                                    Stego = Stego.Insert(j + 1, Join + Join);
                                else//در غیر اینصورت از دیس جوین استفاده میکند
                                    Stego = Stego.Insert(j + 1, DisJoin + DisJoin);
                                point = true;
                                break;
                            }

                           
                        }
                       
                    }
                    else
                    {

                        foreach (int k in oo)
                        {
                            if ((int)Stego[j] == k && (int)Stego[j + 1] != 1600)
                            {//حرف بعدی را درنظر میگیرد اگر در مجموعه حروف الفبا باشد از جوین استفاده میکند
                                if ((1570 < (int)Stego[j + 1] && (int)Stego[j + 1] < 1600) || (1600 < (int)Stego[j + 1] && (int)Stego[j + 1] < 1611) || (1653 < (int)Stego[j + 1] && (int)Stego[j + 1] < 1749) || (int)Stego[j + 1] == 8205)
                                    Stego = Stego.Insert(j + 1, Join + Join);
                                else//در غیر اینصورت از دیس جوین استفاده میکند
                                    Stego = Stego.Insert(j + 1, DisJoin + DisJoin);
                                point = true;
                                break;
                            }  
                        }
                        }
                   
                        if (point)//دو رقم باینری را در متن پنهان کرده است
                        {
                            
                            //نشان دهنده اینست که حرف مربوطه پیدا شده و حرف بعدی از متن را درنظر میگیرد
                            j = j + 3;//همچنین دوحرف بعدی از رشته باینری رمز را
                            i = i + 2;
                            Msglen = i;
                        }
                        else
                        {
                            ++j;
                            //اگر این حرف از متن اصلی در این مجموعه نبود هنوز رشته موردنظر مخفی نشده 
                            //و به سراغ حرف بعدی از متن اصلی میرود
                        }
                 
                    }
              
            } return Stego;
                }

        public string GivePercent()
        {
            Msglen = Msglen / 6;
            float Percent;
            Percent = ((float)Msglen / Coverlen) * 100;
            return (Percent.ToString());

        }

            
           
        }
       



    }

