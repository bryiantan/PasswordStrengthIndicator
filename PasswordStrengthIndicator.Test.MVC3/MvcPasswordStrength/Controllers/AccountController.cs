﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Mvc3PasswordStrength.Models;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Text;

namespace Mvc3PasswordStrength.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (!string.IsNullOrEmpty(model.Password) && !PasswordStrengthIndicator.Core.Helper.IsPasswordMeetPolicy(model.Password))
            {
                ModelState.AddModelError("Password", "Password does not meet policy!");
            }

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        internal string GetPasswordRegex()
        {
            XDocument xmlDoc = XDocument.Load(Request.MapPath("~/xml/PasswordPolicy.xml"));

            var passwordSetting = (from p in xmlDoc.Descendants("Password")
                                  select new PasswordSetting
                                  {
                                      Duration = int.Parse(p.Element("duration").Value),
                                      MinLength = int.Parse(p.Element("minLength").Value),
                                      MaxLength = int.Parse(p.Element("maxLength").Value),
                                      NumsLength = int.Parse(p.Element("numsLength").Value),
                                      SpecialLength = int.Parse(p.Element("specialLength").Value),
                                      UpperLength = int.Parse(p.Element("upperLength").Value),
                                      SpecialChars = p.Element("specialChars").Value
                                  }).First();

            StringBuilder sbPasswordRegx = new StringBuilder(string.Empty);
            //min and max
            sbPasswordRegx.Append(@"(?=^.{" + passwordSetting.MinLength + "," + passwordSetting.MaxLength + "}$)");

            //numbers length
            sbPasswordRegx.Append(@"(?=(?:.*?\d){" + passwordSetting.NumsLength + "})");

            //a-z characters
            sbPasswordRegx.Append(@"(?=.*[a-z])");

            //A-Z length
            sbPasswordRegx.Append(@"(?=(?:.*?[A-Z]){" + passwordSetting.UpperLength + "})");

            //special characters length
            sbPasswordRegx.Append(@"(?=(?:.*?[" + passwordSetting.SpecialChars + "]){" + passwordSetting.SpecialLength + "})");

            //(?!.*\s) - no spaces
            //[0-9a-zA-Z!@#$%*()_+^&] -- valid characters
            sbPasswordRegx.Append(@"(?!.*\s)[0-9a-zA-Z" + passwordSetting.SpecialChars + "]*$");

            return sbPasswordRegx.ToString();
        }

        //
        // GET: /Account/ChangePassword

       // [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        //[Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (!Regex.IsMatch(model.NewPassword, GetPasswordRegex()))
                {
                    ModelState.AddModelError("NewPassword", "Password does not meet policy!");
                }
            }

            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
