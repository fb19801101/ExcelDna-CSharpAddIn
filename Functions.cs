﻿using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Forms = System.Windows.Forms;
using ExcelDna.Integration;
using ExcelDna.Logging;
using Excel = Microsoft.Office.Interop.Excel;
using DotNet.Utilities;
using RDotNet;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace CSharpAddIn
{
    public class CalcFuncs
    {
        [ExcelFunction(Category = "Excel-DNA 扩展计算", Description = "拆解轨道数量文字.")]
        public static int OrbitBlock(string block, string type)
        {
            string _type = "(" + type + ")";
            string _block = block.Replace(")+", ");");
            string[] strs = _block.Split(';');
            int ret = 0;
            if (strs[0] == _block)
            {
                _block = block.Replace("×", ";");
                strs = _block.Split(';');
                if (strs[0] == _block && _block == _type)
                        ret += 1;
                else if (strs.Length == 2 && strs[1] == _type)
                        ret += Convert.ToInt32(strs[0]);
            }
            else if (strs.Length > 1)
            {
                foreach(string str in strs)
                {
                    string _str = str.Replace("×", ";");
                    string[] _strs = _str.Split(';');
                    if (_strs[0] == _str && _str == _type)
                            ret += 1;
                    else if (_strs.Length == 2 && _strs[1] == _type)
                            ret += Convert.ToInt32(_strs[0]);
                }
            }

            return ret;
        }

        [ExcelFunction(Category = "Excel-DNA 扩展计算", Description = "获取轨道数量集中特定数据.")]
        public static double OrbitValue(int idx, double[] rows, double[,] vals)
        {
            int rw = rows.Length;
            int _rw = vals.GetLength(0);
            double ret = 0;
            if (rw == _rw)
            {
                for(int i=0; i<rows.Length; i++)
                    ret += rows[i] * vals[i,idx];
            }

            return ret;
        }

        [ExcelFunction(Category = "Excel-DNA 扩展计算", Description = "查询数据集中特定数据.")]
        public static string MatchValue(
        [ExcelArgument(Name = "[value]", Description = "匹配的查询值", AllowReference = true)] string val,
        [ExcelArgument(Name = "[array]", Description = "查询的数据集", AllowReference = true)] object[,] array,
        [ExcelArgument(Name = "[math]", Description = "匹配的查询列", AllowReference = true)] int math = 1,
        [ExcelArgument(Name = "[look]", Description = "数据表查询列", AllowReference = true)] int look = 0,
        [ExcelArgument(Name = "[transpose]", Description = "是否数据转置", AllowReference = true)] bool transpose = false)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            if (rows == 0 || cols == 0)
            {
                return "#NAME?";
            }

            for (int i = 0; i < rows; i++)
            {
                if ((transpose ? array[look, i].ToString() : array[i, look].ToString()) == val)
                    return array[i, math].ToString();
            }

            return "#NAME?";
        }

        [ExcelFunction(Category = "Excel-DNA 扩展计算", Description = "整数转换为其他数制.")]
        public static string BinaryAndCalculation(
        [ExcelArgument(Name = "[value1]", Description = "计算数值1", AllowReference = true)] int value1,
        [ExcelArgument(Name = "[value2]", Description = "计算数值2", AllowReference = true)] int value2)
        {
            string str = Convert.ToString(value1 & value2, 2);
            if (str.Length < 8) return str.PadLeft(8, '0');

            return Convert.ToString(value1 & value2, 2);
        }

        [ExcelFunction(Category = "Excel-DNA 扩展计算", Description = "整数转换为其他数制.")]
        public static string BinaryOrCalculation(
        [ExcelArgument(Name = "[value1]", Description = "计算数值1", AllowReference = true)] int value1,
        [ExcelArgument(Name = "[value2]", Description = "计算数值2", AllowReference = true)] int value2)
        {
            string str = Convert.ToString(value1 | value2, 2);
            if (str.Length < 8) return str.PadLeft(8, '0');

            return Convert.ToString(value1 | value2, 2);
        }

        [ExcelFunction(Category = "Excel-DNA 扩展计算", Description = "整数转换为其他数制.")]
        public static string DecimalToBaseValue(
        [ExcelArgument(Name = "[value]", Description = "待转换的数制数值", AllowReference = true)] int value,
        [ExcelArgument(Name = "[toBase]", Description = "需转换的数制进制值", AllowReference = true)] int toBase)
        {
            string str = Convert.ToString(value, toBase);
            if (str.Length < 8) return str.PadLeft(8, '0');

            return Convert.ToString(value, toBase);
        }

        [ExcelFunction(Category = "Excel-DNA 扩展计算", Description = "其他数制转换为整数.")]
        public static int DecimalFromBaseValue(
        [ExcelArgument(Name = "[value]", Description = "待转换的数制字符串", AllowReference = true)] string value,
        [ExcelArgument(Name = "[fromBase]", Description = "需转换的数制进制值", AllowReference = true)] int fromBase)
        {
            return Convert.ToInt32(value.Trim(), fromBase);
        }

        [ExcelFunction(Category = "Excel-DNA 扩展计算", Description = "字符转换为ASCII码.")]
        public static string CharToASCII([ExcelArgument(Description = "待转换的字符", AllowReference = true)] string value)
        {
            if(value.Length > 0)
            {
                int num = value[0];
                return Convert.ToString(num, 2);
            }

            return "";
        }

        [ExcelFunction(Category = "Excel-DNA 扩展计算", Description = "ASCII码转换为字符.")]
        public static string ASCIIToChar([ExcelArgument(Description = "待转换的ASCII字符串", AllowReference = true)] string value)
        {
            if (value.Length > 0)
            {
                int num = Convert.ToInt32(value.Trim(), 2);
                return Convert.ToChar(num).ToString();
            }

            return "";
        }
    }

    public class RegularFuncs
    {
        [ExcelFunction(Category = "Excel-DNA 正则计算", Description = "提取文本中汉字.")]
        public static string RegExpHZ(string regex)
        {
            return RegexHelper.GetHZ(regex);
        }

        [ExcelFunction(Category = "Excel-DNA 正则计算", Description = "提取文本中数字.")]
        public static string RegExpNum(string regex)
        {
            return RegexHelper.GetNum(regex);
        }

        [ExcelFunction(Category = "Excel-DNA 正则计算", Description = "提取文本中字符.")]
        public static string RegExpChar(string regex)
        {
            return RegexHelper.GetChar(regex);
        }

        [ExcelFunction(Category = "Excel-DNA 正则计算", Description = "提取文本中指定字符.")]
        public static string RegExpStr(string regex, string rg)
        {
            return RegexHelper.GetStr(regex,rg);
        }

        [ExcelFunction(Category = "Excel-DNA 正则计算", Description = "验证文本为数字.")]
        public static bool RegExpIsNum(string regex)
        {
            return RegexHelper.IsNum(regex);
        }

        [ExcelFunction(Category = "Excel-DNA 正则计算", Description = "验证文本为密码.")]
        public static bool RegExpIsPwdChar(string regex)
        {
            return RegexHelper.IsPwdChar(regex);
        }

        [ExcelFunction(Category = "Excel-DNA 正则计算", Description = "验证文本为电话号码.")]
        public static bool RegExpIsTelNum(string regex)
        {
            return RegexHelper.IsTelNum(regex);
        }

        [ExcelFunction(Category = "Excel-DNA 正则计算", Description = "验证单元格为空.")]
        public static bool RegExpIsNull(object regex)
        {
            return regex == ExcelEmpty.Value;
        }

        [ExcelFunction(Category = "Excel-DNA 正则计算", Description = "验证文本是否与模式字符串匹配.")]
        public static bool RegExpIsMatch(string regex, string rg)
        {
            return RegexHelper.IsMatch(regex,rg);
        }
    }

    public class StringFuncs
    {
        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "加密字符串.")]
        public static string EncryptString(string text)
        {
            return DEncrypt.Encrypt(text);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "解密字符串.")]
        public static string DecryptString(string text)
        {
            return DEncrypt.Decrypt(text);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "获取字符串长度.")]
        public static int GetStringLength(
        [ExcelArgument(Name = "[text]", Description = "单元格地址或字符串")]string text)
        {
            return text.Length;
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "比较两个字符串.")]
        public static int CompareString(
        [ExcelArgument(Name = "[strA]", Description = "比较的第一个字符串")]string strA,
        [ExcelArgument(Name = "[strB]", Description = "比较的第二个字符串")]string strB)
        {
            return string.Compare(strA, strB);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "连接两个字符串.")]
        public static string ConcatString(
        [ExcelArgument(Name = "[strA]", Description = "连接的第一个字符串")]string str0,
        [ExcelArgument(Name = "[strB]", Description = "连接的第一个字符串")]string str1)
        {
            return string.Concat(str0, str1);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "连接多个字符串.")]
        public static string ConcatStrings(
        [ExcelArgument(Name = "[values]", Description = "多个单元格地址或字符串")]object[] values)
        {
            List<object> _values = new List<Object>();
            for (int i = 0; i < values.Length; i++)
            {
                object obj = values[i];
                if (obj != ExcelEmpty.Value)
                {
                    _values.Add(obj);
                }
            }

            return string.Concat(_values.ToArray());
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "对象是否出现在此字符串中.")]
        public static bool ContainsString(
        [ExcelArgument(Name = "[text]", Description = "单元格地址或字符串")]string text,
        [ExcelArgument(Name = "[value]", Description = "查询的单元格地址或字符串")]string value)
        {
            return text.Contains(value);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "获取对象出现在此列表的字符串.")]
        public static bool ContainsStringArray(
        [ExcelArgument(Name = "[text]", Description = "查询的单元格地址或字符串")]string text,
        [ExcelArgument(Name = "[values]", Description = "单元格地址或字符串列表")]object[] values)
        {
            int len = values.Length;
            for (int i = 0; i < len; i++)
            {
                string value = values[i].ToString();
                if (text.Contains(value))
                    return true;
            }

            return false;
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "获取对象出现在此列表的字符串.")]
        public static object ContainsStrings(string text, object[] values)
        {
            int len = values.Length;
            for (int i = 0; i < len; i++)
            {
                string value = values[i].ToString();
                if (value.Contains(text))
                    return value;
            }

            return ExcelError.ExcelErrorNA;
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "获取对象出现在此列表的字符串.")]
        public static object ContainsStringsArray(string text, object[,] values, int col)
        {
            int len = values.Length;
            for (int i = 0; i < len; i++)
            {
                string value = values[i,0].ToString();
                if (value.Contains(text))
                    return values[i,col].ToString();
            }

            return ExcelError.ExcelErrorNA;
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "从头匹配指定字符串.")]
        public static bool StartsWithString(string text, string value)
        {
            return text.StartsWith(value);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "从尾匹配指定的字符串.")]
        public static bool EndsWithString(string text, string value)
        {
            return text.EndsWith(value);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "判断两个字符串是否相等.")]
        public static bool EqualsString(string text, string value)
        {
            return text.Equals(value);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "格式化字符串.")]
        public static string FormatString(string format, object arg0)
        {
            return string.Format(format, arg0);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "指定字符第一次出现的索引.")]
        public static int IndexOfString(string text, string value)
        {
            return text.IndexOf(value);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "指定字符指定位置第一次出现的索引.")]
        public static int IndexOfIndexString(string text, string value, int startIndex)
        {
            return text.IndexOf(value, startIndex);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "任意字符第一次出现的索引.")]
        public static int IndexOfAnyString(string text, string value)
        {
            int len = value.Length;
            char[] sep = new char[len];
            for (int i = 0; i < len; i++)
                sep[i] = value[i];

            return text.IndexOfAny(sep);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "任意字符指定位置第一次出现的索引.")]
        public static int IndexOfAnyIndexString(string text, string anyOf, int startIndex)
        {
            int len = anyOf.Length;
            char[] sep = new char[len];
            for (int i = 0; i < len; i++ )
                sep[i] = anyOf[i];

            return text.IndexOfAny(sep, startIndex);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "最后一个匹配项的索引.")]
        public static int LastIndexOfString(string text, string value)
        {
            return text.LastIndexOf(value);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "任意字符指定位置第一次出现的索引.")]
        public static int LastIndexOfAnyString(string text, string anyOf, int startIndex)
        {
            int len = anyOf.Length;
            char[] sep = new char[len];
            for (int i = 0; i < len; i++)
                sep[i] = anyOf[i];

            return text.LastIndexOfAny(sep, startIndex);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "指定位置插入指定字符串.")]
        public static string InsertString(string text, int startIndex, string value)
        {
            return text.Insert(startIndex, value);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "返回左侧自定长度字符串.")]
        public static bool IsStringEmpty(string text)
        {
            return string.IsNullOrEmpty(text);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "移除指定位置开始所有字符.")]
        public static string RemoveString(string text, int startIndex)
        {
            return text.Remove(startIndex);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "移除指定位置开始指定长度所有字符.")]
        public static string RemoveOfString(string text, int startIndex, int count)
        {
            return text.Remove(startIndex, count);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "填充左对齐字符串.")]
        public static string PadLeftString(string text, int totalWidth, string paddingChar)
        {
            return text.PadLeft(totalWidth, paddingChar[0]);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "填充右对齐字符串.")]
        public static string PadRightString(string text, int totalWidth, string paddingChar)
        {
            return text.PadRight(totalWidth, paddingChar[0]);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "替换字符串.")]
        public static string ReplaceString(string text, string oldValue, string newValue)
        {
            return text.Replace(oldValue, newValue);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "指定分隔符连接字符串.")]
        public static string JoinString(string separator, object[] value)
        {
            return string.Join(separator, value);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "指定分隔符及长度连接字符串.")]
        public static string JoinOfString(string separator, object[] value, int startIndex, int count)
        {
            return string.Join(separator, value, startIndex, count);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "分割字符串.")]
        public static string SplitString(string text, string separator, int index)
        {
            int len = separator.Length;
            char[] sep = new char[len];
            for (int i = 0; i < len; i++)
                sep[i] = separator[i];

            string[] ret = text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            len = ret.Length;
            if (index > len - 1) return ret[len - 1];
            return ret[index];
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "分割字符串并展开行.")]
        public static object SplitRowString(string text, string separator, int index)
        {
            int len = separator.Length;
            char[] sep = new char[len];
            for (int i = 0; i < len; i++)
                sep[i] = separator[i];

            string[] ret = text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            len = ret.Length;
            string [,] objs = new string[1,len];
            for (int i = 0; i < len; i++)
                objs[0,i] = ret[i];

            return ArrayResizerHelper.Resize(objs);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "分割字符串并展开列.")]
        public static object SplitColString(string text, string separator, int index)
        {
            int len = separator.Length;
            char[] sep = new char[len];
            for (int i = 0; i < len; i++)
                sep[i] = separator[i];

            string[] ret = text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            len = ret.Length;
            string[,] objs = new string[len, 1];
            for (int i = 0; i < len; i++)
                objs[i, 0] = ret[i];

            return ArrayResizerHelper.Resize(objs);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "检索指定索引字符串.")]
        public static string SubString(string text, int startIndex)
        {
            return text.Substring(startIndex);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "检索指定长度字符串.")]
        public static string SubOfString(string text, int startIndex, int length)
        {
            return text.Substring(startIndex, length);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "检索左长字符串.")]
        public static string LeftString(string text, int length)
        {
            return text.Substring(0, length);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "检索右长字符串.")]
        public static string RightString(string text, int length)
        {
            int len = text.Length;
            return text.Substring(len-length, length);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "字符串转换为小写形式.")]
        public static string ToLowerString(string text)
        {
            return text.ToLower();
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "字符串转换为大写形式.")]
        public static string ToUpperString(string text)
        {
            return text.ToUpper();
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "去除字符空格.")]
        public static string TrimString(string text)
        {
            return text.Trim();
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "去除字符空格.")]
        public static string TrimCharsString(string text, string chars)
        {
            int len = chars.Length;
            char[] trimChars = new char[len];
            for (int i = 0; i < len; i++)
                trimChars[i] = chars[i];

            return text.Trim(trimChars);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "从结尾去除字符空格.")]
        public static string TrimEndString(string text, string chars)
        {
            int len = chars.Length;
            char[] trimChars = new char[len];
            for (int i = 0; i < len; i++)
                trimChars[i] = chars[i];

            return text.TrimEnd(trimChars);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "从开始去除字符.")]
        public static string TrimStartString(string text, string chars)
        {
            int len = chars.Length;
            char[] trimChars = new char[len];
            for (int i = 0; i < len; i++)
                trimChars[i] = chars[i];

            return text.TrimStart(trimChars);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "金额转大写.")]
        public static string MoneyToChinese(double lowerMoney)
        {
            string LowerMoney = lowerMoney.ToString();
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (LowerMoney.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                LowerMoney = LowerMoney.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49 123.4→123.4
            LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
            if (LowerMoney.IndexOf(".") > 0)
            {
                if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
                    LowerMoney = LowerMoney + "0";
            }
            else
                LowerMoney = LowerMoney + ".00";

            strLower = LowerMoney;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }
                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }
                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }
            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            if (strUpper.Substring(0, 1) == "零")
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            if (strUpper.Substring(0, 1) == "角")
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            if (strUpper.Substring(0, 1) == "分")
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            if (strUpper.Substring(0, 1) == "整")
                strUpper = "零圆整";

            functionReturnValue = strUpper;
            if (IsNegative == true)
                return "负" + functionReturnValue;
            else
                return functionReturnValue;
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "英文字母转数字.")]
        public static object CharConvert(object obj)
        {
            if (obj.GetType() == typeof(string))
            {
                return (int)obj.ToString()[0];
            }
            if (obj.GetType() == typeof(double) || obj.GetType() == typeof(int))
            {
                int n = (int)Convert.ToDouble(obj);
                char c = Convert.ToChar(n);
                return Convert.ToString(c);
            }

            return 0;
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "列名字母转数字.")]
        public static object ExcelColConvert(object obj)
        {
            if (obj.GetType() == typeof(string))
            {
                string col = obj.ToString();
                int a = 0, b = 0;
                a = (int)(col[0]-'A')+1;
                b = (int)(col[0]-'A')+1;
                if (col.Length == 2)
                    b = (int)(col[1] - 'A')+1;

                return col.Length == 2 ? a*26+b : a;
            }
            if (obj.GetType() == typeof(double) || obj.GetType() == typeof(int))
            {
                int n = (int)Convert.ToDouble(obj);
                if (n < 27)
                    return Convert.ToString(Convert.ToChar('A' + n-1));
                else
                {
                    int a = n / 26;
                    int b = n - a * 26;
                    if (b == 0)
                    {
                        a = a- 1;
                        b = 26;
                    }
                    string c1 = Convert.ToString(Convert.ToChar('A' + a - 1));
                    string c2 = Convert.ToString(Convert.ToChar('A' + b - 1));
                    return string.Concat(c1, c2);
                }
            }

            return 0;
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "数字转文字编号.")]
        public static string PartCode(
        [ExcelArgument(Name = "[code]", Description = "数字编号", AllowReference = true)] int code,
        [ExcelArgument(Name = "[format]", Description = "编号格式", AllowReference = true)] int fmt,
        [ExcelArgument(Name = "[level]", Description = "单多级别", AllowReference = true)] bool lv)
        {
            if(lv)
            {
                switch (fmt)
                {
                    case 2:
                        return String.Format("{0:D2}",code);
                    case 3:
                        return String.Format("{0:D3}",code);
                    case 4:
                        return String.Format("{0:D4}", code);
                    case 5:
                        return String.Format("{0:D5}", code);
                }

                return String.Format("{0:D2}", code);
            }
            else
            {
                switch (fmt)
                {
                    case 2:
                        return String.Format("{0:D2}001...{0:D2}999", code);
                    case 3:
                        return String.Format("{0:D3}001...{0:D3}999", code);
                    case 4:
                        return String.Format("{0:D4}001...{0:D4}999", code);
                    case 5:
                        return String.Format("{0:D5}001...{0:D5}999", code);
                }

                return String.Format("{0:D2}001...{0:D2}999", code);
            }
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "工序文字编号.")]
        public static string ProgCode(
        [ExcelArgument(Name = "[unit]", Description = "单位编号", AllowReference = true)] int unit,
        [ExcelArgument(Name = "[unit_sub]", Description = "子单位编号", AllowReference = true)] int unit_sub,
        [ExcelArgument(Name = "[part_type]", Description = "分部类别", AllowReference = true)] int part_type,
        [ExcelArgument(Name = "[part]", Description = "分部编号", AllowReference = true)] int part,
        [ExcelArgument(Name = "[part_sub]", Description = "子分部编号", AllowReference = true)] int part_sub,
        [ExcelArgument(Name = "[part_sub2]", Description = "二子分部编号", AllowReference = true)] int part_sub2,
        [ExcelArgument(Name = "[item]", Description = "分项编号", AllowReference = true)] int item,
        [ExcelArgument(Name = "[prog]", Description = "工序编号", AllowReference = true)] int prog)
        {
            if(part_type > 0)
            {
                if (part_sub2 > 0)
                {
                    return String.Format("{0:D2}{1:D3}{2:D2}{3:D2}{4:D3}{5:D2}{6:D2}{7:D2}",
                            unit, unit_sub, part_type, part, part_sub, part_sub2, item, prog);
                }
                else
                {
                    if (part_sub > 0)
                        return String.Format("{0:D2}{1:D3}{2:D2}{3:D2}{4:D3}{5:D2}{6:D2}",
                               unit, unit_sub, part_type, part, part_sub, item, prog);
                    else
                        return String.Format("{0:D2}{1:D3}{2:D2}{3:D2}{4:D2}{5:D2}",
                               unit, unit_sub, part_type, part, item, prog);
                }
            }
            else
            {
                if (part_sub2 > 0)
                {
                    return String.Format("{0:D2}{1:D3}{2:D2}{3:D3}{4:D2}{5:D2}{6:D2}",
                           unit, unit_sub, part, part_sub, part_sub2, item, prog);
                }
                else
                {
                    if (part_sub > 0)
                        return String.Format("{0:D2}{1:D3}{2:D2}{3:D3}{4:D2}{5:D2}",
                               unit, unit_sub, part, part_sub, item, prog);
                    else
                        return String.Format("{0:D2}{1:D3}{2:D2}{3:D2}{4:D2}",
                               unit, unit_sub, part, item, prog);
                }
            }
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "铁路清单编号分级.")]
        public static double ListLevel_Railway(
        [ExcelArgument(Name = "[list]", Description = "清单编号", AllowReference = true)] string list)
        {
            double lv = 0;
            foreach (char c in list)
            {
                if(RegexHelper.IsNum(c.ToString()))
                    lv = lv + 0.5;
                else
                    lv = lv + 1;
            }

            return lv;
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "公路清单编号分级.")]
        public static double ListLevel_Highway(
        [ExcelArgument(Name = "[list]", Description = "清单编号", AllowReference = true)] string list)
        {
            double lv = 1;
            foreach (char c in list)
            {
                if (!RegexHelper.IsNum(c.ToString()))
                    lv = lv + 1;
            }

            return lv;
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "清单编号敷设.")]
        public static string ListCodeLay(
        [ExcelArgument(Name = "[list]", Description = "清单编号", AllowReference = true)] string list,
        [ExcelArgument(Name = "[type]", Description = "敷设类型", AllowReference = true)] string type,
        [ExcelArgument(Name = "[code]", Description = "敷设编号", AllowReference = true)] int code)
        {
            return String.Format("{0}{1}{2:D3}", list, type, code);
        }
    }

    public class AlgorithmFuncs
    {
        class Integ : Integral
        {
            public override double Func(double x)
            {
                return Math.Log(1.0 + x) / (1.0 + x * x);
            }
        };

        [ExcelFunction(Category = "Excel-DNA 积分计算", Description = "变步长梯形求积法.")]
        public static double IntegralTrapezia(double a, double b, double eps)
        {
            Integ integ = new Integ();
            return integ.GetValueTrapezia(a, b, eps);
        }

        [ExcelFunction(Category = "Excel-DNA 积分计算", Description = "变步长辛卜生求积法.")]
        public static double IntegralSimpson(double a, double b, double eps)
        {
            Integ integ = new Integ();
            return integ.GetValueSimpson(a, b, eps);
        }

        [ExcelFunction(Category = "Excel-DNA 积分计算", Description = "自适应梯形求积法.")]
        public static double IntegralATrapezia(double a, double b, double d, double eps)
        {
            Integ integ = new Integ();
            return integ.GetValueATrapezia(a, b, d, eps);
        }

        [ExcelFunction(Category = "Excel-DNA 积分计算", Description = "龙贝格求积法.")]
        public static double IntegralRomberg(double a, double b, double eps)
        {
            Integ integ = new Integ();
            return integ.GetValueRomberg(a, b, eps);
        }

        [ExcelFunction(Category = "Excel-DNA 积分计算", Description = "计算一维积分的连分式法.")]
        public static double IntegralPq(double a, double b, double eps)
        {
            Integ integ = new Integ();
            return integ.GetValuePq(a, b, eps);
        }

        [ExcelFunction(Category = "Excel-DNA 积分计算", Description = "勒让德－高斯求积法.")]
        public static double IntegralLegdGauss(double a, double b, double eps)
        {
            Integ integ = new Integ();
            return integ.GetValueLegdGauss(a, b, eps);
        }

        [ExcelFunction(Category = "Excel-DNA 积分计算", Description = "拉盖尔－高斯求积法.")]
        public static double IntegralLgreGauss()
        {
            Integ integ = new Integ();
            return integ.GetValueLgreGauss();
        }

        [ExcelFunction(Category = "Excel-DNA 积分计算", Description = "埃尔米特－高斯求积法.")]
        public static double IntegralHermiteGauss()
        {
            Integ integ = new Integ();
            return integ.GetValueHermiteGauss();
        }

        [ExcelFunction(Category = "Excel-DNA 积分计算", Description = "高振荡函数求积法.")]
        public static double IntegralPart(double a, double b, int m, int n, double[] fa, double[] fb, double[] s)
        {
            Integ integ = new Integ();
            return integ.GetValuePart(a, b, m, n, fa, fb, s);
        }
    }

    public class CNDateFuncs
    {
        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "传回公历y年m月的总天数.")]
        public static int GetDaysByMonth(int y, int m)
        {
            return ChinaDate.GetDaysByMonth(y,m);
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "根据日期值获得周一的日期.")]
        public static DateTime GetMondayDateByDate(DateTime dt)
        {
            return ChinaDate.GetMondayDateByDate(dt);
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "计算中国农历节日.")]
        public static string CalendarHoliday(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.newCalendarHoliday;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "按某月第几周第几日计算的节日.")]
        public static string WeekDayHoliday(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.WeekDayHoliday;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "按公历日计算的节日.")]
        public static string DateHoliday(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.DateHoliday;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "取星期几.")]
        public static int WeekDay(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return (int)cc.WeekDay;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "周几的字符.")]
        public static string WeekDayStr(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.WeekDayStr;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "公历日期中文表示法 如一九九七年七月一日.")]
        public static string DateStr(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.DateString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "当前是否公历闰年.")]
        public static bool IsLeapYear(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.IsLeapYear;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "28星宿计算.")]
        public static string ChineseConstellation(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseConstellation;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "时辰.")]
        public static string ChineseHour(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseHour;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "是否闰月.")]
        public static bool IsChineseLeapMonth(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.IsChineseLeapMonth;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "当年是否有闰月.")]
        public static bool IsChineseLeapYear(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.IsChineseLeapYear;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "农历日.")]
        public static int ChineseDay(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseDay;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "农历日中文表示.")]
        public static string ChineseDayStr(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseDayString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "农历的月份.")]
        public static int ChineseMonth(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseMonth;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "农历月份字符串.")]
        public static string ChineseMonthStr(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseMonthString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "取农历年份.")]
        public static int ChineseYear(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseYear;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "取农历年字符串如，一九九七年.")]
        public static string ChineseYearStr(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseYearString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "取农历日期表示法：农历一九九七年正月初五.")]
        public static string ChineseDateStr(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseDateString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "定气法计算二十四节气.")]
        public static string ChineseTwentyFourDay(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseTwentyFourDay;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "当前日期前一个最近节气.")]
        public static string ChineseTwentyFourPrevDay(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseTwentyFourPrevDay;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "当前日期后一个最近节气.")]
        public static string ChineseTwentyFourNextDay(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.ChineseTwentyFourNextDay;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "计算指定日期的星座.")]
        public static string Constellation(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.Constellation;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "计算属相的索引，注意虽然属相是以农历年来区别的，但是目前在实际使用中是按公历来计算的鼠年为1,其它类推.")]
        public static int ChineseAnimal(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.Animal;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "取属相字符串.")]
        public static string ChineseAnimalStr(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.AnimalString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "取农历年的干支表示法如 乙丑年.")]
        public static string ChineseGanZhiYear(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.GanZhiYearString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "取干支的月表示字符串，注意农历的闰月不记干支.")]
        public static string ChineseGanZhiMonth(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.GanZhiMonthString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "取干支日表示法.")]
        public static string ChineseGanZhiDay(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.GanZhiDayString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "取当前日期的干支表示法如 甲子年乙丑月丙庚日.")]
        public static string ChineseGanZhiDate(DateTime dt)
        {
            ChineseCalendar cc = new ChineseCalendar(dt);
            return cc.GanZhiDateString;
        }

        [ExcelFunction(Category = "Excel-DNA 日期计算", Description = "判断时间是否在某个时间段内，在返回━，否则返回空.")]
        public static string DateInPart(DateTime dt,DateTime dt1,DateTime dt2)
        {
            if (dt >= dt1 && dt <= dt2) return "━";
            return "";
        }
    }

    public class ChartFuncs
    {
        [ExcelFunction(Category = "Excel-DNA 图标绘制", Description = "绘制折线图.")]
        public static void WriteData(bool bNew)
        {
            Excel.Application xlApp = (Excel.Application)ExcelDnaUtil.Application;

            Excel.Workbook wb = xlApp.ActiveWorkbook;
            if (wb == null) return;

            Excel.Worksheet ws = wb.ActiveSheet;
            if (bNew) ws = wb.Worksheets.Add(Type: Excel.XlSheetType.xlWorksheet);

            ws.Range["A1"].Value = "Date";
            ws.Range["B1"].Value = "Value";

            Excel.Range headerRow = ws.Range["A1", "B1"];
            headerRow.Font.Size = 12;
            headerRow.Font.Bold = true;

            // Generally it's faster to write an array to a range
            var values = new object[100, 2];
            var startDate = new DateTime(2007, 1, 1);
            var rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                values[i, 0] = startDate.AddDays(i);
                values[i, 1] = rand.NextDouble();
            }

            ws.Range["A2"].Resize[100, 2].Value = values;
            ws.Columns["A:A"].EntireColumn.AutoFit();

            // Add a chart
            Excel.Range dataRange = ws.Range["A1:B101"];
            dataRange.Select();
            ws.Shapes.AddChart(Excel.XlChartType.xlLineMarkers).Select();
            xlApp.ActiveChart.SetSourceData(Source: dataRange);
        }

        [ExcelFunction(Category = "Excel-DNA 图表绘制", Description = "绘制选取数据的折线图.")]
        public static void DrawChart()
        {
            Excel.Application xlApp = (Excel.Application)ExcelDnaUtil.Application;

            Excel.Workbook wb = xlApp.ActiveWorkbook;
            if (wb == null) return;
            Excel.Worksheet ws = wb.ActiveSheet;

            ws.Columns["A:B"].EntireColumn.AutoFit();

            // Add a chart
            Excel.Range dataRange = ws.Application.Selection;
            ws.Shapes.AddChart(Excel.XlChartType.xlLineMarkers).Select();
            xlApp.ActiveChart.SetSourceData(Source: dataRange);
        }

        [ExcelFunction(Category = "Excel-DNA 图表绘制", Description = "绘制柱形图.")]
        public static string DrawColumn()
        {
            OWCChart11 chart = new OWCChart11("c:\\", "柱形图", "产量");
            DataTable tbl = new DataTable("tbl");

            tbl.Columns.Add("X", typeof(Double));
            tbl.Columns.Add("Y", typeof(Double));
            DataColumn id = tbl.Columns.Add("ID", typeof(Int32));
            id.AllowDBNull = false;
            id.Unique = true;
            id.AutoIncrement = true;
            id.AutoIncrementSeed = 0;
            id.AutoIncrementStep = 1;

            DataRow rw;
            for (int i = 0; i < 10; i++)
            {
                rw = tbl.NewRow();
                rw["X"] = i;
                rw["Y"] = i / 2;
                tbl.Rows.Add(rw);
            }

            chart.DataSource = tbl;
            chart.PicWidth = 400;
            chart.PicHight = 200;
            return chart.CreateColumn();
        }

        [ExcelFunction(Category = "Excel-DNA 图表绘制", Description = "绘制饼状图.")]
        public static string DrawPie()
        {
            OWCChart11 chart = new OWCChart11("c:\\", "饼状图", "产量");
            DataTable tbl = new DataTable("tbl");

            tbl.Columns.Add("X", typeof(Double));
            tbl.Columns.Add("Y", typeof(Double));
            DataColumn id = tbl.Columns.Add("ID", typeof(Int32));
            id.AllowDBNull = false;
            id.Unique = true;
            id.AutoIncrement = true;
            id.AutoIncrementSeed = 0;
            id.AutoIncrementStep = 1;

            DataRow rw;
            for (int i = 0; i < 10; i++)
            {
                rw = tbl.NewRow();
                rw["X"] = i;
                rw["Y"] = i / 2;
                tbl.Rows.Add(rw);
            }

            chart.DataSource = tbl;
            chart.PicWidth = 400;
            chart.PicHight = 200;
            return chart.CreatePie();
        }

        [ExcelFunction(Category = "Excel-DNA 图表绘制", Description = "绘制条形图.")]
        public static string DrawBar()
        {
            OWCChart11 chart = new OWCChart11("c:\\", "条形图", "产量");
            DataTable tbl = new DataTable("tbl");

            tbl.Columns.Add("X", typeof(Double));
            tbl.Columns.Add("Y", typeof(Double));
            DataColumn id = tbl.Columns.Add("ID", typeof(Int32));
            id.AllowDBNull = false;
            id.Unique = true;
            id.AutoIncrement = true;
            id.AutoIncrementSeed = 0;
            id.AutoIncrementStep = 1;

            DataRow rw;
            for (int i = 0; i < 10; i++)
            {
                rw = tbl.NewRow();
                rw["X"] = i;
                rw["Y"] = i / 2;
                tbl.Rows.Add(rw);
            }

            chart.DataSource = tbl;
            chart.PicWidth = 400;
            chart.PicHight = 200;
            return chart.CreateBar();
        }
    }

    public class MenuCmds
    {
        [ExcelCommand(MenuName = "Excel-DNA", MenuText = "ExcelVersion")]
        public static void ExcelVersion()
        {
            dynamic xlApp = ExcelDnaUtil.Application;
            string ver = xlApp.Version;
            Forms.MessageBox.Show(ver, "EXCELDNA");
        }

        [ExcelCommand(MenuName = "Excel-DNA", MenuText = "AddWorksheet")]
        public static void AddWorksheet()
        {
            dynamic xlApp = ExcelDnaUtil.Application;

            dynamic wb = xlApp.ActiveWorkbook;
            if (wb == null) return;

            dynamic ws = wb.ActiveSheet;
            dynamic ns = xlApp.ActiveWorkbook.Worksheets.Add(Before: ws);
            ns.Name = "New Sheet";
            dynamic range = ns.Range("A1:B2");
            range.Value = new object[,] { { 1, "Hello" }, { 2, "Goodbye" } };

            ns.Cells(2, 5).Value = DateTime.Now;
        }

        [ExcelCommand(MenuName = "Excel-DNA", MenuText = "TestXlCallCmd")]
        public static void TestXlCallCmd()
        {
            var refSelectionOnActiveSheet = XlCall.Excel(XlCall.xlfSelection);
            LogDisplay.WriteLine("var refSelectionOnActiveSheet = XlCall.Excel(XlCall.xlfSelection);");
            if (refSelectionOnActiveSheet != null)
                LogDisplay.WriteLine("refSelectionOnActiveSheet is {0}!", refSelectionOnActiveSheet);
            else
                LogDisplay.WriteLine("refSelectionOnActiveSheet is null !");

            var refActiveCellOnActiveSheet = XlCall.Excel(XlCall.xlfActiveCell);
            LogDisplay.WriteLine("var refActiveCellOnActiveSheet = XlCall.Excel(XlCall.xlfActiveCell);");
            if (refActiveCellOnActiveSheet != null)
                LogDisplay.WriteLine("refActiveCellOnActiveSheet is {0}!", refActiveCellOnActiveSheet);
            else
                LogDisplay.WriteLine("refActiveCellOnActiveSheet is null !");

            var refActiveSheetId = XlCall.Excel(XlCall.xlSheetId, "Sheet1");
            LogDisplay.WriteLine("string refActiveSheetId = (string)XlCall.Excel(XlCall.xlSheetId, refActiveSheet);");
            if (refActiveSheetId != null)
                LogDisplay.WriteLine("refActiveSheetId is {0}!", refActiveSheetId);
            else
                LogDisplay.WriteLine("refActiveSheetId is null !");

            var refActiveSheetName = XlCall.Excel(XlCall.xlSheetNm, refActiveSheetId);
            LogDisplay.WriteLine("var refActiveSheetName = XlCall.Excel(XlCall.xlSheetNm, refActiveSheetId);");
            if (refActiveSheetName != null)
                LogDisplay.WriteLine("refActiveSheetName is {0}!", refActiveSheetName);
            else
                LogDisplay.WriteLine("refActiveSheetName is null !");

            //var refActiveSheet = XlCall.Excel(XlCall.xlfSheet, refActiveSheetId);
            //LogDisplay.WriteLine("var refActiveSheet = XlCall.Excel(XlCall.xlfSheet, refActiveSheetId);");
            // if (refActiveSheet != null)
            //     LogDisplay.WriteLine("refActiveSheet is {0}!", refActiveSheet);
            //else
            //    LogDisplay.WriteLine("refActiveSheet is null !");

            var refWorkbookSelectOnActiveSheetName = XlCall.Excel(XlCall.xlcWorkbookSelect, new object[] { refActiveSheetName });
            LogDisplay.WriteLine("var refWorkbookSelectOnActiveSheetName = XlCall.Excel(XlCall.xlcWorkbookSelect, new object[] { refActiveSheetName });");
            if (refWorkbookSelectOnActiveSheetName != null)
                LogDisplay.WriteLine("refWorkbookSelectOnActiveSheetName is {0}!", refWorkbookSelectOnActiveSheetName);
            else
                LogDisplay.WriteLine("refWorkbookSelectOnSheetName is null !");

            var refActiveSheetFormulaGoto = XlCall.Excel(XlCall.xlcFormulaGoto, refActiveSheetId);
            LogDisplay.WriteLine("var refActiveSheetFormulaGoto = XlCall.Excel(XlCall.xlcFormulaGoto, refActiveSheetId);");
            if (refActiveSheetFormulaGoto != null)
                LogDisplay.WriteLine("refActiveSheetFormulaGoto is {0}!", refActiveSheetFormulaGoto);
            else
                LogDisplay.WriteLine("refActiveSheetFormulaGoto is null !");

            var refCaller = XlCall.Excel(XlCall.xlfCaller);
            LogDisplay.WriteLine("var refCaller = XlCall.Excel(XlCall.xlfCaller);");
            if (refCaller != null)
                LogDisplay.WriteLine("refCaller is {0}!", refCaller);
            else
                LogDisplay.WriteLine("refCaller is null !");

            var refWorkspaceName = XlCall.Excel(XlCall.xlfGetWorkspace, 44);
            LogDisplay.WriteLine("object refWorkspaceName = XlCall.Excel(XlCall.xlfGetWorkspace, 44);");
            if (refWorkspaceName != null)
                LogDisplay.WriteLine("refWorkspaceName is {0} !", refWorkspaceName);
            else
                LogDisplay.WriteLine("refWorkspaceName is null !");

            var refActiveSheetSin = XlCall.Excel(XlCall.xlfSin, 30*3.1415926/180);
            LogDisplay.WriteLine("var refActiveSheetSin = (double)XlCall.Excel(XlCall.xlfSin, 30);");
            if (refActiveSheetSin != null)
                LogDisplay.WriteLine("refActiveSheetSin is {0:0.##} !", refActiveSheetSin);
            else
                LogDisplay.WriteLine("refActiveSheetSin is null !");

            ExcelReference eref = new ExcelReference(2, 5, 3, 6);
            bool ret = eref.SetValue(new object[,] { { 2.3, 2.4, 2.5, 2.6 }, { 3.3, 3.4, 3.5, 3.6 }, { 4.3, 4.4, 4.5, 4.6 }, { 5.3, 5.4, 5.5, 5.6 } });
            LogDisplay.WriteLine("ExcelReference eref = new ExcelReference(2, 5, 3, 6);");
            LogDisplay.WriteLine("bool ret = eref.SetValue(new object[,] { { 2.3, 2.4, 2.5, 2.6 }, { 3.3, 3.4, 3.5, 3.6 }, { 4.3, 4.4, 4.5, 4.6 }, { 5.3, 5.4, 5.5, 5.6 } });");
            if (ret)
                LogDisplay.WriteLine("ret is true !");
            else
                LogDisplay.WriteLine("ret is false !");
        }

        [ExcelCommand(MenuName = "Excel-DNA", MenuText = "TestDataFrame")]
        public static void TestDataFrame()
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();

            DataFrame df = engine.Evaluate("df <- data.frame(x=c(1,2,3),y=c('中国','美国','日本'),z=c(0.1,0.2,0.3))").AsDataFrame();
            int rows = df.RowCount;
            int cols = df.ColumnCount;

            if (rows == 0 || cols == 0)
            {
                LogDisplay.WriteLine("Error Engine Evaluate DataFrame !");
                return;
            }

            object[,] result = new object[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = df[i, j];

            ArrayResizerHelper.ResizeArray(result);
        }

        [ExcelCommand(MenuName = "Excel-DNA", MenuText = "TestROdbdcSql")]
        public static void TestROdbdcSql()
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();

            engine.Evaluate("library(RODBC)");
            engine.Evaluate("con <- odbcDriverConnect('driver={SQL Server};server=(local);database=ProjectManage;trusted_connection=true')");
            DataFrame df = engine.Evaluate("df <- sqlQuery(con, 'select * from tb_points')").AsDataFrame();
            engine.Evaluate("odbcClose(con)");

            int rows = df.RowCount;
            int cols = df.ColumnCount;

            if (rows == 0 || cols == 0)
            {
                LogDisplay.WriteLine("Error Engine Evaluate DataFrame !");
                return;
            }

            object[,] result = new object[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = df[i, j];

            ArrayResizerHelper.ResizeArray(result);
        }
    }

    public class XlfInfoFuncs
    {
        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the result of xlfGetCell.", IsMacroType = true)]
        public static object GetCell(int type_num, [ExcelArgument(AllowReference = true)]object reference)
        {
            return XlCall.Excel(XlCall.xlfGetCell, type_num, reference);
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the result of xlfGetFormula.", IsMacroType = true)]
        public static string GetFormula([ExcelArgument(AllowReference = true)]object reference, bool style)
        {
            string value = "";
            if (style)
                value = (string)XlCall.Excel(XlCall.xlfGetFormula, reference);
            else
                value = (string)XlCall.Excel(XlCall.xlfGetCell, 41, reference);

            return value.Substring(value.IndexOf("=") + 1);
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the result of xlfEvaluate.", IsMacroType = true)]
        public static object GetEvaluate(string name_text)
        {
            return XlCall.Excel(XlCall.xlfEvaluate, name_text);
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the result of xlfGetNote.", IsMacroType = true)]
        public static object GetNote([ExcelArgument(AllowReference = true)]object reference)
        {
            return XlCall.Excel(XlCall.xlfGetNote, reference);
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the result of xlfGetDocument.", IsMacroType = true)]
        public static object GetDocument(int type_num, string name_text)
        {
            return XlCall.Excel(XlCall.xlfGetDocument, type_num, name_text);
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the result of xlfGetWindow.", IsMacroType = true)]
        public static object GetWindow(int type_num, string name_text)
        {
            return XlCall.Excel(XlCall.xlfGetWindow, type_num, name_text);
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the result of xlfGetWorkbook.", IsMacroType = true)]
        public static object GetWorkbook(int type_num, string name_text)
        {
            return XlCall.Excel(XlCall.xlfGetWorkbook, type_num, name_text);
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the result of xlfGetWorkbook.", IsMacroType = false)]
        public static object GetWorkbookActive(int type_num)
        {
            return XlCall.Excel(XlCall.xlfGetWorkbook, type_num);
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the result of xlfGetWorkspace.", IsMacroType = true)]
        public static object GetWorkspace(int type_num)
        {
            return XlCall.Excel(XlCall.xlfGetWorkspace, type_num);
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the current list separator.", IsMacroType = true)]
        public static string GetListSeparator(int type_num)
        {
            object[,] workspaceSettings = (object[,])XlCall.Excel(XlCall.xlfGetWorkspace, 37);
            string listSeparator = (string)workspaceSettings[0, 4];
            return listSeparator;
        }

        [ExcelFunction(Category = "Excel-DNA XlfInfo", Description = "Returns the information about the selected cells.", IsMacroType = true)]
        public static object GetSelection()
        {
            return XlCall.Excel(XlCall.xlfSelection);
        }

        [ExcelFunction(Category = "Excel-DNA XlfReftext", Description = "Returns the cell reference to a string.", IsMacroType = true)]
        public static string xlfReftext([ExcelArgument(AllowReference = true)]object reference, bool style)
        {
            string value = (string)XlCall.Excel(XlCall.xlfReftext, reference, !style);
            value = value.Substring(value.IndexOf("!")+1);
            return value.Replace("$", "");
        }
    }

    public class ConversionFuncs
    {
        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDescribe.", IsMacroType = true)]
        public static string Describe(object arg)
        {
            // This is the full gamut we need to support
            if (arg is double)
                return "Double: " + (double)arg;
            else if (arg is string)
                return "String: " + (string)arg;
            else if (arg is bool)
                return "Boolean: " + (bool)arg;
            else if (arg is ExcelError)
                return "ExcelError: " + arg.ToString();
            else if (arg is object[,])
                // The object array returned here may contain a mixture of different types,
                // reflecting the different cell contents.
                return string.Format("Array[{0},{1}]", ((object[,])arg).GetLength(0), ((object[,])arg).GetLength(1));
            else if (arg is ExcelMissing)
                return "<<Missing>>"; // Would have been System.Reflection.Missing in previous versions of ExcelDna
            else if (arg is ExcelEmpty)
                return "<<Empty>>"; // Would have been null
            else
                return "!? Unheard Of ?!";
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectObject.", IsMacroType = true)]
        public static object DirectObject(object arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectString.", IsMacroType = true)]
        public static string DirectString(string arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectDouble.", IsMacroType = true)]
        public static double DirectDouble(double arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectInt32.", IsMacroType = true)]
        public static int DirectInt32(int arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectInt64.", IsMacroType = true)]
        public static long DirectInt64(long arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectDateTime.", IsMacroType = true)]
        public static DateTime DirectDateTime(DateTime arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectBoolean.", IsMacroType = true)]
        public static bool DirectBoolean(bool arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectInt16.", IsMacroType = true)]
        public static double DirectInt16(short arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectUInt16.", IsMacroType = true)]
        public static double DirectUInt16(ushort arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectDecimal.", IsMacroType = true)]
        public static decimal DirectDecimal(decimal arg)
        {
            return arg;
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectDouble1D.", IsMacroType = true)]
        public static double DirectDouble1D(double[] arg)
        {
            return arg[0];
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectDouble2D.", IsMacroType = true)]
        public static double DirectDouble2D(double[,] arg)
        {
            return arg[0, 0];
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectObject1D.", IsMacroType = true)]
        public static object DirectObject1D(object[] arg)
        {
            return arg[0];
        }

        [ExcelFunction(Category = "Excel-DNA Direct", Description = "dnaDirectObject2D.", IsMacroType = true)]
        public static object DirectObject2D(object[,] arg)
        {
            return arg[0, 0];
        }

        // Standard Conversion Functions
        [Flags]
        internal enum XlType : int
        {
            XlTypeNumber = 0x0001,
            XlTypeString = 0x0002,
            XlTypeBoolean = 0x0004,
            XlTypeReference = 0x0008,
            XlTypeError = 0x0010,
            XlTypeArray = 0x0040,
            XlTypeMissing = 0x0080,
            XlTypeEmpty = 0x0100,
            XlTypeInt = 0x0800,     // int16 in XlOper, int32 in XlOper12, never passed into UDF
        }

        [ExcelFunction(Category = "Excel-DNA Convert", Description = "dnaConvertToDouble.", IsMacroType = true)]
        public static double ConvertToDouble(object value)
        {
            object result;
            var retVal = XlCall.TryExcel(XlCall.xlCoerce, out result, value, (int)XlType.XlTypeNumber);
            if (retVal == XlCall.XlReturn.XlReturnSuccess)
            {
                return (double)result;
            }

            // We give up.
            throw new InvalidCastException("Value " + value.ToString() + " could not be converted to Int32.");
        }

        [ExcelFunction(Category = "Excel-DNA Convert", Description = "dnaConvertToString.", IsMacroType = true)]
        public static string ConvertToString(object value)
        {
            object result;
            var retVal = XlCall.TryExcel(XlCall.xlCoerce, out result, value, (int)XlType.XlTypeString);
            if (retVal == XlCall.XlReturn.XlReturnSuccess)
            {
                return (string)result;
            }

            // Not sure how this can happen...
            throw new InvalidCastException("Value " + value.ToString() + " could not be converted to String.");
        }

        [ExcelFunction(Category = "Excel-DNA Convert", Description = "dnaConvertToDateTime.", IsMacroType = true)]
        public static DateTime ConvertToDateTime(object value)
        {
            try
            {
                return DateTime.FromOADate(ConvertToDouble(value));
            }
            catch
            {
                // Might exceed range of DateTime
                throw new InvalidCastException("Value " + value.ToString() + " could not be converted to DateTime.");
            }
        }

        [ExcelFunction(Category = "Excel-DNA Convert", Description = "dnaConvertToBoolean.", IsMacroType = true)]
        public static bool ConvertToBoolean(object value)
        {
            object result;
            var retVal = XlCall.TryExcel(XlCall.xlCoerce, out result, value, (int)XlType.XlTypeBoolean);
            if (retVal == XlCall.XlReturn.XlReturnSuccess)
                return (bool)result;

            // failed - as a fallback, try to convert to a double
            retVal = XlCall.TryExcel(XlCall.xlCoerce, out result, value, (int)XlType.XlTypeNumber);
            if (retVal == XlCall.XlReturn.XlReturnSuccess)
                return ((double)result != 0.0);

            // We give up.
            throw new InvalidCastException("Value " + value.ToString() + " could not be converted to Boolean.");
        }

        [ExcelFunction(Category = "Excel-DNA Convert", Description = "dnaConvertToInt32.", IsMacroType = true)]
        public static int ConvertToInt32(object value)
        {
            return checked((int)ConvertToInt64(value));
        }

        [ExcelFunction(Category = "Excel-DNA Convert", Description = "dnaConvertToInt16.", IsMacroType = true)]
        public static short ConvertToInt16(object value)
        {
            return checked((short)ConvertToInt64(value));
        }

        [ExcelFunction(Category = "Excel-DNA Convert", Description = "dnaConvertToUInt16.", IsMacroType = true)]
        public static ushort ConvertToUInt16(object value)
        {
            return checked((ushort)ConvertToInt64(value));
        }

        [ExcelFunction(Category = "Excel-DNA Convert", Description = "dnaConvertToDecimal.", IsMacroType = true)]
        public static decimal ConvertToDecimal(object value)
        {
            return checked((decimal)ConvertToDouble(value));
        }

        [ExcelFunction(Category = "Excel-DNA Convert", Description = "dnaConvertToInt64.", IsMacroType = true)]
        public static long ConvertToInt64(object value)
        {
            return checked((long)Math.Round(ConvertToDouble(value), MidpointRounding.ToEven));
        }
    }

    public class DateBaseResizeFuncs
    {
        public static MSDASC.DataLinks dlg = new MSDASC.DataLinks();
        public static ADODB.Connection con = (ADODB.Connection)dlg.PromptNew();
        public static string con_str = con.ConnectionString;

        public static object[,] MakeTableArray()
        {
            DotNet.Utilities.OleDbBase _con = new DotNet.Utilities.OleDbBase();
            _con.ConStr = con_str;

            string[,] tbls = _con.GetTables();
            int rows = tbls.Length;
            object[,] result = new object[rows, 1];
            for (int i = 0; i < rows; i++)
            {
                result[i, 0] = tbls[i,0];
            }

            return result;
        }
        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建数据表.", IsMacroType = true)]
        public static object MakeTableArrayAndResize()
        {
            object[,] result = MakeTableArray();
            return ArrayResizerHelper.Resize(result);
        }

        public static object[,] MakeRecordArray(string tbl)
        {
            DotNet.Utilities.OleDbBase _con = new DotNet.Utilities.OleDbBase();
            _con.ConStr = con_str;
            _con.Table = tbl;
            object[,] result = _con.GetValues();

            return result;
        }
        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建数据表记录.", IsMacroType = true)]
        public static object MakeRecordArrayAndResize(string tbl)
        {
            object[,] result = MakeRecordArray(tbl);
            return ArrayResizerHelper.Resize(result);
        }

        public static object[,] MakeRecordFindArray(string tbl, string fid, string sql)
        {
            DotNet.Utilities.OleDbBase _con = new DotNet.Utilities.OleDbBase();
            _con.ConStr = con_str;
            _con.Table = tbl;
            _con.Field = fid;
            object[,] result = _con.FindValues(sql);

            return result;
        }
        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建数据表记录查询.", IsMacroType = true)]
        public static object MakeRecordFindArrayAndResize(string tbl, string fid, string sql)
        {
            object[,] result = MakeRecordFindArray(tbl, fid, sql);
            return ArrayResizerHelper.Resize(result);
        }

        public static object[,] MakeFieldFilterArray(string tbl)
        {
            DotNet.Utilities.OleDbBase _con = new DotNet.Utilities.OleDbBase();
            _con.ConStr = con_str;
            _con.Table = tbl;
            object[,] result = _con.GetFields();

            return result;
        }
        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建数据表字段信息.", IsMacroType = true)]
        public static object MakeFieldFilterArrayAndResize(string tbl)
        {
            object[,] result = MakeFieldFilterArray(tbl);
            return ArrayResizerHelper.Resize(result);
        }

        public static object[,] MakeRecordFilterArray(string tbl, string fid, int row)
        {
            DotNet.Utilities.OleDbBase _con = new DotNet.Utilities.OleDbBase();
            _con.ConStr = con_str;
            _con.Table = tbl;
            _con.Field = fid;
            object[] objs = _con.GetValue(row);

            int rows = objs.Length;
            object[,] result = new object[rows, 1];
            for (int i = 0; i < rows; i++)
            {
                result[i, 0] = objs[i];
            }

            return result;
        }
        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建数据行记录查询.", IsMacroType = true)]
        public static object MakeRecordFilterArrayAndResize(string tbl, string fid, int row)
        {
            object[,] result = MakeRecordFilterArray(tbl, fid, row);
            return ArrayResizerHelper.Resize(result);
        }
    }

    public class RDotNetFuncs
    {
        // Just returns an array of the given size
        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建均匀分布随机数组.", IsMacroType = true)]
        public static object RuniformSeed(
        [ExcelArgument(Name = "[seed]", Description = "随机数种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机数最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机数最大值", AllowReference = true)] double max)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            double[] dbls = engine.Evaluate(string.Format("runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            int cols = dbls.Length;
            object[,] result = new object[1, cols];
            for (int i = 0; i < cols; i++)
                result[0, i] = dbls[i];

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建均匀分布随机数组(列模式).", IsMacroType = true)]
        public static object RuniformSeed_c(
        [ExcelArgument(Name = "[seed]", Description = "随机数种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机数最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机数最大值", AllowReference = true)] double max)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            double[] dbls = engine.Evaluate(string.Format("runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            int rows = dbls.Length;
            object[,] result = new object[rows, 1];
            for (int i = 0; i < rows; i++)
                result[i, 0] = dbls[i];

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "按条件提取均匀分布随机数.", IsMacroType = true)]
        public static double RuniformifSeed(
        [ExcelArgument(Name = "[seed]", Description = "随机数种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机数最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机数最大值", AllowReference = true)] double max,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准值", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            engine.Evaluate(string.Format("x <- runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            engine.Evaluate(string.Format("set.seed({0})", s));
            if (cond)
                return engine.Evaluate(string.Format("sample(x[x >= {0}], 1, replace = FALSE)", min)).AsNumeric()[0];
            else
                return engine.Evaluate(string.Format("sample(x[x <= {0}], 1, replace = FALSE)", max)).AsNumeric()[0];
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建均匀分布随机时间组.", IsMacroType = true)]
        public static object RuniformSeed_t(
        [ExcelArgument(Name = "[seed]", Description = "随机时种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机时最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机时最大值", AllowReference = true)] double max,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            double[] dbls = engine.Evaluate(string.Format("runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            int cols = dbls.Length;
            object[,] result = new object[1, cols];
            for (int i = 0; i < cols; i++)
            {
                int _h = Convert.ToInt32(dbls[i]);
                int _m = Convert.ToInt32((dbls[i] - _h) * 60);
                int _s = Convert.ToInt32(((dbls[i] - _h) * 60 - _m) * 60);
                DateTime _dt = dt + new TimeSpan(_h, _m, _s);
                if(!cond) _dt = dt - new TimeSpan(_h, _m, _s);
                result[0, i] = _dt.ToString(fmt);
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建均匀分布随机时间组(列模式).", IsMacroType = true)]
        public static object RuniformSeed_t_c(
        [ExcelArgument(Name = "[seed]", Description = "随机时种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机时最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机时最大值", AllowReference = true)] double max,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            double[] dbls = engine.Evaluate(string.Format("runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            int rows = dbls.Length;
            object[,] result = new object[rows, 1];
            for (int i = 0; i < rows; i++)
            {
                int _h = Convert.ToInt32(dbls[i]);
                int _m = Convert.ToInt32((dbls[i] - _h) * 60);
                int _s = Convert.ToInt32(((dbls[i] - _h) * 60 - _m) * 60);
                DateTime _dt = dt + new TimeSpan(_h, _m, _s);
                if (!cond) _dt = dt - new TimeSpan(_h, _m, _s);
                result[i, 0] = _dt.ToString(fmt);
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "按条件提取均匀分布随机时间.", IsMacroType = true)]
        public static DateTime RuniformifSeed_t(
        [ExcelArgument(Name = "[seed]", Description = "随机时种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机时最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机时最大值", AllowReference = true)] double max,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            engine.Evaluate(string.Format("x <- runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            engine.Evaluate(string.Format("set.seed({0})", s));
            double dbl = engine.Evaluate(string.Format("sample(x[x >= {0}], 1, replace = FALSE)", min)).AsNumeric()[0];
            if (!cond) dbl = engine.Evaluate(string.Format("sample(x[x <= {0}], 1, replace = FALSE)", max)).AsNumeric()[0];

            int _h = Convert.ToInt32(dbl);
            int _m = Convert.ToInt32((dbl - _h) * 60);
            int _s = Convert.ToInt32(((dbl - _h) * 60 - _m) * 60);
            DateTime _dt = dt + new TimeSpan(_h, _m, _s);
            if (!cond) _dt = dt - new TimeSpan(_h, _m, _s);

            return _dt;
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建均匀分布随机数组.", IsMacroType = true)]
        public static object Runiform(
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机数最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机数最大值", AllowReference = true)] double max)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            double[] dbls = engine.Evaluate(string.Format("runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            int cols = dbls.GetLength(0);
            object[,] result = new object[1, cols];
            for (int i = 0; i < cols; i++)
                result[0, i] = dbls[i];

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建均匀分布随机数组(列模式).", IsMacroType = true)]
        public static object Runiform_c(
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机数最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机数最大值", AllowReference = true)] double max)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            double[] dbls = engine.Evaluate(string.Format("runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            int rows = dbls.GetLength(0);
            object[,] result = new object[rows, 1];
            for (int i = 0; i < rows; i++)
                result[i, 0] = dbls[i];

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "按条件提取均匀分布随机数.", IsMacroType = true)]
        public static double Runiformif(
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机数最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机数最大值", AllowReference = true)] double max,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准值", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("x <- runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            if (cond)
                return engine.Evaluate(string.Format("sample(x[x >= {0}], 1, replace = FALSE)", min)).AsNumeric()[0];
            else
                return engine.Evaluate(string.Format("sample(x[x <= {0}], 1, replace = FALSE)", max)).AsNumeric()[0];
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建均匀分布随机时间组.", IsMacroType = true)]
        public static object Runiform_t(
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机时最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机时最大差", AllowReference = true)] double max,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            double[] dbls = engine.Evaluate(string.Format("runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            int cols = dbls.GetLength(0);
            object[,] result = new object[1, cols];
            for (int i = 0; i < cols; i++)
            {
                int _h = Convert.ToInt32(dbls[i]);
                int _m = Convert.ToInt32((dbls[i] - _h) * 60);
                int _s = Convert.ToInt32(((dbls[i] - _h) * 60 - _m) * 60);
                DateTime _dt = dt + new TimeSpan(_h, _m, _s);
                if (!cond) _dt = dt - new TimeSpan(_h, _m, _s);
                result[0, i] = _dt.ToString(fmt);
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建均匀分布随机时间组(列模式).", IsMacroType = true)]
        public static object Runiform_t_c(
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机时最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机时最大差", AllowReference = true)] double max,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            double[] dbls = engine.Evaluate(string.Format("runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            int rows = dbls.GetLength(0);
            object[,] result = new object[rows, 1];
            for (int i = 0; i < rows; i++)
            {
                int _h = Convert.ToInt32(dbls[i]);
                int _m = Convert.ToInt32((dbls[i] - _h) * 60);
                int _s = Convert.ToInt32(((dbls[i] - _h) * 60 - _m) * 60);
                DateTime _dt = dt + new TimeSpan(_h, _m, _s);
                if (!cond) _dt = dt - new TimeSpan(_h, _m, _s);
                result[i, 0] = _dt.ToString(fmt);
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "按条件提取均匀分布随机时间.", IsMacroType = true)]
        public static DateTime Runiformif_t(
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[min]", Description = "随机时最小值", AllowReference = true)] double min,
        [ExcelArgument(Name = "[max]", Description = "随机时最大值", AllowReference = true)] double max,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("x <- runif({0},{1},{2})", n, min, max)).AsNumeric().ToArray();
            double dbl = engine.Evaluate(string.Format("sample(x[x >= {0}], 1, replace = FALSE)", min)).AsNumeric()[0];
            if (!cond) dbl = engine.Evaluate(string.Format("sample(x[x <= {0}], 1, replace = FALSE)", max)).AsNumeric()[0];

            int _h = Convert.ToInt32(dbl);
            int _m = Convert.ToInt32((dbl - _h) * 60);
            int _s = Convert.ToInt32(((dbl - _h) * 60 - _m) * 60);
            DateTime _dt = dt + new TimeSpan(_h, _m, _s);
            if (!cond) _dt = dt - new TimeSpan(_h, _m, _s);

            return _dt;
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建正态分布随机数组.", IsMacroType = true)]
        public static object RNormSeed(
        [ExcelArgument(Name = "[seed]", Description = "随机数种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[avg]", Description = "随机数平均值", AllowReference = true)] double mean,
        [ExcelArgument(Name = "[sd]", Description = "随机数均方差", AllowReference = true)] double sd)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            double[] dbls = engine.Evaluate(string.Format("rnorm({0},{1},{2})",n,mean,sd)).AsNumeric().ToArray();
            int cols = dbls.Length;
            object[,] result = new object[1, cols];
            for (int i = 0; i < cols; i++)
                result[0, i] = dbls[i];

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "按条件提取正态分布随机数.", IsMacroType = true)]
        public static double RNormifSeed(
        [ExcelArgument(Name = "[seed]", Description = "随机数种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[avg]", Description = "随机数平均值", AllowReference = true)] double mean,
        [ExcelArgument(Name = "[sd]", Description = "随机数均方差", AllowReference = true)] double sd,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准值", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            engine.Evaluate(string.Format("x <- rnorm({0},{1},{2})", n, mean, sd)).AsNumeric().ToArray();
            engine.Evaluate(string.Format("set.seed({0})", s));
            if (cond)
                return engine.Evaluate(string.Format("sample(x[x >= {0}], 1, replace = FALSE)", mean)).AsNumeric()[0];
            else
                return engine.Evaluate(string.Format("sample(x[x <= {0}], 1, replace = FALSE)", mean)).AsNumeric()[0];
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建正态分布随机时间组.", IsMacroType = true)]
        public static object RNormSeed_t(
        [ExcelArgument(Name = "[seed]", Description = "随机时种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[avg]", Description = "随机时平均值", AllowReference = true)] double mean,
        [ExcelArgument(Name = "[sd]", Description = "随机时均方差", AllowReference = true)] double sd,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            double[] dbls = engine.Evaluate(string.Format("rnorm({0},{1},{2})", n, mean, sd)).AsNumeric().ToArray();
            int cols = dbls.Length;
            object[,] result = new object[1, cols];
            for (int i = 0; i < cols; i++)
            {
                int _h = Convert.ToInt32(dbls[i]);
                int _m = Convert.ToInt32((dbls[i] - _h) * 60);
                int _s = Convert.ToInt32(((dbls[i] - _h) * 60 - _m) * 60);
                DateTime _dt = dt + new TimeSpan(_h, _m, _s);
                if (!cond) _dt = dt - new TimeSpan(_h, _m, _s);
                result[0, i] = _dt.ToString(fmt);
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "按条件提取正态分布随机时间.", IsMacroType = true)]
        public static DateTime RNormifSeed_t(
        [ExcelArgument(Name = "[seed]", Description = "随机时种子", AllowReference = true)] int s,
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[avg]", Description = "随机时平均值", AllowReference = true)] double mean,
        [ExcelArgument(Name = "[sd]", Description = "随机时均方差", AllowReference = true)] double sd,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("set.seed({0})", s));
            engine.Evaluate(string.Format("x <- rnorm({0},{1},{2})", n, mean, sd)).AsNumeric().ToArray();
            engine.Evaluate(string.Format("set.seed({0})", s));
            double dbl = engine.Evaluate(string.Format("sample(x[x >= {0}], 1, replace = FALSE)", mean)).AsNumeric()[0];
            if (!cond) dbl = engine.Evaluate(string.Format("sample(x[x <= {0}], 1, replace = FALSE)", mean)).AsNumeric()[0];

            int _h = Convert.ToInt32(dbl);
            int _m = Convert.ToInt32((dbl - _h) * 60);
            int _s = Convert.ToInt32(((dbl - _h) * 60 - _m) * 60);
            DateTime _dt = dt + new TimeSpan(_h, _m, _s);
            if (!cond) _dt = dt - new TimeSpan(_h, _m, _s);

            return _dt;
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建正态分布随机数组.", IsMacroType = true)]
        public static object RNorm(
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[avg]", Description = "随机数平均值", AllowReference = true)] double mean,
        [ExcelArgument(Name = "[sd]", Description = "随机数均方差", AllowReference = true)] double sd)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            double[] dbls = engine.Evaluate(string.Format("rnorm({0},{1},{2})", n, mean, sd)).AsNumeric().ToArray();
            int cols = dbls.GetLength(0);
            object[,] result = new object[1, cols];
            for (int i = 0; i < cols; i++)
                result[0, i] = dbls[i];

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "按条件提取正态分布随机数.", IsMacroType = true)]
        public static double RNormif(
        [ExcelArgument(Name = "[num]", Description = "随机数个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[avg]", Description = "随机数平均值", AllowReference = true)] double mean,
        [ExcelArgument(Name = "[sd]", Description = "随机数均方差", AllowReference = true)] double sd,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准值", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("x <- rnorm({0},{1},{2})", n, mean, sd)).AsNumeric().ToArray();
            if (cond)
                return engine.Evaluate(string.Format("sample(x[x >= {0}], 1, replace = FALSE)", mean)).AsNumeric()[0];
            else
                return engine.Evaluate(string.Format("sample(x[x <= {0}], 1, replace = FALSE)", mean)).AsNumeric()[0];
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "创建正态分布随机数组.", IsMacroType = true)]
        public static object RNorm_t(
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[avg]", Description = "随机时平均值", AllowReference = true)] double mean,
        [ExcelArgument(Name = "[sd]", Description = "随机时均方差", AllowReference = true)] double sd,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            double[] dbls = engine.Evaluate(string.Format("rnorm({0},{1},{2})", n, mean, sd)).AsNumeric().ToArray();
            int cols = dbls.GetLength(0);
            object[,] result = new object[1, cols];
            for (int i = 0; i < cols; i++)
            {
                int _h = Convert.ToInt32(dbls[i]);
                int _m = Convert.ToInt32((dbls[i] - _h) * 60);
                int _s = Convert.ToInt32(((dbls[i] - _h) * 60 - _m) * 60);
                DateTime _dt = dt + new TimeSpan(_h, _m, _s);
                if (!cond) _dt = dt - new TimeSpan(_h, _m, _s);
                result[0, i] = _dt.ToString(fmt);
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "按条件提取正态分布随机时间.", IsMacroType = true)]
        public static DateTime RNormif_t(
        [ExcelArgument(Name = "[num]", Description = "随机时个数", AllowReference = true)] int n,
        [ExcelArgument(Name = "[avg]", Description = "随机时平均值", AllowReference = true)] double mean,
        [ExcelArgument(Name = "[sd]", Description = "随机时均方差", AllowReference = true)] double sd,
        [ExcelArgument(Name = "[date]", Description = "基准时间", AllowReference = true)] DateTime dt,
        [ExcelArgument(Name = "[fmt]", Description = "时间格式 yyyy-MM-dd HH:mm:ss", AllowReference = true)] string fmt,
        [ExcelArgument(Name = "[if]", Description = "是否大于等于基准时间", AllowReference = true)] bool cond)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("x <- rnorm({0},{1},{2})", n, mean, sd)).AsNumeric().ToArray();
            double dbl = engine.Evaluate(string.Format("sample(x[x >= {0}], 1, replace = FALSE)", mean)).AsNumeric()[0];
            if (!cond) dbl = engine.Evaluate(string.Format("sample(x[x <= {0}], 1, replace = FALSE)", mean)).AsNumeric()[0];

            int _h = Convert.ToInt32(dbl);
            int _m = Convert.ToInt32((dbl - _h) * 60);
            int _s = Convert.ToInt32(((dbl - _h) * 60 - _m) * 60);
            DateTime _dt = dt + new TimeSpan(_h, _m, _s);
            if (!cond) _dt = dt - new TimeSpan(_h, _m, _s);
            return _dt;
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "获取f(x)函数在x=?处的导数.", IsMacroType = true)]
        public static double REval(
        [ExcelArgument(Name = "[fx]", Description = "求导函数 y=fx", AllowReference = true)] string fx,
        [ExcelArgument(Name = "[x]", Description = "求导参数 x", AllowReference = true)] double x)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("dx=D(expression({0}),'x')", fx));
            engine.Evaluate(string.Format("x={0}", x));
            NumericVector y = engine.Evaluate("y=eval(dx)").AsNumeric();
            return y[0];
        }

        [ExcelFunction(Category = "Excel-DNA R语言计算", Description = "获取f(x)函数在[x,y]区间的积分.", IsMacroType = true)]
        public static double RIntegrate(
        [ExcelArgument(Name = "[fx]", Description = "积分函数 y=fx", AllowReference = true)] string fx,
        [ExcelArgument(Name = "[x1]", Description = "积分下限 x1", AllowReference = true)] double x1,
        [ExcelArgument(Name = "[x2]", Description = "积分上限 x2", AllowReference = true)] double x2)
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            engine.Evaluate(string.Format("x1={0}", x1));
            engine.Evaluate(string.Format("x2={0}", x2));
            NumericVector y = engine.Evaluate(string.Format("y=integrate(function(x) {0},x1,x2)$value", fx)).AsNumeric();
            return y[0];
        }
    }

   // This class defines a few test functions that can be used to explore the automatic array resizing.
    public class ArrayMakerFuncs
	{
        public static object[,] MakeArray(int rows, int cols)
        {
            object[,] result = new object[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i,j] = i + j;
                }
            }
            
            return result;
        }
        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建泛型数组.", IsMacroType = true)]
        public static object MakeArrayAndResize(int rows, int cols)
        {
            object[,] result = MakeArray(rows, cols);
            return ArrayResizerHelper.Resize(result);

            // Can also call Resize via Excel - so if the Resize add-in is not part of this code, it should still work
            // (though calling direct is better for large arrays - it prevents extra marshaling).
            // return XlCall.Excel(XlCall.xlUDF, "Resize", result);
        }

        public static double[,] MakeArrayNumerics(int rows, int cols)
        {
            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i,j] = i + (j/1000.0);
                }
            }
            
            return result;
        }
        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建浮点数组.", IsMacroType = true)]
        public static double[,] MakeArrayAndResizeNumerics(int rows, int cols)
        {
            double[,] result = MakeArrayNumerics(rows, cols);
            return ArrayResizerHelper.ResizeNumerics(result);
        }

        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建混合数组.", IsMacroType = true)]
        public static object MakeArrayAndResizeMixes(int rows, int cols)
        {
            object[,] result = new object[rows, cols];
            for (int j = 0; j < cols; j++)
            {
                result[0, j] = "Col " + j;
            }
            for (int i = 1; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = i + (j * 0.1);
                }
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建转置数组.", IsMacroType = true)]
        public static object MakeArrayTranspose(object[,] objs)
        {
            int rows = objs.GetLength(0);
            int cols = objs.GetLength(1);

            object[,] result = new object[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    result[i, j] = objs[j, i];
                }
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建单列数组.", IsMacroType = true)]
        public static object MakeArrayRow(object[] objs)
        {
            int rows = objs.Length;
            object[,] result = new object[1, rows];
            for (int i = 0; i < rows; i++)
            {
                result[0, i] = objs[i];
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA 展开数组", Description = "创建单列数组.", IsMacroType = true)]
        public static object MakeArrayColumn(object[] objs)
        {
            int rows = objs.Length;
            object[,] result = new object[rows, 1];
            for (int i = 0; i < rows; i++)
            {
                result[i, 0] = objs[i];
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "删除单行数组重复项.", IsMacroType = true)]
        public static object DelArrayRepeatRow(object[] objs)
        {
            List<object> list = new List<object>();
            for (int i = 0; i < objs.Length; i++)
            {
                if (list.IndexOf(objs[i].ToString()) == -1)
                    list.Add(objs[i]);
            }

            int cols = list.ToArray().Length;
            object[,] result = new object[1, cols];
            for (int i = 0; i < cols; i++)
            {
                result[0, i] = list[i];
            }

            return ArrayResizerHelper.Resize(result);
        }

        [ExcelFunction(Category = "Excel-DNA 文字计算", Description = "删除单列数组重复项.", IsMacroType = true)]
        public static object DelArrayRepeatColumn(object[] objs)
        {
            List<object> list = new List<object>();
            for (int i = 0; i < objs.Length; i++)
            {
                if (list.IndexOf(objs[i].ToString()) == -1)
                    list.Add(objs[i]);
            }

            int rows = list.ToArray().Length;
            object[,] result = new object[rows, 1];
            for (int i = 0; i < rows; i++)
            {
                result[i, 0] = list[i];
            }

            return ArrayResizerHelper.Resize(result);
        }
    }

    public class ArrayResizerHelper : XlCall
    {
        // This function will run in the UDF context.
        // Needs extra protection to allow multithreaded use.
        public static dynamic Resize(dynamic array)
        {
            var caller = Excel(xlfCaller) as ExcelReference;
            if (caller == null)
            {
                //"XlCall.xlfCaller is null !";
                return array;
            }

            int rows = array.GetLength(0);
            int columns = array.GetLength(1);
            
            if (rows == 0 || columns == 0)
            {
                //"Array Rows or Columns is null !";
                return array;
            }

            if ((caller.RowLast - caller.RowFirst + 1 == rows) &&
                (caller.ColumnLast - caller.ColumnFirst + 1 == columns))
            {
                //Array Rows or Columns just return result !
                return array;
            }            

            var rowLast = caller.RowFirst + rows - 1;
            var columnLast = caller.ColumnFirst + columns - 1;

            // Check for the sheet limits
            if (rowLast > ExcelDnaUtil.ExcelLimits.MaxRows - 1 ||
                columnLast > ExcelDnaUtil.ExcelLimits.MaxColumns - 1)
            {
                //Array Rows or Columns Big than MaxRows or MaxColumns !;
                return ExcelError.ExcelErrorValue;
            }

            // TODO: Add some kind of guard for ever-changing result?
            ExcelAsyncUtil.QueueAsMacro(() =>
            {
                // Create a reference of the right size
                var target = new ExcelReference(caller.RowFirst, rowLast, caller.ColumnFirst, columnLast, caller.SheetId);
                DoResize(target); // Will trigger a recalc by writing formula
            });
            // Return what we have - to prevent flashing #N/A
            return array;
        }

        public static bool ResizeArray(dynamic array)
        {
            var refActiveCell = XlCall.Excel(XlCall.xlfActiveCell) as ExcelReference;
            if (refActiveCell == null)
            {
                //"XlCall.xlfActiveCell is null !";
                return false;
            }

            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            if (rows == 0 || columns == 0)
            {
                //"Array Rows or Columns is null !";
                return false;
            }

            // TODO: Add some kind of guard for ever-changing result?
            ExcelAsyncUtil.QueueAsMacro(() =>
            {
                // Create a reference of the right size
                var target = new ExcelReference(refActiveCell.RowFirst, refActiveCell.RowFirst + rows - 1, refActiveCell.ColumnFirst, refActiveCell.ColumnFirst + columns - 1, refActiveCell.SheetId);
                target.SetValue(array);
                XlCall.Excel(XlCall.xlcSelect, target);
            });

            return true;
        }

        public static bool ResizeRange(dynamic array)
        {
            Excel.Application xlApp = (Excel.Application)ExcelDnaUtil.Application;
            Excel.Workbook wb = xlApp.ActiveWorkbook;
            if (wb == null) return false;
            Excel.Worksheet ws = wb.ActiveSheet;
            Excel.Range rg1 = ws.Application.ActiveCell;

            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            if (rows == 0 || cols == 0)
            {
                LogDisplay.WriteLine("Array Rows or Cols is null !");
                return false;
            }

            Excel.Range rg2 = rg1.get_Offset(rows-1, cols-1);
            Excel.Range rg = ws.get_Range(rg1, rg2);
            rg.Value2 = array;

            return true;
        }

        public static bool ResizeRows(dynamic array)
        {
            Excel.Application xlApp = (Excel.Application)ExcelDnaUtil.Application;
            Excel.Workbook wb = xlApp.ActiveWorkbook;
            if (wb == null) return false;
            Excel.Worksheet ws = wb.ActiveSheet;
            Excel.Range rg1 = ws.Application.ActiveCell;

            int rows = array.Length;

            if (rows == 0)
            {
                LogDisplay.WriteLine("Array Rows is null !");
                return false;
            }

            Excel.Range rg2 = rg1.get_Offset(0, rows - 1);
            Excel.Range rg = ws.get_Range(rg1, rg2);
            rg.Value2 = array;

            return true;
        }

        public static bool ResizeColumns(dynamic array)
        {
            Excel.Application xlApp = (Excel.Application)ExcelDnaUtil.Application;
            Excel.Workbook wb = xlApp.ActiveWorkbook;
            if (wb == null) return false;
            Excel.Worksheet ws = wb.ActiveSheet;
            Excel.Range rg1 = ws.Application.ActiveCell;

            int rows = array.Length;

            if (rows == 0)
            {
                LogDisplay.WriteLine("Array Rows is null !");
                return false;
            }

            Excel.Range rg2 = rg1.get_Offset(rows - 1, 0);
            Excel.Range rg = ws.get_Range(rg1, rg2);
            rg.Value2 = array;

            return true;
        }

        public static bool ResizeTranspose(dynamic array)
        {
            Excel.Application xlApp = (Excel.Application)ExcelDnaUtil.Application;
            Excel.Workbook wb = xlApp.ActiveWorkbook;
            if (wb == null) return false;
            Excel.Worksheet ws = wb.ActiveSheet;
            Excel.Range rg1 = ws.Application.ActiveCell;

            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            if (rows == 0 || cols == 0)
            {
                LogDisplay.WriteLine("Array Rows or Cols is null !");
                return false;
            }

            object[,] result = new object[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    result[i, j] = array[j, i];
                }
            }

            Excel.Range rg2 = rg1.get_Offset(rows - 1, cols-1);
            Excel.Range rg = ws.get_Range(rg1, rg2);
            rg.Value2 = result;

            return true;
        }

        public static double[,] ResizeNumerics(double[,] array)
        {
            var caller = Excel(xlfCaller) as ExcelReference;
            if (caller == null) 
                return array;

            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            if (rows == 0 || columns == 0) 
                return array;
            
            if ((caller.RowLast - caller.RowFirst + 1 == rows) &&
                (caller.ColumnLast - caller.ColumnFirst + 1 == columns))
            {
                // Size is already OK - just return result
                return array;
            }
            
            var rowLast = caller.RowFirst + rows - 1;
            var columnLast = caller.ColumnFirst + columns - 1;

            if (rowLast > ExcelDnaUtil.ExcelLimits.MaxRows - 1 ||
                columnLast > ExcelDnaUtil.ExcelLimits.MaxColumns - 1)
            {
                // Can't resize - goes beyond the end of the sheet - just return null (for #NUM!)
                // (Can't give message here, or change cells)
                return null;
            }

            // TODO: Add guard for ever-changing result?
            ExcelAsyncUtil.QueueAsMacro(() =>
            {
                // Create a reference of the right size
                var target = new ExcelReference(caller.RowFirst, rowLast, caller.ColumnFirst, columnLast, caller.SheetId);
                DoResize(target); // Will trigger a recalc by writing formula
            });

            // Return what we have - to prevent flashing #N/A
            return array;
        }

        public static void DoResize(ExcelReference target)
        {
            // Get the current state for reset later
            using (new ExcelEchoOffHelper())
            using (new ExcelCalculationManualHelper())
            {
                ExcelReference firstCell = new ExcelReference(target.RowFirst, target.RowFirst, target.ColumnFirst, target.ColumnFirst, target.SheetId);
                
                // Get the formula in the first cell of the target
                string formula = (string)Excel(xlfGetCell, 41, firstCell);
                bool isFormulaArray = (bool)Excel(xlfGetCell, 49, firstCell);
                if (isFormulaArray)
                {
                    // Select the sheet and firstCell - needed because we want to use SelectSpecial.
                    using (new ExcelSelectionHelper(firstCell))
                    {
                        // Extend the selection to the whole array and clear
                        Excel(xlcSelectSpecial, 6);
                        ExcelReference oldArray = (ExcelReference)Excel(xlfSelection);

                        oldArray.SetValue(ExcelEmpty.Value);
                    }
                }
                // Get the formula and convert to R1C1 mode
                bool isR1C1Mode = (bool)Excel(xlfGetWorkspace, 4);
                string formulaR1C1 = formula;
                if (!isR1C1Mode)
                {
                    object formulaR1C1Obj;
                    XlReturn formulaR1C1Return = TryExcel(xlfFormulaConvert, out formulaR1C1Obj, formula, true, false, ExcelMissing.Value, firstCell);
                    if (formulaR1C1Return != XlReturn.XlReturnSuccess || formulaR1C1Obj is ExcelError)
                    {
                        string firstCellAddress = (string)Excel(xlfReftext, firstCell, true);
                        Excel(xlcAlert, "Cannot resize array formula at " + firstCellAddress + " - formula might be too long when converted to R1C1 format.");
                        firstCell.SetValue("'" + formula);
                        return;
                    }
                    formulaR1C1 = (string)formulaR1C1Obj;
                }
                // Must be R1C1-style references
                object ignoredResult;
                //Debug.Print("Resizing START: " + target.RowLast);
                XlReturn formulaArrayReturn = TryExcel(xlcFormulaArray, out ignoredResult, formulaR1C1, target);
                //Debug.Print("Resizing FINISH");

                // TODO: Find some dummy macro to clear the undo stack

                if (formulaArrayReturn != XlReturn.XlReturnSuccess)
                {
                    string firstCellAddress = (string)Excel(xlfReftext, firstCell, true);
                    Excel(xlcAlert, "Cannot resize array formula at " + firstCellAddress + " - result might overlap another array.");
                    // Might have failed due to array in the way.
                    firstCell.SetValue("'" + formula);
                }
            }
        }
    }

    // RIIA-style helpers to deal with Excel selections    
    // Don't use if you agree with Eric Lippert here: http://stackoverflow.com/a/1757344/44264
    public class ExcelEchoOffHelper : XlCall, IDisposable
    {
        object oldEcho;

        public ExcelEchoOffHelper()
        {
            oldEcho = Excel(xlfGetWorkspace, 40);
            Excel(xlcEcho, false);
        }
        
        public void Dispose()
        {
            Excel(xlcEcho, oldEcho);
        }
    }
    
    public class ExcelCalculationManualHelper : XlCall, IDisposable
    {
        object oldCalculationMode;

        public ExcelCalculationManualHelper()
        {
            oldCalculationMode = Excel(xlfGetDocument, 14);
            Excel(xlcOptionsCalculation, 3);
        }
        
        public void Dispose()
        {
            Excel(xlcOptionsCalculation, oldCalculationMode);
        }
    }

    // Select an ExcelReference (perhaps on another sheet) allowing changes to be made there.
    // On clean-up, resets all the selections and the active sheet.
    // Should not be used if the work you are going to do will switch sheets, amke new sheets etc.
    public class ExcelSelectionHelper : XlCall, IDisposable
    {
        object oldSelectionOnActiveSheet;
        object oldActiveCellOnActiveSheet;

        object oldSelectionOnRefSheet;
        object oldActiveCellOnRefSheet;

        public ExcelSelectionHelper(ExcelReference refToSelect)
        {
            // Remember old selection state on the active sheet
            oldSelectionOnActiveSheet = Excel(xlfSelection);
            oldActiveCellOnActiveSheet = Excel(xlfActiveCell);

            // Switch to the sheet we want to select
            string refSheet = (string)Excel(xlSheetNm, refToSelect);
            Excel(xlcWorkbookSelect, new object[] { refSheet });

            // record selection and active cell on the sheet we want to select
            oldSelectionOnRefSheet = Excel(xlfSelection);
            oldActiveCellOnRefSheet = Excel(xlfActiveCell);

            // make the selection
            Excel(xlcFormulaGoto, refToSelect);
        }

        public void Dispose()
        {
            // Reset the selection on the target sheet
            Excel(xlcSelect, oldSelectionOnRefSheet, oldActiveCellOnRefSheet);

            // Reset the sheet originally selected
            string oldActiveSheet = (string)Excel(xlSheetNm, oldSelectionOnActiveSheet);
            Excel(xlcWorkbookSelect, new object[] { oldActiveSheet });

            // Reset the selection in the active sheet (some bugs make this change sometimes too)
            Excel(xlcSelect, oldSelectionOnActiveSheet, oldActiveCellOnActiveSheet);
        }
    }
}