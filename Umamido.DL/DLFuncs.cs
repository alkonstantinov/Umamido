using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Umamido.Common;
using Umamido.Common.Tools;

namespace Umamido.DL
{
    public class DLFuncs
    {
        UmamidoEntities entities;
        public DLFuncs()
        {
            entities = new UmamidoEntities();
        }


        public int? Login(LoginModel model)
        {
            var result = entities.User.FirstOrDefault(u => u.UserName == model.Username && u.Password == model.PasswordMd5 && u.IsActive);
            return (result == null ? null : (int?)result.UserId);
        }

        #region Users

        public UserRowModel[] GetUsers()
        {
            List<UserRowModel> res = new List<UserRowModel>();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, UserRowModel>();
            });
            var usrs = entities.User.OrderBy(u => u.UserName).ToArray();
            foreach (var u in usrs)
                res.Add(Mapper.Map<UserRowModel>(u));
            return res.ToArray();
        }

        public UserRowModel GetUser(int userId)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, UserRowModel>();
            });
            var usr = entities.User.First(u => u.UserId == userId);
            return (Mapper.Map<UserRowModel>(usr));
        }

        public Boolean UserExists(string username)
        {

            return entities.User.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower()) != null; ;
        }


        public void SaveUser(UserRowModel model)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserRowModel, User>();
            });
            if (model.UserId == -1)
            {
                entities.User.Add(Mapper.Map<User>(model));
            }
            else
            {
                var usr = entities.User.First(u => u.UserId == model.UserId);
                usr.UserName = model.UserName;
                usr.IsActive = model.IsActive;
                usr.Password = string.IsNullOrEmpty(model.Password) ? usr.Password : model.PasswordMd5;

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
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Lang, LangRowModel>();
            });
            var langs = entities.Lang.OrderBy(l => l.LangId).ToArray();
            foreach (var l in langs)
                res.Add(Mapper.Map<LangRowModel>(l));
            return res.ToArray();
        }

        public LangRowModel GetLang(int langId)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Lang, LangRowModel>();
            });
            var lang = entities.Lang.First(l => l.LangId == langId);
            return (Mapper.Map<LangRowModel>(lang));
        }



        public void SaveLang(LangRowModel model)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<LangRowModel, Lang>();
            });
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
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Image, ImageRowModel>();
            });
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
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Image, ImageRowModel>();
            });
            var image = entities.Image.First(i => i.ImageId == imageId);
            return (Mapper.Map<ImageRowModel>(image));
        }

        public ImageFileModel GetImageFile(int imageId)
        {
            var image = entities.Image.FirstOrDefault(i => i.ImageId == imageId);
            if (image == null)
                return null;
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Image, ImageFileModel>();
            });
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
            result.IsActive = r.IsActive;
            var rt = r.RestaurantTitle.FirstOrDefault();
            if (rt != null)
                result.FirstTitle = rt.Text;
            return result;
        }


        public void SaveRestaurant(RestaurantRowModel model)
        {
            if (model.RestaurantId == -1)
            {
                Restaurant r = new Restaurant()
                {
                    ImageId = model.ImageId,
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
                    CookMinutes = model.CookTime
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


        public SliderRowModel GetSlider(int restaurantId)
        {
            var r = entities.Slider.First(item => item.SliderId == restaurantId);
            SliderRowModel result = new SliderRowModel();
            result.SliderId = r.SliderId;
            result.ImageId = r.ImageId;
            result.IsActive = r.IsActive;
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
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SearchReq_Result, ReqQueryRowModel>();
            });
            List<ReqQueryRowModel> rows = new List<ReqQueryRowModel>();
            foreach (var item in result)
                rows.Add(Mapper.Map<ReqQueryRowModel>(item));
            model.Result = rows.ToArray();

        }
        #endregion
    }
}
