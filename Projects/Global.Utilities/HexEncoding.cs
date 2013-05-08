using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Utilities
{
    public static class HexEncoding
    {
        public static int GetByteCount(string hexString)
        {
            int numHexChars = 0;
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                    numHexChars++;
            }

            // If string length is odd, remainder will be dropped
            // in integer division
            return numHexChars / 2; // 2 characters per byte
        }
        /// <summary>
        /// Creates a byte array from the hexadecimal string. Each two characters are combined
        /// to create one byte. First two hexadecimal characters become first byte in returned array.
        /// Non-hexadecimal characters are ignored. 
        /// </summary>
        /// <param name="hexString">string to convert to byte array</param>
        /// <param name="discarded">number of characters in string ignored</param>
        /// <returns>byte array, in the same left-to-right order as the hexString</returns>
        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;

            // XML Reader/Writer is highly optimized for BinHex conversions
            // use instead of byte/character replacement for performance on 
            // arrays larger than a few dozen elements

            hexString = "<node>" + hexString + "</node>";

            System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(
                hexString,
                System.Xml.XmlNodeType.Element,
                null);

            tr.Read();

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            int bufLen = 1024;
            int cap = 0;
            byte[] buf = new byte[bufLen];

            do
            {
                cap = tr.ReadBinHex(buf, 0, bufLen);
                ms.Write(buf, 0, cap);
            } while (cap == bufLen);

            return ms.ToArray();
        }

        /// <summary>
        /// Converts a byte array to a hexadecimal string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToString(byte[] bytes)
        {

            // XML Reader/Writer is highly optimized for BinHex conversions
            // use instead of byte/character replacement for performance on 
            // arrays larger than a few dozen elements

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(sw);

            tw.WriteStartElement("node");

            tw.WriteBinHex(bytes, 0, bytes.Length);

            tw.WriteEndElement();

            tw.Flush();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(sw.ToString());
            return doc.DocumentElement.InnerText;
        }
        /// <summary>
        /// Determines if given string is in proper hexadecimal string format
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static bool InHexFormat(string hexString)
        {
            bool hexFormat = true;

            foreach (char digit in hexString)
            {
                if (!IsHexDigit(digit))
                {
                    hexFormat = false;
                    break;
                }
            }
            return hexFormat;
        }

        /// <summary>
        /// Returns true is c is a hexadecimal digit (A-F, a-f, 0-9)
        /// </summary>
        /// <param name="c">Character to test</param>
        /// <returns>true if hex digit, false if not</returns>
        public static bool IsHexDigit(Char c)
        {
            int numChar;
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            c = Char.ToUpper(c);
            numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6))
                return true;
            if (numChar >= num1 && numChar < (num1 + 10))
                return true;
            return false;
        }
    }
}
