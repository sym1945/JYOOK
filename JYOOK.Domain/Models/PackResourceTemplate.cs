using System;
using System.Xml.Serialization;

namespace JYOOK.Domain
{
    public class PackResourceTemplate
    {
        [XmlAttribute]
        public Guid PackingTemplateId { get; set; }

        [XmlAttribute]
        public Guid ExtraResourceId { get; set; }

        [XmlIgnore]
        public ExtraResource Resource { get; set; }

        [XmlAttribute]
        public ushort Count { get; set; }
    }
}