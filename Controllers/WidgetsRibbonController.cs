using System.Linq;
using LinqToDB.Common;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.Ribbon.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.Ribbon.Controllers
{
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class WidgetsRibbonController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IProductService _productService;

        public WidgetsRibbonController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService, 
            IPictureService pictureService,
            ISettingService settingService,
            IStoreContext storeContext, IProductService productService)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _settingService = settingService;
            _storeContext = storeContext;
            _productService = productService;
        }

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();



            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var ribbonSettings = _settingService.LoadSetting<RibbonSettings>(storeScope);
            var model = new ConfigurationModel
            {
                BackgroundColor = ribbonSettings.BackgroundColor,
                Color = ribbonSettings.Color,
                EnableFeatured = ribbonSettings.EnableFeatured,
                EnableNew = ribbonSettings.EnableNew,

                ActiveStoreScopeConfiguration = storeScope
            };




            if (storeScope > 0)
            {
                model.BackgroundColor_OverrideForStore = _settingService.SettingExists(ribbonSettings, x => x.BackgroundColor, storeScope);
                model.Color_OverrideForStore = _settingService.SettingExists(ribbonSettings, x => x.Color, storeScope);
                model.EnableFeatured_OverrideForStore = _settingService.SettingExists(ribbonSettings, x => x.EnableFeatured, storeScope);
                model.EnableNew_OverrideForStore = _settingService.SettingExists(ribbonSettings, x => x.EnableNew, storeScope);
            }

            return View("~/Plugins/Widgets.Ribbon/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var ribbonSettings = _settingService.LoadSetting<RibbonSettings>(storeScope);

            

            ribbonSettings.Color = model.Color;
            ribbonSettings.BackgroundColor = model.BackgroundColor;
            ribbonSettings.EnableFeatured = model.EnableFeatured;
            ribbonSettings.EnableNew = model.EnableNew;


            ///* We do not clear cache after each setting update.
            // * This behavior can increase performance because cached settings will not be cleared 
            // * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(ribbonSettings, x => x.Color, model.Color_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(ribbonSettings, x => x.BackgroundColor, model.BackgroundColor_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(ribbonSettings, x => x.EnableFeatured, model.EnableFeatured_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(ribbonSettings, x => x.EnableNew, model.EnableNew_OverrideForStore, storeScope, false);

            ////now clear settings cache
            _settingService.ClearCache();

            ////get current picture identifiers
            //var currentPictureIds = new[]
            //{
            //    nivoSliderSettings.Picture1Id,
            //    nivoSliderSettings.Picture2Id,
            //    nivoSliderSettings.Picture3Id,
            //    nivoSliderSettings.Picture4Id,
            //    nivoSliderSettings.Picture5Id
            //};

            ////delete an old picture (if deleted or updated)
            //foreach (var pictureId in previousPictureIds.Except(currentPictureIds))
            //{ 
            //    var previousPicture = _pictureService.GetPictureById(pictureId);
            //    if (previousPicture != null)
            //        _pictureService.DeletePicture(previousPicture);
            //}



            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

        public IActionResult Ribbon(int productId)
        {
            var model = new RibbonModel();
            var product = _productService.GetProductById(productId);
            //check for new
            if (product.MarkAsNew)
                model.MarkAsNew = true;

            //check for showonhomepage
            if (product.ShowOnHomepage)
                model.ShowOnHomePage = true;
            return Json(model);
        }
    }
}