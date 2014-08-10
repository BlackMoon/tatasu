using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ModelEditor
{
    /// <summary>
    /// XML Serialization and Deserialization Assistant Class
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// XML Serialization to string
        /// </summary>
        /// <param name="t"> The System.Object that System.Xml.Serialization.XmlSerializer can serilialize.</param>
        /// <param name="root"> XML root node.</param>
        public static string Serialize<T>(T t, string root = null)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            StringWriter sw = new StringWriter();
            using (XmlWriter xw = XmlWriter.Create(sw, settings))
            {
                XmlRootAttribute xra = new XmlRootAttribute(root);
                XmlSerializer xs = new XmlSerializer(typeof(T), xra);
                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                xns.Add("", "");
                xs.Serialize(xw, t, xns);
            }
            return sw.ToString();
        }
        /// <summary>
        /// XML Serialization to file
        /// </summary>
        /// <param name="t"> The System.Object that System.Xml.Serialization.XmlSerializer can serilialize.</param>
        /// <param name="tw"> The System.IO.TextWriter to write to.</param>             
        /// <param name="root"> XML root node.</param>             
        public static void Serialize<T>(T t, TextWriter tw, string root = null)
        {
            TextWriter tws = TextWriter.Synchronized(tw);

            XmlRootAttribute xra = new XmlRootAttribute(root);
            XmlSerializer xs = new XmlSerializer(typeof(T), xra);
            XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
            xns.Add("", "");
            xs.Serialize(tws, t, xns);
        }
        /// <summary>
        /// XML Deserialization from string        
        /// </summary>              
        /// <param name="xmlstring"> The System.String that contains the XML document to deserialize.</param>
        /// <param name="root"> XML root node.</param>
        /// <returns>The System.Object being deserialized.</returns>  
        public static T Deserialize<T>(string xmlstring, string root = null)
        {
            XmlRootAttribute xra = new XmlRootAttribute(root);
            XmlSerializer xs = new XmlSerializer(typeof(T), xra);
            using (TextReader tr = new StringReader(xmlstring))
            {
                return (T)xs.Deserialize(tr);
            }
        }
        /// <summary>
        /// XML Deserialization from file
        /// </summary>        
        /// <param name="tr"> The System.IO.TextReader to read from.</param>             
        /// <param name="root"> XML root node.</param>             
        /// <returns>The System.Object being deserialized.</returns>  
        public static T Deserialize<T>(TextReader tr, string root = null)
        {
            XmlRootAttribute xra = new XmlRootAttribute(root);
            XmlSerializer xs = new XmlSerializer(typeof(T), xra);
            return (T)xs.Deserialize(tr);
        }
    }
}
