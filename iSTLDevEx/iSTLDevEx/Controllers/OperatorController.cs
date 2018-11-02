using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iSTLDevEx.DAL;
using iSTLDevEx.Utilities;
using System.Web.Security;

namespace iSTLDevEx.Controllers
{
    [Authorize]
    public class OperatorController : Controller
    {
        private SMSSysEntities dbContext;
        private CurrentUser _user;

        public OperatorController()
        {
            dbContext = new SMSSysEntities();
            var usr = Membership.GetUser();
            _user = new CurrentUser(dbContext, usr);
            var oprID = _user.OperatorID;
        }

        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }

        // GET: Operator
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult JsonOperatorAgentList()
        {
            //var userInDB = dbContext.T_ReceivedBetArc
            //    .OrderByDescending(x=>x.ReceivedDate)
            //    .ToList();
            //var userInDB = dbContext.T_ReceivedBet.OrderByDescending(x => x.ReceivedDate).ToList();
            var operatorID = _user.OperatorID;
            var userInDB = dbContext.T_ReceivedBet
                .Where(x=>x.OperatorCode == operatorID)
                .OrderByDescending(x => x.ReceivedDate)
                .ToList();
            return new JsonNetResult() { Data = userInDB };
        }


    }
}