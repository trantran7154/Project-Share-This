﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeShare.Frontend.Functions;
using CodeShare.Model.EF;
using MoMo;
using Newtonsoft.Json.Linq;

namespace CodeShare.Frontend.Controllers
{
    public class PaysController : Controller
    {
        DataShareCodeEntities db = new DataShareCodeEntities();
        // GET: Pays
        public ActionResult Index()
        {
            return View(db.Denominations.Where(n=>n.denomination_active == true).OrderBy(n=>n.denomination_coin).ToList());
        }
        public ActionResult Details(int ? id)
        {
            return View(db.Denominations.Find(id));
        }
        public ActionResult PayMoMo(int? id)
        {
            var coo = new FunctionsController();
            var idus = coo.CookieID();

            Denominations pakage = db.Denominations.Find(id);

            var money = pakage.denomination_price;

            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMO5RGX20191128";
            string accessKey = "M8brj9K6E22vXoDB";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string orderInfo = "Nạp " + pakage.denomination_coin + " vào tài khoản " + idus.user_email;
            string returnUrl = "https://localhost:44365/Pays/ReturnUrl";
            string notifyurl = "https://localhost:44365/Pays/NotifyUrl";

            string amount = money;
            string orderid = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, serectkey);
            //build body json request
            JObject message = new JObject
                {
                    { "partnerCode", partnerCode },
                    { "accessKey", accessKey },
                    { "requestId", requestId },
                    { "amount", amount },
                    { "orderId", orderid },
                    { "orderInfo", orderInfo },
                    { "returnUrl", returnUrl },
                    { "notifyUrl", notifyurl },
                    { "extraData", extraData },
                    { "requestType", "captureMoMoWallet" },
                    { "signature", signature }

                };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);

            Session["idpake"] = id; 

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        public ActionResult ReturnUrl(int errorCode, int amount)
        {
            var coo = new FunctionsController();
            var id = coo.CookieID();

            Users user = db.Users.Find(id.user_id); 

            int idpake = int.Parse(Session["idpake"].ToString());
            Denominations denominations = db.Denominations.Find(idpake);

            if(errorCode.Equals(0))
            {

                user.user_coin = user.user_coins + int.Parse(denominations.denomination_coin.ToString());
                db.SaveChanges();


                Bills bills = new Bills
                {
                    billdate = DateTime.Now,
                    active = true,
                    user_id = id.user_id,
                    denomination_id = denominations.denomination_id,
                    billdeadline = DateTime.Now
                };
                db.Bills.Add(bills);
                db.SaveChanges();

                return RedirectToAction("History");
            }
            else
            {
                Bills bills = new Bills
                {
                    billdate = DateTime.Now,
                    active = false,
                    user_id = id.user_id,
                    denomination_id = denominations.denomination_id,
                    billdeadline = DateTime.Now
                };
                db.Bills.Add(bills);
                db.SaveChanges();

                return RedirectToAction("History");
            }
        }
        public ActionResult History()
        {
            var coo = new FunctionsController();
            var id = coo.CookieID();

            return View(db.Bills.Where(n=>n.user_id == id.user_id).ToList());
        }
    }
}