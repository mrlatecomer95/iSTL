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
    
    public partial class M_Users
    {
        public string UserID { get; set; }
        public string EmpName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<bool> isAdmin { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<bool> isAccount { get; set; }
        public Nullable<bool> isTran { get; set; }
    }
}
