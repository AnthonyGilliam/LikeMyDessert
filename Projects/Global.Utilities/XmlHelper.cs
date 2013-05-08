using System;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Collections.Specialized;

namespace Global.Utilities.Xml
{
    /// <summary>
    /// Deprecated. Use Prommis.Utilities.Xml.XmlHelper instead.
    /// Class for helping in reading and writing xml.
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Creates and returns an XmlElement object with the given name and value.
        /// </summary>
        /// <param name="elementName">Name of the xml element</param>
        /// <param name="elementValue">Value in the xml element</param>
        /// <returns></returns>
        public static XmlElement CreateXmlElement(
            XmlDocument document
            , string elementName
            , string elementValue)
        {
            if (elementValue == null)
            {
                elementValue = string.Empty;
            }

            XmlElement element = document.CreateElement(elementName);
            element.InnerXml = XmlEscape(elementValue);
            return element;
        }

        /// <summary>
        /// Creates and returns an XmlElement object of given name and value with xsi:nil attribute set to true.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="elementName">Name of the xml element</param>
        /// <param name="elementValue">Value in the xml element</param>
        /// <param name="nillabel"></param>
        /// <returns></returns>
        public static XmlElement CreateXmlElement(
            XmlDocument document
            , string elementName
            , string elementValue
            , bool nillabel)
        {
            XmlElement element = CreateXmlElement(
                document
                , elementName
                , elementValue);

            if (nillabel && (elementValue == null || elementValue.Length == 0))
            {
                element.SetAttributeNode(CreateXmlNillabelAttribute(document));
            }

            return element;
        }

        /// <summary>
        /// Creates and returns an Nillabel Namespace XmlAttribute object.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static XmlAttribute CreateXmlNillabelNamespaceAttribute(XmlDocument document)
        {
            XmlAttribute attribute = document.CreateAttribute("xmlns:xsi");
            attribute.Value = "http://www.w3.org/2001/XMLSchema-instance";
            return attribute;
        }


        /// <summary>
        /// Creates and returns an xsi:nil='true' XmlAttribute object.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static XmlAttribute CreateXmlNillabelAttribute(XmlDocument document)
        {
            XmlAttribute attribute = document.CreateAttribute(
                "xsi"
                , "nil"
                , "http://www.w3.org/2001/XMLSchema-instance");
            attribute.Value = "true";
            return attribute;
        }

        /// <summary>
        /// Creates and returns an XmlAttribute object with the given name and value.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="attributeName">Name in the xml attribute</param>
        /// <param name="attributeValue">Value in the xml attribute</param>
        /// <returns></returns>
        public static XmlAttribute CreateXmlAttribute(
            XmlDocument document
            , string attributeName
            , string attributeValue)
        {
            XmlAttribute attribute = document.CreateAttribute(attributeName);
            attribute.Value = attributeValue;
            return attribute;
        }

        /// <summary>
        /// Method to help generate friendly xml. Special characters
        /// should be handled when returned from this fuction.
        /// </summary>
        /// <param name="toEscape">The string that may contain special
        /// characters to be escaped.</param>
        /// <returns>The xml friendly version of the string.</returns>
        public static string XmlEscape(string toEscape)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, writerSettings))
            {
                xmlWriter.WriteString(toEscape);
            }
            return stringWriter.ToString();
        }

        /// <summary>
        /// Validates the xml file with a schema that is linked to the Document Type
        /// </summary>
        /// <param name="filePath">The path to the XML file</param>
        /// <param name="documentType">The Document Type passed in by the client.</param>
        public static void SchemaValidation(
            string schemaPath
            , string filePath)
        {
            if (schemaPath == null)
            {
                throw new ArgumentNullException("Schema path cannot be null.");
            }
            if (filePath == null)
            {
                throw new ArgumentNullException("File path cannot be null.");
            }

            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(null, schemaPath);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;

            XmlReader reader = null;
            try
            {
                reader = XmlReader.Create(
                filePath
                , settings);
                while (reader.Read())
                {
                    // Reads the xml to verify that it is valid. If we are able to read through
                    // the entire file, then are assuming that the xml is valid.
                }
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Validates the xml file with a schema that is linked to the Document Type
        /// </summary>
        /// <param name="filePath">The path to the XML file</param>
        /// <param name="documentType">The Document Type passed in by the client.</param>
        public static void ValidateFileAgainstSchema(
            XmlSchema schema
            , string xml)
        {
            if (schema == null)
            {
                throw new ArgumentNullException("Schema cannot be null.");
            }
            if (xml == null)
            {
                throw new ArgumentNullException("Xml cannot be null.");
            }

            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(schema);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;

            XmlReader reader = null;
            try
            {
                System.IO.StringReader stringReader = new StringReader(xml);
                reader = XmlReader.Create(
                    stringReader
                    , settings);
                while (reader.Read())
                {
                    // Reads the xml to verify that it is valid. If we are able to read through
                    // the entire file, then are assuming that the xml is valid.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not validate: " + ex.Message);
                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Formats the datetime to xml datetime field.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatDateTimeToXMLDateTime(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return dateTime.Value.ToString("yyyy-MM-ddThh:mm:ss");
        }

        /// <summary>
        /// Formats the datetime to xml datetime field.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatDateTimeToXMLTime(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return dateTime.Value.ToString("hh:mm:ss");
        }

        /// <summary>
        /// Formats the datetime to xml date field.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatDateTimeToXMLShortDate(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return dateTime.Value.ToString("yyyy-MM-dd");
        }              
    }
}