using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.Ribbon.Models
{
    public class PublicInfoModel : BaseNopModel
    {
        public string BackgroundColor { get; set; }
        public string Color { get; set; }
        public bool EnableNew { get; set; }
        public bool EnableFeatured { get; set; }
    }
}