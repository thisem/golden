using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace HotelManagementSystem
{
    class Program {
       
        static void Main(string[] args)
        {
            string str = args[1];
            string[] data = str.Split('#');

            string intruksi = data[0];
            string address = data[1];
            string datakartu = data[2];
            string getReturn = "";



            if (intruksi == "tulisKartu")
            {
                getReturn = tulis(datakartu, address);
            }
            else if (intruksi == "bacaKartu")
            {
                getReturn = baca();
            }
            else
            {
                getReturn = "Error";
            }

            Console.WriteLine(getReturn);
            /*Console.ReadKey();*/
        }



    [DllImport("KIARARF.dll", EntryPoint = "ReadMessage")]
    public static extern int BacaKartuRF(
          int Com,
          int nBlock,
          int Encrypt,
          ref int DBCardno,
          ref int DBCardtype,
          ref int DBPassLevel,
          [MarshalAs(UnmanagedType.LPStr)] string CardPass,
          [MarshalAs(UnmanagedType.LPStr)] string DBSystemcode,
          [MarshalAs(UnmanagedType.LPStr)] string DBAddress,
          [MarshalAs(UnmanagedType.LPStr)] string SDateTime);

    [DllImport("KIARARF.dll", EntryPoint = "KeyCard")]
    public static extern int TulisKartuRF(
      int Com,
      int Cardno,
      int nBlock,
      int Encrypt,
      [MarshalAs(UnmanagedType.LPStr)] string CardPass,
      [MarshalAs(UnmanagedType.LPStr)] string SystemCode,
      [MarshalAs(UnmanagedType.LPStr)] string HotelCode,
      [MarshalAs(UnmanagedType.LPStr)] string RoomPass,
      [MarshalAs(UnmanagedType.LPStr)] string Address,
      [MarshalAs(UnmanagedType.LPStr)] string SDIn,
      [MarshalAs(UnmanagedType.LPStr)] string STIn,
      [MarshalAs(UnmanagedType.LPStr)] string SDOut,
      [MarshalAs(UnmanagedType.LPStr)] string STOut,
      int LEVEL_Pass,
      int PassMode,
      int AddressMode,
      int AddressQty,
      int Timemode,
      int V8,
      int V16,
      int V24,
      int AlwaysOpen,
      int OpenBolt,
      int TerminateOld,
      int ValidTimes);

    [DllImport("kernel32.dll")]
    private static extern int WritePrivateProfileString(
      string ApplicationName,
      string KeyName,
      string StrValue,
      string FileName);

    [DllImport("kernel32.dll")]
    private static extern int GetPrivateProfileString(
      string ApplicationName,
      string KeyName,
      string DefaultValue,
      StringBuilder ReturnString,
      int nSize,
      string FileName);

    public static string ReadINIValue(string SectionName, string KeyName, string FileName)
    {
        StringBuilder ReturnString = new StringBuilder((int)byte.MaxValue);
        GetPrivateProfileString(SectionName, KeyName, "", ReturnString, (int)byte.MaxValue, FileName);
        return ReturnString.ToString().Trim();
    }

    public static string calcxor(string hex1, string hex2)
    {
        return (Convert.ToInt32(hex1, 16) ^ Convert.ToInt32(hex2, 16)).ToString("X");
    }

    public static string DEC_TO_HEX(long dec)
    {
        string str1 = "";
        for (; dec > 0L; dec /= 16L)
        {
            string str2 = (dec % 16L).ToString();
            switch (str2)
            {
                case "10":
                    str2 = "A";
                    break;
                case "11":
                    str2 = "B";
                    break;
                case "12":
                    str2 = "C";
                    break;
                case "13":
                    str2 = "D";
                    break;
                case "14":
                    str2 = "E";
                    break;
                case "15":
                    str2 = "F";
                    break;
            }
            str1 = str2 + str1;
        }
        return str1;
    }

        public static string tulis(string iData, string Address)
        {
            //DATA
            char[] chArray = new char[1] { '/' };
            string[] strArray = iData.Split(chArray);

             string str;

             string configFile = Directory.GetCurrentDirectory() + "\\exe\\kiara.ini";
             if (File.Exists(configFile)) {

                string SectionName = "USING DATABASE FOR DEMO";
                 int Com = string.IsNullOrWhiteSpace(ReadINIValue(SectionName, "COM", configFile)) ? 0 : int.Parse(ReadINIValue(SectionName, "COM", configFile));
                 int Cardno = 1;
                 int Timemode = string.IsNullOrWhiteSpace(ReadINIValue(SectionName, "verify_checkin_time", configFile)) ? -1 : int.Parse(ReadINIValue(SectionName, "verify_checkin_time", configFile));
                 string SN = ReadINIValue(SectionName, "serial_number", configFile);
                 string InstallCode = ReadINIValue(SectionName, "instalation_code", configFile);

                 //SystemCode & Hotel Code

                 string hex2_1 = "26101994";
                 string hex2_2 = "26111997";
                 string hex1_1 = SN.Substring(7, 1) + SN.Substring(0, 7);
                 string hex1_2 = InstallCode.Substring(7, 1) + InstallCode.Substring(0, 7);
                 string HotelCode = calcxor(hex1_1, hex2_1);
                 string SystemCode = calcxor(hex1_2, hex2_2);
                //RoomPass
                //string str2 = "NEW GUEST CARD";

                int TerminateOld = 1;
                 int PassMode = 2;
                 DateTime now;
                 now = DateTime.Now;
                 int num2 = int.Parse(now.Year.ToString());
                 now = DateTime.Now;
                 int num3 = now.Month;
                 int num4 = int.Parse(num3.ToString());
                 now = DateTime.Now;
                 num3 = now.Day;
                 int num5 = int.Parse(num3.ToString());
                 now = DateTime.Now;
                 num3 = now.Hour;
                 int num6 = int.Parse(num3.ToString());
                 now = DateTime.Now;
                 num3 = now.Minute;
                 int num7 = (int)((double)(int.Parse(num3.ToString()) / 4) + (double)num6 * Math.Pow(2.0, 4.0) + (double)num5 * Math.Pow(2.0, 9.0) + (double)num4 * Math.Pow(2.0, 14.0) + (double)((num2 - 8) % 1000 % 63) * Math.Pow(2.0, 18.0));
                 string RoomPass = "";
                 string str9 = "#" + RoomPass;
                 int int32 = Convert.ToInt32(RoomPass, 16);

                RoomPass = num7 > int32 ? DEC_TO_HEX((long)num7) : DEC_TO_HEX((long)(int32 + 1));



                if (File.Exists(Directory.GetCurrentDirectory() + "\\exe\\KIARARF.dll"))
                 {

                    int tls = TulisKartuRF(Com, Cardno, 4, 1, "82A094FFFFFF", SystemCode, HotelCode, RoomPass, Address, strArray[1], strArray[2], strArray[3], strArray[4], 3, PassMode, 0, 1, Timemode, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, 0, 1, TerminateOld, (int)byte.MaxValue);
                     str = tls.ToString();
                 }
                 else
                 {
                     str = "kiararf.dll not found";
                 }




            }
            else
            {
                 str ="kiara.ini not found";
             }

             return str;
        }

            /*public static string tulis(string iData, string Address)
            {
                    string str1 = "";
                    char[] chArray = new char[1] { '/' };
                    string str2 = "";
                    int num1 = 0;
                    int Cardno = 1;
                    string RoomPass = "";
                    int PassMode = 0;
                    int TerminateOld = 0;
                    string[] strArray = iData.Split(chArray);
                    for (int index = 0; (long)index < strArray.LongLength; ++index)
                    {
                        if (string.IsNullOrEmpty(strArray[index]))
                            return "#1000";
                        if (strArray.LongLength < 10L)
                            return "#1500";
                        string str3 = strArray[8];
                        num1 = int.Parse(strArray[9]);
                    }
                    string str4 = Directory.GetCurrentDirectory() + "\\exe\\kiara.ini";
                    if (!File.Exists(str4))
                        return "#1001";
                    string SectionName = "USING DATABASE FOR DEMO";
                    string path = ReadINIValue(SectionName, "DATABASE", str4);
                    int Com = string.IsNullOrWhiteSpace(ReadINIValue(SectionName, "COM", str4)) ? 0 : int.Parse(ReadINIValue(SectionName, "COM", str4));
                    string str5 = ReadINIValue(SectionName, "serial_number", str4);
                    string str6 = ReadINIValue(SectionName, "instalation_code", str4);
                    int Timemode = string.IsNullOrWhiteSpace(ReadINIValue(SectionName, "verify_checkin_time", str4)) ? -1 : int.Parse(ReadINIValue(SectionName, "verify_checkin_time", str4));
                    string str7 = ReadINIValue(SectionName, "tipe", str4);
                    if (!File.Exists(path))
                        return "#1002";
                    if (Com == 0)
                        return "#1003";
                    if (str5 == null)
                        return "#1004";
                    if (str6 == null)
                        return "#1005";
                    if (Timemode == -1)
                        return "#1006";
                    if (str7 == null)
                        return "#1008";
                    if (true)
                    {
                        string hex2_1 = "26101994";
                        string hex2_2 = "26111997";
                        string hex1_1 = str5.Substring(7, 1) + str5.Substring(0, 7);
                        string hex1_2 = str6.Substring(7, 1) + str6.Substring(0, 7);
                        string HotelCode = calcxor(hex1_1, hex2_1);
                        string SystemCode = calcxor(hex1_2, hex2_2);

                        DateTime now;
                        switch (num1)
                        {
                            case 1:
                                str2 = "NEW GUEST CARD";
                                TerminateOld = 1;
                                PassMode = 2;
                                now = DateTime.Now;
                                int num2 = int.Parse(now.Year.ToString());
                                now = DateTime.Now;
                                int num3 = now.Month;
                                int num4 = int.Parse(num3.ToString());
                                now = DateTime.Now;
                                num3 = now.Day;
                                int num5 = int.Parse(num3.ToString());
                                now = DateTime.Now;
                                num3 = now.Hour;
                                int num6 = int.Parse(num3.ToString());
                                now = DateTime.Now;
                                num3 = now.Minute;
                                int num7 = (int)((double)(int.Parse(num3.ToString()) / 4) + (double)num6 * Math.Pow(2.0, 4.0) + (double)num5 * Math.Pow(2.0, 9.0) + (double)num4 * Math.Pow(2.0, 14.0) + (double)((num2 - 8) % 1000 % 63) * Math.Pow(2.0, 18.0));
                                string str9 = "#" + RoomPass;
                                int int32 = Convert.ToInt32(RoomPass, 16);
                                RoomPass = num7 > int32 ? DEC_TO_HEX((long)num7) : DEC_TO_HEX((long)(int32 + 1));
                                break;
                            case 2:
                                str2 = "COPY GUEST CARD";
                                TerminateOld = 0;
                                PassMode = 0;
                                break;
                            case 3:
                                str2 = "CHECK OUT";
                                TerminateOld = 0;
                                PassMode = 0;
                                Address = "00000000000";
                                strArray[1] = "01-01-01";
                                strArray[2] = "00:00:00";
                                strArray[3] = "01-01-01";
                                strArray[4] = "00:00:00";
                                strArray[5] = strArray[5] + "OUT";
                                strArray[8] = "99";
                                break;
                        }//end switch

                        int numRF = 1;
                        if (str7.ToUpper() == "RF")
                        {
                            if (File.Exists(Directory.GetCurrentDirectory() + "\\exe\\kiararf.dll"))
                            {
                                numRF = TulisKartuRF(Com, Cardno, 4, 1, "82A094FFFFFF", SystemCode, HotelCode, RoomPass, Address, strArray[1], strArray[2], strArray[3], strArray[4], 3, PassMode, 0, 1, Timemode, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, 0, 1, TerminateOld, (int)byte.MaxValue);
                            }
                            else
                            {
                                str1 = "#1009";
                                return str1;
                            }
                        }

                        str1 = "#" + numRF;


                        return str1;
                        }
                    return "testulis";
                }*/



            public static string baca()
        {
            //string str1 = "";
            char[] chArray = new char[1] { '/' };
            int DBCardno = 0;
            int DBCardtype = 0;
            int DBPassLevel = 0;
            string str2 = Directory.GetCurrentDirectory() + "\\exe\\kiara.ini";
            if (!File.Exists(str2))
                return "#1001";
            string SectionName = "USING DATABASE FOR DEMO";
            string path = ReadINIValue(SectionName, "DATABASE", str2);
            int Com = string.IsNullOrWhiteSpace(ReadINIValue(SectionName, "COM", str2)) ? 0 : int.Parse(ReadINIValue(SectionName, "COM", str2));
            string str3 = ReadINIValue(SectionName, "serial_number", str2);
            string configFile = ReadINIValue(SectionName, "instalation_code", str2);
            int num1 = string.IsNullOrWhiteSpace(ReadINIValue(SectionName, "verify_checkin_time", str2)) ? -1 : int.Parse(ReadINIValue(SectionName, "verify_checkin_time", str2));
            string str5 = ReadINIValue(SectionName, "tipe", str2);
            if (!File.Exists(path))
                return "#1002";
            if (Com == 0)
                return "#1003";
            if (str3 == null)
                return "#1004";
            if (configFile == null)
                return "#1005";
            if (num1 == -1)
                return "#1006";
            if (str5 == null)
                return "#1008";
            if (true)
            {
                string hex2_1 = "26101994";
                string hex2_2 = "26111997";
                string hex1_1 = str3.Substring(7, 1) + str3.Substring(0, 7);
                string hex1_2 = configFile.Substring(7, 1) + configFile.Substring(0, 7);
                calcxor(hex1_1, hex2_1);
                string DBSystemcode = calcxor(hex1_2, hex2_2);
                int num2 = 1;
                if (str5.ToUpper() == "RF")
                {
                    if (!File.Exists(Directory.GetCurrentDirectory() + "\\exe\\kiararf.dll"))
                        return "#1009";
                    num2 = BacaKartuRF(Com, 4, 1, ref DBCardno, ref DBCardtype, ref DBPassLevel, "82A094FFFFFF", DBSystemcode, "123456789012345678901234567890", "123456789012345678901234567890");
                }
                if (num2 != 0)
                    return "#" + (object)num2;
            }

            return DBCardno.ToString();
        }
    }

}
