using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Umamido.Common;
using Umamido.Common.Tools;

namespace Umamido.DL
{
    public class DLFuncs
    {
        public static void MapperInitialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserLevel, UserLevelModel>();
                cfg.CreateMap<User, UserRowModel>();
                cfg.CreateMap<UserRowModel, User>();
                cfg.CreateMap<Lang, LangRowModel>();
                cfg.CreateMap<LangRowModel, Lang>();
                cfg.CreateMap<Image, ImageRowModel>();
                cfg.CreateMap<Image, ImageFileModel>();
                cfg.CreateMap<SearchReq_Result, ReqQueryRowModel>();
                cfg.CreateMap<ForDispatch_Result, ReqModel>();
                cfg.CreateMap<ForCollect_Result, ReqModel>();
                cfg.CreateMap<CollectDetails_Result, CollectDetailsModel>();
                cfg.CreateMap<ForDeliver_Result, ReqModel>();
                cfg.CreateMap<MessageModel, Message>();
                cfg.CreateMap<Message, MessageModel>();
                cfg.CreateMap<AddressCheck, AddressCheckModel>();
                cfg.CreateMap<AddressCheckModel, AddressCheck>();

            });
        }
        UmamidoEntities entities;
        public DLFuncs()
        {
            entities = new UmamidoEntities();
        }


        public UserRowModel Login(LoginModel model)
        {
            var result = entities.User.FirstOrDefault(u => u.UserName == model.Username && u.Password == model.PasswordMd5 && u.IsActive);
            return (result == null ? null :
                new UserRowModel()
                {
                    IsActive = true,
                    UserId = result.UserId,
                    UserLevelId = result.UserLevelId,
                    UserName = result.UserName
                }
                );
        }

        #region Users

        public UserLevelModel[] GetUserLevels()
        {
            List<UserLevelModel> res = new List<UserLevelModel>();
            var lvls = entities.UserLevel.OrderBy(u => u.UserLevelId).ToArray();
            foreach (var u in lvls)
                res.Add(Mapper.Map<UserLevelModel>(u));
            return res.ToArray();
        }

        public UserRowModel[] GetUsers(int userLevelId = -1)
        {
            List<UserRowModel> res = new List<UserRowModel>();
            var usrs = entities.User.Where(u => userLevelId == -1 || u.UserLevelId == userLevelId).OrderBy(u => u.UserName).ToArray();
            foreach (var u in usrs)
                res.Add(Mapper.Map<UserRowModel>(u));
            return res.ToArray();
        }

        public UserRowModel GetUser(int userId)
        {
            var usr = entities.User.First(u => u.UserId == userId);
            return (Mapper.Map<UserRowModel>(usr));
        }

        public Boolean UserExists(string username)
        {

            return entities.User.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower()) != null; ;
        }


        public void SaveUser(UserRowModel model)
        {
            if (model.UserId == -1)
            {
                var usr = new User();
                usr.UserName = model.UserName;
                usr.IsActive = model.IsActive;
                usr.Password = model.PasswordMd5;
                usr.UserLevelId = model.UserLevelId;
                entities.User.Add(usr);
            }
            else
            {
                var usr = entities.User.First(u => u.UserId == model.UserId);
                usr.UserName = model.UserName;
                usr.IsActive = model.IsActive;
                usr.Password = string.IsNullOrEmpty(model.Password) ? usr.Password : model.PasswordMd5;
                usr.UserLevelId = model.UserLevelId;
            }
            entities.SaveChanges();
        }


        public void UserChangeActive(int userId)
        {
            var usr = entities.User.First(u => u.UserId == userId);
            usr.IsActive = !usr.IsActive;
            entities.SaveChanges();
        }


        #endregion


        #region Langs

        public LangRowModel[] GetLangs()
        {
            List<LangRowModel> res = new List<LangRowModel>();
            var langs = entities.Lang.OrderBy(l => l.LangId).ToArray();
            foreach (var l in langs)
                res.Add(Mapper.Map<LangRowModel>(l));
            return res.ToArray();
        }

        public LangRowModel GetLang(int langId)
        {
            var lang = entities.Lang.First(l => l.LangId == langId);
            return (Mapper.Map<LangRowModel>(lang));
        }



        public void SaveLang(LangRowModel model)
        {
            if (model.LangId == -1)
            {
                entities.Lang.Add(Mapper.Map<Lang>(model));
            }
            else
            {
                var lang = entities.Lang.First(l => l.LangId == model.LangId);
                lang.LangName = model.LangName;
                lang.IsActive = model.IsActive;

            }
            entities.SaveChanges();
        }


        public void LangChangeActive(int langId)
        {
            var lng = entities.Lang.First(l => l.LangId == langId);
            lng.IsActive = !lng.IsActive;
            entities.SaveChanges();
        }
        #endregion

        #region Images
        public ImageRowModel[] GetImages()
        {
            List<ImageRowModel> res = new List<ImageRowModel>();
            var images = entities.Image.OrderBy(i => i.ImageId).ToArray();
            foreach (var i in images)
            {
                res.Add(Mapper.Map<ImageRowModel>(i));
                res[res.Count - 1].Content = null;
            }
            return res.ToArray();
        }


        public ImageRowModel GetImage(int imageId)
        {
            var image = entities.Image.First(i => i.ImageId == imageId);
            return (Mapper.Map<ImageRowModel>(image));
        }

        public ImageFileModel GetImageFile(int imageId)
        {
            var image = entities.Image.FirstOrDefault(i => i.ImageId == imageId);
            if (image == null)
                return null;
            return Mapper.Map<ImageFileModel>(image);
        }

        public void SaveImage(ImageRowModel model)
        {
            if (model.ImageId == -1)
            {
                entities.Image.Add(new Image()
                {
                    Content = model.Content,
                    Filename = model.FileName,
                    ImageName = model.ImageName,
                    IsActive = model.IsActive
                });
            }
            else
            {
                var image = entities.Image.First(i => i.ImageId == model.ImageId);
                image.ImageName = model.ImageName;
                if (image.Content != null)
                {
                    image.Filename = model.FileName;
                    image.Content = model.Content;
                }
                image.IsActive = model.IsActive;

            }
            entities.SaveChanges();
        }

        public void ImageChangeActive(int imageId)
        {
            var img = entities.Image.First(i => i.ImageId == imageId);
            img.IsActive = !img.IsActive;
            entities.SaveChanges();
        }
        #endregion


        #region Restaurants

        public RestaurantRowModel[] GetRestaurants()
        {
            List<RestaurantRowModel> res = new List<RestaurantRowModel>();
            foreach (var r in entities.Restaurant)
            {
                RestaurantRowModel item = new RestaurantRowModel();
                item.RestaurantId = r.RestaurantId;
                item.ImageId = r.ImageId;
                item.BigImageId = r.BigImageId;
                item.LogoImageId = r.LogoImageId;
                item.IsActive = r.IsActive;
                List<TranslatableItemModel> titles = new List<TranslatableItemModel>();
                List<TranslatableItemModel> descs = new List<TranslatableItemModel>();
                foreach (var l in entities.Lang)
                {
                    var t = entities.RestaurantTitle.FirstOrDefault(row => row.RestaurantId == r.RestaurantId && row.LangId == l.LangId);
                    if (t != null && item.FirstTitle == null)
                        item.FirstTitle = t.Text;
                    TranslatableItemModel timTitle = new TranslatableItemModel()
                    {
                        ID = t != null ? t.RestaurantTitleId : -1,
                        LangId = l.LangId,
                        LangName = l.LangName,
                        Text = t != null ? t.Text : ""
                    };
                    titles.Add(timTitle);

                    var d = entities.RestaurantDesc.FirstOrDefault(row => row.RestaurantId == r.RestaurantId && row.LangId == l.LangId);
                    TranslatableItemModel timDesc = new TranslatableItemModel()
                    {
                        ID = d != null ? d.RestaurantDescId : -1,
                        LangId = l.LangId,
                        LangName = l.LangName,
                        Text = d != null ? d.Text : ""
                    };
                    descs.Add(timDesc);
                }


                item.Descriptions = descs.ToArray();
                item.Titles = titles.ToArray();
                res.Add(item);
            }

            return res.ToArray();
        }


        public RestaurantRowModel GetRestaurant(int restaurantId)
        {
            var r = entities.Restaurant.First(item => item.RestaurantId == restaurantId);
            RestaurantRowModel result = new RestaurantRowModel();
            result.RestaurantId = r.RestaurantId;
            result.ImageId = r.ImageId;
            result.BigImageId = r.BigImageId;
            result.LogoImageId = r.LogoImageId;

            result.IsActive = r.IsActive;
            var rt = r.RestaurantTitle.FirstOrDefault();
            if (rt != null)
                result.FirstTitle = rt.Text;
            return result;
        }

        public RestaurantPresentationModel GetRestaurantByLang(string lang, int restaurantId)
        {
            var r = entities.Restaurant.First(item => item.RestaurantId == restaurantId);
            RestaurantPresentationModel result = new RestaurantPresentationModel();
            result.RestaurantId = r.RestaurantId;
            result.ImageId = r.ImageId;
            result.BigImageId = r.BigImageId;
            result.LogoImageId = r.LogoImageId;

            result.IsActive = r.IsActive;
            var rt = r.RestaurantTitle.FirstOrDefault(t => t.Lang.LangName == lang);
            if (rt != null)
                result.FirstTitle = rt.Text;
            var rd = r.RestaurantDesc.FirstOrDefault(t => t.Lang.LangName == lang);
            if (rd != null)
                result.FirstDescription = rd.Text;

            result.AllRestaurants = this.GetRestaurantsByLang(lang);


            return result;


        }


        public void SaveRestaurant(RestaurantRowModel model)
        {
            if (model.RestaurantId == -1)
            {
                Restaurant r = new Restaurant()
                {
                    ImageId = model.ImageId,
                    BigImageId = model.BigImageId,
                    LogoImageId = model.LogoImageId,
                    IsActive = model.IsActive
                };
                entities.Restaurant.Add(r);
                entities.SaveChanges();
                model.RestaurantId = r.RestaurantId;
            }
            else
            {
                Restaurant r = entities.Restaurant.First(item => item.RestaurantId == model.RestaurantId);
                r.ImageId = model.ImageId;
                r.BigImageId = model.BigImageId;
                r.LogoImageId = model.LogoImageId;

                r.IsActive = model.IsActive;
                entities.SaveChanges();
            }
            foreach (var tt in model.Titles)
            {
                if (tt.ID == -1)
                {
                    entities.RestaurantTitle.Add(
                        new RestaurantTitle()
                        {
                            LangId = tt.LangId,
                            RestaurantId = model.RestaurantId,
                            Text = tt.Text
                        }
                        );
                }
                else
                {
                    entities.RestaurantTitle.First(item => item.RestaurantTitleId == tt.ID).Text = tt.Text;
                }
            }

            foreach (var tt in model.Descriptions)
            {
                if (tt.ID == -1)
                {
                    entities.RestaurantDesc.Add(
                        new RestaurantDesc()
                        {
                            LangId = tt.LangId,
                            RestaurantId = model.RestaurantId,
                            Text = tt.Text
                        }
                        );
                }
                else
                {
                    entities.RestaurantDesc.First(item => item.RestaurantDescId == tt.ID).Text = tt.Text;
                }
            }
            entities.SaveChanges();
        }


        public void RestaurantChangeActive(int restaurantId)
        {
            var r = entities.Restaurant.First(item => item.RestaurantId == restaurantId);
            r.IsActive = !r.IsActive;
            entities.SaveChanges();
        }

        public RestaurantRowModel[] GetRestaurantsByLang(string lang)
        {
            List<RestaurantRowModel> res = new List<RestaurantRowModel>();
            foreach (var r in entities.Restaurant.Where(rest => rest.IsActive))
            {
                RestaurantRowModel item = new RestaurantRowModel();
                item.RestaurantId = r.RestaurantId;
                item.ImageId = r.ImageId;
                item.BigImageId = r.BigImageId;
                item.LogoImageId = r.LogoImageId;
                var title = entities.RestaurantTitle.FirstOrDefault(rt => rt.Lang.LangName == lang && rt.RestaurantId == r.RestaurantId);
                item.FirstTitle = title == null ? "" : title.Text;
                res.Add(item);
            }

            return res.ToArray();
        }
        #endregion


        #region Goods

        public GoodRowModel[] GetGoods(int restaurantId)
        {
            List<GoodRowModel> res = new List<GoodRowModel>();
            foreach (var r in entities.Good.Where(item => item.RestaurantId == restaurantId))
            {
                GoodRowModel item = new GoodRowModel();
                item.GoodId = r.GoodId;
                item.Price = r.Price;
                item.RestaurantId = r.RestaurantId;
                item.ImageId = r.ImageId;
                item.CookTime = r.CookMinutes;
                item.IsActive = r.IsActive;
                item.Similar1Id = r.Similar1Id.HasValue ? r.Similar1Id.Value : 0;
                item.Similar2Id = r.Similar2Id.HasValue ? r.Similar2Id.Value : 0;
                item.Similar3Id = r.Similar3Id.HasValue ? r.Similar3Id.Value : 0;
                List<TranslatableItemModel> titles = new List<TranslatableItemModel>();
                List<TranslatableItemModel> descs = new List<TranslatableItemModel>();
                foreach (var l in entities.Lang)
                {
                    var t = entities.GoodTitle.FirstOrDefault(row => row.GoodId == r.GoodId && row.LangId == l.LangId);
                    if (t != null && item.FirstTitle == null)
                        item.FirstTitle = t.Text;
                    TranslatableItemModel timTitle = new TranslatableItemModel()
                    {
                        ID = t != null ? t.GoodTitleId : -1,
                        LangId = l.LangId,
                        LangName = l.LangName,
                        Text = t != null ? t.Text : ""
                    };
                    titles.Add(timTitle);

                    var d = entities.GoodDesc.FirstOrDefault(row => row.GoodId == r.GoodId && row.LangId == l.LangId);
                    TranslatableItemModel timDesc = new TranslatableItemModel()
                    {
                        ID = d != null ? d.GoodDescId : -1,
                        LangId = l.LangId,
                        LangName = l.LangName,
                        Text = d != null ? d.Text : ""
                    };
                    descs.Add(timDesc);
                }


                item.Descriptions = descs.ToArray();
                item.Titles = titles.ToArray();
                res.Add(item);
            }

            return res.ToArray();
        }

        public GoodRowModel[] GetGoodsByLang(int restaurantId, string lang)
        {
            List<GoodRowModel> res = new List<GoodRowModel>();
            foreach (var r in entities.Good.Where(item => restaurantId == -1 || item.RestaurantId == restaurantId))
            {
                GoodRowModel item = new GoodRowModel();
                item.GoodId = r.GoodId;
                item.Price = r.Price;
                item.RestaurantId = r.RestaurantId;
                item.ImageId = r.ImageId;
                item.CookTime = r.CookMinutes;
                item.IsActive = r.IsActive;
                var gt = r.GoodTitle.FirstOrDefault(t => t.Lang.LangName == lang);
                if (gt != null)
                    item.FirstTitle = gt.Text;
                var gd = r.GoodDesc.FirstOrDefault(t => t.Lang.LangName == lang);
                if (gd != null)
                    item.FirstDescription = gd.Text;
                res.Add(item);
            }

            return res.ToArray();
        }


        public GoodPresentationModel GetGoodByLang(int goodId, string lang)
        {
            GoodPresentationModel item = new GoodPresentationModel();
            var r = entities.Good.First(g => g.GoodId == goodId);
            item.GoodId = r.GoodId;
            item.Price = r.Price;
            item.RestaurantId = r.RestaurantId;
            item.ImageId = r.ImageId;
            item.CookTime = r.CookMinutes;
            item.IsActive = r.IsActive;
            item.Similar1Id = r.Similar1Id.Value;
            item.Similar2Id = r.Similar2Id.Value;
            item.Similar3Id = r.Similar3Id.Value;

            var gt = r.GoodTitle.FirstOrDefault(t => t.Lang.LangName == lang);
            if (gt != null)
                item.FirstTitle = gt.Text;
            var gd = r.GoodDesc.FirstOrDefault(t => t.Lang.LangName == lang);
            if (gd != null)
                item.FirstDescription = gd.Text;

            item.AllRestaurants = this.GetRestaurantsByLang(lang);
            List<GoodRowModel> similar = new List<GoodRowModel>();

            foreach (var g in GetGoodsByLang(r.RestaurantId, lang).Where(good => good.GoodId != r.GoodId).OrderBy(good => Guid.NewGuid().ToString()).Take(3))
                similar.Add(g);
            item.SimilarGoods = similar.ToArray();



            return item;
        }



        public void SaveGood(GoodRowModel model)
        {
            if (model.GoodId == -1)
            {
                Good r = new Good()
                {
                    ImageId = model.ImageId,
                    RestaurantId = model.RestaurantId,
                    IsActive = model.IsActive,
                    Price = model.Price,
                    CookMinutes = model.CookTime,
                    Similar1Id = model.Similar1Id == 0 ? null : (int?)model.Similar1Id,
                    Similar2Id = model.Similar2Id == 0 ? null : (int?)model.Similar2Id,
                    Similar3Id = model.Similar3Id == 0 ? null : (int?)model.Similar3Id
                };
                entities.Good.Add(r);
                entities.SaveChanges();
                model.GoodId = r.GoodId;
            }
            else
            {
                Good r = entities.Good.First(item => item.GoodId == model.GoodId);
                r.ImageId = model.ImageId;
                r.IsActive = model.IsActive;
                r.Price = model.Price;
                r.CookMinutes = model.CookTime;
                r.Similar1Id = model.Similar1Id;
                r.Similar2Id = model.Similar2Id;
                r.Similar3Id = model.Similar3Id;
                entities.SaveChanges();
            }
            foreach (var tt in model.Titles)
            {
                if (tt.ID == -1)
                {
                    entities.GoodTitle.Add(
                        new GoodTitle()
                        {
                            LangId = tt.LangId,
                            GoodId = model.GoodId,
                            Text = tt.Text
                        }
                        );
                }
                else
                {
                    entities.GoodTitle.First(item => item.GoodTitleId == tt.ID).Text = tt.Text;
                }
            }

            foreach (var tt in model.Descriptions)
            {
                if (tt.ID == -1)
                {
                    entities.GoodDesc.Add(
                        new GoodDesc()
                        {
                            LangId = tt.LangId,
                            GoodId = model.GoodId,
                            Text = tt.Text
                        }
                        );
                }
                else
                {
                    entities.GoodDesc.First(item => item.GoodDescId == tt.ID).Text = tt.Text;
                }
            }
            entities.SaveChanges();
        }


        public void GoodChangeActive(int GoodId)
        {
            var r = entities.Good.First(item => item.GoodId == GoodId);
            r.IsActive = !r.IsActive;
            entities.SaveChanges();
        }

        #endregion


        #region Sliders

        public SliderRowModel[] GetSliders()
        {
            List<SliderRowModel> res = new List<SliderRowModel>();
            foreach (var r in entities.Slider)
            {
                SliderRowModel item = new SliderRowModel();
                item.SliderId = r.SliderId;
                item.ImageId = r.ImageId;
                item.IsActive = r.IsActive;
                item.ButtonUrl = r.ButtonUrl;
                List<TranslatableItemModel> titles = new List<TranslatableItemModel>();
                List<TranslatableItemModel> descs = new List<TranslatableItemModel>();
                foreach (var l in entities.Lang)
                {
                    var t = entities.SliderTitle.FirstOrDefault(row => row.SliderId == r.SliderId && row.LangId == l.LangId);
                    if (t != null && item.FirstTitle == null)
                        item.FirstTitle = t.Text;
                    TranslatableItemModel timTitle = new TranslatableItemModel()
                    {
                        ID = t != null ? t.SliderTitleId : -1,
                        LangId = l.LangId,
                        LangName = l.LangName,
                        Text = t != null ? t.Text : ""
                    };
                    titles.Add(timTitle);

                    var d = entities.SliderDesc.FirstOrDefault(row => row.SliderId == r.SliderId && row.LangId == l.LangId);
                    TranslatableItemModel timDesc = new TranslatableItemModel()
                    {
                        ID = d != null ? d.SliderDescId : -1,
                        LangId = l.LangId,
                        LangName = l.LangName,
                        Text = d != null ? d.Text : ""
                    };
                    descs.Add(timDesc);
                }


                item.Descriptions = descs.ToArray();
                item.Titles = titles.ToArray();
                res.Add(item);
            }

            return res.ToArray();
        }


        public SliderRowModel[] GetSlidersByLang(string lang)
        {
            List<SliderRowModel> res = new List<SliderRowModel>();
            foreach (var r in entities.Slider)
            {
                SliderRowModel item = new SliderRowModel();
                item.SliderId = r.SliderId;
                item.ImageId = r.ImageId;
                item.IsActive = r.IsActive;
                item.ButtonUrl = r.ButtonUrl;
                var gt = r.SliderTitle.FirstOrDefault(t => t.Lang.LangName == lang);
                if (gt != null)
                    item.FirstTitle = gt.Text;
                var gd = r.SliderDesc.FirstOrDefault(t => t.Lang.LangName == lang);
                if (gd != null)
                    item.FirstDescription = gd.Text;

                res.Add(item);
            }

            return res.ToArray();
        }


        public SliderRowModel GetSlider(int restaurantId)
        {
            var r = entities.Slider.First(item => item.SliderId == restaurantId);
            SliderRowModel result = new SliderRowModel();
            result.SliderId = r.SliderId;
            result.ImageId = r.ImageId;
            result.IsActive = r.IsActive;
            result.ButtonUrl = r.ButtonUrl;
            var rt = r.SliderTitle.FirstOrDefault();
            if (rt != null)
                result.FirstTitle = rt.Text;
            return result;
        }


        public void SaveSlider(SliderRowModel model)
        {
            if (model.SliderId == -1)
            {
                Slider r = new Slider()
                {
                    ImageId = model.ImageId,
                    ButtonUrl = model.ButtonUrl,
                    IsActive = model.IsActive
                };
                entities.Slider.Add(r);
                entities.SaveChanges();
                model.SliderId = r.SliderId;
            }
            else
            {
                Slider r = entities.Slider.First(item => item.SliderId == model.SliderId);
                r.ImageId = model.ImageId;
                r.ButtonUrl = model.ButtonUrl;
                r.IsActive = model.IsActive;
                entities.SaveChanges();
            }
            foreach (var tt in model.Titles)
            {
                if (tt.ID == -1)
                {
                    entities.SliderTitle.Add(
                        new SliderTitle()
                        {
                            LangId = tt.LangId,
                            SliderId = model.SliderId,
                            Text = tt.Text
                        }
                        );
                }
                else
                {
                    entities.SliderTitle.First(item => item.SliderTitleId == tt.ID).Text = tt.Text;
                }
            }

            foreach (var tt in model.Descriptions)
            {
                if (tt.ID == -1)
                {
                    entities.SliderDesc.Add(
                        new SliderDesc()
                        {
                            LangId = tt.LangId,
                            SliderId = model.SliderId,
                            Text = tt.Text
                        }
                        );
                }
                else
                {
                    entities.SliderDesc.First(item => item.SliderDescId == tt.ID).Text = tt.Text;
                }
            }
            entities.SaveChanges();
        }


        public void SliderChangeActive(int restaurantId)
        {
            var r = entities.Slider.First(item => item.SliderId == restaurantId);
            r.IsActive = !r.IsActive;
            entities.SaveChanges();
        }
        #endregion

        #region Texts

        public TextRowModel[] GetTexts()
        {
            List<TextRowModel> res = new List<TextRowModel>();
            foreach (var r in entities.Text)
            {
                TextRowModel item = new TextRowModel();
                item.TextId = r.TextId;
                item.TextName = r.TextName;
                List<TranslatableItemModel> descs = new List<TranslatableItemModel>();
                foreach (var l in entities.Lang)
                {

                    var d = entities.TextDesc.FirstOrDefault(row => row.TextId == r.TextId && row.LangId == l.LangId);
                    TranslatableItemModel timDesc = new TranslatableItemModel()
                    {
                        ID = d != null ? d.TextDescId : -1,
                        LangId = l.LangId,
                        LangName = l.LangName,
                        Text = d != null ? d.Text : ""
                    };
                    descs.Add(timDesc);
                }


                item.Descriptions = descs.ToArray();
                res.Add(item);
            }

            return res.ToArray();
        }





        public void SaveText(TextRowModel model)
        {
            if (model.TextId == -1)
            {
                Text r = new Text()
                {
                    TextName = model.TextName
                };
                entities.Text.Add(r);
                entities.SaveChanges();
                model.TextId = r.TextId;
            }
            else
            {
                Text r = entities.Text.First(item => item.TextId == model.TextId);
                r.TextName = model.TextName;
                entities.SaveChanges();
            }


            foreach (var tt in model.Descriptions)
            {
                if (tt.ID == -1)
                {
                    entities.TextDesc.Add(
                        new TextDesc()
                        {
                            LangId = tt.LangId,
                            TextId = model.TextId,
                            Text = tt.Text
                        }
                        );
                }
                else
                {
                    entities.TextDesc.First(item => item.TextDescId == tt.ID).Text = tt.Text;
                }
            }
            entities.SaveChanges();
        }


        public TextModel GetTextByLang(string name, string lang)
        {

            var txt = entities.TextDesc.FirstOrDefault(t => t.Text1.TextName == name && t.Lang.LangName == lang);
            if (txt != null)
                return new TextModel() { Text = txt.Text };
            txt = entities.TextDesc.FirstOrDefault(t => t.Text1.TextName == name);
            if (txt != null)
                return new TextModel() { Text = txt.Text };
            else
                return new TextModel() { Text = "NULL" };

        }


        #endregion

        #region Reports
        public void ReqQuery(ReqQueryModel model)
        {
            DateTime from = DateTime.Now;
            DateTime to = DateTime.Now;
            switch (model.SelectedTimeFrame)
            {
                case 1:
                    Tools.GetDay(DateTime.Now, ref from, ref to);
                    break;
                case 2:
                    Tools.GetWeek(DateTime.Now, ref from, ref to);
                    break;
                case 3:
                    Tools.GetMonth(DateTime.Now, ref from, ref to);
                    break;
                case 4:
                    Tools.GetYear(DateTime.Now, ref from, ref to);
                    break;
                case 5:
                    Tools.Get2Years(DateTime.Now, ref from, ref to);
                    break;
            }


            var result = entities.SearchReq(from, to, model.Restaurant);
            List<ReqQueryRowModel> rows = new List<ReqQueryRowModel>();
            foreach (var item in result)
                rows.Add(Mapper.Map<ReqQueryRowModel>(item));
            model.Result = rows.ToArray();

        }
        #endregion

        #region Dispatch
        public ReqModel[] GetReqsForDispatch()
        {
            List<ReqModel> result = new List<ReqModel>();
            foreach (var item in entities.ForDispatch())
            {
                result.Add(Mapper.Map<ReqModel>(item));
            }
            return result.ToArray();
        }

        public void Dispatch(DispatchModel model)
        {
            Req2Status r2s = new Req2Status()
            {
                UserId = model.UserId,
                OnDate = DateTime.Now,
                ReqId = model.ReqId,
                StatusId = 3
            };
            entities.Req2Status.Add(r2s);
            entities.SaveChanges();
        }

        public ReqModel[] GetReqsForCollect(int userId)
        {
            List<ReqModel> result = new List<ReqModel>();
            foreach (var item in entities.ForCollect(userId))
                result.Add(Mapper.Map<ReqModel>(item));
            return result.ToArray();
        }

        public CollectDetailsModel[] CollectDetails(int reqId)
        {
            List<CollectDetailsModel> result = new List<CollectDetailsModel>();
            foreach (var item in entities.CollectDetails(reqId))
                result.Add(Mapper.Map<CollectDetailsModel>(item));
            return result.ToArray();
        }

        public void Collected(DispatchModel model)
        {
            Req2Status r2s = new Req2Status()
            {
                UserId = model.UserId,
                OnDate = DateTime.Now,
                ReqId = model.ReqId,
                StatusId = 4
            };
            entities.Req2Status.Add(r2s);
            entities.SaveChanges();
        }


        public ReqModel[] GetReqsForDelivery(int userId)
        {
            List<ReqModel> result = new List<ReqModel>();
            foreach (var item in entities.ForDeliver(userId))
                result.Add(Mapper.Map<ReqModel>(item));
            return result.ToArray();
        }


        public void Delivered(DispatchModel model)
        {
            Req2Status r2s = new Req2Status()
            {
                UserId = model.UserId,
                OnDate = DateTime.Now,
                ReqId = model.ReqId,
                StatusId = model.NewStatus,
                Note = model.Note
            };
            entities.Req2Status.Add(r2s);
            entities.SaveChanges();
        }

        #endregion

        #region messages
        public void AddMessage(MessageModel model)
        {
            entities.Message.Add(Mapper.Map<Message>(model));
            entities.SaveChanges();
        }

        public string[] GetMessageDates()
        {
            HashSet<string> lDates = new HashSet<string>();
            foreach (Message m in entities.Message.OrderByDescending(msg => msg.OnDate))
                if (!lDates.Contains(m.OnDate.ToShortDateString()))
                    lDates.Add(m.OnDate.ToShortDateString());
            return lDates.ToArray();
        }

        public MessageModel[] GetMessagesByDate(string date)
        {
            if (date == null)
                return null;
            DateTime d = DateTime.Parse(date);
            List<MessageModel> result = new List<MessageModel>();
            foreach (Message m in entities.Message.Where(msg => msg.OnDate == d))
                result.Add(Mapper.Map<MessageModel>(m));
            return result.ToArray();
        }

        #endregion



        #region Blog

        public BlogRowModel[] GetBlogs()
        {
            List<BlogRowModel> res = new List<BlogRowModel>();
            foreach (var r in entities.Blog)
            {
                BlogRowModel item = new BlogRowModel();
                item.BlogId = r.BlogId;
                item.ImageId = r.ImageId;
                item.OnDate = r.OnDate.Value;
                item.IsActive = r.IsActive;
                List<TranslatableItemModel> titles = new List<TranslatableItemModel>();
                List<TranslatableItemModel> descs = new List<TranslatableItemModel>();
                foreach (var l in entities.Lang)
                {
                    var t = entities.BlogTitle.FirstOrDefault(row => row.BlogId == r.BlogId && row.LangId == l.LangId);
                    if (t != null && item.FirstTitle == null)
                        item.FirstTitle = t.Text;
                    TranslatableItemModel timTitle = new TranslatableItemModel()
                    {
                        ID = t != null ? t.BlogTitleId : -1,
                        LangId = l.LangId,
                        LangName = l.LangName,
                        Text = t != null ? t.Text : ""
                    };
                    titles.Add(timTitle);

                    var d = entities.BlogDesc.FirstOrDefault(row => row.BlogId == r.BlogId && row.LangId == l.LangId);
                    TranslatableItemModel timDesc = new TranslatableItemModel()
                    {
                        ID = d != null ? d.BlogDescId : -1,
                        LangId = l.LangId,
                        LangName = l.LangName,
                        Text = d != null ? d.Text : ""
                    };
                    descs.Add(timDesc);
                }


                item.Descriptions = descs.ToArray();
                item.Titles = titles.ToArray();
                res.Add(item);
            }

            return res.ToArray();
        }

        public BlogRowModel[] GetBlogsByLang(string lang)
        {
            List<BlogRowModel> res = new List<BlogRowModel>();
            foreach (var r in entities.Blog.Where(b => b.IsActive).OrderByDescending(b => b.OnDate).Take(3))
            {
                BlogRowModel item = new BlogRowModel();
                item.BlogId = r.BlogId;
                item.ImageId = r.ImageId;
                item.OnDate = r.OnDate.Value;
                item.IsActive = r.IsActive;
                var gt = r.BlogTitle.FirstOrDefault(t => t.Lang.LangName == lang);
                if (gt != null)
                    item.FirstTitle = gt.Text;
                var gd = r.BlogDesc.FirstOrDefault(t => t.Lang.LangName == lang);
                if (gd != null)
                    item.FirstDescription = gd.Text;
                res.Add(item);
            }

            return res.ToArray();
        }


        public BlogRowModel GetBlogByLang(int blogId, string lang)
        {
            BlogRowModel item = new BlogRowModel();
            var r = entities.Blog.First(g => g.BlogId == blogId);
            item.BlogId = r.BlogId;
            item.OnDate = r.OnDate.Value;
            item.ImageId = r.ImageId;
            item.IsActive = r.IsActive;
            var gt = r.BlogTitle.FirstOrDefault(t => t.Lang.LangName == lang);
            if (gt != null)
                item.FirstTitle = gt.Text;
            var gd = r.BlogDesc.FirstOrDefault(t => t.Lang.LangName == lang);
            if (gd != null)
                item.FirstDescription = gd.Text;

            var prev = entities.Blog.Where(b => b.BlogId != blogId && b.OnDate <= item.OnDate).OrderByDescending(b => b.OnDate).ThenByDescending(b => b.BlogId).FirstOrDefault();
            if (prev != null)
                item.PrevId = prev.BlogId;
            var next = entities.Blog.Where(b => b.BlogId != blogId && b.OnDate >= item.OnDate).OrderBy(b => b.OnDate).ThenBy(b => b.BlogId).FirstOrDefault();
            if (next != null)
                item.NextId = next.BlogId;

            return item;
        }



        public void SaveBlog(BlogRowModel model)
        {
            if (model.BlogId == -1)
            {
                Blog r = new Blog()
                {
                    ImageId = model.ImageId,
                    OnDate = model.OnDate,
                    IsActive = model.IsActive
                };
                entities.Blog.Add(r);
                entities.SaveChanges();
                model.BlogId = r.BlogId;
            }
            else
            {
                Blog r = entities.Blog.First(item => item.BlogId == model.BlogId);
                r.ImageId = model.ImageId;
                r.IsActive = model.IsActive;
                r.OnDate = model.OnDate;
                entities.SaveChanges();
            }
            foreach (var tt in model.Titles)
            {
                if (tt.ID == -1)
                {
                    entities.BlogTitle.Add(
                        new BlogTitle()
                        {
                            LangId = tt.LangId,
                            BlogId = model.BlogId,
                            Text = tt.Text
                        }
                        );
                }
                else
                {
                    entities.BlogTitle.First(item => item.BlogTitleId == tt.ID).Text = tt.Text;
                }
            }

            foreach (var tt in model.Descriptions)
            {
                if (tt.ID == -1)
                {
                    entities.BlogDesc.Add(
                        new BlogDesc()
                        {
                            LangId = tt.LangId,
                            BlogId = model.BlogId,
                            Text = tt.Text
                        }
                        );
                }
                else
                {
                    entities.BlogDesc.First(item => item.BlogDescId == tt.ID).Text = tt.Text;
                }
            }
            entities.SaveChanges();
        }


        public void BlogChangeActive(int BlogId)
        {
            var r = entities.Blog.First(item => item.BlogId == BlogId);
            r.IsActive = !r.IsActive;
            entities.SaveChanges();
        }

        #endregion

        #region AddressCheck
        public void AddAddressCheck(AddressCheckModel model)
        {
            if (!entities.AddressCheck.Any(a => a.Address == model.Address))
            {
                entities.AddressCheck.Add(Mapper.Map<AddressCheck>(model));
                entities.SaveChanges();
            }
        }
        public AddressCheckModel CheckAddress(String address)
        {
            var found = entities.AddressCheck.FirstOrDefault(a => a.Address == address);
            if (found != null)
                return Mapper.Map<AddressCheckModel>(found);
            else
                return null;
        }


        #endregion
        #region Client
        public int? ClientLogin(LoginModel model)
        {
            var result = entities.Client.FirstOrDefault(u => u.Name == model.Username && u.Password == model.PasswordMd5);
            if (result == null)
                return null;
            else
                return result.ClientId;
        }

        public void SaveDistantAddress(DistantAddressModel model)
        {
            entities.DistantAddress.Add(new DistantAddress() { Address = model.Address, eMail = model.Email });
            entities.SaveChanges();
        }
        #endregion

        #region MyProfile
        public string OrderDetails(int reqId, string lang)
        {
            List<string> od = new List<string>();
            foreach (var item in entities.Req2Good.Where(r => r.ReqId == reqId))
            {
                od.Add(Tools.StripHtmlTags(item.Good.GoodTitle.First(gt => gt.Lang.LangName == lang).Text));
            }

            return string.Join(", ", od.ToArray());

        }

        public decimal OrderTotal(int reqId)
        {
            decimal result = entities.Req2Good.Sum(r => r.Price * r.Quantity);
            return result;

        }

        public MyProfileModel GetMyProfile(int clientId, string lang)
        {
            MyProfileModel result = new MyProfileModel();
            List<OrderInfoModel> prevOrders = new List<OrderInfoModel>();

            var lastOrder = entities.vwReq.Where(r => r.StatusId < 5).OrderByDescending(r => r.OnDate).FirstOrDefault();
            if (lastOrder != null)
            {
                result.LastOrder = new OrderInfoModel();
                result.LastOrder.OrderProducts = this.OrderDetails(lastOrder.ReqId, lang);
                result.LastOrder.OrderDate = lastOrder.OnDate;
                result.LastOrder.OrderNum = lastOrder.ReqId;
                result.LastOrder.OrderStatus = lastOrder.StatusId;
                result.LastOrder.Total = this.OrderTotal(lastOrder.ReqId);
            }
            foreach (var item in entities.vwReq.Where(r => r.StatusId >= 5).OrderByDescending(r => r.OnDate).Take(5))
            {
                var order = new OrderInfoModel();
                order.OrderProducts = this.OrderDetails(item.ReqId, lang);
                order.OrderDate = item.OnDate;
                order.OrderNum = item.ReqId;
                order.OrderStatus = item.StatusId;
                order.Total = this.OrderTotal(item.ReqId);
                prevOrders.Add(order);
            }



            result.PrevOrders = prevOrders.ToArray();
            return result;

        }


        public List<CartSessionModel> GetForOrderAgain(int reqId)
        {
            List<CartSessionModel> result = new List<CartSessionModel>();
            foreach (var item in entities.Req2Good.Where(rg => rg.ReqId == reqId))
            {
                CartSessionModel element = new CartSessionModel()
                {
                    GoodId = item.GoodId,
                    Quantity = item.Quantity
                };
                result.Add(element);
            }
            return result;
        }

        public MyAccountProfileModel GetAccountProfile(int clientId)
        {
            var rec = entities.Client.First(c => c.ClientId == clientId);
            return new MyAccountProfileModel()
            {
                EMail = rec.eMail,
                Family = rec.Familyname,
                Name = rec.Firstname
            };
        }

        public void UpdateMyAccountProfile(MyAccountProfileModel model)
        {
            if (model.Password != null && entities.Client.FirstOrDefault(cl => cl.ClientId == model.ClientId && cl.Password == model.PasswordMd5) == null)
            {
                model.Error = Umamido.Resources.Resources.InvalidUsernamePassword;
                return;
            }

            if (model.NewPassword == null && model.Password != null)
            {
                model.Error = Umamido.Resources.Resources.EnterPassword;
                return;
            }
            if (model.NewPassword != model.RePassword)
            {
                model.Error = Umamido.Resources.Resources.PasswordDoNotMatch;
                return;
            }

            if (model.Name == null)
            {
                model.Error = Umamido.Resources.Resources.EnterName;
                return;
            }

            if (model.Family == null)
            {
                model.Error = Umamido.Resources.Resources.EnterFamily;
                return;
            }

            try
            {
                MailAddress m = new MailAddress(model.EMail);

            }
            catch (FormatException)
            {
                model.Error = Umamido.Resources.Resources.EnterEmail;
                return;
            }

            var c = entities.Client.First(cl => cl.ClientId == model.ClientId);
            c.Firstname = model.Name;
            c.Familyname = model.Family;
            c.eMail = model.EMail;
            if (model.Password != null)
                c.Password = model.NewPasswordMd5;
            entities.SaveChanges();

        }


        public InvoiceAddressModel GetInvoiceAddressModel(int clientId)
        {
            InvoiceAddressModel model = new InvoiceAddressModel();
            var el = entities.Client.FirstOrDefault(c => c.ClientId == clientId);
            model.CompanyAddress = el.CompanyAddress;
            model.CompanyName = el.CompanyName;
            model.Country = el.Country;
            model.EIK = el.EIK;
            model.PersonName = el.PersonName;
            model.PK = el.PK;
            model.VAT = el.VAT;

            return model;


        }

        public void SetInvoiceAddressModel(InvoiceAddressModel model)
        {

            var el = entities.Client.FirstOrDefault(c => c.ClientId == model.ClientId);
            el.CompanyAddress = model.CompanyAddress;
            el.CompanyName = model.CompanyName;
            el.Country = model.Country;
            el.EIK = model.EIK;
            el.PersonName = model.PersonName;
            el.PK = model.PK;
            el.VAT = model.VAT;
            entities.SaveChanges();


        }


        public void GetProfileAddress(AddressModel model)
        {
            var el = entities.ClientAddress.FirstOrDefault(ca => ca.ClientId == model.ClientId && ca.AddressNum == model.AddressNum);
            if (el == null)
                return;
            model.Address = el.Address;
            model.Family = el.Faimly;
            model.Name = el.FirstName;
            model.Phone = el.Phone;

        }

        public void StoreProfileAddress(AddressModel model)
        {
            var el = entities.ClientAddress.FirstOrDefault(ca => ca.ClientId == model.ClientId && ca.AddressNum == model.AddressNum);
            if (el != null)
            {
                entities.ClientAddress.Remove(el);
                entities.SaveChanges();
            }
            entities.ClientAddress.Add(
                new ClientAddress()
                {
                    Address = model.Address,
                    AddressNum = model.AddressNum,
                    ClientId = model.ClientId,
                    Faimly = model.Family,
                    FirstName = model.Name,
                    Phone = model.Phone
                }
                );
            entities.SaveChanges();
        }

        #endregion

        #region Checkout
        public void CreateReq(CheckOutModel model)
        {
            var ca = entities.ClientAddress.First(c => c.AddressNum == model.AddressNum && c.ClientId == model.ClientId.Value);
            var req = new Req()
            {
                Address = ca.Address,
                ClientId = model.ClientId.Value,
                Invoice = model.Invoice,
                Note = model.Note,
                Address2 = model.Address2,
                OnDate = DateTime.Now,
                PaymentTypeId = model.PaymentTypeId,
                Receiver = ca.FirstName + " " + ca.Faimly

            };
            entities.Req.Add(req);
            entities.SaveChanges();
            foreach (var item in model.CartItems)
            {
                entities.Req2Good.Add(
                    new Req2Good()
                    {
                        GoodId = item.GoodId,
                        Price = entities.Good.First(g => g.GoodId == item.GoodId).Price,
                        Quantity = item.Count,
                        ReqId = req.ReqId
                    }
                    );
            }

            entities.Req2Status.Add(
                new Req2Status()
                {
                    ReqId = req.ReqId,
                    OnDate = DateTime.Now,
                    StatusId = 1
                }
                );

            entities.SaveChanges();
        }

        public bool ClientNameExists(string clientName)
        {
            return entities.Client.Any(c => c.Name == clientName);
        }

        public int CreateClient(CheckOutModel model)
        {
            Client c = new Client();
            c.CompanyAddress = model.CompanyAddress;
            c.CompanyName = model.CompanyName;
            c.Country = model.Country;
            c.EIK = model.EIK;
            c.PersonName = model.PersonName;
            c.PK = model.PK;
            c.VAT = model.VAT;
            c.Name = model.UserName;
            c.Password = model.PasswordMd5;


            c.eMail = model.EMail;
            c.Familyname = model.Family;
            c.Firstname = model.FirstName;
            entities.Client.Add(c);
            entities.SaveChanges();

            ClientAddress cla = new ClientAddress();
            cla.Address = model.Address;
            cla.Address2 = model.Address2;
            cla.AddressNum = 1;
            cla.ClientId = c.ClientId;
            cla.Faimly = model.Family;
            cla.FirstName = model.FirstName;
            cla.Phone = model.Phone;
            entities.ClientAddress.Add(cla);
            entities.SaveChanges();


            return c.ClientId;

        }

        public string[] GetAddresses(int clientId)
        {
            List<string> lAddresses = new List<string>();
            foreach (var addr in entities.ClientAddress.Where(c => c.ClientId == clientId).OrderBy(a => a.AddressNum))
            {
                lAddresses.Add(addr.Address);
            }
            return lAddresses.ToArray();
        }


        #endregion

        #region Invoices
        public ReqForInvoiceModel[] GetReqForInvoice()
        {
            DateTime date = DateTime.Now.Date;
            List<ReqForInvoiceModel> result = new List<ReqForInvoiceModel>();

            foreach (var item in entities.Req.Where(r => r.OnDate == date && r.Invoice))
                result.Add(
                    new ReqForInvoiceModel()
                    {
                        ReqId = item.ReqId,
                        Mail = item.Client.eMail

                    }
                    );
            return result.ToArray();
        }

        public InvModel GetInvoiceFromReq(int reqId)
        {
            var req = entities.Req.First(r => r.ReqId == reqId);
            var inv = entities.Inv.FirstOrDefault(i => i.ReqId == reqId);
            if (inv == null)
            {
                inv = new Inv() { ReqId = reqId };
                entities.Inv.Add(inv);
                entities.SaveChanges();
            }


            InvModel model = new InvModel()
            {
                Address = req.Client.CompanyAddress,
                EIK = req.Client.EIK,
                InvDate = DateTime.Now,
                InvNo = inv.InvId,
                IsOriginal = true,
                MOL = req.Client.PersonName,
                Receiver = req.Client.CompanyName,
                VATNo = req.Client.VAT,
                Rows = new List<InvRowModel>()
            };

            foreach (var item in entities.Req2Good.Where(r2g => r2g.ReqId == reqId))
            {
                model.Rows.Add(
                    new InvRowModel()
                    {
                        Article = Tools.StripHtmlTags(item.Good.GoodTitle.First().Text),
                        Count = item.Quantity,
                        Price = item.Good.Price / (decimal)1.2
                    }
                    );
            }

            return model;
        }
        #endregion
    }
}
