using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Net.Mail;
using CmsData.Registration;

namespace CmsWeb.Areas.OnlineReg.Models
{
    public partial class OnlineRegModel
    {
        private Regex donationtext = new Regex(@"\{donation(?<text>.*)donation\}", RegexOptions.Singleline | RegexOptions.Multiline);

        public bool EnrollAndConfirm()
        {
            ExpireRegisterTag();
            AddPeopleToTransaction();

            if (masterorgid.HasValue)
                return EnrollAndConfirmMultipleOrgs();

            var message = DoEnrollments();

            if (SupportMissionTrip && TotalAmount() > 0)
                return DoMissionTripSupporter();

            if (IsMissionTripGoerWithPayment())
                DoMissionTripGoer();
            else if (Transaction.Donate > 0)
                message = DoDonationModifyMessage(message);
            else
                message = donationtext.Replace(message, "");

            SendAllConfirmations(message);

            return true;
        }

        private bool EnrollAndConfirmMultipleOrgs()
        {
            var paylink = GetPayLink();
            foreach (var p in List)
            {
                p.Enroll(Transaction, paylink);
                p.DoGroupToJoin();
                p.CheckNotifyDiffEmails();

                if (p.IsCreateAccount())
                    p.CreateAccount();
                DbUtil.Db.SubmitChanges();

                SendSingleConfirmationForOrg(p);
            }
            return true;
        }

        private bool IsMissionTripGoerWithPayment()
        {
            return org != null
                   && Transaction.Amt > 0
                   && org.IsMissionTrip == true
                   && SupportMissionTrip == false;
        }

