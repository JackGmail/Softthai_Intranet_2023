// using ST_API.Models.Global;
// using static Extensions.Systems.AllClass;

using ST.INFRA;
using ST.INFRA.Common;
using static Extensions.Systems.AllClass;
using Extensions.Common.STResultAPI;

namespace Backend.Models
{
    #region sun
    public class cReqEmployee : STGrid.PaginationData
    {
        public int? nStatus { get; set; }
        public List<string>? lstSearch { get; set; }
    }
    public class ReqEmpName : ResultAPI
    {
        public List<string>? lstSelectEmployee { get; set; }
        public List<cSelectOption>? lstEmployee { get; set; }
    }

    public class cReturnEmployee : Pagination
    {
        public List<ObjectResultEmployee>? lstDataEmp { get; set; }
    }
    public class ObjectResultEmployee
    {
        public string? sID { get; set; }
        public int? nEmpID { get; set; }
        public string? sNickName { get; set; }
        public string? sFullname { get; set; }
        public string? sWorkStart { get; set; }
        public string? sRetire { get; set; }
        public string? sBDYear { get; set; }
        public bool IsRetire { get; set; }
        public string? sTotalDate { get; set; }
        public string? sBirth { get; set; }
        public string? sEmail { get; set; }
        public string? sTelephone { get; set; }
        public bool? isActive { get; set; }
        public string? sEmpType { get; set; }
        public DateTime? dWorkStart { get; set; }
        public string? sPosition { get; set; }
        public string? sFileLink { get; set; }
    }

    public class cFamily : ResultAPI
    {
        public string sEmployeeID { get; set; } = "";
        public int? nTotalChild { get; set; }
        public int? nTotalSibling { get; set; }
        public int? nChildPosition { get; set; }
        public List<ObjectFamilyData>? lstAllData { get; set; }
    }
    public class ObjectFamilyData
    {
        public string sID { get; set; }
        public int nFamilyPersonID { get; set; }
        public string? sFName { get; set; }
        public string? sSName { get; set; }
        public int? nAge { get; set; }
        public string? sOccupation { get; set; }
        public string? sWorkplace { get; set; }
        public string? sPosition { get; set; }
        public int? nRelationship { get; set; }
        public bool IsDel { get; set; }
    }

    public class cLanguage : ResultAPI
    {
        public string sEmployeeID { get; set; } = "";
        public List<ObjectLanguage>? lstLanguage { get; set; }
    }
    public class ObjectLanguage
    {
        public string sID { get; set; }
        public string? sName { get; set; }
        public List<ObjectSkill_Level>? lstSpeakingSkill_Level { get; set; }
        public List<ObjectSkill_Level>? lstWritingSkill_Level { get; set; }
        public List<ObjectSkill_Level>? lstReadingSkill_Level { get; set; }
        public int? nSpeaking { get; set; }
        public int? nWriting { get; set; }
        public int? nReading { get; set; }
    }
    public class ObjectSkill_Level
    {
        public string? value { get; set; }
        public string? label { get; set; }
    }
    public class ObjectDataLanguage
    {
        public int nLanguage { get; set; }
        public int? nSpeaking { get; set; }
        public int? nWriting { get; set; }
        public int? nReading { get; set; }
    }

    public class cPosition : ResultAPI
    {
        public string sEmployeeID { get; set; } = "";
        public string? sName { get; set; }
        public string? sPosition { get; set; }
        public string? sUserType { get; set; }
        public string? sWorkStart { get; set; }
        public DateTime? dWorkStart { get; set; }
        public string? sPromote { get; set; }
        public DateTime? dPromote { get; set; }
        public string? sLongevity { get; set; }
        public string? sRetire { get; set; }
        public DateTime? dRetire { get; set; }
        public List<ObjectPosition>? lstData { get; set; }
    }
    public class ObjectPosition
    {
        public string sID { get; set; } = "";
        public string? sOriginalPosition { get; set; }
        public string sNewPosition { get; set; } = "";
        public DateTime? dStartDate { get; set; }
        public string? sRemark { get; set; }
        public int? nOrder { get; set; }
        public bool? IsDel { get; set; }
    }
    #endregion

