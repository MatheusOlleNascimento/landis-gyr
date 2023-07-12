using LandisGyr.Enums;

namespace LandisGyr.Models
{
    public class ConsumerUnit
    {
        public string SerialNumber { get; set; } = default!;
        public Meter Meter { get; set; } = default!;
        public SwitchState SwitchState { get; set; }
    }
}
