using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;
using iSTLDevEx.Filters;
using iSTLDevEx.Models;
using System.Net;
using iSTLDevEx.DAL;

namespace iSTLDevEx.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private SMSSysEntities dbContext;

        public AccountController()
        {
            dbContext = new SMSSysEntities();
        }

        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var usrInDB = dbContext.UserProfiles.SingleOrDefault(x => x.Email == model.UserName);
                if (usrInDB == null)
                {
                    usrInDB = dbContext.UserProfiles.SingleOrDefault(x => x.UserName == model.UserName);
                }

                if (usrInDB != null)
                {
                    var oprID = usrInDB.OperatorID;
                    var oprInDB = dbContext.M_Operator.SingleOrDefault(x => x.OperatorID == oprID && x.InActive == 0);
                    if (oprInDB == null)
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        return View(model);
                    }

                }


                if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe.Value))
                {
                    //return Redirect(returnUrl ?? "/");
                    return RedirectToAction("Index", "Home");
                }
                var usrProf = dbContext.UserProfiles.SingleOrDefault(x => x.Email == model.UserName);
                //User Email For Login
                if (usrProf != null)
                {
                    if (WebSecurity.Login(usrProf.UserName, model.Password, persistCookie: model.RememberMe.Value))
                    {
                        //return Redirect(returnUrl ?? "/");
                        return RedirectToAction("Index", "Home");
                    }
                }



                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/PreRegistration
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PreRegistration()
        {
            return View();
        }

        //
        // POST: //Account/PreRegistraition
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PreRegistration(PreRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var operatorInDb = dbContext.M_Operator.SingleOrDefault(x => x.OperatorCode == model.OperatorCode && x.MobileNo == model.OperatorMobileNumber);

            if (operatorInDb != null)
            {
                var userInDIb = dbContext.UserProfiles.SingleOrDefault(x => x.OperatorID == operatorInDb.OperatorID);
                if (userInDIb != null)
                {
                    //Invalid Operator is already Registered
                    ModelState.AddModelError("", "Operator is already registred");
                    return View(model);
                }
                else
                {
                    //Redirect To registration FOrm
                    return RedirectToAction("Register", new { ID = operatorInDb.OperatorID });
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Operator Details.");
                return View(model);
            }

        }


        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register(string ID)
        {
            return View(new RegisterModel() { OperatorID = ID });
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    //WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, propertyValues: new { model.Email, model.OperatorID });
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
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