    public class cResultEmployee : ResultAPI
    {
        public List<cSelectOption>? lstPosition { get; set; }
        public List<cSelectOption>? lstMilitaryConditions { get; set; }
        public List<cSelectOption>? lstMilitaryStatus { get; set; }
        public List<cSelectOption>? lstSex { get; set; }
        public List<cSelectOption>? lstReligion { get; set; }
        public List<cSelectOption>? lstNationality { get; set; }
        public List<cSelectOption>? lstEthnicity { get; set; }
        public List<cSelectOption>? lstHousingType { get; set; }
        public List<cSelectOption>? lstProvince { get; set; }
        public List<cSelectDataOption>? lstDistrict { get; set; }
        public List<cSelectDataOption>? lstSubDistrict { get; set; }
        public List<cSelectOption>? lstEducational_Level { get; set; }
        public List<cSelectOption>? lstRelationship { get; set; }
        public List<cSelectOption>? lstYesNo { get; set; }
        public List<cSelectOption>? lstTypeEmployee { get; set; }
        public List<cSelectOption>? lstAddressType { get; set; }
    }

    public class cSelectDataOption
    {
        public string? label { get; set; }
        public string? value { get; set; }
        public int nHeadID { get; set; }
        public int nZip_code { get; set; }
    }

    public class cEducation : ResultAPI
    {
        public string? sID { get; set; }
        public List<cEducationSchool> lstDataSchool { get; set; } = new List<cEducationSchool>();
        public List<cEducationSchool> lstAllDataEducation { get; set; } = new List<cEducationSchool>();
    }

    public class cEducationSchool
    {
        public string? sID { get; set; }
        public int nRow { get; set; }
        public int nEducationID { get; set; }
        public int nEmployeeID { get; set; }
        public int nEducational_Level { get; set; }
        public string? sAcademy { get; set; }
        public string? sMajor { get; set; }
        public int? nEducationStart { get; set; }
        public int? nEducationEnd { get; set; }
        public bool IsDel { get; set; }
    }


    public class cWorkExperien : ResultAPI
    {
        public string? sID { get; set; }
        public int nEmployeeID { get; set; }
        public List<objWorkExperien> lstDataWork { get; set; } = new List<objWorkExperien>();
        public List<objWorkExperien> lstAllDataWorkExperien { get; set; } = new List<objWorkExperien>();
    }


    public class objWorkExperien
    {
        public int nRow { get; set; }
        public string? sID { get; set; }
        public int nWorkExperienceID { get; set; }
        public int nEmployeeID { get; set; }
        public DateTime? dWorkStart { get; set; }
        public DateTime? dWorkEnd { get; set; }
        public string? sPosition { get; set; }
        public string? sJobDescription { get; set; }
        public string? sWorkCompany { get; set; }
        public bool IsDel { get; set; }
    }

    public class clsProfileEmp : ResultAPI
    {


        public List<objEmployeeInfo> listData { get; set; }
    }

    public class objEmployeeInfo
    {

        /// <summary>
        /// รหัสตารางพนักงาน Refer.TB_Employee
        /// </summary>
        public int nEmployeeID { get; set; }
        /// <summary>
        /// </summary>
        public string? sPosition { get; set; }
        /// <summary>
        /// </summary>
        public string? sEmail { get; set; }
        /// <summary>
        /// </summary>
        public string? sNameTH { get; set; }
        /// <summary>
        /// </summary>
        public string? sSurnameTH { get; set; }
        /// <summary>
        /// </summary>
        public string? sTelephone { get; set; }
        /// <summary>
        /// </summary>
        public DateTime dBirth { get; set; }
        /// <summary>
        /// </summary>
        public string sBirth
        {
            get
            {

                return dBirth.ToStringFromDate(ST.INFRA.Enum.DateTimeFormat.ddMMyyyy, ST.INFRA.Enum.CultureName.th_TH);
                //return ((DateTime)dBirth).ToString("dddd, dd MMMM yyyy" , "th-TH");
            }
        }
        public string? sNationality { get; set; }
        /// <summary>
        /// บ้านเลขที่
        /// </summary>
        public string? sPresentAddress { get; set; }

        /// <summary>
        /// หมู่ที่
        /// </summary>
        public string? sMoo { get; set; }

        /// <summary>
        /// ถนน
        /// </summary>
        public string? sRoad { get; set; }

        /// <summary>
        /// ตำบล Refer.TM_
        /// </summary>
        public string? sSubDistrict { get; set; }

        /// <summary>
        /// อำเภอ Refer.TM_
        /// </summary>
        public string? nDistrict { get; set; }

        /// <summary>
        /// จังหวัด Refer.TM_
        /// </summary>
        public string? nProvince { get; set; }

        /// <summary>
        /// รหัสไปรษณีย์ Refer.TM_
        /// </summary>
        public int? nPostcode { get; set; }
        /// <summary>
        /// [ที่อยู่]
        /// </summary>
        public string? sHome
        {
            get
            {
                return sPresentAddress + " " + sMoo + " " + sRoad + " " + sSubDistrict + " " + nDistrict + " " + nProvince + " " + nPostcode;
            }
        }

