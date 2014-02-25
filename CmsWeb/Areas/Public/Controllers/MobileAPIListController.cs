﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CmsData;
using CmsWeb.MobileAPI;
using Newtonsoft.Json;

namespace CmsWeb.Areas.Public.Controllers
{
	public class MobileAPIListController : Controller
	{
		public ActionResult Authenticate()
		{
			if (CmsWeb.Models.AccountModel.AuthenticateMobile()) return null;
			else
			{
				return BaseReturn.createErrorReturn("You are not authorized!");
			}
		}

		public ActionResult fetchCountries()
		{
			// Authenticate first
			var authError = Authenticate();
			if (authError != null) return authError;

			var countries = (from e in DbUtil.Db.Countries
								  orderby e.Id
								  select e).ToList();

			BaseReturn br = new BaseReturn();
			List<MobileCountry> ma = new List<MobileCountry>();

			br.error = 0;
			br.type = BaseReturn.API_TYPE_SYSTEM_COUNTRIES;
			br.count = countries.Count();

			foreach (var country in countries)
			{
				ma.Add(new MobileCountry().populate(country));
			}

			br.data = JsonConvert.SerializeObject(ma);
			return br;
		}

		public ActionResult fetchStates()
		{
			// Authenticate first
			var authError = Authenticate();
			if (authError != null) return authError;

			var states = (from e in DbUtil.Db.StateLookups
							  orderby e.StateCode
							  select e).ToList();

			BaseReturn br = new BaseReturn();
			List<MobileState> ma = new List<MobileState>();

			br.error = 0;
			br.type = BaseReturn.API_TYPE_SYSTEM_STATES;
			br.count = states.Count();

			foreach (var state in states)
			{
				ma.Add(new MobileState().populate(state));
			}

			br.data = JsonConvert.SerializeObject(ma);
			return br;
		}

		public ActionResult fetchMaritalStatuses()
		{
			// Authenticate first
			var authError = Authenticate();
			if (authError != null) return authError;

			var statuses = (from e in DbUtil.Db.MaritalStatuses
								 orderby e.Id
								 select e).ToList();

			BaseReturn br = new BaseReturn();
			List<MobileMaritalStatus> ma = new List<MobileMaritalStatus>();

			br.error = 0;
			br.type = BaseReturn.API_TYPE_SYSTEM_MARITAL_STATUSES;
			br.count = statuses.Count();

			foreach (var status in statuses)
			{
				ma.Add(new MobileMaritalStatus().populate(status));
			}

			br.data = JsonConvert.SerializeObject(ma);
			return br;
		}
	}
}
