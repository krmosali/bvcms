using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData
{
	[Table(Name="dbo.EnrollmentTransaction")]
	public partial class EnrollmentTransaction : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _TransactionId;
		
		private bool _TransactionStatus;
		
		private int? _CreatedBy;
		
		private DateTime? _CreatedDate;
		
		private DateTime _TransactionDate;
		
		private int _TransactionTypeId;
		
		private int _OrganizationId;
		
		private string _OrganizationName;
		
		private int _PeopleId;
		
		private int _MemberTypeId;
		
		private int _RollSheetSectionId;
		
		private DateTime? _EnrollmentDate;
		
		private bool? _FeePaid;
		
		private bool? _AllergyInfo;
		
		private int? _RobeNumber;
		
		private int? _GroupNumber;
		
		private int? _SectionId;
		
		private string _ClothingSizeInfo;
		
		private string _EmergencyContactInfo;
		
		private string _WhoBringsChildInfo;
		
		private string _AllergyText;
		
		private string _TransactionNotes;
		
		private bool _PromotionFlag;
		
		private decimal? _AttendancePercentage;
		
		private int? _ChurchYear;
		
		private int? _ChurchWeek;
		
		private int? _ChurchQuarter;
		
		private int? _ChurchMonth;
		
		private int? _PositionId;
		
		private bool? _VipWeek1;
		
		private bool? _VipWeek2;
		
		private bool? _VipWeek3;
		
		private bool? _VipWeek4;
		
		private bool? _VipWeek5;
		
		private DateTime? _NextTranChangeDate;
		
		private int? _EnrollmentTransactionId;
		
   		
   		private EntitySet< BadET> _BadETs;
		
    	
		private EntityRef< Organization> _Organization;
		
		private EntityRef< Person> _Person;
		
		private EntityRef< MemberType> _MemberType;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnTransactionIdChanging(int value);
		partial void OnTransactionIdChanged();
		
		partial void OnTransactionStatusChanging(bool value);
		partial void OnTransactionStatusChanged();
		
		partial void OnCreatedByChanging(int? value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime? value);
		partial void OnCreatedDateChanged();
		
		partial void OnTransactionDateChanging(DateTime value);
		partial void OnTransactionDateChanged();
		
		partial void OnTransactionTypeIdChanging(int value);
		partial void OnTransactionTypeIdChanged();
		
		partial void OnOrganizationIdChanging(int value);
		partial void OnOrganizationIdChanged();
		
		partial void OnOrganizationNameChanging(string value);
		partial void OnOrganizationNameChanged();
		
		partial void OnPeopleIdChanging(int value);
		partial void OnPeopleIdChanged();
		
		partial void OnMemberTypeIdChanging(int value);
		partial void OnMemberTypeIdChanged();
		
		partial void OnRollSheetSectionIdChanging(int value);
		partial void OnRollSheetSectionIdChanged();
		
		partial void OnEnrollmentDateChanging(DateTime? value);
		partial void OnEnrollmentDateChanged();
		
		partial void OnFeePaidChanging(bool? value);
		partial void OnFeePaidChanged();
		
		partial void OnAllergyInfoChanging(bool? value);
		partial void OnAllergyInfoChanged();
		
		partial void OnRobeNumberChanging(int? value);
		partial void OnRobeNumberChanged();
		
		partial void OnGroupNumberChanging(int? value);
		partial void OnGroupNumberChanged();
		
		partial void OnSectionIdChanging(int? value);
		partial void OnSectionIdChanged();
		
		partial void OnClothingSizeInfoChanging(string value);
		partial void OnClothingSizeInfoChanged();
		
		partial void OnEmergencyContactInfoChanging(string value);
		partial void OnEmergencyContactInfoChanged();
		
		partial void OnWhoBringsChildInfoChanging(string value);
		partial void OnWhoBringsChildInfoChanged();
		
		partial void OnAllergyTextChanging(string value);
		partial void OnAllergyTextChanged();
		
		partial void OnTransactionNotesChanging(string value);
		partial void OnTransactionNotesChanged();
		
		partial void OnPromotionFlagChanging(bool value);
		partial void OnPromotionFlagChanged();
		
		partial void OnAttendancePercentageChanging(decimal? value);
		partial void OnAttendancePercentageChanged();
		
		partial void OnChurchYearChanging(int? value);
		partial void OnChurchYearChanged();
		
		partial void OnChurchWeekChanging(int? value);
		partial void OnChurchWeekChanged();
		
		partial void OnChurchQuarterChanging(int? value);
		partial void OnChurchQuarterChanged();
		
		partial void OnChurchMonthChanging(int? value);
		partial void OnChurchMonthChanged();
		
		partial void OnPositionIdChanging(int? value);
		partial void OnPositionIdChanged();
		
		partial void OnVipWeek1Changing(bool? value);
		partial void OnVipWeek1Changed();
		
		partial void OnVipWeek2Changing(bool? value);
		partial void OnVipWeek2Changed();
		
		partial void OnVipWeek3Changing(bool? value);
		partial void OnVipWeek3Changed();
		
		partial void OnVipWeek4Changing(bool? value);
		partial void OnVipWeek4Changed();
		
		partial void OnVipWeek5Changing(bool? value);
		partial void OnVipWeek5Changed();
		
		partial void OnNextTranChangeDateChanging(DateTime? value);
		partial void OnNextTranChangeDateChanged();
		
		partial void OnEnrollmentTransactionIdChanging(int? value);
		partial void OnEnrollmentTransactionIdChanged();
		
    #endregion
		public EnrollmentTransaction()
		{
			
			this._BadETs = new EntitySet< BadET>(new Action< BadET>(this.attach_BadETs), new Action< BadET>(this.detach_BadETs)); 
			
			
			this._Organization = default(EntityRef< Organization>); 
			
			this._Person = default(EntityRef< Person>); 
			
			this._MemberType = default(EntityRef< MemberType>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="TransactionId", UpdateCheck=UpdateCheck.Never, Storage="_TransactionId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int TransactionId
		{
			get { return this._TransactionId; }

			set
			{
				if (this._TransactionId != value)
				{
				
                    this.OnTransactionIdChanging(value);
					this.SendPropertyChanging();
					this._TransactionId = value;
					this.SendPropertyChanged("TransactionId");
					this.OnTransactionIdChanged();
				}

			}

		}

		
		[Column(Name="TransactionStatus", UpdateCheck=UpdateCheck.Never, Storage="_TransactionStatus", DbType="bit NOT NULL")]
		public bool TransactionStatus
		{
			get { return this._TransactionStatus; }

			set
			{
				if (this._TransactionStatus != value)
				{
				
                    this.OnTransactionStatusChanging(value);
					this.SendPropertyChanging();
					this._TransactionStatus = value;
					this.SendPropertyChanged("TransactionStatus");
					this.OnTransactionStatusChanged();
				}

			}

		}

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="int")]
		public int? CreatedBy
		{
			get { return this._CreatedBy; }

			set
			{
				if (this._CreatedBy != value)
				{
				
                    this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}

			}

		}

		
		[Column(Name="CreatedDate", UpdateCheck=UpdateCheck.Never, Storage="_CreatedDate", DbType="datetime")]
		public DateTime? CreatedDate
		{
			get { return this._CreatedDate; }

			set
			{
				if (this._CreatedDate != value)
				{
				
                    this.OnCreatedDateChanging(value);
					this.SendPropertyChanging();
					this._CreatedDate = value;
					this.SendPropertyChanged("CreatedDate");
					this.OnCreatedDateChanged();
				}

			}

		}

		
		[Column(Name="TransactionDate", UpdateCheck=UpdateCheck.Never, Storage="_TransactionDate", DbType="datetime NOT NULL")]
		public DateTime TransactionDate
		{
			get { return this._TransactionDate; }

			set
			{
				if (this._TransactionDate != value)
				{
				
                    this.OnTransactionDateChanging(value);
					this.SendPropertyChanging();
					this._TransactionDate = value;
					this.SendPropertyChanged("TransactionDate");
					this.OnTransactionDateChanged();
				}

			}

		}

		
		[Column(Name="TransactionTypeId", UpdateCheck=UpdateCheck.Never, Storage="_TransactionTypeId", DbType="int NOT NULL")]
		public int TransactionTypeId
		{
			get { return this._TransactionTypeId; }

			set
			{
				if (this._TransactionTypeId != value)
				{
				
                    this.OnTransactionTypeIdChanging(value);
					this.SendPropertyChanging();
					this._TransactionTypeId = value;
					this.SendPropertyChanged("TransactionTypeId");
					this.OnTransactionTypeIdChanged();
				}

			}

		}

		
		[Column(Name="OrganizationId", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationId", DbType="int NOT NULL")]
		public int OrganizationId
		{
			get { return this._OrganizationId; }

			set
			{
				if (this._OrganizationId != value)
				{
				
					if (this._Organization.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOrganizationIdChanging(value);
					this.SendPropertyChanging();
					this._OrganizationId = value;
					this.SendPropertyChanged("OrganizationId");
					this.OnOrganizationIdChanged();
				}

			}

		}

		
		[Column(Name="OrganizationName", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationName", DbType="varchar(60) NOT NULL")]
		public string OrganizationName
		{
			get { return this._OrganizationName; }

			set
			{
				if (this._OrganizationName != value)
				{
				
                    this.OnOrganizationNameChanging(value);
					this.SendPropertyChanging();
					this._OrganizationName = value;
					this.SendPropertyChanged("OrganizationName");
					this.OnOrganizationNameChanged();
				}

			}

		}

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int NOT NULL")]
		public int PeopleId
		{
			get { return this._PeopleId; }

			set
			{
				if (this._PeopleId != value)
				{
				
					if (this._Person.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPeopleIdChanging(value);
					this.SendPropertyChanging();
					this._PeopleId = value;
					this.SendPropertyChanged("PeopleId");
					this.OnPeopleIdChanged();
				}

			}

		}

		
		[Column(Name="MemberTypeId", UpdateCheck=UpdateCheck.Never, Storage="_MemberTypeId", DbType="int NOT NULL")]
		public int MemberTypeId
		{
			get { return this._MemberTypeId; }

			set
			{
				if (this._MemberTypeId != value)
				{
				
					if (this._MemberType.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnMemberTypeIdChanging(value);
					this.SendPropertyChanging();
					this._MemberTypeId = value;
					this.SendPropertyChanged("MemberTypeId");
					this.OnMemberTypeIdChanged();
				}

			}

		}

		
		[Column(Name="RollSheetSectionId", UpdateCheck=UpdateCheck.Never, Storage="_RollSheetSectionId", DbType="int NOT NULL")]
		public int RollSheetSectionId
		{
			get { return this._RollSheetSectionId; }

			set
			{
				if (this._RollSheetSectionId != value)
				{
				
                    this.OnRollSheetSectionIdChanging(value);
					this.SendPropertyChanging();
					this._RollSheetSectionId = value;
					this.SendPropertyChanged("RollSheetSectionId");
					this.OnRollSheetSectionIdChanged();
				}

			}

		}

		
		[Column(Name="EnrollmentDate", UpdateCheck=UpdateCheck.Never, Storage="_EnrollmentDate", DbType="datetime")]
		public DateTime? EnrollmentDate
		{
			get { return this._EnrollmentDate; }

			set
			{
				if (this._EnrollmentDate != value)
				{
				
                    this.OnEnrollmentDateChanging(value);
					this.SendPropertyChanging();
					this._EnrollmentDate = value;
					this.SendPropertyChanged("EnrollmentDate");
					this.OnEnrollmentDateChanged();
				}

			}

		}

		
		[Column(Name="FeePaid", UpdateCheck=UpdateCheck.Never, Storage="_FeePaid", DbType="bit")]
		public bool? FeePaid
		{
			get { return this._FeePaid; }

			set
			{
				if (this._FeePaid != value)
				{
				
                    this.OnFeePaidChanging(value);
					this.SendPropertyChanging();
					this._FeePaid = value;
					this.SendPropertyChanged("FeePaid");
					this.OnFeePaidChanged();
				}

			}

		}

		
		[Column(Name="AllergyInfo", UpdateCheck=UpdateCheck.Never, Storage="_AllergyInfo", DbType="bit")]
		public bool? AllergyInfo
		{
			get { return this._AllergyInfo; }

			set
			{
				if (this._AllergyInfo != value)
				{
				
                    this.OnAllergyInfoChanging(value);
					this.SendPropertyChanging();
					this._AllergyInfo = value;
					this.SendPropertyChanged("AllergyInfo");
					this.OnAllergyInfoChanged();
				}

			}

		}

		
		[Column(Name="RobeNumber", UpdateCheck=UpdateCheck.Never, Storage="_RobeNumber", DbType="int")]
		public int? RobeNumber
		{
			get { return this._RobeNumber; }

			set
			{
				if (this._RobeNumber != value)
				{
				
                    this.OnRobeNumberChanging(value);
					this.SendPropertyChanging();
					this._RobeNumber = value;
					this.SendPropertyChanged("RobeNumber");
					this.OnRobeNumberChanged();
				}

			}

		}

		
		[Column(Name="GroupNumber", UpdateCheck=UpdateCheck.Never, Storage="_GroupNumber", DbType="int")]
		public int? GroupNumber
		{
			get { return this._GroupNumber; }

			set
			{
				if (this._GroupNumber != value)
				{
				
                    this.OnGroupNumberChanging(value);
					this.SendPropertyChanging();
					this._GroupNumber = value;
					this.SendPropertyChanged("GroupNumber");
					this.OnGroupNumberChanged();
				}

			}

		}

		
		[Column(Name="SectionId", UpdateCheck=UpdateCheck.Never, Storage="_SectionId", DbType="int")]
		public int? SectionId
		{
			get { return this._SectionId; }

			set
			{
				if (this._SectionId != value)
				{
				
                    this.OnSectionIdChanging(value);
					this.SendPropertyChanging();
					this._SectionId = value;
					this.SendPropertyChanged("SectionId");
					this.OnSectionIdChanged();
				}

			}

		}

		
		[Column(Name="ClothingSizeInfo", UpdateCheck=UpdateCheck.Never, Storage="_ClothingSizeInfo", DbType="varchar(256)")]
		public string ClothingSizeInfo
		{
			get { return this._ClothingSizeInfo; }

			set
			{
				if (this._ClothingSizeInfo != value)
				{
				
                    this.OnClothingSizeInfoChanging(value);
					this.SendPropertyChanging();
					this._ClothingSizeInfo = value;
					this.SendPropertyChanged("ClothingSizeInfo");
					this.OnClothingSizeInfoChanged();
				}

			}

		}

		
		[Column(Name="EmergencyContactInfo", UpdateCheck=UpdateCheck.Never, Storage="_EmergencyContactInfo", DbType="varchar(256)")]
		public string EmergencyContactInfo
		{
			get { return this._EmergencyContactInfo; }

			set
			{
				if (this._EmergencyContactInfo != value)
				{
				
                    this.OnEmergencyContactInfoChanging(value);
					this.SendPropertyChanging();
					this._EmergencyContactInfo = value;
					this.SendPropertyChanged("EmergencyContactInfo");
					this.OnEmergencyContactInfoChanged();
				}

			}

		}

		
		[Column(Name="WhoBringsChildInfo", UpdateCheck=UpdateCheck.Never, Storage="_WhoBringsChildInfo", DbType="varchar(256)")]
		public string WhoBringsChildInfo
		{
			get { return this._WhoBringsChildInfo; }

			set
			{
				if (this._WhoBringsChildInfo != value)
				{
				
                    this.OnWhoBringsChildInfoChanging(value);
					this.SendPropertyChanging();
					this._WhoBringsChildInfo = value;
					this.SendPropertyChanged("WhoBringsChildInfo");
					this.OnWhoBringsChildInfoChanged();
				}

			}

		}

		
		[Column(Name="AllergyText", UpdateCheck=UpdateCheck.Never, Storage="_AllergyText", DbType="varchar(256)")]
		public string AllergyText
		{
			get { return this._AllergyText; }

			set
			{
				if (this._AllergyText != value)
				{
				
                    this.OnAllergyTextChanging(value);
					this.SendPropertyChanging();
					this._AllergyText = value;
					this.SendPropertyChanged("AllergyText");
					this.OnAllergyTextChanged();
				}

			}

		}

		
		[Column(Name="TransactionNotes", UpdateCheck=UpdateCheck.Never, Storage="_TransactionNotes", DbType="varchar(256)")]
		public string TransactionNotes
		{
			get { return this._TransactionNotes; }

			set
			{
				if (this._TransactionNotes != value)
				{
				
                    this.OnTransactionNotesChanging(value);
					this.SendPropertyChanging();
					this._TransactionNotes = value;
					this.SendPropertyChanged("TransactionNotes");
					this.OnTransactionNotesChanged();
				}

			}

		}

		
		[Column(Name="PromotionFlag", UpdateCheck=UpdateCheck.Never, Storage="_PromotionFlag", DbType="bit NOT NULL")]
		public bool PromotionFlag
		{
			get { return this._PromotionFlag; }

			set
			{
				if (this._PromotionFlag != value)
				{
				
                    this.OnPromotionFlagChanging(value);
					this.SendPropertyChanging();
					this._PromotionFlag = value;
					this.SendPropertyChanged("PromotionFlag");
					this.OnPromotionFlagChanged();
				}

			}

		}

		
		[Column(Name="AttendancePercentage", UpdateCheck=UpdateCheck.Never, Storage="_AttendancePercentage", DbType="Decimal(5,3)")]
		public decimal? AttendancePercentage
		{
			get { return this._AttendancePercentage; }

			set
			{
				if (this._AttendancePercentage != value)
				{
				
                    this.OnAttendancePercentageChanging(value);
					this.SendPropertyChanging();
					this._AttendancePercentage = value;
					this.SendPropertyChanged("AttendancePercentage");
					this.OnAttendancePercentageChanged();
				}

			}

		}

		
		[Column(Name="ChurchYear", UpdateCheck=UpdateCheck.Never, Storage="_ChurchYear", DbType="int")]
		public int? ChurchYear
		{
			get { return this._ChurchYear; }

			set
			{
				if (this._ChurchYear != value)
				{
				
                    this.OnChurchYearChanging(value);
					this.SendPropertyChanging();
					this._ChurchYear = value;
					this.SendPropertyChanged("ChurchYear");
					this.OnChurchYearChanged();
				}

			}

		}

		
		[Column(Name="ChurchWeek", UpdateCheck=UpdateCheck.Never, Storage="_ChurchWeek", DbType="int")]
		public int? ChurchWeek
		{
			get { return this._ChurchWeek; }

			set
			{
				if (this._ChurchWeek != value)
				{
				
                    this.OnChurchWeekChanging(value);
					this.SendPropertyChanging();
					this._ChurchWeek = value;
					this.SendPropertyChanged("ChurchWeek");
					this.OnChurchWeekChanged();
				}

			}

		}

		
		[Column(Name="ChurchQuarter", UpdateCheck=UpdateCheck.Never, Storage="_ChurchQuarter", DbType="int")]
		public int? ChurchQuarter
		{
			get { return this._ChurchQuarter; }

			set
			{
				if (this._ChurchQuarter != value)
				{
				
                    this.OnChurchQuarterChanging(value);
					this.SendPropertyChanging();
					this._ChurchQuarter = value;
					this.SendPropertyChanged("ChurchQuarter");
					this.OnChurchQuarterChanged();
				}

			}

		}

		
		[Column(Name="ChurchMonth", UpdateCheck=UpdateCheck.Never, Storage="_ChurchMonth", DbType="int")]
		public int? ChurchMonth
		{
			get { return this._ChurchMonth; }

			set
			{
				if (this._ChurchMonth != value)
				{
				
                    this.OnChurchMonthChanging(value);
					this.SendPropertyChanging();
					this._ChurchMonth = value;
					this.SendPropertyChanged("ChurchMonth");
					this.OnChurchMonthChanged();
				}

			}

		}

		
		[Column(Name="PositionId", UpdateCheck=UpdateCheck.Never, Storage="_PositionId", DbType="int")]
		public int? PositionId
		{
			get { return this._PositionId; }

			set
			{
				if (this._PositionId != value)
				{
				
                    this.OnPositionIdChanging(value);
					this.SendPropertyChanging();
					this._PositionId = value;
					this.SendPropertyChanged("PositionId");
					this.OnPositionIdChanged();
				}

			}

		}

		
		[Column(Name="VipWeek1", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek1", DbType="bit")]
		public bool? VipWeek1
		{
			get { return this._VipWeek1; }

			set
			{
				if (this._VipWeek1 != value)
				{
				
                    this.OnVipWeek1Changing(value);
					this.SendPropertyChanging();
					this._VipWeek1 = value;
					this.SendPropertyChanged("VipWeek1");
					this.OnVipWeek1Changed();
				}

			}

		}

		
		[Column(Name="VipWeek2", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek2", DbType="bit")]
		public bool? VipWeek2
		{
			get { return this._VipWeek2; }

			set
			{
				if (this._VipWeek2 != value)
				{
				
                    this.OnVipWeek2Changing(value);
					this.SendPropertyChanging();
					this._VipWeek2 = value;
					this.SendPropertyChanged("VipWeek2");
					this.OnVipWeek2Changed();
				}

			}

		}

		
		[Column(Name="VipWeek3", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek3", DbType="bit")]
		public bool? VipWeek3
		{
			get { return this._VipWeek3; }

			set
			{
				if (this._VipWeek3 != value)
				{
				
                    this.OnVipWeek3Changing(value);
					this.SendPropertyChanging();
					this._VipWeek3 = value;
					this.SendPropertyChanged("VipWeek3");
					this.OnVipWeek3Changed();
				}

			}

		}

		
		[Column(Name="VipWeek4", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek4", DbType="bit")]
		public bool? VipWeek4
		{
			get { return this._VipWeek4; }

			set
			{
				if (this._VipWeek4 != value)
				{
				
                    this.OnVipWeek4Changing(value);
					this.SendPropertyChanging();
					this._VipWeek4 = value;
					this.SendPropertyChanged("VipWeek4");
					this.OnVipWeek4Changed();
				}

			}

		}

		
		[Column(Name="VipWeek5", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek5", DbType="bit")]
		public bool? VipWeek5
		{
			get { return this._VipWeek5; }

			set
			{
				if (this._VipWeek5 != value)
				{
				
                    this.OnVipWeek5Changing(value);
					this.SendPropertyChanging();
					this._VipWeek5 = value;
					this.SendPropertyChanged("VipWeek5");
					this.OnVipWeek5Changed();
				}

			}

		}

		
		[Column(Name="NextTranChangeDate", UpdateCheck=UpdateCheck.Never, Storage="_NextTranChangeDate", DbType="datetime")]
		public DateTime? NextTranChangeDate
		{
			get { return this._NextTranChangeDate; }

			set
			{
				if (this._NextTranChangeDate != value)
				{
				
                    this.OnNextTranChangeDateChanging(value);
					this.SendPropertyChanging();
					this._NextTranChangeDate = value;
					this.SendPropertyChanged("NextTranChangeDate");
					this.OnNextTranChangeDateChanged();
				}

			}

		}

		
		[Column(Name="EnrollmentTransactionId", UpdateCheck=UpdateCheck.Never, Storage="_EnrollmentTransactionId", DbType="int")]
		public int? EnrollmentTransactionId
		{
			get { return this._EnrollmentTransactionId; }

			set
			{
				if (this._EnrollmentTransactionId != value)
				{
				
                    this.OnEnrollmentTransactionIdChanging(value);
					this.SendPropertyChanging();
					this._EnrollmentTransactionId = value;
					this.SendPropertyChanged("EnrollmentTransactionId");
					this.OnEnrollmentTransactionIdChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_BadET_EnrollmentTransaction", Storage="_BadETs", OtherKey="TranId")]
   		public EntitySet< BadET> BadETs
   		{
   		    get { return this._BadETs; }

			set	{ this._BadETs.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="ENROLLMENT_TRANSACTION_ORG_FK", Storage="_Organization", ThisKey="OrganizationId", IsForeignKey=true)]
		public Organization Organization
		{
			get { return this._Organization.Entity; }

			set
			{
				Organization previousValue = this._Organization.Entity;
				if (((previousValue != value) 
							|| (this._Organization.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Organization.Entity = null;
						previousValue.EnrollmentTransactions.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.EnrollmentTransactions.Add(this);
						
						this._OrganizationId = value.OrganizationId;
						
					}

					else
					{
						
						this._OrganizationId = default(int);
						
					}

					this.SendPropertyChanged("Organization");
				}

			}

		}

		
		[Association(Name="ENROLLMENT_TRANSACTION_PPL_FK", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
		public Person Person
		{
			get { return this._Person.Entity; }

			set
			{
				Person previousValue = this._Person.Entity;
				if (((previousValue != value) 
							|| (this._Person.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Person.Entity = null;
						previousValue.EnrollmentTransactions.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.EnrollmentTransactions.Add(this);
						
						this._PeopleId = value.PeopleId;
						
					}

					else
					{
						
						this._PeopleId = default(int);
						
					}

					this.SendPropertyChanged("Person");
				}

			}

		}

		
		[Association(Name="FK_ENROLLMENT_TRANSACTION_TBL_MemberType", Storage="_MemberType", ThisKey="MemberTypeId", IsForeignKey=true)]
		public MemberType MemberType
		{
			get { return this._MemberType.Entity; }

			set
			{
				MemberType previousValue = this._MemberType.Entity;
				if (((previousValue != value) 
							|| (this._MemberType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._MemberType.Entity = null;
						previousValue.EnrollmentTransactions.Remove(this);
					}

					this._MemberType.Entity = value;
					if (value != null)
					{
						value.EnrollmentTransactions.Add(this);
						
						this._MemberTypeId = value.Id;
						
					}

					else
					{
						
						this._MemberTypeId = default(int);
						
					}

					this.SendPropertyChanged("MemberType");
				}

			}

		}

		
	#endregion
	
		public event PropertyChangingEventHandler PropertyChanging;
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
				this.PropertyChanging(this, emptyChangingEventArgs);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

   		
		private void attach_BadETs(BadET entity)
		{
			this.SendPropertyChanging();
			entity.EnrollmentTransaction = this;
		}

		private void detach_BadETs(BadET entity)
		{
			this.SendPropertyChanging();
			entity.EnrollmentTransaction = null;
		}

		
	}

}

