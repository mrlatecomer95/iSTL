using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iSTLDevEx.DAL
{
    public class rptWinningBetAgent
    {
        public string DateReceived { get; set; }
        public int StrBet { get; set; }
        public int RamBet { get; set; }
        public decimal TotalAmt { get; set; }
        public long OperatorMobile { get; set; }
        public string ForDraw { get; set; }
        public string AgentName { get; set; }
        public string DrawResult { get; set; }
        public string MobileNo { get; set; }
        public string OperatorName { get; set; }
        public string AgentCode { get; set; }
        public int WonCnt { get; set; }
        public decimal WonStr { get; set; }
        public decimal WonRam { get; set; }

        public string PersonViewed { get; set; }
        public string PrinterDetails
        {
            get
            {
                return DateTime.Now.ToString("dddd, dd MMMM yyyy") + " / " + PersonViewed;
            }
        }

    }

    public class rptAgentBetDetail
    {
        public string DateRec { get; set; }
        public int StraightAmtBet { get; set; }
        public int RambolAmtBet { get; set; }
        public string ForDraw { get; set; }
        public string OperatorName { get; set; }
        public string AgentNameCode { get; set; }
        public string DrawResult { get; set; }
        public string AgentCode { get; set; }
        public int TotalBet { get; set; }
        public string DrawDate { get; set; }
        public string CombinationNo { get; set; }
        public string AgentName { get; set; }
        public DateTime ReceivedDate { get; set; }

        public string PersonViewed { get; set; }
        public string PrinterDetails
        {
            get
            {
                return DateTime.Now.ToString("dddd, dd MMMM yyyy") + " / " + PersonViewed;
            }
        }
    }

    public class rptOperatorGross
    {
        public string DateRec { get; set; }
        public int TotalStr { get; set; }
        public int TotalRam { get; set; }
        public string ForDraw { get; set; }
        public string OperatorName { get; set; }
        public string AgentName { get; set; }
        public string DrawResult { get; set; }
        public string AgentCode { get; set; }
        public int TotalBet { get; set; }
        public string DrawDate { get; set; }

        public string PersonViewed { get; set; }
        public string PrinterDetails
        {
            get
            {
                return DateTime.Now.ToString("dddd, dd MMMM yyyy") + " / " + PersonViewed;
            }
        }
    }

    public class rptAgentBetDetailsCurrent
    {
        public string DateRec { get; set; }
        public int StraightAmtBet { get; set; }
        public int RambolAmtBet { get; set; }
        public string ForDraw { get; set; }
        public string OperatorName { get; set; }
        public string AgentNameCode { get; set; }
        public string DrawResult { get; set; }
        public string AgentCode { get; set; }
        public int TotalBet { get; set; }
        public string DrawDate { get; set; }
        public string CombinationNo { get; set; }
        public string AgentName { get; set; }
        public DateTime ReceivedDate { get; set; }

        public string PersonViewed { get; set; }
        public string PrinterDetails
        {
            get
            {
                return DateTime.Now.ToString("dddd, dd MMMM yyyy") + " / " + PersonViewed;
            }
        }
    }


    public class rptOperatorGrossCurrent
    {
        public string DateRec { get; set; }
        public int TotalStr { get; set; }
        public int TotalRam { get; set; }
        public string ForDraw { get; set; }
        public string OperatorName { get; set; }
        public string AgentName { get; set; }
        public string DrawResult { get; set; }
        public string AgentCode { get; set; }
        public int TotalBet { get; set; }
        public string DrawDate { get; set; }

        public string PersonViewed { get; set; }
        public string PrinterDetails
        {
            get
            {
                return DateTime.Now.ToString("dddd, dd MMMM yyyy") + " / " + PersonViewed;
            }
        }
    }

    public class rptDailyOperatorGross
    {
        public string DateRec { get; set; }
        public string OperatorName { get; set; }
        public int GrossAmt { get; set; }
        public string DrawDate { get; set; }
        public decimal PayoutAmt { get; set; }
        public decimal BalAmt { get; set; }

        public string ForDrawLabel { get {
                var strSplit = DateRec.Split(' ');

                return "For Draw Date:" +  new DateTime(year: Convert.ToInt32(strSplit[2]), month: Convert.ToInt32(strSplit[1]), day: Convert.ToInt32(strSplit[0])).ToString("MMMM dd yyyy");
            } }

        public string PersonViewed { get; set; }
        public string PrinterDetails
        {
            get
            {
                return DateTime.Now.ToString("dddd, dd MMMM yyyy") + " / " + PersonViewed;
            }
        }
    }

    public class rptDailyAgentGross
    {
        public string DateRec { get; set; }
        public string OperatorName { get; set; }
        public string AgentName { get; set; }
        public string AgentCode { get; set; }
        public int GrossAmt { get; set; }
        public string DrawDate { get; set; }
        public decimal PayoutAmt { get; set; }
        public decimal BalAmt { get; set; }

        public string ForDrawLabel
        {
            get
            {
                var strSplit = DateRec.Split(' ');

                return "For Draw Date:" + new DateTime(year: Convert.ToInt32(strSplit[2]), month: Convert.ToInt32(strSplit[1]), day: Convert.ToInt32(strSplit[0])).ToString("MMMM dd yyyy");
            }
        }

        public string PersonViewed { get; set; }
        public string PrinterDetails
        {
            get
            {
                return DateTime.Now.ToString("dddd, dd MMMM yyyy") + " / " + PersonViewed;
            }
        }
    }

}