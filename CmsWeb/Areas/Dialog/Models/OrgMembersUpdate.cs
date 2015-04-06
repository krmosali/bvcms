﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CmsData;
using CmsData.Codes;
using CmsData.View;
using CmsWeb.Code;
using UtilityExtensions;

namespace CmsWeb.Areas.Dialog.Models
{
    public class OrgMembersUpdate : IValidatableObject
    {
        public OrgMembersUpdate()
        {
            MemberType = new CodeInfo(0, "MemberType");
        }
        private int? id;
        public int? Id
        {
            get
            {
                if (!id.HasValue)
                {
//                    if (DbUtil.Db.CurrentOrg == null)
//                        throw new Exception("Current org no longer exists, aborting");
                    id = DbUtil.Db.CurrentOrgId0;
                }
//                if (id != DbUtil.Db.CurrentOrgId0)
//                    throw new Exception("Current org has changed from {0} to {1}, aborting".Fmt(id, DbUtil.Db.CurrentOrgId0));
                return id;
            }
            set
            {
                id = value;
                if (id > 0)
                {
                    OrgName = DbUtil.Db.LoadOrganizationById(id).OrganizationName;
                    if (DbUtil.Db.CurrentOrg.GroupSelect == GroupSelectCode.Pending)
                        Pending = true;
                }
            }
        }

        private int? count;

        public int Count
        {
            get
            {
                if (!count.HasValue)
                    count = People(DbUtil.Db.CurrentOrg).Count();
                return count.Value;
            }
        }

        public string OrgName;

