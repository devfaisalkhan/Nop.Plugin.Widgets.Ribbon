using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.Ribbon
{
    public class RibbonSettings : ISettings
    {
        public string BackgroundColor { get; set; }
        public string Color { get; set; }
        public bool EnableNew { get; set; }
        public bool EnableFeatured { get; set; }
    }
}