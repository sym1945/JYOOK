using System.Xml.Serialization;

namespace JYOOK.Domain
{
    public class SalesProduct
    {
        [XmlIgnore]
        public Production Production { get; set; }

        [XmlAttribute]
        public int ProductionId { get; set; }

        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public double Weight { get; set; }

        [XmlAttribute]
        public double ShippingCost { get; set; }

        [XmlAttribute]
        public double CommssionRate { get; set; }

        [XmlIgnore]
        public PackingInfo PackingInfo { get; set; }
    }
}