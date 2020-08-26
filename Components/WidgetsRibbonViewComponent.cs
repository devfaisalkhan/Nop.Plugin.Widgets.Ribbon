using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.Ribbon.Infrastructure.Cache;
using Nop.Plugin.Widgets.Ribbon.Models;
using Nop.Services.Caching;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Ribbon.Components
{
    [ViewComponent(Name = "WidgetsRibbon")]
    public class WidgetsRibbonViewComponent : NopViewComponent
    {
        private readonly ICacheKeyService _cacheKeyService;
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;

        public WidgetsRibbonViewComponent(ICacheKeyService cacheKeyService,
            IStoreContext storeContext,
            IStaticCacheManager staticCacheManager,
            ISettingService settingService,
            IPictureService pictureService,
            IWebHelper webHelper)
        {
            _cacheKeyService = cacheKeyService;
            _storeContext = storeContext;
            _staticCacheManager = staticCacheManager;
            _settingService = settingService;
            _pictureService = pictureService;
            _webHelper = webHelper;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            

            var ribbonSetting = _settingService.LoadSetting<RibbonSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel
            {
                Color = ribbonSetting.Color,
                BackgroundColor = ribbonSetting.BackgroundColor,
                EnableFeatured = ribbonSetting.EnableFeatured,
                EnableNew = ribbonSetting.EnableNew
            };
            return View("~/Plugins/Widgets.Ribbon/Views/PublicInfo.cshtml", model);
        }
    }
}
