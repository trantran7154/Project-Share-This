﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeShare.Model.EF;

namespace CodeShare.Frontend.Areas.Admin.Controllers
{
    public class JsonAdminController : Controller
    {
        // GET: Admin/JsonAdmin
        DataShareCodeEntities db = new DataShareCodeEntities();
        public JsonResult Codes()
        {
            var list = from item in db.Codes
                       where item.code_del == false
                       orderby item.code_datecreate descending
                       select new
                       {
                           active = (int)item.code_active,
                           code = item.code_code,
                           coin = (int)item.code_coin,
                           datecreate = item.code_datecreate.ToString(),
                           dateupdate = item.code_dateupdate.ToString(),
                           del = item.code_del,
                           des = item.code_des,
                           disk = (int)(item.code_disk == null ? 0 : item.code_disk),
                           id = item.code_id,
                           id_cate = (int)item.category_id,
                           id_us = (int)item.user_id,
                           info = item.code_info,
                           linkdemo = item.code_linkdemo,
                           linkdown = item.code_linkdown,
                           option = item.code_option,
                           pass = item.code_pass,
                           setting = item.code_setting,
                           tag = item.code_tag,
                           title = item.code_title,
                           view = (int)item.code_view,
                           viewdown = (int)item.code_viewdown,
                           img = item.code_img,
                           cate_name = item.Category.category_name,
                           user_name = item.User.user_name
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Users()
        {
            var list = from item in db.Users
                       where item.user_del == false
                       orderby item.user_datecreate descending
                       select new
                       {
                           id = (int)item.user_id,
                           email = item.user_email,
                           phone = item.user_phone,
                           sex = item.user_sex,
                           birth = item.user_birth.ToString(),
                           token = item.user_token,
                           role = (int)item.user_role,
                           name = item.user_name,
                           coin = (int)item.user_coin,
                           datecreate = item.user_datecreate.ToString(),
                           dateupdate = item.user_dateupdate.ToString(),
                           code = item.user_code,
                           active = (int)item.user_active,
                           option = item.user_option,
                           del = item.user_del,
                           fa = item.user_fa,
                           none = item.user_none,
                           view = (int)item.user_view,
                           facode = item.user_facode,
                           pass = item.user_pass,
                           img = item.user_img
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Bills()
        {
            var list = from item in db.Bills
                       orderby item.bill_datecreate descending
                       select new
                       {
                           id = (int)item.bill_id,
                           datecreate = item.bill_datecreate.ToString(),
                           pk_id = item.pakege_id,
                           pk_coin = (int)item.Pakage.pakage_coin,
                           user_name = item.User.user_name,
                           active = item.bill_active,
                           dealine = item.bill_dealine.ToString()
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TakePrices()
        {
            var list = from item in db.TakePrices
                       orderby item.tp_datecreate descending
                       select new
                       {
                           id = (int)item.tp_id,
                           user_id = (int)item.user_id,
                           user_name = item.User.user_name,
                           user_email = item.User.user_email,
                           coin = item.tp_coin,
                           datecreate = item.tp_datecreate.ToString(),
                           note = item.tp_note,
                           active = (int)item.tp_active,
                           accountnumber = item.tp_accountnumber,
                           customer = item.tp_customer,
                           momo = item.tp_momo
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Categorys()
        {
            var list = from item in db.Categorys
                       where item.category_del == false
                       orderby item.category_datecreate descending
                       select new
                       {
                           id = (int)item.category_id,
                           name = item.category_name,
                           active = item.category_active,
                           item = item.category_item,
                           img = item.category_img,
                           datecreate = item.category_datecreate.ToString(),
                           update = item.category_dateupdate.ToString(),
                           del = item.category_del
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Languages()
        {
            var list = from item in db.Languages
                       where item.language_active == 1
                       select new
                       {
                           id = (int)item.language_id,
                           name = item.language_name,
                           active = item.language_active,
                           img = item.language_img,
                           view = (int)item.language_view
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Banners()
        {
            var list = from item in db.Banners
                       where item.banner_active == true
                       orderby item.banner_datecreate descending
                       select new
                       {
                           id = (int)item.banner_id,
                           title = item.banner_title,
                           active = item.banner_active,
                           img = item.banner_image,
                           link = item.banner_link,
                           datecreate = item.banner_datecreate.ToString()
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Packages()
        {
            var list = from item in db.Pakages
                       where item.pakage_active == 1
                       select new
                       {
                           id = (int)item.pakege_id,
                           coin = (int)item.pakage_coin,
                           money = item.pakage_money.ToString(),
                           active = (int)item.pakage_active
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult News()
        {
            var list = from item in db.News
                       where item.news_del == false
                       orderby item.news_datecreate descending
                       select new
                       {
                           id = (int)item.news_id,
                           name = item.news_name.ToString(),
                           view = (int)item.news_view,
                           content = item.news_content.ToString(),
                           tag = item.news_tag.ToString(),
                           user_id = (int)item.user_id,
                           user_name = item.User.user_name.ToString(),
                           datecreate = item.news_datecreate.ToString(),
                           update = item.news_dateupdate.ToString(),
                           active = (int)item.news_active,
                           option = item.news_option,
                           del = item.news_del,
                           img = item.news_img
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}