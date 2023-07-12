using LandisGyr.Enums;

namespace LandisGyr.Models
{
    public class Meter
    {
        public MeterModel Id { get; set; }
        public int Number { get; set; }
        public string FirmwareVersion { get; set; } = default!;
    }
}