        public decimal? Amount { get; set; }
        public decimal? Payment { get; set; }
        public bool AdjustFee { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public string Group
        {
            get
            {
                switch (DbUtil.Db.CurrentOrg.GroupSelect)
                {
                    case GroupSelectCode.Member:
                        return "Current Members";
                        break;
                    case GroupSelectCode.Inactive:
                        return "Inactive Members";
                        break;
                    case GroupSelectCode.Pending:
                        return "Pending Members";
                        break;
                    case GroupSelectCode.Prospect:
                        return "Prospects";
                        break;
                }
                return "People";
            }
        }

        public CodeInfo MemberType { get; set; }
        public DateTime? InactiveDate { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public bool MakeMemberTypeOriginal { get; set; }
        public bool Pending { get; set; }
        public bool RemoveFromEnrollmentHistory { get; set; }
        public bool RemoveInactiveDate { get; set; }
        public DateTime? DropDate { get; set; }
        public string NewGroup { get; set; }

        public IQueryable<OrgPerson> People(ICurrentOrg co)
        {
            var q = from p in DbUtil.Db.OrgPeople(id, co.GroupSelect,
                        co.First(), co.Last(), co.SgFilter, co.ShowHidden,
                        Util2.CurrentTag, Util2.CurrentTagOwnerId,
                        co.FilterIndividuals, co.FilterTag, false, Util.UserPeopleId)
                    select p;
            return q;
        }

        public void Drop()
        {
            var pids = (from p in People(DbUtil.Db.CurrentOrg) select p.PeopleId).ToList();
            foreach (var pid in pids)
            {
                DbUtil.DbDispose();
                DbUtil.Db = new CMSDataContext(Util.ConnectionString);
                var om = DbUtil.Db.OrganizationMembers.Single(mm => mm.PeopleId == pid && mm.OrganizationId == Id);
                if (DropDate.HasValue)
                    om.Drop(DbUtil.Db, DropDate.Value);
                else
                    om.Drop(DbUtil.Db);
                DbUtil.Db.SubmitChanges();
                if (RemoveFromEnrollmentHistory)
                {
                    DbUtil.DbDispose();
                    DbUtil.Db = new CMSDataContext(Util.ConnectionString);
                    var q = DbUtil.Db.EnrollmentTransactions.Where(tt => tt.OrganizationId == Id && tt.PeopleId == pid);
                    DbUtil.Db.EnrollmentTransactions.DeleteAllOnSubmit(q);
                    DbUtil.Db.SubmitChanges();
                }
            }
        }
        public void Update()
        {
            var pids = (from p in People(DbUtil.Db.CurrentOrg) select p.PeopleId).ToList();
            foreach (var pid in pids)
            {
                DbUtil.DbDispose();
                DbUtil.Db = new CMSDataContext(Util.ConnectionString);
                var om = DbUtil.Db.OrganizationMembers.Single(mm => mm.PeopleId == pid && mm.OrganizationId == Id);

                if (InactiveDate.HasValue)
                    om.InactiveDate = InactiveDate;
                if (RemoveInactiveDate)
                    om.InactiveDate = null;

                if (EnrollmentDate.HasValue)
                    om.EnrollmentDate = EnrollmentDate;

                om.Pending = Pending;

                if (MemberType.Value != "0")
                    om.MemberTypeId = MemberType.Value.ToInt();

                if (MakeMemberTypeOriginal)
                {
                    var et = (from e in DbUtil.Db.EnrollmentTransactions
                              where e.PeopleId == om.PeopleId
                              where e.OrganizationId == Id
                              orderby e.TransactionDate
                              select e).First();
                    et.MemberTypeId = om.MemberTypeId;
                }

                DbUtil.Db.SubmitChanges();
            }
        }

        public void AddSmallGroup(int sgtagid)
        {
            var pids = (from p in People(DbUtil.Db.CurrentOrg) select p.PeopleId).ToList();
            foreach (var pid in pids)
            {
                DbUtil.DbDispose();
                DbUtil.Db = new CMSDataContext(Util.ConnectionString);
                var om = DbUtil.Db.OrganizationMembers.Single(mm => mm.PeopleId == pid && mm.OrganizationId == Id);
                om.OrgMemMemTags.Add(new OrgMemMemTag { MemberTagId = sgtagid });
                DbUtil.Db.SubmitChanges();
            }
        }

        public void RemoveSmallGroup(int sgtagid)
        {
            var pids = (from p in People(DbUtil.Db.CurrentOrg) select p.PeopleId).ToList();
            foreach (var pid in pids)
            {
                DbUtil.DbDispose();
                DbUtil.Db = new CMSDataContext(Util.ConnectionString);
                var om = DbUtil.Db.OrganizationMembers.Single(mm => mm.PeopleId == pid && mm.OrganizationId == Id);
                var mt = om.OrgMemMemTags.SingleOrDefault(t => t.MemberTagId == sgtagid);
                if (mt != null)
                    DbUtil.Db.OrgMemMemTags.DeleteOnSubmit(mt);
                DbUtil.Db.SubmitChanges();
            }
            DbUtil.Db = new CMSDataContext(Util.ConnectionString);
            DbUtil.Db.ExecuteCommand(@"
DELETE dbo.MemberTags 
WHERE Id = {1} AND OrgId = {0} 
AND NOT EXISTS(SELECT NULL FROM dbo.OrgMemMemTags WHERE OrgId = {0} AND MemberTagId = {1})
", Id, sgtagid);
        }

        public string CurrentOrgError;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (id != DbUtil.Db.CurrentOrgId0)
            {
                CurrentOrgError = "Current org has changed from {0} to {1}".Fmt(id, DbUtil.Db.CurrentOrgId0);
                results.Add(new ValidationResult(CurrentOrgError));
                throw new Exception(CurrentOrgError);
            }
            return results;
        }
        internal void PostTransactions()
        {
            var pids = (from p in People(DbUtil.Db.CurrentOrg) select p.PeopleId).ToList();
            foreach (var pid in pids)
            {
                var db = new CMSDataContext(Util.ConnectionString);
                var om = db.OrganizationMembers.Single(mm => mm.PeopleId == pid && mm.OrganizationId == Id);
                var ts = db.ViewTransactionSummaries.SingleOrDefault(
                        tt => tt.RegId == om.TranId && tt.PeopleId == om.PeopleId);
                var reason = ts == null ? "Inital Tran" : "Adjustment";
                om.AddTransaction(db, reason, Payment ?? 0, Description, Amount, AdjustFee);
                db.SubmitChanges();
            }
        }

        public void AddNewSmallGroup()
        {
            var o = DbUtil.Db.LoadOrganizationById(Id);
            var mt = new MemberTag() { Name = NewGroup };
            o.MemberTags.Add(mt);
            DbUtil.Db.SubmitChanges();
            AddSmallGroup(mt.Id);
            NewGroup = null;
        }
    }
}