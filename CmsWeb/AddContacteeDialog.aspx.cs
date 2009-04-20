﻿/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Generic;
using System.Web;

namespace CMSWeb
{
    public partial class AddContacteeDialog : System.Web.UI.Page
    {
        public int ContactId;

        protected void Page_Load(object sender, EventArgs e)
        {
            ContactId = Request.QueryString["id"].ToInt();
            if (ContactId == 0)
                throw new Exception("Cannot visit AddContacteeDialog this way");
            if (!IsPostBack)
                PersonSearchDialogController.ResetSearchTags();
            Parameters.SearchButtonClicked += new EventHandler(SearchButton_Click);
            Parameters.ClearButtonClicked += new EventHandler(Parameters_ClearButtonClicked);
            AddNew1.Enabled = User.IsInRole("Edit");
        }

        void Parameters_ClearButtonClicked(object sender, EventArgs e)
        {
            ListView1.Visible = false;
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ListView1.Visible = true;
            var ctl = new PersonSearchDialogController();
            ListView1.DataSource = ctl.FetchSearchList(Parameters, false);
            ListView1.DataBind();
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            return SearchDialog.ToggleTag(PeopleId, controlid);
        }
        protected void AddSelectedContactees_Click(object sender, EventArgs e)
        {
            var q = from c in DbUtil.Db.Contactees
                    where c.ContactId == ContactId
                    select c.PeopleId;
            var q2 = from p in SearchDialog.SelectedPeople()
                     where !q.Contains(p.PeopleId)
                     select p.PeopleId;
            foreach (var pid in q2)
                DbUtil.Db.Contactees.InsertOnSubmit(new Contactee
                {
                    PeopleId = pid,
                    ContactId = ContactId
                });
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Added Contactees");
            this.Page.ClientScript.RegisterStartupScript(typeof(AddContacteeDialog), 
                "closeThickBox", "self.parent.AddSelected();", true);
        }
        protected void AddNew1_Click(object sender, EventArgs e)
        {
            CustomValidator1.Text = "Must Select a Family";
            if (!Parameters.Name.HasValue())
            {
                CustomValidator1.IsValid = false;
                CustomValidator1.Text = "Must have a name when adding";
            }
            else
            {
                var OrginId = 0;
                var c = DbUtil.Db.NewContacts.Single(co => co.ContactId == ContactId);
                if (c.ContactTypeId == (int)NewContact.ContactTypeCode.PhoneIn)
                    OrginId = (int)Person.OriginCode.PhoneIn;
                if (c.ContactTypeId == (int)NewContact.ContactTypeCode.SurveyEE)
                    OrginId = (int)Person.OriginCode.SurveyEE;


                CustomValidator1.IsValid = PersonSearchDialogController
                    .AddNewPerson(Parameters.Name, 
                                   Parameters.DOB, 
                                   FamilyOption.SelectedValue, 
                                   Parameters.Gender, 
                                   OrginId,
                                   null);
            }
            if (!CustomValidator1.IsValid)
                return;
            var ctl = new PersonSearchDialogController();
            ListView1.DataSource = ctl.FetchSearchList(Parameters, false);
            ListView1.DataBind();
        }
    }
}
