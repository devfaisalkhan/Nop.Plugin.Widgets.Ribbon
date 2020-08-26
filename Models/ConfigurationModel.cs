using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.Ribbon.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        public string BackgroundColor { get; set; }
        public bool BackgroundColor_OverrideForStore { get; set; }
        public string Color { get; set; }
        public bool Color_OverrideForStore { get; set; }
        public bool EnableNew { get; set; }
        public bool EnableNew_OverrideForStore { get; set; }
        public bool EnableFeatured { get; set; }
        public bool EnableFeatured_OverrideForStore { get; set; }
    }
}