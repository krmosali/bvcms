﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DiscData;

namespace BellevueTeachers
{
    public partial class StopNotifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var u = Request.QueryString<string>("user");
            var user = Util.GetUser(u);
            var id = Request.QueryString<int>("blog");
            var b = DiscData.Blog.LoadById(id);
            //var Profile = new WebProfile().GetProfile(u);
            //Profile.NotifyAll = false;
            //Profile.NotifyEnabled = false;
            //Profile.Save();
            b.BlogNotifications.Add(new BlogNotify { UserId = user.UserId });
            DbUtil.Db.SubmitChanges();
            Label1.Text = "you will no longer receive blog post notifications from: {0} ({1})"
                .Fmt(b.Name, b.User.Name);
        }
    }
}