        /// <summary>
        /// รูปภาพ/parth
        /// </summary>
        public string? sFileLink { get; set; }
        public List<objEmployeeInfoEducation> listEducation { get; set; } = new List<objEmployeeInfoEducation>();
        public List<objEmployeeInfoLanguage> listLanguage { get; set; } = new List<objEmployeeInfoLanguage>();
        public List<objEmployeeInfoWork> listWorkEx { get; set; } = new List<objEmployeeInfoWork>();
        public List<objSpecial>? listSpecial { get; set; }
    }
    public class objEmployeeInfoEducation
    {
        /// <summary>
        /// ระดับการศึกษา Refer.TM_Data
        /// </summary>
        public string sEducational_Level { get; set; }
        /// <summary>
        /// ระดับการศึกษา Refer.TM_Data
        /// </summary>
        public int nEducational_Level { get; set; }

        /// <summary>
        /// สถานบันการศึกษา
        /// </summary>
        public string? sAcademy { get; set; }

        /// <summary>
        /// สาขาวิชา
        /// </summary>
        public string? sMajor { get; set; }

        /// <summary>
        /// ปีที่เริ่มศึกษา
        /// </summary>
        public string? sEducationStart { get; set; }

        /// <summary>
        /// ปีที่จบการศึกษา
        /// </summary>
        public string? sEducationEnd { get; set; }
        public int? nEducationEnd { get; set; }


    }

    public class objEmployeeInfoWork
    {


        /// <summary>
        /// สถานที่ทำงาน
        /// </summary>
        public string? sWorkCompany { get; set; }

        /// <summary>
        /// วันที่เรี่มงาน
        /// </summary>
        public string? sWorkStart { get; set; }

        /// <summary>
        /// วันที่สิ้นสุดการทำงาน
        /// </summary>
        public string? sWorkEnd { get; set; }
        public DateTime? dWorkEnd { get; set; }

        /// <summary>
        /// ตำแหน่งงาน
        /// </summary>
        public string? sPosition { get; set; }

        /// <summary>
        /// ลักษณะงาน
        /// </summary>
        public string? sJobDescription { get; set; }

        /// <summary>
        /// เงินเดือน
        /// </summary>
        public decimal? nSalary { get; set; }

        /// <summary>
        /// เหตุผลที่ออก
        /// </summary>
        public string? sReasonsOfResignation { get; set; }
    }

    public class objEmployeeInfoLanguage
    {


        /// <summary>
        /// ภาษา Refer.TM_Data
        /// </summary>
        public string? sLanguage { get; set; }
        public int? nLanguage { get; set; }

        /// <summary>
        /// ภาษาอื่น ๆ 
        /// </summary>
        public string? sOtherLang { get; set; }

        /// <summary>
        /// ระดับการพูด Refer.TM_Data
        /// </summary>
        public string? sSpeaking { get; set; }

        /// <summary>
        /// ระดับการเขียน Refer.TM_Data
        /// </summary>
        public string? sWriting { get; set; }

        /// <summary>
        /// ระดับการอ่าน Refer.TM_Data
        /// </summary>
        public string? sReading { get; set; }
        public string? sRendar
        {
            get
            {
                return "พูด : " + sSpeaking + " เขียน : " + sWriting + "  อ่าน : " + sReading;
            }
        }

    }

    public class objOption
    {
        public string value { get; set; }
        public string label { get; set; }
    }

    public class cEmployeeForm : ResultAPI
    {
        public string? sEmployeeID { get; set; }
        public string? sEmplyeeCode { get; set; }
        public string sUsername { get; set; } = "";
        public string sPassword { get; set; } = "";
        public string sNameTH { get; set; } = "";
        public string sSurnameTH { get; set; } = "";
        public string? sNickname { get; set; }
        public string sNameEN { get; set; } = "";
        public string sSurnameEN { get; set; } = "";
        public string sIDCard { get; set; } = "";
        public string? sBirthday { get; set; }
        public DateTime? dBirthday { get; set; }
        public int? nHeight { get; set; }
        public decimal? nWeight { get; set; }
        public string? sPositionName { get; set; }
        public string? sTelephone { get; set; }
        public string? sEmail { get; set; }
        public string? sSex { get; set; }
        public string? sEthnicity { get; set; }
        public string? sNationality { get; set; }
        public string? sReligion { get; set; }
        public string? sMilitaryConditions { get; set; }
        public string? sMaritalStatus { get; set; }
        public string? sPresentAddress { get; set; }
        public string? sMoo { get; set; }
        public string? sRoad { get; set; }
        public string? sProvinceID { get; set; }
        public string? sDistrictID { get; set; }
        public string? sSubDistrictID { get; set; }
        public int? nPostcode { get; set; }
        public string? sAdressType { get; set; }
        public bool? IsActive { get; set; }
        public List<ItemFileData>? lstFile { get; set; }
    }

