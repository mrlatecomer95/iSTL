using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iSTLDevEx.DAL;
using iSTLDevEx.Models;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Web.Security;
using iSTLDevEx.Utilities;

namespace iSTLDevEx.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {

        private SMSSysEntities dbContext;
        private CurrentUser _user;
        public ReportController()
        {

            dbContext = new SMSSysEntities();
            var usr = Membership.GetUser();
            _user = new CurrentUser(dbContext, usr);

            var oprID = _user.OperatorID;
            //AgentList = dbContext.M_Agent.Where(x => x.OperatorID == oprID);
            //InitializeAgent();
        }


        [HttpGet]
        public ActionResult LoadAgent(string Period, string DrawTime)
        {
            var OparatorName = _user.OperatorName;
            IEnumerable<sp_rptAgentViewModel> AgentList = dbContext.Database.SqlQuery<sp_rptAgentViewModel>("[dbo].[sp_rptAgent] @Period, @DrawTime, @OperatorName",
                new SqlParameter("Period", Period),
                new SqlParameter("DrawTime", DrawTime),
                new SqlParameter("OperatorName", OparatorName)).ToList();

            return new JsonNetResult() { Data = AgentList };
        }

        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }

        // GET: Report
        [HttpGet]
        public ActionResult Index()
        {
            var sOperatorID = _user.OperatorID;
            //var agentList = AgentList.Select(x => new SelectListItem() { Text = x.AgentName.ToUpper(), Value = x.AgentID }).OrderBy(x=>x.Text).ToList();
            //var agentList = AgentList.Select(x => new SelectListItem() { Text = x.AgentName, Value = x.AgentName }).OrderBy(x => x.Text).ToList();
            return View(new ReportIndexViewModel()
            {
                //AgentList = agentList,
                DateFilter = DateTime.Now
            });
        }


        public ActionResult JsonReportList()
        {
            var rptList = new List<ReportListViewModel>();
            rptList.Add(new ReportListViewModel() { ID = 1, ReportName = "Agent Winning Bet" });
            rptList.Add(new ReportListViewModel() { ID = 2, ReportName = "Bet Per Agent" });
            //rptList.Add(new ReportListViewModel() { ID = 4, ReportName = "Current Draw Bet Per Agent" });
            rptList.Add(new ReportListViewModel() { ID = 3, ReportName = "Operator Gross" });
            //rptList.Add(new ReportListViewModel() { ID = 5, ReportName = "Current Draw Operator Gross" });
            rptList.Add(new ReportListViewModel() { ID = 6, ReportName = "Operator Daily Gross" });
            rptList.Add(new ReportListViewModel() { ID = 7, ReportName = "Daily Agent Gross" });
            return new JsonNetResult() { Data = rptList };
        }

        [HttpGet]
        public ActionResult JsonDrawResultList(DateTime DateFilter)
        {
            var ResultList = dbContext.T_DrawResult.Where(x => x.DrawDate == DateFilter).ToList();
            return new JsonNetResult() { Data = ResultList };
        }


        //Get AgentCode
        private string getAgentCode(string AgentID)
        {
            //return AgentList.SingleOrDefault(x => x.AgentID == AgentID).AgentCode;
            return AgentID;
        }

        //Get Agent Name
        //private string getAgentName(string AgentID)
        //{

        //}


        #region Reports

        //Agent Winning Bet
        #region rptWinningBetAgent

        private string DateRecToDateRecieved(string DateRec)
        {
            var strArr = DateRec.Split(' ');
            return strArr[2] + strArr[1] + strArr[0];
        }

        private const string s_rptWinningBetAgent = "s_rptWinningBetAgent";

        [HttpGet]
        public ActionResult rptWinningBetAgent(ReportItems model)
        {
            var operatorID = _user.OperatorID;


            var operatorName = _user.OperatorName;
            var AgentID = model.AgentID;
            var DateReceived = DateRecToDateRecieved(model.DateRec);
            var DrawResult = model.DrawResult;
            var ForDraw = model.ForDraw; //Time

            List<rptWinningBetAgent> rptData = null;
            var strQuery = "SELECT [DateReceived],[StrBet],[RamBet],[TotalAmt],[OperatorMobile],[ForDraw],[AgentName],[DrawResult],[MobileNo]" +
                          ",[OperatorName],[AgentCode],[WonCnt],[WonStr],[WonRam] FROM [dbo].[rptWinningBetAgent]" +
                          " WHERE OperatorName = @OperatorName" +
                          " AND [DateReceived] = @DateReceived " +
                          " AND [DrawResult] = @DrawResult " +
                          " AND [ForDraw] = @ForDraw" +
                          " AND [TotalAmt] > 0 ";
            if (AgentID != null)
            {
                var agentCode = getAgentCode(AgentID);//Agent Name
                strQuery = strQuery + " AND [AgentName] = @AgentName "; //Agent Code Filter
                rptData = dbContext.Database.SqlQuery<rptWinningBetAgent>(strQuery,
                          new SqlParameter("OperatorName", operatorName),
                          new SqlParameter("DateReceived", DateReceived),
                          new SqlParameter("AgentName", agentCode),
                          new SqlParameter("DrawResult", DrawResult),
                          new SqlParameter("ForDraw", ForDraw))
                          .ToList();
            }
            else
            {
                rptData = dbContext.Database.SqlQuery<rptWinningBetAgent>(strQuery,
                          new SqlParameter("OperatorName", operatorName),
                          new SqlParameter("DateReceived", DateReceived),
                          new SqlParameter("DrawResult", DrawResult),
                          new SqlParameter("ForDraw", ForDraw))
                          .ToList();
            }

            rptData.ForEach(x => x.PersonViewed = operatorName);
            rptData.OrderBy(x => x.OperatorName).OrderBy(x => x.AgentName).OrderBy(x => x.AgentCode); //Sorting

            Reports.XtraReport1 r_rptWinningAgentBet = new Reports.XtraReport1();
            r_rptWinningAgentBet.DataSource = rptData as IEnumerable<rptWinningBetAgent>;
            Session[s_rptWinningBetAgent] = r_rptWinningAgentBet;
            return PartialView();
        }

        public ActionResult DocumentViewer1Partial()
        {
            return PartialView("_DocumentViewer1Partial", Session[s_rptWinningBetAgent] as Reports.XtraReport1);
        }

        public ActionResult DocumentViewer1PartialExport()
        {
            return DocumentViewerExtension.ExportTo(Session[s_rptWinningBetAgent] as Reports.XtraReport1, Request);
        }

        #endregion

        //Bet Per Agent
        #region rtpAgentBetDetail

        private const string s_rtpAgentBetDetail = "s_rtpAgentBetDetail";
        [HttpGet]
        public ActionResult rptAgentBetDetail(ReportItems model)
        {
            var operatorID = _user.OperatorID;
            var operatorName = _user.OperatorName;
            var strDateRec = model.DateRec;
            var strForDraw = model.ForDraw;
            var AgentID = model.AgentID;
            var DrawResult = model.DrawResult;

            List<rptAgentBetDetail> rptData = null;

            var strQuery = "SELECT [DateRec],[StraightAmtBet],[RambolAmtBet],[ForDraw],[OperatorName],[AgentNameCode],[DrawResult]" +
                       ",[TotalBet],[DrawDate],[CombinationNo],[AgentName],[ReceivedDate]" +
                       " FROM [SMSSys].[dbo].[rptAgentBetDetail] " +
                       " WHERE OperatorName=@OperatorName " +
                       " AND DateRec = @DateRec " +
                       " AND DrawResult = @DrawResult " +
                       " AND ForDraw = @ForDraw ";
            if (AgentID != null)
            {
                var agentCode = getAgentCode(model.AgentID);
                strQuery = strQuery + " AND [AgentName] = @AgentName ";
                rptData = dbContext.Database.SqlQuery<rptAgentBetDetail>(strQuery,
                       new SqlParameter("OperatorName", operatorName),
                       new SqlParameter("AgentName", agentCode),
                       new SqlParameter("DrawResult", DrawResult),
                       new SqlParameter("DateRec", strDateRec),
                       new SqlParameter("ForDraw", strForDraw))
                       .OrderBy(x => x.ReceivedDate).ToList();
            }
            else
            {
                rptData = dbContext.Database.SqlQuery<rptAgentBetDetail>(strQuery,
                    new SqlParameter("OperatorName", operatorName),
                    new SqlParameter("DrawResult", DrawResult),
                    new SqlParameter("DateRec", strDateRec),
                    new SqlParameter("ForDraw", strForDraw))
                    .OrderBy(x => x.ReceivedDate).ToList();
            }


            rptData.ForEach(x => x.PersonViewed = operatorName);
            rptData.OrderBy(x => x.OperatorName).OrderBy(x => x.AgentName).OrderBy(x => x.ReceivedDate);
            Reports.rptBetAgentDetails report = new Reports.rptBetAgentDetails();
            report.DataSource = rptData as IEnumerable<rptAgentBetDetail>;
            Session[s_rtpAgentBetDetail] = report;
            return PartialView();
        }

        public ActionResult rptAgentBetDetails_DocumentViewerPartial()
        {
            return PartialView("_rptAgentBetDetails_DocumentViewerPartial", Session[s_rtpAgentBetDetail] as Reports.rptBetAgentDetails);
        }

        public ActionResult rptAgentBetDetails_DocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(Session[s_rtpAgentBetDetail] as Reports.rptBetAgentDetails, Request);
        }

        #endregion

        //Operator Gross
        #region rptOperatorGross
        private const string s_rptOperatorGross = "s_rptOperatorGross";
        [HttpGet]
        public ActionResult rptOperatorGross(ReportItems model)
        {

            var operatorName = _user.OperatorName;
            var strDateRec = model.DateRec;
            var strForDraw = model.ForDraw;
            var agentID = model.AgentID;
            var DrawResult = model.DrawResult;
            try
            {
                List<rptOperatorGross> rptData = null;
                var strQuery = "SELECT [DateRec],[TotalStr],[TotalRam],[ForDraw]" +
                        ",[OperatorName],[AgentName],[DrawResult],[AgentCode],[TotalBet]" +
                        ",[DrawDate] FROM[SMSSys].[dbo].[rptOperatorGross] " +
                        "WHERE [OperatorName] = @OperatorName " +
                        " AND DateRec = @DateRec " +
                        " AND DrawResult = @DrawResult " +
                        " AND ForDraw = @ForDraw";
                if (agentID != null)
                {
                    var AgentCode = getAgentCode(agentID);
                    strQuery = strQuery + " AND [AgentName] = @AgentName";
                    rptData = dbContext.Database.SqlQuery<rptOperatorGross>(strQuery,
                        new SqlParameter("OperatorName", operatorName),
                        new SqlParameter("AgentName", AgentCode),
                        new SqlParameter("DrawResult", DrawResult),
                        new SqlParameter("DateRec", strDateRec),
                        new SqlParameter("ForDraw", strForDraw))
                        .ToList();

                }
                else
                {
                    rptData = dbContext.Database.SqlQuery<rptOperatorGross>(strQuery,
                        new SqlParameter("OperatorName", operatorName),
                        new SqlParameter("DrawResult", DrawResult),
                        new SqlParameter("DateRec", strDateRec),
                        new SqlParameter("ForDraw", strForDraw))
                        .ToList();
                }

                rptData.ForEach(x => x.PersonViewed = operatorName);
                rptData.OrderBy(x => x.OperatorName).OrderBy(x => x.AgentName).OrderBy(x => x.AgentCode);
                Reports.rptOperatorGross report = new Reports.rptOperatorGross();

                report.DataSource = rptData as IEnumerable<rptOperatorGross>;
                Session[s_rptOperatorGross] = report;
                return PartialView();
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                throw;
            }
          
        }

        public ActionResult rptOperatorGrossDocumentViewerPartial()
        {
            return PartialView("_rptOperatorGrossDocumentViewerPartial", Session[s_rptOperatorGross] as Reports.rptOperatorGross);
        }

        public ActionResult rptOperatorGrossDocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(Session[s_rptOperatorGross] as Reports.rptOperatorGross, Request);
        }
        #endregion 


        //Current Draw Bet Per Agent
        #region rptAgentBetDetailCurrent
        private const string s_rptAgentBetDetailCurrent = "s_rptAgentBetDetailCurrent";
        [HttpGet]
        public ActionResult rptAgentBetDetailCurrent(ReportItems model)
        {

            var operatorName = _user.OperatorName;
            var strDateRec = model.DateRec;
            var strForDraw = model.ForDraw;
            var DrawResult = model.DrawResult;
            var AgentID = model.AgentID;
            List<rptAgentBetDetailsCurrent> rptData = null;
            var strQuery = "SELECT [DateRec],[StraightAmtBet],[RambolAmtBet],[ForDraw]" +
                                ",[OperatorName],[AgentNameCode],[DrawResult],[AgentCode],[TotalBet]" +
                                ",[DrawDate],[CombinationNo],[AgentName],[ReceivedDate]" +
                                " FROM [SMSSys].[dbo].[rptAgentBetDetailCurrent] " +
                                " WHERE [OperatorName] = @OperatorName " +
                                " AND [DateRec] = @DateRec " +
                                " AND [ForDraw] = @ForDraw " +
                                " AND [DrawResult] = @DrawResult ";
            if (AgentID != null)
            {
                var AgentCode = getAgentCode(AgentID);
                strQuery = strQuery + " AND [AgentName] = @AgentName ";
                rptData = dbContext.Database.SqlQuery<rptAgentBetDetailsCurrent>(strQuery,
                        new SqlParameter("OperatorName", operatorName),
                        new SqlParameter("AgentName", AgentCode),
                        new SqlParameter("DateRec", strDateRec),
                        new SqlParameter("DrawResult", DrawResult),
                        new SqlParameter("ForDraw", strForDraw)).OrderBy(x => x.ReceivedDate).ToList();
            }
            else
            {
                rptData = dbContext.Database.SqlQuery<rptAgentBetDetailsCurrent>(strQuery,
                  new SqlParameter("OperatorName", operatorName),
                  new SqlParameter("DateRec", strDateRec),
                  new SqlParameter("DrawResult", DrawResult),
                  new SqlParameter("ForDraw", strForDraw)).OrderBy(x => x.ReceivedDate).ToList();
            }

            rptData.ForEach(x => x.PersonViewed = operatorName);
            rptData.OrderBy(x => x.OperatorName).OrderBy(x => x.AgentName).OrderBy(x => x.ReceivedDate); //Sorting
            Reports.rptAgentBetDetailCurrent report = new Reports.rptAgentBetDetailCurrent();
            report.DataSource = rptData as IEnumerable<rptAgentBetDetailsCurrent>;
            Session[s_rptAgentBetDetailCurrent] = report;
            return PartialView();
        }

        public ActionResult rptAgentBetDetailCurrentDocumentViewerPartial()
        {
            return PartialView("_rptAgentBetDetailCurrentDocumentViewerPartial", Session[s_rptAgentBetDetailCurrent] as Reports.rptAgentBetDetailCurrent);
        }

        public ActionResult rptAgentBetDetailCurrentDocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(Session[s_rptAgentBetDetailCurrent] as Reports.rptAgentBetDetailCurrent, Request);
        }
        #endregion

        //Current Draw Operator Gross
        #region rptOperatorGrossCurrent
        private const string s_rptOperatorGrossCurrent = "s_rptOperatorGrossCurrent";

        [HttpGet]
        public ActionResult rptOperatorGrossCurrent(ReportItems model)
        {

            var operatorName = _user.OperatorName;
            var strDateRec = model.DateRec;
            var strForDraw = model.ForDraw;
            var DrawResult = model.DrawResult;
            var AgentID = model.AgentID;

            List<rptOperatorGrossCurrent> rptData = null;
            var strQuery = "SELECT TOP 1000 [DateRec],[TotalStr],[TotalRam]" +
                           ",[ForDraw],[OperatorName],[AgentName],[DrawResult]" +
                           ",[AgentCode],[TotalBet],[DrawDate] " +
                           " FROM[SMSSys].[dbo].[rptOperatorGrossCurrent]" +
                           " WHERE [OperatorName] = @OperatorName" +
                           " AND [ForDraw] = @ForDraw " +
                           " AND [DrawResult] = @DrawResult " +
                           " AND [DateRec] = @DateRec ";
            if (AgentID != null)
            {
                var AgentCode = getAgentCode(AgentID);
                strQuery = strQuery + " AND [AgentName] = @AgentName ";
                rptData = dbContext.Database.SqlQuery<rptOperatorGrossCurrent>(strQuery,
                    new SqlParameter("OperatorName", operatorName),
                    new SqlParameter("AgentName", AgentCode),
                    new SqlParameter("ForDraw", strForDraw),
                    new SqlParameter("DrawResult", DrawResult),
                    new SqlParameter("DateRec", strDateRec)).ToList();

            }
            else
            {
                rptData = dbContext.Database.SqlQuery<rptOperatorGrossCurrent>(strQuery,
                    new SqlParameter("OperatorName", operatorName),
                    new SqlParameter("ForDraw", strForDraw),
                    new SqlParameter("DrawResult", DrawResult),
                    new SqlParameter("DateRec", strDateRec)).ToList();
            }

            rptData.ForEach(x => x.PersonViewed = operatorName);
            rptData.OrderBy(x => x.OperatorName).OrderBy(x => x.AgentName).OrderBy(x => x.AgentCode); //Sorting
            Reports.rptOperatorGrossCurrent report = new Reports.rptOperatorGrossCurrent();
            report.DataSource = rptData as IEnumerable<rptOperatorGrossCurrent>;
            Session[s_rptOperatorGrossCurrent] = report;
            return PartialView();
        }


        public ActionResult rptOperatorGrossCurrentDocumentViewerPartial()
        {
            return PartialView("_rptOperatorGrossCurrentDocumentViewerPartial", Session[s_rptOperatorGrossCurrent] as Reports.rptOperatorGrossCurrent);
        }

        public ActionResult rptOperatorGrossCurrentDocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(Session[s_rptOperatorGrossCurrent] as Reports.rptOperatorGrossCurrent, Request);
        }

        #endregion



        //Operator Daily Gross/Payout Report
        #region rptDailyOperatorGross

        private const string s_rptDailyOperatorGross = "s_rptDailyOperatorGross";

        [HttpGet]
        public ActionResult rptDailyOperatorGross(ReportItems model)
        {
            var operatorName = _user.OperatorName;
            var strDateRec = model.DateRec;
            var strForDraw = model.ForDraw;
            var DrawResult = model.DrawResult;
            var AgentID = model.AgentID;

            List<rptDailyOperatorGross> rptData = null;
            var strQuery = "SELECT [DateRec]" +
                        " ,[OperatorName] " +
                        " ,[GrossAmt] " +
                        " ,[DrawDate] " +
                        " ,[PayoutAmt] " +
                        " ,[BalAmt] " +
                        " FROM[SMSSys].[dbo].[rptDailyOperatorGross] " +
                        " WHERE [OperatorName] = @OperatorName" +
                        " AND [DateRec] = @DateRec ";

            rptData = dbContext.Database.SqlQuery<rptDailyOperatorGross>(strQuery,
                        new SqlParameter("OperatorName", operatorName),
                        new SqlParameter("DateRec", strDateRec)).ToList();
            rptData.ForEach(x => x.PersonViewed = operatorName);
            rptData.OrderBy(x => x.OperatorName); //Sorting
            Reports.rptDailyOperatorGross report = new Reports.rptDailyOperatorGross();
            report.DataSource = rptData as IEnumerable<rptDailyOperatorGross>;
            Session[s_rptDailyAgentGross] = report;

            return PartialView();


        }


        public ActionResult rptDailyOperatorGrossDocumentViewerPartial()
        {
            return PartialView("_rptDailyOperatorGrossDocumentViewerPartial", Session[s_rptDailyAgentGross] as Reports.rptDailyOperatorGross);
        }

        public ActionResult rptDailyOperatorGrossDocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(Session[s_rptDailyAgentGross] as Reports.rptDailyOperatorGross, Request);
        }

        #endregion


        //Operator Daily Gross/Payout Report
        #region rptDailyAgentGross

        private const string s_rptDailyAgentGross = "s_rptDailyAgentGross";
        [HttpGet]
        public ActionResult rptDailyAgentGross(ReportItems model)
        {
            var operatorName = _user.OperatorName;
            var strDateRec = model.DateRec;
            var strForDraw = model.ForDraw;
            var DrawResult = model.DrawResult;
            var AgentID = model.AgentID;

            List<rptDailyAgentGross> rptData = null;
            var strQuery = "SELECT TOP 1000 [DateRec] " +
                          " ,[OperatorName] " +
                          " ,[AgentName] " +
                          " ,[AgentCode] " +
                          " ,[GrossAmt] " +
                          " ,[DrawDate] " +
                          " ,[PayoutAmt] " +
                          " ,[BalAmt] " +
                          " FROM[SMSSys].[dbo].[rptDailyAgentGross]" +
                          " WHERE [OperatorName] = @OperatorName " +
                          " AND [DateRec] = @DateRec ";


            if (AgentID != null)
            {
                var AgentCode = getAgentCode(AgentID);
                strQuery = strQuery + " AND [AgentName] = @AgentName ";
                rptData = dbContext.Database.SqlQuery<rptDailyAgentGross>(strQuery,
                    new SqlParameter("OperatorName", operatorName),
                    new SqlParameter("AgentName", AgentCode),
                    new SqlParameter("DateRec", strDateRec)).ToList();
            }
            else
            {
                rptData = dbContext.Database.SqlQuery<rptDailyAgentGross>(strQuery,
                    new SqlParameter("OperatorName", operatorName),
                    new SqlParameter("ForDraw", strForDraw),
                    new SqlParameter("DrawResult", DrawResult),
                    new SqlParameter("DateRec", strDateRec)).ToList();
            }

            rptData.ForEach(x => x.PersonViewed = operatorName);
            rptData.OrderBy(x => x.OperatorName).OrderBy(x => x.AgentName).OrderBy(x => x.AgentCode); //Sorting
            iSTLDevEx.Reports.rptDailyAgentGross report = new iSTLDevEx.Reports.rptDailyAgentGross();
            report.DataSource = rptData as IEnumerable<rptDailyAgentGross>;
            Session[s_rptDailyAgentGross] = report;

            return PartialView();
        }

        public ActionResult rptDailyAgentGrossDocumentViewerPartial()
        {
            return PartialView("_rptDailyAgentGrossDocumentViewerPartial", Session[s_rptDailyAgentGross] as Reports.rptDailyAgentGross);
        }

        public ActionResult rptDailyAgentGrossDocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(Session[s_rptDailyAgentGross] as Reports.rptDailyAgentGross, Request);
        }

        #endregion

    }

    #endregion
}


