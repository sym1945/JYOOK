using System;
using System.ComponentModel;

namespace JYOOK.Domain
{
    [Flags]
    public enum ResourceType
    {
        [Description("스티로폼")]
        Box = 0b0000_0001,

        [Description("아이스팩")]
        IcePack = 0b0000_0010,

        [Description("진공지")]
        PackingSheet = 0b0000_0100,

        [Description("배송비")]
        ShippingCost = 0b0000_1000,
    }
}