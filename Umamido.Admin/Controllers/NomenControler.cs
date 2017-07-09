using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Umamido.Common;
using Umamido.Common.Tools;

namespace Umamido.Admin.Controllers
{
    public class NomenController : BaseController
    {
    
        #region Langs
        [HttpGet]
        public ActionResult Langs()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AllLangs()
        {
            

            var result = this.DL.GetLangs();
            return Json(result, JsonRequestBehavior.AllowGet);            
        }

        [HttpPost]
        public ActionResult LangChangeActive(int langId)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.LangChangeActive(langId);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetLang(LangRowModel model)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.SaveLang(model);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLang(int langId)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            return Json(this.DL.GetLang(langId), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Images
        [HttpGet]
        public ActionResult Images()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AllImages()
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            var result = this.DL.GetImages();            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //запис в кеша
        [HttpPost]
        public ActionResult StoreImageInCache(string guid, string content, string filename)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            ImageFileModel ifm = new ImageFileModel();
            ifm.Content = Convert.FromBase64String(content.Split(',')[1]); 
            ifm.Guid = guid;
            ifm.FileName = filename;
            HttpContext.Cache.Add(ifm.Guid, ifm, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        //показване от кеша
        [HttpGet]
        public ActionResult GetImageContentFromCache(string guid)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            ImageFileModel ifm = (ImageFileModel)HttpContext.Cache[guid];
            string filename = ifm.FileName;            
            byte[] filedata = ifm.Content;
            string contentType = MimeMapping.GetMimeMapping(ifm.FileName);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }



        //запис в базата
        [HttpPost]
        public ActionResult SetImage(ImageRowModel model)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            if (!string.IsNullOrEmpty(model.Guid))
            {
                ImageFileModel ifm = (ImageFileModel)HttpContext.Cache[model.Guid];
                model.FileName = ifm.FileName;
                model.Content = ifm.Content;
                HttpContext.Cache.Remove(model.Guid);
            }
            DL.SaveImage(model);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }


        //взимане на запис от базата
        [HttpPost]
        public ActionResult GetImage(int imageId)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            var img = this.DL.GetImage(imageId);
            img.Content = null;
            return Json(img, JsonRequestBehavior.AllowGet);
        }

        //показване от базата
        [HttpGet]
        public ActionResult GetImageContentFromDB(int imageId)
        {
            ImageFileModel ifm = DL.GetImageFile(imageId);
            string filename = ifm.FileName;
            byte[] filedata = ifm.Content;
            string contentType = MimeMapping.GetMimeMapping(ifm.FileName);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }
        //деактивиране
        [HttpPost]
        public ActionResult ImageChangeActive(int imageId)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.ImageChangeActive(imageId);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Restaurants
        [HttpGet]
        public ActionResult Restaurants()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AllRestaurants()
        {
           

            var result = this.DL.GetRestaurants();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RestaurantChangeActive(int restaurantId)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.RestaurantChangeActive(restaurantId);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetRestaurant(RestaurantRowModel model)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.SaveRestaurant(model);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Goods
        [HttpGet]
        public ActionResult Goods(int restaurantId)
        {

            RestaurantRowModel rm = DL.GetRestaurant(restaurantId);
            rm.FirstTitle = Tools.StripHtmlTags(rm.FirstTitle);
            return View(rm);
        }
        [HttpGet]
        public ActionResult AllGoods(int restaurantId)
        {
            var result = this.DL.GetGoods(restaurantId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GoodChangeActive(int restaurantId)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.GoodChangeActive(restaurantId);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetGood(GoodRowModel model)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.SaveGood(model);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Sliders
        [HttpGet]
        public ActionResult Sliders()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AllSliders()
        {
            var result = this.DL.GetSliders();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SliderChangeActive(int sliderId)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.SliderChangeActive(sliderId);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetSlider(SliderRowModel model)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.SaveSlider(model);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Texts
        [HttpGet]
        public ActionResult Texts()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AllTexts()
        {
            var result = this.DL.GetTexts();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public ActionResult SetText(TextRowModel model)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.SaveText(model);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }


        #endregion


    }
}