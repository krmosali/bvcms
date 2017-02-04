﻿using System.Linq;
using System.Web.Mvc;
using CmsData;
using CmsData.Codes;
using CmsWeb.Areas.Finance.Models;
using UtilityExtensions;


namespace CmsWeb.Areas.Finance.Controllers
{
    [Authorize(Roles = "Finance")]
    [RouteArea("Finance", AreaPrefix= "Bundle"), Route("{action}/{id?}")]
    public class BundleController : CmsStaffController
    {
        [Route("~/Bundle/{id:int}")]
        public ActionResult Index(int id, bool? create)
        {
            var m = new BundleModel(id);
            if (m.Bundle == null)
                return Content("no bundle");
            return View(m);
        }

        [HttpPost]
        public ActionResult Results(BundleModel m)
        {
            return View(m);
        }

        public ActionResult Edit(int id)
        {
            TempData["editbundle"] = true;
            return RedirectToAction("Index", new {id = id});
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            var m = new BundleModel(id);
            return View(m);
        }

        [HttpPost]
        public ActionResult Update(int id)
        {
            var m = new BundleModel(id);
            UpdateModel<BundleModel>(m);
            UpdateModel<BundleHeader>(m.Bundle, "Bundle");
            var q = from d in DbUtil.Db.BundleDetails
                    where d.BundleHeaderId == m.Bundle.BundleHeaderId
                    select d.Contribution;
            var dt = q.Select(cc => cc.ContributionDate).FirstOrDefault();
            if (m.Bundle.ContributionDateChanged && q.All(cc => cc.ContributionDate == dt))
            {
                foreach (var c in q)
                    c.ContributionDate = m.Bundle.ContributionDate;
            }
            var fid = q.Select(cc => cc.FundId).FirstOrDefault();
            if (m.Bundle.FundIdChanged && q.All(cc => cc.FundId == fid))
            {
                foreach (var c in q)
                    c.FundId = m.Bundle.FundId ?? 1;
            }
            var postingdt = Util.Now;
            if (m.Bundle.BundleStatusIdChanged && m.Bundle.BundleStatusId == BundleStatusCode.Closed)
            {
                foreach (var d in m.Bundle.BundleDetails)
                    d.Contribution.PostingDate = postingdt;
            }
            DbUtil.Db.SubmitChanges();
            m.BundleId = id; // refresh values
            return View("Display", m);
        }

        [HttpPost]
        public ActionResult Cancel(int id)
        {
            var m = new BundleModel(id);
            return View("Display", m);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var m = new BundleModel(id);
            var q = from d in m.Bundle.BundleDetails
                    select d.Contribution;
            DbUtil.Db.Contributions.DeleteAllOnSubmit(q);
            DbUtil.Db.BundleDetails.DeleteAllOnSubmit(m.Bundle.BundleDetails);
            DbUtil.Db.BundleHeaders.DeleteOnSubmit(m.Bundle);
            DbUtil.Db.SubmitChanges();
            return Content("/Bundles");
        }
    }
}
