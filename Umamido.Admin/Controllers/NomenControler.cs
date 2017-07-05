﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Umamido.Common;

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
            this.DL.LangChangeActive(langId);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetLang(LangRowModel model)
        {
            this.DL.SaveLang(model);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLang(int langId)
        {            
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
            var result = this.DL.GetImages();            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //запис в кеша
        [HttpPost]
        public ActionResult StoreImageInCache(string guid, string content, string filename)
        {
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
            this.DL.RestaurantChangeActive(restaurantId);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetRestaurant(RestaurantRowModel model)
        {
            this.DL.SaveRestaurant(model);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        
        #endregion
    }
}