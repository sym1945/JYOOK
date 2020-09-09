using System.Xml.Serialization;

namespace JYOOK.Domain
{
    public class LivestockProduct
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public double Weight { get; set; }

        [XmlAttribute]
        public uint UnitPrice { get; set; }
    }
}