    public class objSpecial
    {
        /// <summary>
        /// ประเภทความสามารถพิเศษ Refer.TM_Data
        /// </summary>
        public string sSpecialAbilityTypeID { get; set; }
        /// <summary>
        /// ลำดับ
        /// </summary>
        public List<objChildSpecial>? listDrescription { get; set; }
    }
    public class objChildSpecial
    {
        public int? nOrder { get; set; }
        public string? sDrescription { get; set; }
    }

    public class cOtherParts : ResultAPI
    {
        public string? sID { get; set; }
        public int nEmployeeID { get; set; }
        public int nOtherPartsID { get; set; }
        public int nAllergyType { get; set; }
        public int nSpecialAbilityTypeID { get; set; }
        public int? nCan { get; set; }
        public List<objPersonContact> lstDataPersonContact { get; set; } = new List<objPersonContact>();
        public List<objAllergy> lstAllDataAllergy { get; set; } = new List<objAllergy>();
        public List<objPersonContact> lstAllDataContact { get; set; } = new List<objPersonContact>();
    }
    public class objPersonContact
    {

        public int nRow { get; set; }
        public string? sID { get; set; }
        public int nPersonContactID { get; set; }
        public int nEmployeeID { get; set; }
        public int nContactType { get; set; }
        public int? nOrder { get; set; }
        public string sName { get; set; } = null!;
        public string? sSurename { get; set; }
        public int? nRelationship { get; set; }
        public string? sAddress { get; set; }
        public string? sTelephone { get; set; }
        public bool IsDel { get; set; }
    }
    public class objAllergy
    {
        public int nRow { get; set; }
        public string? sID { get; set; }
        public int nAllergyID { get; set; }
        public int nEmployeeID { get; set; }
        public int nAllergyType { get; set; }
        public string? nAllergyName { get; set; } = "";
        public string? nDrescription { get; set; }
        public bool IsDel { get; set; }
        public int nOrder { get; set; }
    }

    public class cSpecialAbility : ResultAPI
    {
        public string? sID { get; set; }
        public int? nSpecialAbilityTypeID { get; set; }
        public List<ObjectSpecialAbility>? lstAllData { get; set; } = new List<ObjectSpecialAbility>();
        public List<ObjectSpecialAbility>? lstAllDataSpecialAbility { get; set; } = new List<ObjectSpecialAbility>();

    }
    public class ObjectSpecialAbility
    {
        public string? sID { get; set; }
        public int nSpecialAbilityID { get; set; }
        public int nEmployeeID { get; set; }
        public int nSpecialAbilityTypeID { get; set; }
        public string? sDrescription { get; set; }
        public bool IsDel { get; set; }
        public int? nCan { get; set; }
        public int? nOrder { get; set; }
    }

    #region ตี้
    /// <summary>
    /// 
    /// </summary>
    public class cReturnExport
    /// <summary>
    /// 
    /// </summary>
    {
        public byte[] objFile { get; set; }
        /// <summary>
        /// </summary>
        public string sFileType { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string sFileName { get; set; } = "";
    }
    /// <summary>
    /// </summary>
    public class cReturnImage
    { /// <summary>
      /// </summary>
        public int nHeight { get; set; }
        /// <summary>
        /// </summary>
        public int nWidth { get; set; }
        /// <summary>
        /// </summary>
        public string sBase64 { get; set; }
    }
    /// <summary>
    /// </summary>
    public class objParam
    {
        /// <summary>
        /// </summary>
        public string sID { get; set; } = "";
    }
    #endregion


    public class CEmployeeID : ResultAPI
    {
        public string sEmployeeID { get; set; } = "";
    }
    public class CMenueEmployee : ResultAPI
    {
        public List<lstMenuEmployee> lstMenuEmployees { get; set; } = new List<lstMenuEmployee>();
    }
    public class lstMenuEmployee
    {
        public int nMenuID { get; set; }
        public int? nParentID { get; set; }
        public string? sMenuName { get; set; }
        public bool isFontEnd { get; set; }
        public string? sIcon { get; set; }
        public string? sRoute { get; set; }
        public int? nLevel { get; set; }
        public decimal? nOrder { get; set; }
        public int nMenuType { get; set; }
    }

}

