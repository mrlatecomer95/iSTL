//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iSTLDevEx.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_MsgReply
    {
        public string MsgID { get; set; }
        public string MobileNo { get; set; }
        public string ReplyMsg { get; set; }
        public string DrawTime { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string MsgType { get; set; }
        public Nullable<int> SentTryCnt { get; set; }
        public Nullable<int> SentHandler { get; set; }
        public Nullable<byte> isValidBet { get; set; }
    }
}
