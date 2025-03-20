using System.Xml.Serialization;

namespace AltaSoft.Simpra.Tests.Models;

public enum ColorX
{
    [XmlEnum("R")]
    Red,
    [XmlEnum("G")]
    Green,
    [XmlEnum("B")]
    Blue
}