        private void SendAllConfirmations(string message)
        {
            DbUtil.Db.SetCurrentOrgId(org.OrganizationId);
            var subject = GetSubject();
            var amtpaid = Transaction.Amt ?? 0;

            var firstPerson = List[0].person;
            if (user != null)
                firstPerson = user;

            var notifyIds = GetNotifyIds();
            if (subject != "DO NOT SEND")
                DbUtil.Db.Email(notifyIds[0].FromEmail, firstPerson, listMailAddress, subject, message, false);

            DbUtil.Db.SubmitChanges();
            // notify the staff
            foreach (var p in List)
            {
                DbUtil.Db.Email(Util.PickFirst(p.person.FromEmail, notifyIds[0].FromEmail), notifyIds, Header,
                    @"{7}{0} has registered for {1}<br/>
Total Fee for this registrant: {2:C}<br/>
Total Fee for this registration: {3:C}<br/>
Total Fee paid today: {4:C}<br/>
AmountDue: {5:C}<br/>
<pre>{6}</pre>".Fmt(p.person.Name,
                        Header,
                        p.TotalAmount(),
                        TotalAmount(),
                        amtpaid,
                        Transaction.Amtdue, 
                        p.PrepareSummaryText(Transaction),
                        usedAdminsForNotify
                            ? @"<span style='color:red'>THERE ARE NO NOTIFY IDS ON THIS REGISTRATION!!</span><br/>
<a href='http://docs.touchpointsoftware.com/OnlineRegistration/MessagesSettings.html'>see documentation</a><br/><br/>"
                            : ""));
            }
        }
        private void SendSingleConfirmationForOrg(OnlineRegPersonModel p)
        {
            DbUtil.Db.SetCurrentOrgId(p.orgid);
            var emailSubject = GetSubject(p);
            string details = p.PrepareSummaryText(Transaction);
            var message = GetMessage(details, p);

            var NotifyIds = DbUtil.Db.StaffPeopleForOrg(p.org.OrganizationId);
            var notify = NotifyIds[0];

            string location = p.org.Location;
            if (!location.HasValue())
                location = masterorg.Location;

            message = CmsData.API.APIOrganization.MessageReplacements(DbUtil.Db, p.person, 
                masterorg.OrganizationName, p.org.OrganizationName, location, message);

            var amtpaid = Transaction.Amt ?? 0;

            if (Transaction.Donate > 0)
                message = DoDonationModifyMessage(message);
            else
                message = donationtext.Replace(message, "");

            // send confirmations
            if (emailSubject != "DO NOT SEND")
                DbUtil.Db.Email(notify.FromEmail, p.person, Util.EmailAddressListFromString(p.fromemail),
                    emailSubject, message, redacted: false);

            // notify the staff
            DbUtil.Db.Email(Util.PickFirst(p.person.FromEmail, notify.FromEmail),
                NotifyIds, Header,
                @"{0} has registered for {1}<br/>
Feepaid for this registrant: {2:C}<br/>
Others in this registration session: {3:C}<br/>
Total Fee paid for this registration session: {4:C}<br/>
<pre>{5}</pre>".Fmt(p.person.Name,
                    Header,
                    p.AmountToPay(),
                    p.GetOthersInTransaction(Transaction),
                    amtpaid,
                    p.PrepareSummaryText(Transaction)));
        }

        private Settings _masterSettings;
        private Settings GetMasterOrgSettings()
        {
            if (_masterSettings != null)
                return _masterSettings;
            if (masterorgid == null)
                throw new Exception("masterorgid was null in SendConfirmation");
            if (settings == null)
                throw new Exception("settings was null");
            if (!settings.ContainsKey(masterorgid.Value))
                throw new Exception("setting not found for masterorgid " + masterorgid.Value);
            ParseSettings();
            return _masterSettings = settings[masterorgid.Value];
        }


        private string DoDonationModifyMessage(string message)
        {
            var p = List[donor.Value];
            Transaction.Fund = p.setting.DonationFund();
            var desc = "{0}; {1}; {2}, {3} {4}".Fmt(
                p.person.Name,
                p.person.PrimaryAddress,
                p.person.PrimaryCity,
                p.person.PrimaryState,
                p.person.PrimaryZip);
            if (!Transaction.TransactionId.StartsWith("Coupon"))
                p.person.PostUnattendedContribution(DbUtil.Db, Transaction.Donate.Value, p.setting.DonationFundId, desc,
                    tranid: Transaction.Id);
            var subject = GetSubject();
            var ma = donationtext.Match(message);
            if (ma.Success)
            {
                var v = ma.Groups["text"].Value;
                message = donationtext.Replace(message, v);
            }
            message = message.Replace("{donation}", Transaction.Donate.ToString2("N2"));
            // send donation confirmations
            var notifyIds = GetNotifyIds();
            DbUtil.Db.Email(notifyIds[0].FromEmail, notifyIds, subject + "-donation",
                "${0:N2} donation received from {1}".Fmt(Transaction.Donate, Transaction.FullName(Transaction)));
            return message;
        }

        private void DoMissionTripGoer()
        {
            var p = List[0];
            Transaction.Fund = p.setting.DonationFund();

            DbUtil.Db.GoerSenderAmounts.InsertOnSubmit(
                new GoerSenderAmount
                {
                    Amount = Transaction.Amt,
                    GoerId = p.PeopleId,
                    Created = DateTime.Now,
                    OrgId = p.orgid.Value,
                    SupporterId = p.PeopleId.Value
                });
            if (Transaction.TransactionId.StartsWith("Coupon") || !Transaction.Amt.HasValue)
                return;

            p.person.PostUnattendedContribution(DbUtil.Db,
                Transaction.Amt.Value, p.setting.DonationFundId,
                "MissionTrip: org={0}; goer={1}".Fmt(p.orgid, p.PeopleId), tranid: Transaction.Id);
            Transaction.Description = "Mission Trip Giving";
        }

        private bool usedAdminsForNotify;
        private List<Person> _notifyIds;
        private List<Person> GetNotifyIds()
        {
            if (_notifyIds != null)
                return _notifyIds;
            return _notifyIds = DbUtil.Db.StaffPeopleForOrg(org.OrganizationId, out usedAdminsForNotify);
        }

        private bool DoMissionTripSupporter()
        {
            var notifyIds = GetNotifyIds();
            var p = List[0];
            Transaction.Fund = p.setting.DonationFund();
            var goerid = p.Parent.GoerId > 0
                ? p.Parent.GoerId
                : p.MissionTripGoerId;
            if (p.MissionTripSupportGoer > 0)
            {
                var gsa = new GoerSenderAmount
                {
                    Amount = p.MissionTripSupportGoer.Value,
                    Created = DateTime.Now,
                    OrgId = p.orgid.Value,
                    SupporterId = p.PeopleId.Value,
                    NoNoticeToGoer = p.MissionTripNoNoticeToGoer,
                };
                if (goerid > 0)
                    gsa.GoerId = goerid;
                DbUtil.Db.GoerSenderAmounts.InsertOnSubmit(gsa);
                if (p.Parent.GoerSupporterId.HasValue)
                {
                    var gs = DbUtil.Db.GoerSupporters.Single(gg => gg.Id == p.Parent.GoerSupporterId);
                    if (!gs.SupporterId.HasValue)
                        gs.SupporterId = p.PeopleId;
                }
                if (!Transaction.TransactionId.StartsWith("Coupon"))
                {
                    p.person.PostUnattendedContribution(DbUtil.Db,
                        p.MissionTripSupportGoer.Value, p.setting.DonationFundId,
                        "SupportMissionTrip: org={0}; goer={1}".Fmt(p.orgid, goerid), tranid: Transaction.Id);
                    // send notices
                    if (goerid > 0 && !p.MissionTripNoNoticeToGoer)
                    {
                        var goer = DbUtil.Db.LoadPersonById(goerid.Value);
                        DbUtil.Db.Email(notifyIds[0].FromEmail, goer, org.OrganizationName + "-donation",
                            "{0:C} donation received from {1}".Fmt(p.MissionTripSupportGoer.Value,
                                Transaction.FullName(Transaction)));
                    }
                }
            }
            if (p.MissionTripSupportGeneral > 0)
            {
                DbUtil.Db.GoerSenderAmounts.InsertOnSubmit(
                    new GoerSenderAmount
                    {
                        Amount = p.MissionTripSupportGeneral.Value,
                        Created = DateTime.Now,
                        OrgId = p.orgid.Value,
                        SupporterId = p.PeopleId.Value
                    });
                if (!Transaction.TransactionId.StartsWith("Coupon"))
                {
                    p.person.PostUnattendedContribution(DbUtil.Db,
                        p.MissionTripSupportGeneral.Value, p.setting.DonationFundId,
                        "SupportMissionTrip: org={0}".Fmt(p.orgid), tranid: Transaction.Id);
                }
            }
            var notifyids = DbUtil.Db.NotifyIds(org.GiftNotifyIds);
            DbUtil.Db.Email(notifyIds[0].FromEmail, notifyids, org.OrganizationName + "-donation",
                "${0:N2} donation received from {1}".Fmt(Transaction.Amt, Transaction.FullName(Transaction)));

            var orgsettings = settings[org.OrganizationId];
            var senderSubject = orgsettings.SenderSubject ?? "NO SUBJECT SET";
            var senderBody = orgsettings.SenderBody ?? "NO SENDEREMAIL MESSAGE HAS BEEN SET";
            senderBody = CmsData.API.APIOrganization.MessageReplacements(DbUtil.Db, p.person,
                org.DivisionName, org.OrganizationName, org.Location, senderBody);
            senderBody = senderBody.Replace("{phone}", org.PhoneNumber.FmtFone7());
            senderBody = senderBody.Replace("{paid}", Transaction.Amt.ToString2("c"));

            Transaction.Description = "Mission Trip Giving";
            DbUtil.Db.Email(notifyids[0].FromEmail, p.person, listMailAddress, senderSubject, senderBody, false);
            DbUtil.Db.SubmitChanges();
            return true;
        }

        private string GetSubject(OnlineRegPersonModel p)
        {
            if (p.setting.Subject.HasValue())
                return p.setting.Subject;
            var os = GetMasterOrgSettings();
            return Util.PickFirst(os.Subject, "no subject");
        }
        private string GetMessage(OnlineRegPersonModel p)
        {
            if (p.setting.Body.HasValue())
                return p.setting.Body;
            var os = GetMasterOrgSettings();
            return Util.PickFirst(os.Body, "no body");
        }

        private string _subject;
        private string GetSubject()
        {
            if (_subject.HasValue())
                return _subject;
            var orgsettings = settings[org.OrganizationId];
            _subject = Util.PickFirst(orgsettings.Subject, "no subject");
            return _subject = _subject.Replace("{org}", Header);
        }

        private string GetMessage(string details, OnlineRegPersonModel p)
        {
            var amtpaid = Transaction.Amt ?? 0;
            var paylink = GetPayLink();
            var orgsettings = settings[org.OrganizationId];
            var message = Util.PickFirst(orgsettings.Body, "no message");

            var location = org.Location;
            if (!location.HasValue() && masterorg != null)
                location = masterorg.Location;

            message = CmsData.API.APIOrganization.MessageReplacements(DbUtil.Db,
                p.person, org.DivisionName, org.OrganizationName, location, message);
            message = message.Replace("{phone}", org.PhoneNumber.FmtFone7())
                .Replace("{tickets}", p.ntickets.ToString())
                .Replace("{details}", details)
                .Replace("{paid}", amtpaid.ToString("c"))
                .Replace("{sessiontotal}", amtpaid.ToString("c"))
                .Replace("{participants}", details);
            if (Transaction.Amtdue > 0)
                message = message.Replace("{paylink}",
                    "<a href='{0}'>Click this link to make a payment on your balance of {1:C}</a>.".Fmt(paylink,
                        Transaction.Amtdue));
            else
                message = message.Replace("{paylink}", "You have a zero balance.");
            return message;
        }


        private string DoEnrollments()
        {
            const string amountRowFormat = @"
<tr><td colspan='2'>
    <table cellpadding='4'>
        <tr><td>Total Paid</td><td>Total Due</td></tr>
        <tr><td align='right'>{0:c}</td><td align='right'>{1:c}</td></tr>
    </table>
    </td>
</tr>
";
            const string summaryRow = @"
<tr><td colspan='2'><hr/></td></tr>
{0}
</td></tr>";

            var amtpaid = Transaction.Amt ?? 0;
            var details = new StringBuilder("<table cellpadding='4'>");
            if (Transaction.Amt > 0)
                details.AppendFormat(amountRowFormat, amtpaid, Transaction.Amtdue);
            foreach (var p in List)
            {
                p.Enroll(Transaction, GetPayLink());
                p.DoGroupToJoin();
                p.CheckNotifyDiffEmails();

                if (p.IsCreateAccount())
                    p.CreateAccount();
                DbUtil.Db.SubmitChanges();

                details.AppendFormat(summaryRow, p.PrepareSummaryText(Transaction));
            }
            details.Append("\n</table>\n");
            return GetMessage(details.ToString(), List[0]);
        }

        private string _paylink;
        private string GetPayLink()
        {
            if (_paylink.HasValue())
                return _paylink;
            if (org.IsMissionTrip == true)
                return _paylink = DbUtil.Db.ServerLink("/OnlineReg/{0}?goerid={1}".Fmt(Orgid, List[0].PeopleId));
            var estr = HttpUtility.UrlEncode(Util.Encrypt(Transaction.OriginalId.ToString()));
            return _paylink = DbUtil.Db.ServerLink("/OnlineReg/PayAmtDue?q=" + estr);
        }

        private List<MailAddress> listMailAddress;
        private void AddPeopleToTransaction()
        {
            listMailAddress = GetEmailList();
            var participants = GetParticipants(listMailAddress);
            var transactionPeople = new List<TransactionPerson>();
            foreach (var p in List)
            {
                if (p.PeopleId == null)
                    throw new Exception("no PeopleId in List");
                if (transactionPeople.Any(pp => pp.PeopleId == p.PeopleId))
                    continue;
                transactionPeople.Add(new TransactionPerson
                {
                    PeopleId = p.PeopleId.Value,
                    Amt = p.TotalAmount(),
                    OrgId = p.orgid ?? Orgid,
                });
            }

            if (SupportMissionTrip && GoerId == _list[0].PeopleId)
            {
                // reload transaction because it is not in this context
                var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(mm => mm.PeopleId == GoerId && mm.OrganizationId == Orgid);
                if (om != null && om.TranId.HasValue)
                    Transaction.OriginalId = om.TranId;
            }
            else
            {
                Transaction.OriginalTrans.TransactionPeople.AddRange(transactionPeople);
            }
            Transaction.Emails = listMailAddress.EmailAddressListToString();
            Transaction.Participants = participants;
            Transaction.TransactionDate = DateTime.Now;
        }


        private string GetParticipants(List<MailAddress> elist)
        {
            var participants = new StringBuilder();
            for (var i = 0; i < List.Count; i++)
            {
                var p = List[i];
                if (p.IsNew)
                {
                    Person uperson = null;
                    if (i > 0)
                    {
                        if(List[i].AddressLineOne.HasValue() && List[i].AddressLineOne == List[i-1].AddressLineOne)
                            uperson = List[i - 1].person; // add to previous family
                    }
                    p.AddPerson(uperson, p.org.EntryPointId ?? 0);
                }
                Util.AddGoodAddress(elist, p.fromemail);
                participants.Append(p.ToString());
            }
            return participants.ToString();
        }

        private void ExpireRegisterTag()
        {
            if (registertag.HasValue())
            {
                var guid = registertag.ToGuid();
                var ot = DbUtil.Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == guid.Value);
                ot.Used = true;
            }
        }

        private List<MailAddress> GetEmailList()
        {
            var elist = new List<MailAddress>();
            if (UserPeopleId.HasValue)
            {
                if (user.SendEmailAddress1 ?? true)
                    Util.AddGoodAddress(elist, user.FromEmail);
                if (user.SendEmailAddress2 ?? false)
                    Util.AddGoodAddress(elist, user.FromEmail2);
            }
            return elist;
        }


        public void UseCoupon(string TransactionID, decimal AmtPaid)
        {
            string matchcoupon = @"Coupon\((?<coupon>[^)]*)\)";
            if (Regex.IsMatch(TransactionID, matchcoupon, RegexOptions.IgnoreCase))
            {
                var match = Regex.Match(TransactionID, matchcoupon, RegexOptions.IgnoreCase);
                var coup = match.Groups["coupon"];
                var coupon = "";
                if (coup != null)
                    coupon = coup.Value.Replace(" ", "");
                if (coupon != "Admin")
                {
                    var c = DbUtil.Db.Coupons.SingleOrDefault(cp => cp.Id == coupon);
                    if (c != null)
                    {
                        c.RegAmount = AmtPaid;
                        c.Used = DateTime.Now;
                        c.PeopleId = List[0].PeopleId;
                    }
                }
            }
        }
        public void ConfirmReregister()
        {
            var p = List[0];
            var message = DbUtil.Db.ContentHtml("ReregisterLinkEmail", @"Hi {name},
<p>Here is your <a href=""{url}"">MANAGE REGISTRATION</a> link to manage {orgname}. This link will work only once. Creating an account will allow you to do this again without having to email the link.</p>");
            message = message.Replace("{orgname}", Header).Replace("{org}", Header);

            var Staff = DbUtil.Db.StaffPeopleForOrg(Orgid.Value);
            p.SendOneTimeLink(Staff.First().FromEmail,
                DbUtil.Db.ServerLink("/OnlineReg/RegisterLink/"), "Manage Your Registration for " + Header, message);
        }
        public ConfirmEnum ConfirmManageSubscriptions()
        {
            var p = List[0];
            if (p.IsNew)
                p.AddPerson(null, GetEntryPoint());
            if (p.CreatingAccount == true)
                p.CreateAccount();

            var c = DbUtil.Content("OneTimeConfirmation");
            if (c == null)
                c = new Content();

            var message = Util.PickFirst(c.Body,
                    @"Hi {name},
<p>Here is your <a href=""{url}"">link</a> to manage your subscriptions. (note: it will only work once for security reasons)</p> ");

            var Staff = DbUtil.Db.StaffPeopleForOrg(masterorgid.Value);
            p.SendOneTimeLink(
                Staff.First().FromEmail,
                DbUtil.Db.ServerLink("/OnlineReg/ManageSubscriptions/"), "Manage Your Subscriptions", message);
            return ConfirmEnum.ConfirmAccount;
        }
        private ConfirmEnum ConfirmPickSlots()
        {
            var p = List[0];
            if (p.IsNew)
                p.AddPerson(null, GetEntryPoint());
            if (p.CreatingAccount == true)
                p.CreateAccount();

            var c = DbUtil.Content("OneTimeConfirmationVolunteer");
            if (c == null)
                c = new Content();

            var message = Util.PickFirst(c.Body,
                    @"Hi {name},
<p>Here is your <a href=""{url}"">link</a> to manage your volunteer commitments. (note: it will only work once for security reasons)</p> ");

            List<Person> Staff = null;
            Staff = DbUtil.Db.StaffPeopleForOrg(Orgid.Value);
            p.SendOneTimeLink(
                Staff.First().FromEmail,
                DbUtil.Db.ServerLink("/OnlineReg/ManageVolunteer/"), "Manage Your Volunteer Commitments", message);
            URL = null;
            return ConfirmEnum.ConfirmAccount;
        }
        internal ConfirmEnum SendLinkForPledge()
        {
            var p = List[0];
            if (p.IsNew)
                p.AddPerson(null, p.org.EntryPointId ?? 0);
            if (p.CreatingAccount == true)
                p.CreateAccount();

            var c = DbUtil.Content("OneTimeConfirmationPledge");
            if (c == null)
            {
                c = new Content();
                c.Title = "Manage your pledge";
                c.Body = @"Hi {name},
<p>Here is your <a href=""{url}"">link</a> to manage your pledge. (note: it will only work once for security reasons)</p> ";
            }

            p.SendOneTimeLink(
                DbUtil.Db.StaffPeopleForOrg(Orgid.Value).First().FromEmail,
                DbUtil.Db.ServerLink("/OnlineReg/ManagePledge/"), c.Title, c.Body);
            return ConfirmEnum.ConfirmAccount;
        }
        internal ConfirmEnum SendLinkToManageGiving()
        {
            var p = List[0];
            if (p.IsNew)
                p.AddPerson(null, p.org.EntryPointId ?? 0);
            if (p.CreatingAccount == true)
                p.CreateAccount();

            var c = DbUtil.Content("OneTimeManageGiving");
            if (c == null)
            {
                c = new Content();
                c.Title = "Manage your recurring giving";
                c.Body = @"Hi {name},
<p>Here is your <a href=""{url}"">link</a> to manage your recurring giving. (note: it will only work once for security reasons)</p> ";
            }

            p.SendOneTimeLink(
                DbUtil.Db.StaffPeopleForOrg(Orgid.Value).First().FromEmail,
                DbUtil.Db.ServerLink("/OnlineReg/ManageGiving/"), c.Title, c.Body);
            return ConfirmEnum.ConfirmAccount;
        }
        public int GetEntryPoint()
        {
            if (org != null && org.EntryPointId != null)
                return org.EntryPointId.Value;
            if (masterorg != null && masterorg.EntryPointId != null)
                return masterorg.EntryPointId.Value;
            return 0;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("orgid: {0}<br/>\n", this.Orgid);
            sb.AppendFormat("masterorgid: {0}<br/>\n", this.masterorgid);
            sb.AppendFormat("userid: {0}<br/>\n", this.UserPeopleId);
            foreach (var li in List)
            {
                sb.AppendFormat("--------------------------------\nList: {0}<br/>\n", li.Index);
                sb.Append(li.ToString());
            }
            return sb.ToString();
        }
    }
}