﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeShare.Model.EF;
using CodeShare.Frontend.Functions;
using CodeShare.Frontend.Models;
using System.Data.Entity.SqlServer;

namespace CodeShare.Frontend.Controllers
{
    public class JsonController : Controller
    {
        DataShareCodeEntities db = new DataShareCodeEntities();
        // GET: Json
        public ActionResult Index()
        {
            return View();
        }
        //Tra ve nguoi dung
        public JsonResult Users()
        {
            var co = new FunctionsController();
            var id = co.CookieID();
            var list = from item in db.Users
                       where item.user_id == id.user_id
                       select new { 
                            id = item.user_id,
                            birth = item.user_birth.ToString(),
                            name = item.user_name,
                            sex = item.user_sex,
                            phone = item.user_phone,
                            email = item.user_email
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //Danh sách code
        public JsonResult Codes()
        {
            var co = new FunctionsController();
            var id = co.CookieID();
            List<Code> codes = db.Codes.Where(n => n.user_id == id.user_id).ToList();
            List<jCodes> list = codes.Select(n => new jCodes
            {
                active = (int)n.code_active,
                code = n.code_code,
                coin = (int)n.code_coin,
                datecreate = n.code_datecreate.ToString(),
                dateupdate = n.code_dateupdate.ToString(),
                del = n.code_del,
                des = n.code_des,
                disk = (int)(n.code_disk == null ? 0 : n.code_disk),
                id = n.code_id,
                id_cate = (int)n.category_id,
                id_us = (int)n.user_id,
                info = n.code_info,
                linkdemo = n.code_linkdemo,
                linkdown = n.code_linkdown,
                option = n.code_option,
                pass = n.code_pass,
                setting = n.code_setting,
                title = n.code_title,
                view = (int)n.code_view,
                viewdown = (int)n.code_viewdown,
                img = n.code_img,
                cate_name = n.Category.category_name
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Comment(int? id)
        {
            List<Comment> comments = db.Comments.Where(n => n.comment_id == id).ToList();
            List<JComments> list = comments.Select(n => new JComments
            {
                code_id = n.code_id,
                comment_content = n.comment_content,
                comment_datecreate = n.comment_datecreate,
                comment_dateupdate = n.comment_dateupdate,
                comment_id = n.comment_id,
                news_id = n.news_id,
                user_id = n.user_id

            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Rep(int? id)
        {
            List<Rep> reps = db.Reps.Where(n => n.rep_id == id).ToList();
            List<JRep> list = reps.Select(n => new JRep
            {
                comment_id = n.comment_id,
                rep_content = n.rep_content,
                rep_datecreate = n.rep_datecreate,
                rep_dateupdate = n.rep_dateupdate,
                rep_id = n.rep_id,
                user_id = n.user_id

            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //Quản lý code mua

        public JsonResult CodesOder()
        {
            var co = new FunctionsController();
            var id = co.CookieID();

            var oders = from item in db.Orders
                        where item.user_id == id.user_id
                        orderby item.oder_datecreate descending
                        select new {
                            id = item.code_id,
                            img = item.Code.code_img,
                            title = item.Code.code_title,
                            coin = item.Code.code_coin,
                            date = item.oder_datecreate.ToString(),
                            coder = item.User.user_name,
                            sum = item.Code.code_coin * 1000
                        };
            return Json(oders, JsonRequestBehavior.AllowGet);
        }
        //Quản lý code bán
        public JsonResult CodesSell()
        {
            var co = new FunctionsController();
            var id = co.CookieID();

            var order = from item in db.Orders
                        where item.id_coder == id.user_id
                        orderby item.oder_datecreate descending
                        select new
                        {
                            id = item.code_id,
                            img = item.Code.code_img,
                            title = item.Code.code_title,
                            coin = item.Code.code_coin,
                            date = item.oder_datecreate.ToString(),
                            coder = item.User.user_name,
                            sum = item.Code.code_coin * 1000,
                            buy = item.User.user_name
                        };
            return Json(order, JsonRequestBehavior.AllowGet);
        }
        //Quản lý rút tiền
        public JsonResult HistoryTakePrice()
        {
            var coo = new FunctionsController();
            var idus = coo.CookieID();
            var history = from tp in db.TakePrices
                          where tp.user_id == idus.user_id
                          select new { id = tp.tp_id, user_id = tp.user_id, tp_coin = tp.tp_coin, tp_note = tp.tp_note, tp_active = tp.tp_active, tp_accountnumber = tp.tp_accountnumber, tp_customer = tp.tp_customer };
            return Json(history, JsonRequestBehavior.AllowGet);
        }

        // All Code
        public JsonResult AllCode()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var code = from c in db.Codes
                       where c.code_active == 1 && c.code_del == false && c.code_option == true
                       select new
                       {
                           active = c.code_active,
                           code = c.code_code,
                           coin = c.code_coin,
                           datecreate = c.code_datecreate.ToString(),
                           dateupdate = c.code_dateupdate.ToString(),
                           del = c.code_del,
                           des = c.code_des,
                           disk = c.code_disk,
                           id = c.code_id,
                           id_cate = c.category_id,
                           id_us = c.user_id,
                           info = c.code_info,
                           linkdemo = c.code_linkdemo,
                           linkdown = c.code_linkdown,
                           option = c.code_option,
                           pass = c.code_pass,
                           setting = c.code_setting,
                           tag = c.code_tag,
                           title = c.code_title,
                           view = c.code_view,
                           key = c.code_coin == 0 ? "mienphi" : c.code_coin < 100 ? "thamkhao" : "chatluong",
                           viewdown = c.code_viewdown,
                           img = c.code_img,
                           groupp = c.Groups.Select(t => t.Language.language_name)
                       };
            return Json(code, JsonRequestBehavior.AllowGet);
        }
        //Binh luan id code
        public JsonResult CommentID(int ? id)
        {
            var list = from item in db.Comments
                       where item.code_id == id
                       orderby item.comment_dateupdate descending
                       select new
                       {
                           id = item.comment_id,
                           idcode = item.code_id,
                           idus = item.user_id,
                           date = item.comment_datecreate,
                           update = item.comment_dateupdate,
                           content = item.comment_content,
                           nameid = item.User.user_name,
                           imgid = item.User.user_img

                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllCodeTop()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var code = from c in db.Codes
                       where c.code_active == 1 && c.code_del == false && c.code_option == true
                       orderby c.code_view descending
                       select new
                       {
                           active = c.code_active,
                           code = c.code_code,
                           coin = c.code_coin,
                           datecreate = c.code_datecreate.ToString(),
                           dateupdate = c.code_dateupdate.ToString(),
                           del = c.code_del,
                           des = c.code_des,
                           disk = c.code_disk,
                           id = c.code_id,
                           id_cate = c.category_id,
                           id_us = c.user_id,
                           info = c.code_info,
                           linkdemo = c.code_linkdemo,
                           linkdown = c.code_linkdown,
                           option = c.code_option,
                           pass = c.code_pass,
                           setting = c.code_setting,
                           tag = c.code_tag,
                           title = c.code_title,
                           view = c.code_view,
                           key = c.code_coin == 0 ? "mienphi" : c.code_coin < 100 ? "thamkhao" : "chatluong",
                           viewdown = c.code_viewdown,
                           img = c.code_img,
                           groupp = c.Groups.Select(t => t.Language.language_name)
                       };
            return Json(code, JsonRequestBehavior.AllowGet);
        }
    }
}