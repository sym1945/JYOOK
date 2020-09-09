using System.Xml.Serialization;

namespace JYOOK.Domain
{
    public class Production
    {
        [XmlIgnore]
        public LivestockProduct LivestockProduct { get; set; }

        [XmlAttribute]
        public int LivestockProductId { get; set; }

        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public double Weight { get; set; }

        [XmlAttribute]
        public double TransferRate { get; set; }

        [XmlAttribute]
        public double MarginRate { get; set; }

    }
}