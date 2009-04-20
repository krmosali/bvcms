﻿using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Web.SessionState;

namespace CMSWeb
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ToggleOrgMembersOnly : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            Util.OrgMembersOnly = !Util.OrgMembersOnly;
            if (Util.OrgMembersOnly)
                DbUtil.Db.SetOrgMembersOnly();
            context.Response.Redirect("~/");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
