using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Utilities.functions
{
    public static class GetUrlByName
    {
        public static string[] vnChar =
        {
            "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ",
            "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ",
            "ì|í|ị|ỉ|ĩ",
            "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ",
            "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ",
            "ỳ|ý|ỵ|ỷ|ỹ",
            "đ",
            " "
        };
        public static char[] replaceChar = { 'a', 'e', 'i', 'o', 'u', 'y', 'd', '-' };

        public static char getReplaceChar(char c)
        {
            for (int i = 0; i < vnChar.Length; i++)
            {
                if (vnChar[i].Contains(c)) return replaceChar[i];
            }
            return c;
        }

        public static string converts(string text)
        {
            string s = text.ToLower();
            string result = "";
            for (int i = 0; i < s.Length; i++)
            {
                result += getReplaceChar(s[i]);
            }
            return result;

        }
    }
}
