using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backend.EF.ST_Intranet;

public partial class ST_IntranetEntity : DbContext
{
    public ST_IntranetEntity()
    {
    }

    public ST_IntranetEntity(DbContextOptions<ST_IntranetEntity> options)
        : base(options)
    {
    }

    public virtual DbSet<TB_Banner> TB_Banner { get; set; }

    public virtual DbSet<TB_ContractPoint> TB_ContractPoint { get; set; }

    public virtual DbSet<TB_Customer> TB_Customer { get; set; }

    public virtual DbSet<TB_Employee> TB_Employee { get; set; }

    public virtual DbSet<TB_Employee_Address> TB_Employee_Address { get; set; }

    public virtual DbSet<TB_Employee_Allergy> TB_Employee_Allergy { get; set; }

    public virtual DbSet<TB_Employee_Contact> TB_Employee_Contact { get; set; }

    public virtual DbSet<TB_Employee_Dental_Year> TB_Employee_Dental_Year { get; set; }

    public virtual DbSet<TB_Employee_Education> TB_Employee_Education { get; set; }

    public virtual DbSet<TB_Employee_Family> TB_Employee_Family { get; set; }

    public virtual DbSet<TB_Employee_FamilyPerson> TB_Employee_FamilyPerson { get; set; }

    public virtual DbSet<TB_Employee_Image> TB_Employee_Image { get; set; }

    public virtual DbSet<TB_Employee_Language> TB_Employee_Language { get; set; }

    public virtual DbSet<TB_Employee_LineAccount> TB_Employee_LineAccount { get; set; }

    public virtual DbSet<TB_Employee_LineConfirm> TB_Employee_LineConfirm { get; set; }

    public virtual DbSet<TB_Employee_OtherParts> TB_Employee_OtherParts { get; set; }

    public virtual DbSet<TB_Employee_PersonContact> TB_Employee_PersonContact { get; set; }

    public virtual DbSet<TB_Employee_Position> TB_Employee_Position { get; set; }

    public virtual DbSet<TB_Employee_Position_History> TB_Employee_Position_History { get; set; }

    public virtual DbSet<TB_Employee_Report_To> TB_Employee_Report_To { get; set; }

    public virtual DbSet<TB_Employee_Salary_History> TB_Employee_Salary_History { get; set; }

    public virtual DbSet<TB_Employee_SpecialAbility> TB_Employee_SpecialAbility { get; set; }

    public virtual DbSet<TB_Employee_TimeStemp> TB_Employee_TimeStemp { get; set; }

    public virtual DbSet<TB_Employee_WorkExperience> TB_Employee_WorkExperience { get; set; }

    public virtual DbSet<TB_Floor> TB_Floor { get; set; }

    public virtual DbSet<TB_HolidayDay> TB_HolidayDay { get; set; }

    public virtual DbSet<TB_HolidayYear> TB_HolidayYear { get; set; }

    public virtual DbSet<TB_Image_Leave> TB_Image_Leave { get; set; }

    public virtual DbSet<TB_LeaveChange> TB_LeaveChange { get; set; }

    public virtual DbSet<TB_LeaveProportion> TB_LeaveProportion { get; set; }

    public virtual DbSet<TB_LeaveSetting> TB_LeaveSetting { get; set; }

    public virtual DbSet<TB_LeaveSetting_History> TB_LeaveSetting_History { get; set; }

    public virtual DbSet<TB_LeaveSummary> TB_LeaveSummary { get; set; }

    public virtual DbSet<TB_LeaveType> TB_LeaveType { get; set; }

    public virtual DbSet<TB_LeaveType_History> TB_LeaveType_History { get; set; }

    public virtual DbSet<TB_Leave_File> TB_Leave_File { get; set; }

    public virtual DbSet<TB_LogEmail> TB_LogEmail { get; set; }

    public virtual DbSet<TB_LogLogin> TB_LogLogin { get; set; }

    public virtual DbSet<TB_LogTask> TB_LogTask { get; set; }

    public virtual DbSet<TB_Log_Beacon> TB_Log_Beacon { get; set; }

    public virtual DbSet<TB_Log_WebhookLine> TB_Log_WebhookLine { get; set; }

    public virtual DbSet<TB_MasterProcess> TB_MasterProcess { get; set; }

    public virtual DbSet<TB_MasterProcess_Task> TB_MasterProcess_Task { get; set; }

    public virtual DbSet<TB_Meeting> TB_Meeting { get; set; }

    public virtual DbSet<TB_Meeting_Event> TB_Meeting_Event { get; set; }

    public virtual DbSet<TB_Meeting_Files> TB_Meeting_Files { get; set; }

    public virtual DbSet<TB_Meeting_Flow> TB_Meeting_Flow { get; set; }

    public virtual DbSet<TB_Meeting_Person> TB_Meeting_Person { get; set; }

    public virtual DbSet<TB_Position> TB_Position { get; set; }

    public virtual DbSet<TB_Position_Secondary> TB_Position_Secondary { get; set; }

    public virtual DbSet<TB_Project> TB_Project { get; set; }

    public virtual DbSet<TB_Project_Action> TB_Project_Action { get; set; }

    public virtual DbSet<TB_Project_ContractPoint> TB_Project_ContractPoint { get; set; }

    public virtual DbSet<TB_Project_Person> TB_Project_Person { get; set; }

    public virtual DbSet<TB_Project_Plan> TB_Project_Plan { get; set; }

    public virtual DbSet<TB_Project_PlanGroup> TB_Project_PlanGroup { get; set; }

    public virtual DbSet<TB_Project_Process> TB_Project_Process { get; set; }

    public virtual DbSet<TB_Project_ProcessManday> TB_Project_ProcessManday { get; set; }

    public virtual DbSet<TB_Project_Proposal> TB_Project_Proposal { get; set; }

    public virtual DbSet<TB_Project_UpdateProgress> TB_Project_UpdateProgress { get; set; }

    public virtual DbSet<TB_Request_Leave> TB_Request_Leave { get; set; }

    public virtual DbSet<TB_Request_Leave_History> TB_Request_Leave_History { get; set; }

    public virtual DbSet<TB_Request_OT> TB_Request_OT { get; set; }

    public virtual DbSet<TB_Request_OT_History> TB_Request_OT_History { get; set; }

    public virtual DbSet<TB_Request_OT_Reason> TB_Request_OT_Reason { get; set; }

    public virtual DbSet<TB_Request_OT_Reason_History> TB_Request_OT_Reason_History { get; set; }

    public virtual DbSet<TB_Request_OT_Task> TB_Request_OT_Task { get; set; }

    public virtual DbSet<TB_Request_OT_Task_History> TB_Request_OT_Task_History { get; set; }

    public virtual DbSet<TB_Request_WF_Allowance> TB_Request_WF_Allowance { get; set; }

    public virtual DbSet<TB_Request_WF_Allowance_History> TB_Request_WF_Allowance_History { get; set; }

    public virtual DbSet<TB_Request_WF_Dental> TB_Request_WF_Dental { get; set; }

    public virtual DbSet<TB_Request_WF_Dental_History> TB_Request_WF_Dental_History { get; set; }

    public virtual DbSet<TB_Request_WF_Dental_Type> TB_Request_WF_Dental_Type { get; set; }

    public virtual DbSet<TB_Request_WF_Privatecar> TB_Request_WF_Privatecar { get; set; }

    public virtual DbSet<TB_Request_WF_Privatecar_File> TB_Request_WF_Privatecar_File { get; set; }

    public virtual DbSet<TB_Request_WF_Privatecar_History> TB_Request_WF_Privatecar_History { get; set; }

    public virtual DbSet<TB_Request_WF_PublicTransport> TB_Request_WF_PublicTransport { get; set; }

    public virtual DbSet<TB_Request_WF_PublicTransport_History> TB_Request_WF_PublicTransport_History { get; set; }

    public virtual DbSet<TB_Request_WF_TravelExpenses> TB_Request_WF_TravelExpenses { get; set; }

    public virtual DbSet<TB_Request_WF_TravelExpenses_History> TB_Request_WF_TravelExpenses_History { get; set; }

    public virtual DbSet<TB_Room> TB_Room { get; set; }

    public virtual DbSet<TB_Task> TB_Task { get; set; }

    public virtual DbSet<TB_Team> TB_Team { get; set; }

    public virtual DbSet<TB_UserGroup> TB_UserGroup { get; set; }

    public virtual DbSet<TB_UserGroupPermisson> TB_UserGroupPermisson { get; set; }

    public virtual DbSet<TB_UserMappingGroup> TB_UserMappingGroup { get; set; }

    public virtual DbSet<TB_UserMappingRole> TB_UserMappingRole { get; set; }

    public virtual DbSet<TB_UserPermission> TB_UserPermission { get; set; }

    public virtual DbSet<TB_UserRole> TB_UserRole { get; set; }

    public virtual DbSet<TB_UserRolePermission> TB_UserRolePermission { get; set; }

    public virtual DbSet<TB_WFH> TB_WFH { get; set; }

    public virtual DbSet<TB_WFHFlow> TB_WFHFlow { get; set; }

    public virtual DbSet<TB_WFHFlowHistory> TB_WFHFlowHistory { get; set; }

    public virtual DbSet<TB_WFHTask> TB_WFHTask { get; set; }

    public virtual DbSet<TB_WF_Dental_File> TB_WF_Dental_File { get; set; }

    public virtual DbSet<TM_Config> TM_Config { get; set; }

    public virtual DbSet<TM_ConfigType> TM_ConfigType { get; set; }

    public virtual DbSet<TM_Data> TM_Data { get; set; }

    public virtual DbSet<TM_DataType> TM_DataType { get; set; }

    public virtual DbSet<TM_District> TM_District { get; set; }

    public virtual DbSet<TM_Language> TM_Language { get; set; }

    public virtual DbSet<TM_LineTemplate> TM_LineTemplate { get; set; }

    public virtual DbSet<TM_LoginType> TM_LoginType { get; set; }

    public virtual DbSet<TM_Menu> TM_Menu { get; set; }

    public virtual DbSet<TM_Provinces> TM_Provinces { get; set; }

    public virtual DbSet<TM_RequestType> TM_RequestType { get; set; }

    public virtual DbSet<TM_Status> TM_Status { get; set; }

    public virtual DbSet<TM_Subdistrict> TM_Subdistrict { get; set; }

    public virtual DbSet<TM_Task_Activity> TM_Task_Activity { get; set; }

    public virtual DbSet<TM_Task_Activity_Mapping> TM_Task_Activity_Mapping { get; set; }

    public virtual DbSet<TM_Task_Activity_PositionMapping> TM_Task_Activity_PositionMapping { get; set; }

    public virtual DbSet<TM_Task_Activity_Type> TM_Task_Activity_Type { get; set; }

    public virtual DbSet<TM_Task_Progress> TM_Task_Progress { get; set; }

    public virtual DbSet<TM_UserType> TM_UserType { get; set; }

    public virtual DbSet<TM_WFHFlowProcess> TM_WFHFlowProcess { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=147.50.231.79;database=ST_Intranet;User ID=stuser;Password=cstuser;Persist Security Info=False; MultipleActiveResultSets=true;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Thai_CI_AS");

        modelBuilder.Entity<TB_Banner>(entity =>
        {
            entity.HasKey(e => e.nBannerID);

            entity.Property(e => e.nBannerID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.sBannerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.sEndDate).HasColumnType("datetime");
            entity.Property(e => e.sFileName)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.sNote)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.sPath)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sStartDate).HasColumnType("datetime");
            entity.Property(e => e.sSystemFileName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_ContractPoint>(entity =>
        {
            entity.HasKey(e => e.nContractPointID);

            entity.Property(e => e.nContractPointID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Email");
            entity.Property(e => e.sNameEN)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อภาษาอังกฤษ");
            entity.Property(e => e.sNameTH)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อภาษาไทย");
            entity.Property(e => e.sNickname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("ชื่อเล่น");
            entity.Property(e => e.sSurnameEN)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("นามสกุลภาษาอังกฤษ");
            entity.Property(e => e.sSurnameTH)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("นามสกุลภาษาไทย");
            entity.Property(e => e.sTelephone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("เบอร์ติดต่อ");
        });

        modelBuilder.Entity<TB_Customer>(entity =>
        {
            entity.HasKey(e => e.nCustomerID);

            entity.Property(e => e.nCustomerID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = StandProcess , false = OtherProcess");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sCustomerName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Employee>(entity =>
        {
            entity.HasKey(e => e.nEmployeeID);

            entity.Property(e => e.nEmployeeID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsActive).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.IsLoginChangePassword).HasComment("การเปลี่ยนรหัสผ่าน");
            entity.Property(e => e.dBirth)
                .HasComment("วันเกิด")
                .HasColumnType("date");
            entity.Property(e => e.dChangePasswordDate)
                .HasComment("วันที่เปลี่ยนรหัสผ่าน")
                .HasColumnType("datetime");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dExpirationDate)
                .HasComment("วันที่บัตรประชาชนหมดอายุ")
                .HasColumnType("date");
            entity.Property(e => e.dPromote)
                .HasComment("วันที่ผ่านโปร")
                .HasColumnType("date");
            entity.Property(e => e.dRetire).HasColumnType("date");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dWorkEnd)
                .HasComment("วันที่ลาออก")
                .HasColumnType("date");
            entity.Property(e => e.dWorkStart)
                .HasComment("วันที่เริ่มงาน")
                .HasColumnType("date");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nEmployeeTypeID).HasComment("ประเภทพนักงาน Refer.TM_Data : Type 7");
            entity.Property(e => e.nIDCard)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasComment("หมายเลขบัตรประชาชน");
            entity.Property(e => e.nMaritalStatus).HasComment("สถานภาพรส Refer.TM_Data");
            entity.Property(e => e.nMilitaryConditions).HasComment("ภาวะทางการทหาร Refer.TM_Data");
            entity.Property(e => e.nNationality).HasComment("สัญชาติ  Refer.TM_Data");
            entity.Property(e => e.nRace).HasComment("เชื้อชาติ  Refer.TM_Data");
            entity.Property(e => e.nReligion).HasComment("ศาสนา  Refer.TM_Data");
            entity.Property(e => e.nSalaryID).HasComment("รหัสเงินเดือน Refer.TB_Salary_History");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.nWeight)
                .HasComment("น้ำหนัก")
                .HasColumnType("numeric(5, 2)");
            entity.Property(e => e.sEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Email");
            entity.Property(e => e.sEmplyeeCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("รหัสพนักงาน");
            entity.Property(e => e.sNameEN)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อภาษาอังกฤษ");
            entity.Property(e => e.sNameTH)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อภาษาไทย");
            entity.Property(e => e.sNickname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("ชื่อเล่น");
            entity.Property(e => e.sPassword)
                .HasMaxLength(200)
                .HasComment("รหัสผ่าน");
            entity.Property(e => e.sSex).HasComment("เพศ Refer.TM_Data");
            entity.Property(e => e.sSurnameEN)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("นามสกุลภาษาอังกฤษ");
            entity.Property(e => e.sSurnameTH)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("นามสกุลภาษาไทย");
            entity.Property(e => e.sTelephone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("เบอร์ติดต่อ");
            entity.Property(e => e.sUsername)
                .HasMaxLength(50)
                .HasComment("ชื่อผู้ใช้");
        });

        modelBuilder.Entity<TB_Employee_Address>(entity =>
        {
            entity.HasKey(e => e.nAddressID).HasName("PK_TB_ Address");

            entity.Property(e => e.nAddressID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nAdressType).HasComment("TM_Data.nDatatype_ID = 12");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nDistrictID).HasComment("อำเภอ Refer.TM_District");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nOrder).HasComment("ลำดับ");
            entity.Property(e => e.nPostcode).HasComment("รหัสไปรษณีย์ Refer.TM_SubDistrictID");
            entity.Property(e => e.nProvinceID).HasComment("จังหวัด Refer.TM_Province");
            entity.Property(e => e.nResidenceType).HasComment("TM_Data.nDatatype_ID = 23");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sMoo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("หมู่ที่");
            entity.Property(e => e.sPresentAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("บ้านเลขที่");
            entity.Property(e => e.sRoad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("ถนน");
            entity.Property(e => e.sSubDistrictID)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasComment("ตำบล Refer.TM_SubDistrict");
        });

        modelBuilder.Entity<TB_Employee_Allergy>(entity =>
        {
            entity.HasKey(e => e.nAllergyID);

            entity.Property(e => e.nAllergyID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nAllergyName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ชื่อ");
            entity.Property(e => e.nAllergyType).HasComment("ประเภทการแพ้/โรคประจำตัว");
            entity.Property(e => e.nDrescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("อาการ");
        });

        modelBuilder.Entity<TB_Employee_Contact>(entity =>
        {
            entity.HasKey(e => e.nContactID).HasName("PK_TB_Emp_Contact");

            entity.Property(e => e.nContactID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsActive).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nContactType).HasComment("ประเภทข้อมูลการติดต่อ Ref.TM_Data");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("วันที่ลบรายการ");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nOrder).HasComment("ลำดับ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sContactData)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ข้อมูลติดต่อ เช่น เบอร์มือถือ ไลน์ ฯลฯ");
            entity.Property(e => e.sOther)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("อื่น ๆ ");
        });

        modelBuilder.Entity<TB_Employee_Dental_Year>(entity =>
        {
            entity.HasKey(e => e.nEmpDentalID);

            entity.Property(e => e.nEmpDentalID).ValueGeneratedNever();
            entity.Property(e => e.dCreateDate).HasColumnType("datetime");
            entity.Property(e => e.dUpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TB_Employee_Education>(entity =>
        {
            entity.HasKey(e => e.nEducationID).HasName("PK_TB_Emp_Education");

            entity.Property(e => e.nEducationID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nEducationEnd).HasComment("ปีที่จบการศึกษา");
            entity.Property(e => e.nEducationStart).HasComment("ปีที่เริ่มศึกษา");
            entity.Property(e => e.nEducational_Level).HasComment("ระดับการศึกษา Refer.TM_Data");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sAcademy)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("สถานบันการศึกษา");
            entity.Property(e => e.sMajor)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("สาขาวิชา");
            entity.Property(e => e.sOther)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ระดับการศึกษาอื่น ๆ ");
        });

        modelBuilder.Entity<TB_Employee_Family>(entity =>
        {
            entity.HasKey(e => e.nFamilyID).HasName("PK_TB_Emp_Family");

            entity.Property(e => e.nFamilyID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nBrother).HasComment("จำนวนพี่ชาย น้องชาย");
            entity.Property(e => e.nChildPosition).HasComment("เป็นบุตรคนที่");
            entity.Property(e => e.nChilden).HasComment("จำนวนบุตร");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nSiblings).HasComment("จำนวนพี่น้อง รวมพนักงาน");
            entity.Property(e => e.nSister).HasComment("จำนวนพี่สาว น้องสาว");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
        });

        modelBuilder.Entity<TB_Employee_FamilyPerson>(entity =>
        {
            entity.HasKey(e => e.nFamilyPersonID).HasName("PK_TB_Emp_FamilyPerson");

            entity.Property(e => e.nFamilyPersonID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nAge).HasComment("อายุ");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nOrder).HasComment("ลำดับ");
            entity.Property(e => e.nRelationship).HasComment("ความสัมพันธ์ Refer.TM_Data");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อ");
            entity.Property(e => e.sOccupation)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("อาชีพ");
            entity.Property(e => e.sPosition)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ตำแหน่ง");
            entity.Property(e => e.sSureName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("นามสกุล");
            entity.Property(e => e.sWorkplace)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("สถานที่ทำงาน");
        });

        modelBuilder.Entity<TB_Employee_Image>(entity =>
        {
            entity.HasKey(e => e.nEmployeeImageID);

            entity.Property(e => e.nEmployeeImageID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nImageType).HasComment("รหัสประเภทรูป TM_Data.nDatatype_ID = 24");
            entity.Property(e => e.sFileName)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasComment("ชื่อเก่า");
            entity.Property(e => e.sPath)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sSystemFileName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อใหม่");
        });

        modelBuilder.Entity<TB_Employee_Language>(entity =>
        {
            entity.HasKey(e => e.nLanguageID).HasName("PK_TB_Emp_Language");

            entity.Property(e => e.nLanguageID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nLanguage).HasComment("ภาษา Refer.TM_Data");
            entity.Property(e => e.nOrder).HasComment("ลำดับ");
            entity.Property(e => e.nReading).HasComment("ระดับการอ่าน Refer.TM_Data");
            entity.Property(e => e.nSpeaking).HasComment("ระดับการพูด Refer.TM_Data");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.nWriting).HasComment("ระดับการเขียน Refer.TM_Data");
            entity.Property(e => e.sOther)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("ภาษาอื่น ๆ ");
        });

        modelBuilder.Entity<TB_Employee_LineAccount>(entity =>
        {
            entity.HasKey(e => e.nLineAccountID);

            entity.Property(e => e.sEmail).HasMaxLength(100);
            entity.Property(e => e.sNameAccount).HasMaxLength(1000);
            entity.Property(e => e.sTokenID).HasMaxLength(1000);
        });

        modelBuilder.Entity<TB_Employee_LineConfirm>(entity =>
        {
            entity.HasKey(e => e.nConfirmEmail_ID);

            entity.Property(e => e.nConfirmEmail_ID).ValueGeneratedNever();
            entity.Property(e => e.sEmail).HasMaxLength(100);
        });

        modelBuilder.Entity<TB_Employee_OtherParts>(entity =>
        {
            entity.HasKey(e => e.nOtherPartsID).HasName("PK_TB_Emp_OtherParts");

            entity.Property(e => e.nOtherPartsID).ValueGeneratedNever();
            entity.Property(e => e.IsApplyforWork).HasComment("เคยสมัครงานมาก่อน true = เคย , false = ไม่เคย");
            entity.Property(e => e.IsSeriousDisease).HasComment("โรคร้ายแรง  true = เคย , false = ไม่เคย");
            entity.Property(e => e.dCreate)
                .HasComment("")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.dWhen)
                .HasComment("วันที่ที่เคยสมัครงาน")
                .HasColumnType("date");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.sIntroduce)
                .IsUnicode(false)
                .HasComment("แนะนำตัว");
            entity.Property(e => e.sNameDisease)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ถ้าเคย ระบุชื่อโรค");
            entity.Property(e => e.sOther)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("อื่น ๆ ระบุ");
            entity.Property(e => e.sSourcesofJob)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Employee_PersonContact>(entity =>
        {
            entity.HasKey(e => e.nPersonContactID);

            entity.Property(e => e.nPersonContactID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nContactType).HasComment("ประเภทผู้ตือต่อของพนักงาน Refer.TM_Data");
            entity.Property(e => e.nDistrict).HasComment("อำเภอ");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nPostcode).HasComment("รหัสไปรษณีย์");
            entity.Property(e => e.nProvince).HasComment("จังหวัด");
            entity.Property(e => e.nRelationship).HasComment("ประเภทความสัมพันธ์ Refer.TM_Data");
            entity.Property(e => e.nSubDistrict).HasComment("ตำบล");
            entity.Property(e => e.sAddress)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.sMoo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("หมู่ที่");
            entity.Property(e => e.sName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.sPresentAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("บ้านเลขที่");
            entity.Property(e => e.sRoad)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ถนน");
            entity.Property(e => e.sSurename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("นามสกุลผู้ติดต่อ");
            entity.Property(e => e.sTelephone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("เบอร์มือถือ");
        });

        modelBuilder.Entity<TB_Employee_Position>(entity =>
        {
            entity.HasKey(e => e.nEmpPositionID);

            entity.Property(e => e.nEmpPositionID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dEndDate)
                .HasComment("วันที่สิ้นสุด")
                .HasColumnType("date");
            entity.Property(e => e.dStartDate)
                .HasComment("วันที่เริ่มต้น")
                .HasColumnType("date");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nLevelPosition).HasComment("ระดับตำแหน่ง Refer.TM_Data");
            entity.Property(e => e.nOrder).HasComment("ลำดับ");
            entity.Property(e => e.nPositionID).HasComment("รหัสตำแหน่งงาน Refer.TB_Position");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sRemark)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment(" หมายเหตุ");
        });

        modelBuilder.Entity<TB_Employee_Position_History>(entity =>
        {
            entity.HasKey(e => e.PositionHistoryID).HasName("PK_TB_Employee_Position_His");

            entity.Property(e => e.PositionHistoryID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dEndDate)
                .HasComment("วันที่สิ้นสุด")
                .HasColumnType("date");
            entity.Property(e => e.dStartDate)
                .HasComment("วันที่เริ่มต้น")
                .HasColumnType("date");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nEmpPositionID).HasComment("Refer.TB_Employee_Position");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nLevelPosition).HasComment("ระดับตำแหน่ง Refer.TM_Data");
            entity.Property(e => e.nPositionID).HasComment("รหัสตำแหน่งงาน Refer.TB_Position");
            entity.Property(e => e.nPromotePositionID).HasComment("รหัสตำแหน่งงาน Refer.TB_Position");
            entity.Property(e => e.sRemark)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Employee_Report_To>(entity =>
        {
            entity.HasKey(e => e.nReportTOID);

            entity.Property(e => e.nReportTOID).ValueGeneratedNever();
        });

        modelBuilder.Entity<TB_Employee_Salary_History>(entity =>
        {
            entity.HasKey(e => e.nSalaryID).HasName("PK_TB_Salary_History");

            entity.Property(e => e.nSalaryID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsActive).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dEndDate)
                .HasComment("วันที่สิ้นสุด")
                .HasColumnType("date");
            entity.Property(e => e.dStartDate)
                .HasComment("วันที่เริ่มต้น")
                .HasColumnType("date");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nOrder).HasComment("การจัดเรียง");
            entity.Property(e => e.nSalary)
                .HasComment("เงินเดือน")
                .HasColumnType("numeric(6, 2)");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
        });

        modelBuilder.Entity<TB_Employee_SpecialAbility>(entity =>
        {
            entity.HasKey(e => e.nSpecialAbilityID);

            entity.Property(e => e.nSpecialAbilityID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nCan).HasComment("TM_Data.nData_ID = 93 ได้  TM_Data.nData_ID = 97 ไม่ได้");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nSpecialAbilityTypeID).HasComment("ประเภทความสามารถพิเศษ Refer.TM_Data");
            entity.Property(e => e.sDrescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.sOther)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Employee_TimeStemp>(entity =>
        {
            entity.HasKey(e => e.nTimeStampID).HasName("PK_NewTable");

            entity.Property(e => e.nTimeStampID).ValueGeneratedNever();
            entity.Property(e => e.dTimeDate).HasColumnType("date");
            entity.Property(e => e.dTimeEndDate).HasColumnType("datetime");
            entity.Property(e => e.dTimeStartDate).HasColumnType("datetime");
            entity.Property(e => e.sComment).HasMaxLength(3000);
            entity.Property(e => e.sLocation).HasMaxLength(3000);
        });

        modelBuilder.Entity<TB_Employee_WorkExperience>(entity =>
        {
            entity.HasKey(e => e.nWorkExperienceID).HasName("PK_TB_Emp_WorkExperience");

            entity.Property(e => e.nWorkExperienceID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dWorkEnd)
                .HasComment("วันที่สิ้นสุดการทำงาน")
                .HasColumnType("date");
            entity.Property(e => e.dWorkStart)
                .HasComment("วันที่เรี่มงาน")
                .HasColumnType("date");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nEmployeeID).HasComment(" รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nOrder).HasComment("ลำดับ");
            entity.Property(e => e.nSalary)
                .HasComment("เงินเดือน")
                .HasColumnType("numeric(6, 2)");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sJobDescription)
                .IsUnicode(false)
                .HasComment("ลักษณะงาน");
            entity.Property(e => e.sPosition)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ตำแหน่งงาน");
            entity.Property(e => e.sReasonsOfResignation)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("เหตุผลที่ออก");
            entity.Property(e => e.sWorkCompany)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("สถานที่ทำงาน");
        });

        modelBuilder.Entity<TB_Floor>(entity =>
        {
            entity.HasKey(e => e.nFloorID);

            entity.Property(e => e.nFloorID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("จำนวนคนที่รองรับ");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nOrder).HasComment("ลำดับรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sFloorName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_HolidayDay>(entity =>
        {
            entity.HasKey(e => e.nHolidayDayID);

            entity.Property(e => e.nHolidayDayID).ValueGeneratedNever();
            entity.Property(e => e.IsActivity).HasComment("เป็นวันหยุดกิจกรรม true = ใช่ , false = ไม่ใช่");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDate)
                .HasComment("วันที่")
                .HasColumnType("date");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.sDrescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("คำอธิบาย");
            entity.Property(e => e.sHolidayName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ชื่อวันหยุด");
        });

        modelBuilder.Entity<TB_HolidayYear>(entity =>
        {
            entity.HasKey(e => e.nHolidayYearID);

            entity.Property(e => e.nHolidayYearID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nActivity)
                .HasComment("จำนวนกิจกรรมประจำปี (วัน)")
                .HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nHoliday)
                .HasComment("จำนวนวันหยุดประจำปี (วัน)")
                .HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nYear).HasComment("ปี\r\n");
        });

        modelBuilder.Entity<TB_Image_Leave>(entity =>
        {
            entity.HasKey(e => e.nImageID).HasName("PK_TB_Image");

            entity.Property(e => e.nImageID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nLeaveTypeID).HasComment("ประเภทการลา");
            entity.Property(e => e.sExpireName)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasComment("ชื่อเก่า");
            entity.Property(e => e.sImageParh)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("รูปภาพ/parth");
            entity.Property(e => e.sSystemName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อใหม่");
        });

        modelBuilder.Entity<TB_LeaveChange>(entity =>
        {
            entity.HasKey(e => e.nID);

            entity.Property(e => e.nID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.dWorkNow).HasColumnType("datetime");
            entity.Property(e => e.dWorkStart).HasColumnType("datetime");
            entity.Property(e => e.nIntoMoney).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nLeaveRemain).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nQuantity).HasColumnType("decimal(3, 1)");
        });

        modelBuilder.Entity<TB_LeaveProportion>(entity =>
        {
            entity.HasKey(e => e.LeaveProportionID).HasName("PK_TB_LeaveProportion_1");

            entity.Property(e => e.LeaveProportionID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nApr).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nAug).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nDec).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nEmployeeTypeID).HasComment("ประเภทพนักงาน");
            entity.Property(e => e.nFeb).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nJan).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nJul).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nJun).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nLeaveTypeID).HasComment("Refer. TB_LeaveType");
            entity.Property(e => e.nMar).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nMay).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nNov).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nOct).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nSep).HasColumnType("decimal(2, 1)");
        });

        modelBuilder.Entity<TB_LeaveSetting>(entity =>
        {
            entity.HasKey(e => e.LeaveSettingID);

            entity.Property(e => e.LeaveSettingID).ValueGeneratedNever();
            entity.Property(e => e.IsProportion).HasComment("ต้องคิดสัดส่วนหรือไม่ true = ต้องคิด , false = ไม่ต้อง");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nEmployeeTypeID).HasComment("TM_Data.nDatatype_ID = 7");
            entity.Property(e => e.nLeaveRights)
                .HasComment("สิทธิ์การลา(วัน)")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nLeaveTypeID).HasComment("รหัสประเภทการลา Refer.TB_LeaveType");
            entity.Property(e => e.nWorkingAgeEnd).HasComment("อายุงานสิ้นสุด (เดือน)");
            entity.Property(e => e.nWorkingAgeStart).HasComment("อายุงานเเริ่มต้น (เดือน)");
        });

        modelBuilder.Entity<TB_LeaveSetting_History>(entity =>
        {
            entity.HasKey(e => e.nLeaveSettingHisID).HasName("PK_Table_1TB_LeaveSetting_History");

            entity.Property(e => e.nLeaveSettingHisID).ValueGeneratedNever();
            entity.Property(e => e.IsProportion).HasComment("ต้องคิดสัดส่วนหรือไม่ true = ต้องคิด , false = ไม่ต้อง");
            entity.Property(e => e.LeaveSettingID).HasComment("รหัสการตั้งค่า Refer.TB_LeaveSetting");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nEmployeeTypeID).HasComment("TM_Data.nDatatype_ID = 7");
            entity.Property(e => e.nLeaveRights)
                .HasComment("สิทธิ์การลา(วัน)")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nLeaveTypeID).HasComment("ประเภทการลา Refer.TB_LeaveType");
            entity.Property(e => e.nWorkingAgeEnd).HasComment("อายุงานสิ้นสุด (เดือน)");
            entity.Property(e => e.nWorkingAgeStart).HasComment("อายุงานเเริ่มต้น (เดือน)");
        });

        modelBuilder.Entity<TB_LeaveSummary>(entity =>
        {
            entity.HasKey(e => e.nLeaveSummaryID).HasName("PK_TB_InitialLeave");

            entity.Property(e => e.nLeaveSummaryID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตารางพนักงาน Refer.TB_Employee");
            entity.Property(e => e.nIntoMoney)
                .HasComment("วันลาที่เปลี่ยนเป็นเงิน")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nLeaveRemain).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nLeaveTypeID).HasComment("รหัสประเภทการลา Refer.TB_LeaveType");
            entity.Property(e => e.nLeaveUse)
                .HasComment("จำนวนวันลาที่ใช้ไปแล้ว")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nQuantity)
                .HasComment("จำนวนวันลาเริ่มต้นของปี")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nTransferred)
                .HasComment("ยกยอดจากปีที่แล้ว(กรณีประเภทการลาที่มีสมทบ)")
                .HasColumnType("decimal(3, 1)");
        });

        modelBuilder.Entity<TB_LeaveType>(entity =>
        {
            entity.HasKey(e => e.nLeaveTypeID);

            entity.Property(e => e.nLeaveTypeID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.IsChangeIntoMoney).HasComment("เปลี่ยนวันหยุดเป็นเงินได้หรือไม่ true = ได้, false = ไม่ได้");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.LeaveTypeName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ชื่อประเภทการลา");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nAdvanceLeave)
                .HasComment("กำหนดลาล่วงหน้า (วัน)")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nAssociate)
                .HasComment("สมทบได้ไม่เกิน")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nMaximum)
                .HasComment("จำนวนวันที่สามารถลาติดต่อกันได้สูงสุด (วัน)")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nOrder).HasComment("ลำดับ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sCondition)
                .IsUnicode(false)
                .HasComment("เงื่อนไขการลา");
            entity.Property(e => e.sLeaveTypeCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("รหัสอ้างอิง");
        });

        modelBuilder.Entity<TB_LeaveType_History>(entity =>
        {
            entity.HasKey(e => e.nLeaveTypeHisID);

            entity.Property(e => e.nLeaveTypeHisID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.LeaveTypeName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ชื่อประเภทการลา");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.isChangeIntoMoney).HasComment("เปลี่ยนวันหยุดเป็นเงินได้หรือไม่ true = ได้, false = ไม่ได้");
            entity.Property(e => e.nAdvanceLeave)
                .HasComment("กำหนดลาล่วงหน้า (วัน)")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nAssociate)
                .HasComment("สมทบได้ไม่เกิน")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nMaximum)
                .HasComment("จำนวนวันที่สามารถลาติดต่อกันได้สูงสุด (วัน)")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nOrder).HasComment("ลำดับ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sCondition)
                .IsUnicode(false)
                .HasComment("เงื่อนไขการลา");
            entity.Property(e => e.sLeaveTypeCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("รหัสอ้างอิง");
        });

        modelBuilder.Entity<TB_Leave_File>(entity =>
        {
            entity.HasKey(e => e.nLeaveFileID);

            entity.Property(e => e.nLeaveFileID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nRequestLeaveID).HasComment("รหัสใบคำขอ");
            entity.Property(e => e.sFilename)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.sPath)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.sSystemFilename)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_LogEmail>(entity =>
        {
            entity.HasKey(e => e.nEmailID).HasName("PK_TM_LogEmail");

            entity.Property(e => e.nEmailID).HasComment("Email ID");
            entity.Property(e => e.IsSuccess).HasComment("1 = Success, 0 = Error");
            entity.Property(e => e.dSend)
                .HasComment("Send Date")
                .HasColumnType("datetime");
            entity.Property(e => e.sBcc)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Bcc");
            entity.Property(e => e.sCc)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Cc");
            entity.Property(e => e.sFrom)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("From");
            entity.Property(e => e.sMessage)
                .IsUnicode(false)
                .HasComment("Message");
            entity.Property(e => e.sMessage_Error)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("Message Error");
            entity.Property(e => e.sSubject)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("Subject");
            entity.Property(e => e.sTo)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("To");
        });

        modelBuilder.Entity<TB_LogLogin>(entity =>
        {
            entity.HasKey(e => e.nLogID);

            entity.Property(e => e.nLogID)
                .ValueGeneratedNever()
                .HasComment("Log ID");
            entity.Property(e => e.dLoginDate)
                .HasComment("Login Date")
                .HasColumnType("datetime");
            entity.Property(e => e.dLogoutDate)
                .HasComment("Logout Date")
                .HasColumnType("datetime");
            entity.Property(e => e.nStatus).HasComment("Status: 1=success, 0=err");
            entity.Property(e => e.sDeviceName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComment("Device Name");
            entity.Property(e => e.sErrorMsg)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Erroe Message");
            entity.Property(e => e.sIPAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("IP Address");
            entity.Property(e => e.sUsername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Username");
        });

        modelBuilder.Entity<TB_LogTask>(entity =>
        {
            entity.HasKey(e => e.nLogID).HasName("PK_T_LogTask");

            entity.Property(e => e.nLogID).HasComment("Log ID");
            entity.Property(e => e.dEndDate)
                .HasComment("End Date")
                .HasColumnType("datetime");
            entity.Property(e => e.dStartDate)
                .HasComment("Start Date")
                .HasColumnType("datetime");
            entity.Property(e => e.sDescription)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Description");
            entity.Property(e => e.sErrorMsg)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("Erroe Message");
            entity.Property(e => e.sStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Status");
            entity.Property(e => e.sTaskName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Task Name");
        });

        modelBuilder.Entity<TB_Log_Beacon>(entity =>
        {
            entity.HasKey(e => e.nLogID);

            entity.Property(e => e.dSend).HasColumnType("datetime");
            entity.Property(e => e.dTimeStamp).HasColumnType("datetime");
            entity.Property(e => e.sReplyToken)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.sUserId)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Log_WebhookLine>(entity =>
        {
            entity.HasKey(e => e.nLogID);

            entity.Property(e => e.dSend).HasColumnType("datetime");
        });

        modelBuilder.Entity<TB_MasterProcess>(entity =>
        {
            entity.HasKey(e => e.nMasterProcessID).HasName("PK_TM_MasterProcess");

            entity.Property(e => e.nMasterProcessID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = StandProcess , false = OtherProcess");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.IsStandProcess).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nOrder).HasComment("ลำดับรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sMasterProcessName)
                .HasMaxLength(220)
                .IsUnicode(false);
            entity.Property(e => e.sNote)
                .HasMaxLength(2000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_MasterProcess_Task>(entity =>
        {
            entity.HasKey(e => e.nMasterProcessTaskID);

            entity.Property(e => e.nMasterProcessTaskID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.sMasterProcessTaskName)
                .HasMaxLength(220)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Meeting>(entity =>
        {
            entity.HasKey(e => e.nMeetingID).HasName("PK_TB_Booking_Meeting");

            entity.Property(e => e.nMeetingID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("จำนวนคนที่รองรับ");
            entity.Property(e => e.IsAllDay).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.IsOther).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.IsOtherProcess).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dEnd).HasColumnType("datetime");
            entity.Property(e => e.dStart).HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sOther)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.sOtherProcess)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.sRemark)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.sTitle)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Meeting_Event>(entity =>
        {
            entity.HasKey(e => e.nEventID);

            entity.Property(e => e.nEventID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dEventEnd).HasColumnType("datetime");
            entity.Property(e => e.dEventStart).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TB_Meeting_Files>(entity =>
        {
            entity.HasKey(e => e.nFileID);

            entity.Property(e => e.nFileID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sFilename)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.sPath)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.sSystemFilename)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Meeting_Flow>(entity =>
        {
            entity.HasKey(e => e.nMeetingFlowID).HasName("PK_TB_MeetingFlow");

            entity.Property(e => e.nMeetingFlowID).ValueGeneratedNever();
            entity.Property(e => e.dAction).HasColumnType("datetime");
            entity.Property(e => e.sRemark)
                .HasMaxLength(3000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Meeting_Person>(entity =>
        {
            entity.HasKey(e => e.nPersonMeetingID);

            entity.Property(e => e.nPersonMeetingID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nMeetingID).HasComment("รหัส TB_Meeting");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
        });

        modelBuilder.Entity<TB_Position>(entity =>
        {
            entity.HasKey(e => e.nPositionID);

            entity.Property(e => e.nPositionID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nOrder).HasComment("ลำดับรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sPositionAbbr)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.sPositionName)
                .HasMaxLength(75)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Position_Secondary>(entity =>
        {
            entity.HasKey(e => e.nPositionSecondaryID);

            entity.Property(e => e.nPositionSecondaryID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nOrder).HasComment("ลำดับรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sPositionSecondaryName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Project>(entity =>
        {
            entity.HasKey(e => e.nProjectID);

            entity.Property(e => e.nProjectID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nProcessActual).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.nProcessPlan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sIntroduce)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.sProjectAbbr)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.sProjectCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.sProjectName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Project_Action>(entity =>
        {
            entity.HasKey(e => e.nActionID).HasName("PK_TB_PROJECT_ACTION");

            entity.Property(e => e.nActionID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nManhour).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<TB_Project_ContractPoint>(entity =>
        {
            entity.HasKey(e => e.nProjectContractPointID).HasName("PK_TB_PROJECT_CONTRACTPOINT");

            entity.Property(e => e.nProjectContractPointID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TB_Project_Person>(entity =>
        {
            entity.HasKey(e => e.nProjectPersonID);

            entity.Property(e => e.nProjectPersonID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสตาราง");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
        });

        modelBuilder.Entity<TB_Project_Plan>(entity =>
        {
            entity.HasKey(e => e.nPlanID).HasName("PK_TB_PROJECT_PLAN");

            entity.Property(e => e.nPlanID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dPlanEnd).HasColumnType("datetime");
            entity.Property(e => e.dPlanStart).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nActualProgress).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nDevelopManhour).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nDevelopProgress).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nPlanManhour).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nPlanProgress).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nPlanTypeID).HasDefaultValueSql("((111))");
            entity.Property(e => e.nProgressTypeID).HasDefaultValueSql("((140))");
            entity.Property(e => e.nTesterManhour).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nTesterProgress).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nWeight).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nWeightPlan).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.sChildID)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.sDetail)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.sParentID)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Project_PlanGroup>(entity =>
        {
            entity.HasKey(e => new { e.nPlanGroupID, e.nProjectID, e.nPlanID }).HasName("PK_TB_PROJECT_PLANGROUP");

            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TB_Project_Process>(entity =>
        {
            entity.HasKey(e => e.nProcessID);

            entity.Property(e => e.nProcessID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = ใช้งาน , false = ไม่ใช้งาน");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.IsManhour).HasComment("Require Manhour");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nManhour).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
        });

        modelBuilder.Entity<TB_Project_ProcessManday>(entity =>
        {
            entity.HasKey(e => e.nMandayID).HasName("PK_TB_PROJECT_PROCESSMANDAY");

            entity.Property(e => e.nMandayID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nManday).HasColumnType("decimal(5, 1)");

            entity.HasOne(d => d.nProcess).WithMany(p => p.TB_Project_ProcessManday)
                .HasForeignKey(d => d.nProcessID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_PROJE_REFERENCE_TB_PROJE");
        });

        modelBuilder.Entity<TB_Project_Proposal>(entity =>
        {
            entity.HasKey(e => e.nProposalID).HasName("PK_TB_PROJECT_PROPOSAL");

            entity.Property(e => e.nProposalID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.sCode)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.sDetail)
                .HasMaxLength(2000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Project_UpdateProgress>(entity =>
        {
            entity.HasKey(e => e.nTaskID).HasName("PK_TB_PROJECT_UPDATEPROGRESS");

            entity.Property(e => e.nTaskID).ValueGeneratedNever();
            entity.Property(e => e.dAction).HasColumnType("datetime");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nManhour).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nProgress).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.sDelayDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.sDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_Leave>(entity =>
        {
            entity.HasKey(e => e.nRequestLeaveID).HasName("PK_Table_1TB_Request_Leave");

            entity.Property(e => e.nRequestLeaveID)
                .ValueGeneratedNever()
                .HasComment("รหัสใบคำขอ");
            entity.Property(e => e.IsEmergency).HasComment("ลาแบบฉุกเฉิน true = ใช่ , false = ไม่ใช่");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dEndDateTime)
                .HasComment("วันเวลาสิ้นสุด")
                .HasColumnType("datetime");
            entity.Property(e => e.dStartDateTime)
                .HasComment("วันเวลาเริ่มต้น")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสพนักงาน");
            entity.Property(e => e.nLeaveTypeID).HasComment("ประเภทการลา");
            entity.Property(e => e.nLeaveUse)
                .HasComment("จำนวนวันลาที่ใช้ไปแล้ว")
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.nRequestTypeID).HasComment("TM_RequestType .nRequestTypeID = 1");
            entity.Property(e => e.nStatusID).HasComment("สถานะใบคำร้อง");
            entity.Property(e => e.sReason)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("เหตุผลการลา");
        });

        modelBuilder.Entity<TB_Request_Leave_History>(entity =>
        {
            entity.HasKey(e => e.nRequestLeaveHisID);

            entity.Property(e => e.nRequestLeaveHisID).ValueGeneratedNever();
            entity.Property(e => e.IsEmergency).HasComment("ลาแบบฉุกเฉิน true = ใช่ , false = ไม่ใช่");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dEndDateTime)
                .HasComment("วันเวลาสิ้นสุด")
                .HasColumnType("datetime");
            entity.Property(e => e.dStartDateTime)
                .HasComment("วันเวลาเริ่มต้น")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสพนักงาน");
            entity.Property(e => e.nLeaveTypeID).HasComment("ประเภทการลา");
            entity.Property(e => e.nStatusID).HasComment("สถานะใบคำร้อง");
            entity.Property(e => e.nSumLeave).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.sComment)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.sReason)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("เหตุผลการลา");
        });

        modelBuilder.Entity<TB_Request_OT>(entity =>
        {
            entity.HasKey(e => e.nRequestOTID);

            entity.Property(e => e.nRequestOTID).ValueGeneratedNever();
            entity.Property(e => e.IsHoliday).HasComment("0 = Normal, 1 = Holiday");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dEndActionDateTime).HasColumnType("datetime");
            entity.Property(e => e.dPlanDateTime).HasColumnType("datetime");
            entity.Property(e => e.dStartActionDateTime).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nActionHour).HasComment("0 = Request, 1 = Result");
            entity.Property(e => e.nEstimateHour).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.nRequestTypeID).HasComment("รหัสประเภทใบคำขอ (TM_RequestType)");
            entity.Property(e => e.sNoteApprover)
                .HasMaxLength(3000)
                .IsUnicode(false);
            entity.Property(e => e.sTopic)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_OT_History>(entity =>
        {
            entity.HasKey(e => e.nRequestOTHisID);

            entity.Property(e => e.nRequestOTHisID).ValueGeneratedNever();
            entity.Property(e => e.IsHoliday).HasComment("0 = Normal, 1 = Holiday");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dEndActionDateTime).HasColumnType("datetime");
            entity.Property(e => e.dPlanDateTime).HasColumnType("datetime");
            entity.Property(e => e.dStartActionDateTime).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nActionHour).HasComment("0 = Request, 1 = Result");
            entity.Property(e => e.nEstimateHour).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.nRequestTypeID).HasComment("รหัสประเภทใบคำขอ (TM_RequestType)");
            entity.Property(e => e.sNoteApprover)
                .HasMaxLength(3000)
                .IsUnicode(false);
            entity.Property(e => e.sTopic)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_OT_Reason>(entity =>
        {
            entity.HasKey(e => e.nRequestOTReasonID);

            entity.Property(e => e.nRequestOTReasonID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sOther)
                .HasMaxLength(300)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_OT_Reason_History>(entity =>
        {
            entity.HasKey(e => e.nRequestOTReasonHisID);

            entity.Property(e => e.nRequestOTReasonHisID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sOther)
                .HasMaxLength(300)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_OT_Task>(entity =>
        {
            entity.HasKey(e => e.nRequestTaskID);

            entity.Property(e => e.nRequestTaskID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nOTHour).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sDescription).HasMaxLength(3000);
            entity.Property(e => e.sReason).HasMaxLength(3000);
        });

        modelBuilder.Entity<TB_Request_OT_Task_History>(entity =>
        {
            entity.HasKey(e => e.nRequestTaskHisID);

            entity.Property(e => e.nRequestTaskHisID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nOTHour).HasColumnType("decimal(18, 1)");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sDescription).HasMaxLength(3000);
            entity.Property(e => e.sReason).HasMaxLength(3000);
        });

        modelBuilder.Entity<TB_Request_WF_Allowance>(entity =>
        {
            entity.HasKey(e => e.nRequestAllowanceID).HasName("PK_TB_Request_Allowance");

            entity.Property(e => e.nRequestAllowanceID)
                .ValueGeneratedNever()
                .HasComment("รหัสใบคำขอเบี้ยเลี้ยง");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dEndDate_EndTime).HasColumnType("datetime");
            entity.Property(e => e.dEndDate_StartTime).HasColumnType("datetime");
            entity.Property(e => e.dStartDate_EndTime).HasColumnType("datetime");
            entity.Property(e => e.dStartDate_StartTime).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nAllowanceTypeID).HasComment("รหัสประเภทเบี้ยเลี้ยง ( แบบค้างคืน=TM_Data.nData_ID=122/ แบบไปกลับ =TM_Data.nData_ID=123)");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสพนักงาน");
            entity.Property(e => e.nProjectID).HasComment("รหัสโครงการ");
            entity.Property(e => e.nRequestTypeID).HasComment("รหัสประเภทใบคำขอ (TM_RequestType)");
            entity.Property(e => e.nSumDay)
                .HasComment("รวมวันที่เบิกเบี้ยเลี้ยง")
                .HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nSumMoney).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.sComment)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("คอมเม้นจากผู้อนุมัติ");
            entity.Property(e => e.sDescription)
                .HasMaxLength(2000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_WF_Allowance_History>(entity =>
        {
            entity.HasKey(e => e.nRequestAllowanceID_His).HasName("PK_TB_Request_Allowance_History");

            entity.Property(e => e.nRequestAllowanceID_His).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dEndDate_EndTime).HasColumnType("datetime");
            entity.Property(e => e.dEndDate_StartTime).HasColumnType("datetime");
            entity.Property(e => e.dStartDate_EndTime).HasColumnType("datetime");
            entity.Property(e => e.dStartDate_StartTime).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nAllowanceTypeID).HasComment("รหัสประเภทเบี้ยเลี้ยง ( แบบค้างคืน=TM_Data.nData_ID=122/ แบบไปกลับ =TM_Data.nData_ID=123)");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสพนักงาน");
            entity.Property(e => e.nProjectID).HasComment("รหัสโครงการ");
            entity.Property(e => e.nRequestAllowanceID).HasComment("รหัสใบคำขอเบี้ยเลี้ยง");
            entity.Property(e => e.nRequestTypeID).HasComment("รหัสประเภทใบคำขอ (TM_RequestType)");
            entity.Property(e => e.nSumDay)
                .HasComment("รวมวันที่เบิกเบี้ยเลี้ยง")
                .HasColumnType("decimal(2, 1)");
            entity.Property(e => e.nSumMoney).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.sComment)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasComment("คอมเม้นจากผู้อนุมัติ");
            entity.Property(e => e.sDescription)
                .HasMaxLength(2000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_WF_Dental>(entity =>
        {
            entity.HasKey(e => e.nRequestDentalID).HasName("PK_TB_WF_Request_Dental");

            entity.Property(e => e.nRequestDentalID)
                .ValueGeneratedNever()
                .HasComment("รหัสใบคำขอเบิกค่าทันตกรรม");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDate)
                .HasComment("วันที่รับบริการทันตกรรม")
                .HasColumnType("date");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nAmountWithdrawn)
                .HasComment("จำนวนเงินที่ขอเบิก")
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nConditionAmount)
                .HasComment("จำนวนเงินที่เบิกได้ตามเงื่อนไขบริษัท")
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสพนักงาน");
            entity.Property(e => e.nMoney)
                .HasComment("จำนวนเงินทั้งสิ้น")
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nRemain)
                .HasComment("ยอดคงเหลือ")
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nRequestTypeID).HasComment("รหัสประเภทใบคำขอ (TM_RequestType)");
            entity.Property(e => e.sComent)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.sMedicalFacility)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("ชื่อสถานพยาาล");
        });

        modelBuilder.Entity<TB_Request_WF_Dental_History>(entity =>
        {
            entity.HasKey(e => e.nRequestDentalID_His).HasName("PK_TB_WF_Request_Dental_History");

            entity.Property(e => e.nRequestDentalID_His).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDate)
                .HasComment("วันที่รับบริการทันตกรรม")
                .HasColumnType("date");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nAmountWithdrawn)
                .HasComment("จำนวนเงินที่ขอเบิก")
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nConditionAmount)
                .HasComment("จำนวนเงินที่เบิกได้ตามเงื่อนไขบริษัท")
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nDentalTypeID).HasComment("รหัสประเภททันตกรรม (TM_Data.nDataTypeID = 36)");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสพนักงาน");
            entity.Property(e => e.nMoney)
                .HasComment("จำนวนเงินทั้งสิ้น")
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nRemain)
                .HasComment("ยอดคงเหลือ")
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nRequestDentalID).HasComment("รหัสใบคำขอเบิกค่าทันตกรรม");
            entity.Property(e => e.nRequestTypeID).HasComment("รหัสประเภทใบคำขอ (TM_RequestType)");
            entity.Property(e => e.sComent)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.sMedicalFacility)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("ชื่อสถานพยาาล");
            entity.Property(e => e.sOther)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("ทันตกรรมอื่น ๆ ");
        });

        modelBuilder.Entity<TB_Request_WF_Dental_Type>(entity =>
        {
            entity.HasKey(e => e.nRequestDentalTypeID);

            entity.Property(e => e.nRequestDentalTypeID).ValueGeneratedNever();
            entity.Property(e => e.nQuantity).HasComment("จำนวนซี่ฟัน (กรณีเป็นอุดฟัน กับภอนฟัน จะมีให้กรอกจำนวนของซี่ฟันด้วย)");
            entity.Property(e => e.sOther)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_WF_Privatecar>(entity =>
        {
            entity.HasKey(e => e.nRequestPrivatecarID).HasName("PK_TB_Request_Privatecar");

            entity.Property(e => e.nRequestPrivatecarID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDate).HasColumnType("date");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nDestinationDepartture).HasComment("ปลายทาง ไป (TM_Data.nData_ID = 35)");
            entity.Property(e => e.nDestinationReturn).HasComment("ปลายทาง กลับ (TM_Data.nData_ID = 35)");
            entity.Property(e => e.nDistance)
                .HasComment("ระยะทาง")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nMoney).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nOriginDepartture).HasComment("ต้นทาง ไป (TM_Data.nData_ID = 35)");
            entity.Property(e => e.nOriginReturn).HasComment("ต้นทาง กลับ (TM_Data.nData_ID = 35)");
            entity.Property(e => e.nRequestTravelExpensesID).HasComment("รหัสใบคำขอการเบิกค่าเดินทาง (TB_Request_TravelExpenses)");
            entity.Property(e => e.sOther1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther4)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_WF_Privatecar_File>(entity =>
        {
            entity.HasKey(e => e.nRequestFileID);

            entity.Property(e => e.nRequestFileID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nRequestPrivatecarID).HasComment("รหัสใบคำขอ");
            entity.Property(e => e.sFilename)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.sPath)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.sSystemFilename)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_WF_Privatecar_History>(entity =>
        {
            entity.HasKey(e => e.nRequestPrivatecarID_His).HasName("PK_TB_Request_Privatecar_History");

            entity.Property(e => e.nRequestPrivatecarID_His).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDate).HasColumnType("date");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nDestinationDepartture).HasComment("ปลายทาง ไป (TM_Data.nData_ID = 35)");
            entity.Property(e => e.nDestinationReturn).HasComment("ปลายทาง กลับ (TM_Data.nData_ID = 35)");
            entity.Property(e => e.nDistance)
                .HasComment("ระยะทาง")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nMoney).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.nOriginDepartture).HasComment("ต้นทาง ไป (TM_Data.nData_ID = 35)");
            entity.Property(e => e.nOriginReturn).HasComment("ต้นทาง กลับ (TM_Data.nData_ID = 35)");
            entity.Property(e => e.nRequestTravelExpensesID).HasComment("รหัสใบคำขอการเบิกค่าเดินทาง (TB_Request_TravelExpenses)");
            entity.Property(e => e.sOther1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther4)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_WF_PublicTransport>(entity =>
        {
            entity.HasKey(e => e.nRequestPublicTransportID).HasName("PK_TB_Request_PublicTransport");

            entity.Property(e => e.nRequestPublicTransportID)
                .ValueGeneratedNever()
                .HasComment("รหัสคำขอเบิกค่าเดินทางรถสาธารณะ");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDate)
                .HasComment("วันที่เดินทาง")
                .HasColumnType("date");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nDestinationDepartture).HasComment("ปลายทาง ไป (TM_Data.nDataTypeID = 35)");
            entity.Property(e => e.nDestinationReturn).HasComment("ปลายทาง กลับ (TM_Data.nDataTypeID = 35)");
            entity.Property(e => e.nMoney)
                .HasComment("จำนวนเงิน")
                .HasColumnType("decimal(8, 2)");
            entity.Property(e => e.nOriginDepartture).HasComment("ต้นทาง ไป (TM_Data.nDataTypeID = 35)");
            entity.Property(e => e.nOriginReturn).HasComment("ต้นทาง กลับ (TM_Data.nDataTypeID = 35)");
            entity.Property(e => e.nProjectID).HasComment("รหัสโครงการ");
            entity.Property(e => e.nRequestTravelExpensesID).HasComment("รหัสใบคำขอการเบิกค่าเดินทาง (TB_Request_TravelExpenses)");
            entity.Property(e => e.nVehicleType).HasComment("รหัสประเภทยานพาหนะ  (TM_Data.nDataTypeID = 34)");
            entity.Property(e => e.sOther1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther4)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOtherVehicle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("ชื่อยานพาหนะอื่น ๆ");
        });

        modelBuilder.Entity<TB_Request_WF_PublicTransport_History>(entity =>
        {
            entity.HasKey(e => e.nRequestPublicTransportID_His).HasName("PK_TB_Request_PublicTransport_History");

            entity.Property(e => e.nRequestPublicTransportID_His).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDate)
                .HasComment("วันที่เดินทาง")
                .HasColumnType("date");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nDestinationDepartture).HasComment("ปลายทาง ไป (TM_Data.nDataTypeID = 35)");
            entity.Property(e => e.nDestinationReturn).HasComment("ปลายทาง กลับ (TM_Data.nDataTypeID = 35)");
            entity.Property(e => e.nMoney)
                .HasComment("จำนวนเงิน")
                .HasColumnType("decimal(8, 2)");
            entity.Property(e => e.nOriginDepartture).HasComment("ต้นทาง ไป (TM_Data.nDataTypeID = 35)");
            entity.Property(e => e.nOriginReturn).HasComment("ต้นทาง กลับ (TM_Data.nDataTypeID = 35)");
            entity.Property(e => e.nProjectID).HasComment("รหัสโครงการ");
            entity.Property(e => e.nRequestPublicTransportID).HasComment("รหัสคำขอเบิกค่าเดินทางรถสาธารณะ");
            entity.Property(e => e.nRequestTravelExpensesID).HasComment("รหัสใบคำขอการเบิกค่าเดินทาง (TB_Request_TravelExpenses)");
            entity.Property(e => e.nVehicleType).HasComment("รหัสประเภทยานพาหนะ  (TM_Data.nDataTypeID = 34)");
            entity.Property(e => e.sOther1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOther4)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sOtherVehicle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("ชื่อยานพาหนะอื่น ๆ");
        });

        modelBuilder.Entity<TB_Request_WF_TravelExpenses>(entity =>
        {
            entity.HasKey(e => e.nRequestTravelExpensesID).HasName("PK_TB_Request_TravelExpenses");

            entity.Property(e => e.nRequestTravelExpensesID)
                .ValueGeneratedNever()
                .HasComment("รหัสใบคำขอการเบิกค่าเดินทาง");
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dMonthRequest).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสพนักงาน");
            entity.Property(e => e.nRequestTypeID).HasComment("รหัสประเภทใบคำขอ (TM_RequestType)");
            entity.Property(e => e.nTotalAmount).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.sComment)
                .HasMaxLength(2000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Request_WF_TravelExpenses_History>(entity =>
        {
            entity.HasKey(e => e.nRequestTravelExpensesID_His).HasName("PK_TB_Request_TravelExpenses_History");

            entity.Property(e => e.nRequestTravelExpensesID_His).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dMonthRequest).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nEmployeeID).HasComment("รหัสพนักงาน");
            entity.Property(e => e.nRequestTravelExpensesID).HasComment("รหัสใบคำขอการเบิกค่าเดินทาง");
            entity.Property(e => e.nRequestTypeID).HasComment("รหัสประเภทใบคำขอ (TM_RequestType)");
            entity.Property(e => e.nTotalAmount).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.sComment)
                .HasMaxLength(2000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Room>(entity =>
        {
            entity.HasKey(e => e.nRoomID);

            entity.Property(e => e.nRoomID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("จำนวนคนที่รองรับ");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nOrder).HasComment("ลำดับรายการ");
            entity.Property(e => e.nPerson).HasComment("ลำดับรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sEquipment)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.sFilename)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("ชื่อไฟล์");
            entity.Property(e => e.sPath)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("Path เก็บไฟล์");
            entity.Property(e => e.sRoomCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.sRoomName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.sSystemFilename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อไฟล์ที่เก็บในระบบ");
        });

        modelBuilder.Entity<TB_Task>(entity =>
        {
            entity.HasKey(e => e.nTaskID);

            entity.Property(e => e.nTaskID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dTask).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nActual).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nActualProcess).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nPlan).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nPlanProcess).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nTypeRequest).HasComment("1 = Task , 2 = WFH");
            entity.Property(e => e.sDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.sDescriptionDelay)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_Team>(entity =>
        {
            entity.HasKey(e => e.nTeamID);

            entity.Property(e => e.nTeamID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = StandProcess , false = OtherProcess");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sTeamName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_UserGroup>(entity =>
        {
            entity.HasKey(e => e.nUserGroupID).HasName("PK_TB_UserGroup_1");

            entity.Property(e => e.nUserGroupID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUserRoleID).HasComment("Refer. TM_UserRole");
            entity.Property(e => e.sUserGroupDescription)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.sUserGroupName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_UserGroupPermisson>(entity =>
        {
            entity.HasKey(e => new { e.nUserGroupID, e.nMenuID });

            entity.Property(e => e.nMenuID).HasComment("Refer.TM_Menu");
            entity.Property(e => e.nPermission).HasComment("Permission 2=Full Control, 1=Read Only, 0=Disable");
        });

        modelBuilder.Entity<TB_UserMappingGroup>(entity =>
        {
            entity.HasKey(e => new { e.nEmployeeID, e.nUserGroupID });

            entity.Property(e => e.nUserGroupID).HasComment("Refer.TM_UserGroup");
        });

        modelBuilder.Entity<TB_UserMappingRole>(entity =>
        {
            entity.HasKey(e => new { e.nEmployeeID, e.nUserRoleID });

            entity.Property(e => e.nEmployeeID).HasComment("Refer.TB_Employee");
            entity.Property(e => e.nUserRoleID).HasComment("Refer.TM_UserRole");
        });

        modelBuilder.Entity<TB_UserPermission>(entity =>
        {
            entity.HasKey(e => new { e.nEmployeeID, e.nMenuID });

            entity.Property(e => e.nEmployeeID).HasComment("Refer.TB_Employee");
            entity.Property(e => e.nMenuID).HasComment("Refer.TM_Menu");
            entity.Property(e => e.nPermission).HasComment("Permission 2=Full Control, 1=Read Only, 0=Disable");
        });

        modelBuilder.Entity<TB_UserRole>(entity =>
        {
            entity.HasKey(e => e.nUserRoleID);

            entity.Property(e => e.nUserRoleID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sUserRoleDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.sUserRoleName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_UserRolePermission>(entity =>
        {
            entity.HasKey(e => new { e.nUserRoleID, e.nMenuID });

            entity.Property(e => e.nUserRoleID).HasComment("Refer.TM_UserRole");
            entity.Property(e => e.nMenuID).HasComment("Refer.TM_Menu");
            entity.Property(e => e.nPermission).HasComment("Permission 2=Full Control, 1=Read Only, 0=Disable");
        });

        modelBuilder.Entity<TB_WFH>(entity =>
        {
            entity.HasKey(e => e.nWFHID);

            entity.Property(e => e.nWFHID).ValueGeneratedNever();
            entity.Property(e => e.dCreate).HasColumnType("datetime");
            entity.Property(e => e.dDelete).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.dWFH).HasColumnType("datetime");
            entity.Property(e => e.nManhour).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<TB_WFHFlow>(entity =>
        {
            entity.HasKey(e => e.nFlowID);

            entity.ToTable(tb => tb.HasComment("ตารางเก็บ WorkFlow WFH"));

            entity.Property(e => e.nFlowID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsDelete).HasComment("true = รายการถูกลบ , false = รายการยังไม่ถูกลบ");
            entity.Property(e => e.IsLineApprover).HasComment("true = LineApprover , false = Not Line Approver");
            entity.Property(e => e.dApprove)
                .HasComment("วันที่สร้าง")
                .HasColumnType("datetime");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้าง")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไข")
                .HasColumnType("datetime");
            entity.Property(e => e.nApproveBy).HasComment("ผู้อนุมัติ");
            entity.Property(e => e.nCreateBy).HasComment("รหัสผู้สร้าง");
            entity.Property(e => e.nDeleteBy).HasComment("รหัสผู้ลบ");
            entity.Property(e => e.nFlowProcessID).HasComment("อ้างอิง TM_WFHFlowProcess Column nFlowProcessID");
            entity.Property(e => e.nUpdateBy).HasComment("รหัสผู้ที่แก้ไข");
            entity.Property(e => e.nWFHID).HasComment("รหัสตาราง");

            entity.HasOne(d => d.nWFH).WithMany(p => p.TB_WFHFlow)
                .HasForeignKey(d => d.nWFHID)
                .HasConstraintName("FK_TB_WFHFL_REFERENCE_TB_WFH");
        });

        modelBuilder.Entity<TB_WFHFlowHistory>(entity =>
        {
            entity.HasKey(e => e.nHistoryID);

            entity.ToTable(tb => tb.HasComment("ตารางเก็บ WorkFlow WFH"));

            entity.Property(e => e.nHistoryID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsDelete).HasComment("true = รายการถูกลบ , false = รายการยังไม่ถูกลบ");
            entity.Property(e => e.dApprove)
                .HasComment("วันที่สร้าง")
                .HasColumnType("datetime");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้าง")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไข")
                .HasColumnType("datetime");
            entity.Property(e => e.nApproveBy).HasComment("ผู้อนุมัติ");
            entity.Property(e => e.nCreateBy).HasComment("รหัสผู้สร้าง");
            entity.Property(e => e.nDeleteBy).HasComment("รหัสผู้ลบ");
            entity.Property(e => e.nFlowProcessID).HasComment("อ้างอิง TM_WFHFlowProcess Column nFlowProcessID");
            entity.Property(e => e.nUpdateBy).HasComment("รหัสผู้ที่แก้ไข");
            entity.Property(e => e.nWFHID).HasComment("รหัสตาราง");
            entity.Property(e => e.sDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("หมายเหตุ");

            entity.HasOne(d => d.nWFH).WithMany(p => p.TB_WFHFlowHistory)
                .HasForeignKey(d => d.nWFHID)
                .HasConstraintName("FK_TB_WFHFlowHistory");
        });

        modelBuilder.Entity<TB_WFHTask>(entity =>
        {
            entity.HasKey(e => e.nTaskID);

            entity.ToTable(tb => tb.HasComment("ตารางเก็บ Task WFH"));

            entity.Property(e => e.nTaskID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsDelete).HasComment("true = รายการถูกลบ , false = รายการยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้าง")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไข")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("รหัสผู้สร้าง");
            entity.Property(e => e.nDeleteBy).HasComment("รหัสผู้ลบ");
            entity.Property(e => e.nOrder).HasComment("ลำดับรายการ");
            entity.Property(e => e.nPlanType).HasComment("อ้างอิง TM_Data Column nTypeID = 31");
            entity.Property(e => e.nUpdateBy).HasComment("รหัสผู้ที่แก้ไข");
            entity.Property(e => e.nWFHID).HasComment("รหัสตาราง");

            entity.HasOne(d => d.nWFH).WithMany(p => p.TB_WFHTask)
                .HasForeignKey(d => d.nWFHID)
                .HasConstraintName("FK_TB_WFHTA_REFERENCE_TB_WFH");
        });

        modelBuilder.Entity<TB_WF_Dental_File>(entity =>
        {
            entity.HasKey(e => e.nDentalFileID);

            entity.Property(e => e.nDentalFileID)
                .ValueGeneratedNever()
                .HasComment("รหัสไฟล์เอกสารท้นตกรรม");
            entity.Property(e => e.IsDelete)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.dCreate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.dDelete)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.dUpdate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.nCreateBy)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.nDeleteBy)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.nRequestDentalID).HasComment("รหัสใบคำขอเบิกค่าท้นตกรรม (TB_Request_WF_Dental)");
            entity.Property(e => e.nUpdateBy)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.sFilename)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.sPath)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.sSystemFilename)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_Config>(entity =>
        {
            entity.HasKey(e => e.nConfigID);

            entity.Property(e => e.nConfigID)
                .ValueGeneratedNever()
                .HasComment("Config ID");
            entity.Property(e => e.nConfigTypeID).HasComment("Config Type ID Ref. Table: TM_ConfigType");
            entity.Property(e => e.nSortOrder).HasComment("Sort Order");
            entity.Property(e => e.nValue)
                .HasComment("Number Value")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.sConfigName)
                .HasMaxLength(250)
                .HasComment("Name");
            entity.Property(e => e.sDescription)
                .HasMaxLength(500)
                .HasComment("Description");
            entity.Property(e => e.sValue)
                .HasMaxLength(2000)
                .HasComment("String Value");

            entity.HasOne(d => d.nConfigType).WithMany(p => p.TM_Config)
                .HasForeignKey(d => d.nConfigTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TM_Config_TM_ConfigType");
        });

        modelBuilder.Entity<TM_ConfigType>(entity =>
        {
            entity.HasKey(e => e.nConfigTypeID);

            entity.Property(e => e.nConfigTypeID)
                .ValueGeneratedNever()
                .HasComment("Config Type ID");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("1 = Active, 0 = Inactive");
            entity.Property(e => e.IsManageByUser).HasComment("Is set Admin digital can mangement  ? ture,false");
            entity.Property(e => e.nParentID).HasComment("Parent ID");
            entity.Property(e => e.nSortOrder).HasComment("Sort Order");
            entity.Property(e => e.sConfigTypeName)
                .HasMaxLength(50)
                .HasComment("Config Type Name");
        });

        modelBuilder.Entity<TM_Data>(entity =>
        {
            entity.HasKey(e => e.nData_ID);

            entity.Property(e => e.nData_ID).ValueGeneratedNever();
            entity.Property(e => e.dCraeteDate).HasColumnType("datetime");
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.sAbbr)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.sDescription).IsUnicode(false);
            entity.Property(e => e.sNameEng)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.sNameTH)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_DataType>(entity =>
        {
            entity.HasKey(e => e.nDatatype_ID);

            entity.Property(e => e.nDatatype_ID).ValueGeneratedNever();
            entity.Property(e => e.dUpdate).HasColumnType("datetime");
            entity.Property(e => e.sDescription)
                .HasMaxLength(3000)
                .IsUnicode(false);
            entity.Property(e => e.sNameEng)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.sNameTH)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_District>(entity =>
        {
            entity.HasKey(e => e.nDistrictID).HasName("PK__district__E1F3F87FC74F279C");

            entity.Property(e => e.nDistrictID).ValueGeneratedNever();
            entity.Property(e => e.sDistrictNameEN)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.sDistrictNameTH)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_Language>(entity =>
        {
            entity.HasKey(e => e.sLanguageCode).HasName("PK_TM_SystemConfig");

            entity.Property(e => e.sLanguageCode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("Language Code");
            entity.Property(e => e.isActive).HasComment("1 = Active, 0 = Inactive");
            entity.Property(e => e.isMainEmailLanguage).HasComment("1 = Main Email Language, 0 = Not Main Email Language");
            entity.Property(e => e.isMainLanguage).HasComment("1 = Main Language, 0 = Not Main Language");
            entity.Property(e => e.nSortOrder).HasComment("Sort Order");
            entity.Property(e => e.sLanguageDescription)
                .HasMaxLength(2000)
                .HasComment("Language Description");
            entity.Property(e => e.sLanguageFlag)
                .HasMaxLength(150)
                .HasComment("Language Flag");
            entity.Property(e => e.sLanguageName)
                .HasMaxLength(150)
                .HasComment("Language Value");
            entity.Property(e => e.sNativeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Native Name");
            entity.Property(e => e.sThreeLetterCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasComment("Three Letter Code");
        });

        modelBuilder.Entity<TM_LineTemplate>(entity =>
        {
            entity.HasKey(e => e.nID);

            entity.Property(e => e.nID).ValueGeneratedNever();
            entity.Property(e => e.sDescription)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.sDetail).IsUnicode(false);
            entity.Property(e => e.sSubject)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_LoginType>(entity =>
        {
            entity.HasKey(e => e.nLoginTypeID);

            entity.Property(e => e.nLoginTypeID)
                .ValueGeneratedNever()
                .HasComment("Login Type ID");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("1 = Active, 0 = Inactive");
            entity.Property(e => e.sLoginTypeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Login Type Name");
            entity.Property(e => e.sUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("Url");
        });

        modelBuilder.Entity<TM_Menu>(entity =>
        {
            entity.HasKey(e => e.nMenuID);

            entity.Property(e => e.nMenuID)
                .ValueGeneratedNever()
                .HasComment("PK TM_Menu");
            entity.Property(e => e.IsActive).HasComment("1 = ใช้งาน , 0 = ไม่ใช้งาน");
            entity.Property(e => e.IsDisable).HasComment("1 = แสดงให้กำหนด Permission Disable , 0 = ไม่แสดงให้กำหนด Permission Disable");
            entity.Property(e => e.IsDisplay).HasComment("1 = แสดงที่เมนู Bar , 0 = ไม่แสดงที่เมนู Bar");
            entity.Property(e => e.IsManage).HasComment("1 = แสดงให้กำหนด Permission Manage , 0 = ไม่แสดงให้กำหนด Permission Manage");
            entity.Property(e => e.IsSetPermission).HasComment("1 = แสดงให้กำหนด Permission , 0 = ไม่แสดงให้กำหนด Permission ");
            entity.Property(e => e.IsShowBreadcrumb).HasComment("1 = แสดงใน Breadcrum , 0 = ไม่แสดงใน Breadcrum");
            entity.Property(e => e.IsView).HasComment("1 = แสดงให้กำหนด Permission View , 0 = ไม่แสดงให้กำหนด Permission View");
            entity.Property(e => e.nMenuType).HasComment("1 = Frontend , 2 = Backend");
            entity.Property(e => e.nOrder)
                .HasComment("ลำดับการแสดงผล")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.nParentID).HasComment("Parent MenuID , Refer Table TM_Menu");
            entity.Property(e => e.sIcon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.sMenuName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อเมนู");
            entity.Property(e => e.sRoute)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_Provinces>(entity =>
        {
            entity.HasKey(e => e.nProvinceID).HasName("PK__TB_Provi__B634BC04EF3F7DF6");

            entity.Property(e => e.nProvinceID).ValueGeneratedNever();
            entity.Property(e => e.geography_id).HasDefaultValueSql("((0))");
            entity.Property(e => e.sProvinceNameEN)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.sProvinceNameTH)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.sProvinceShort)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<TM_RequestType>(entity =>
        {
            entity.HasKey(e => e.nRequestTypeID);

            entity.Property(e => e.nRequestTypeID)
                .ValueGeneratedNever()
                .HasComment("รหัสประเภทใบคำขอ");
            entity.Property(e => e.nRequestTypeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("ชื่อประเภทใบคำขอ");
            entity.Property(e => e.sDrescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("รายละเอียดประเภทใบคำขอ");
        });

        modelBuilder.Entity<TM_Status>(entity =>
        {
            entity.HasKey(e => new { e.nStatusID, e.nRequestTypeID });

            entity.Property(e => e.nStatusID).HasComment("รหัสสถานะ");
            entity.Property(e => e.nRequestTypeID).HasComment("รหัสประเภทใบคำขอ Refer.TM_RequestType");
            entity.Property(e => e.nNextStatusID).HasComment("สถานะต่อไป");
            entity.Property(e => e.sDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.sNextStatusName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.sStatusName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("ชื่อสถานะ");
        });

        modelBuilder.Entity<TM_Subdistrict>(entity =>
        {
            entity.HasKey(e => e.sSubDistrictID).HasName("PK__subdistr__037C7D86BA8E7331");

            entity.Property(e => e.sSubDistrictID)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.sSubdistrictNameEN)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.sSubdistrictNameTH)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_Task_Activity>(entity =>
        {
            entity.HasKey(e => e.nActivityID).HasName("PK_TB_Activity");

            entity.Property(e => e.nActivityID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = StandProcess , false = OtherProcess");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sActivity)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_Task_Activity_Mapping>(entity =>
        {
            entity.HasKey(e => e.nMappingActivityID).HasName("PK_TB_Activity_Mapping_1");

            entity.Property(e => e.nMappingActivityID).ValueGeneratedNever();
        });

        modelBuilder.Entity<TM_Task_Activity_PositionMapping>(entity =>
        {
            entity.HasKey(e => new { e.nPositionID, e.nMappingActivityID }).HasName("PK_TB_Activity_PositionMapping");
        });

        modelBuilder.Entity<TM_Task_Activity_Type>(entity =>
        {
            entity.HasKey(e => e.nActivityTypeID).HasName("PK_TB_ActivityType");

            entity.Property(e => e.nActivityTypeID).ValueGeneratedNever();
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sActivityType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.sActivityTypeAbbr)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_Task_Progress>(entity =>
        {
            entity.HasKey(e => e.nProgressID).HasName("PK_TM_TaskProgress");

            entity.Property(e => e.nProgressID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasComment("true = StandProcess , false = OtherProcess");
            entity.Property(e => e.IsDelete).HasComment("true = ลบ , false = ยังไม่ถูกลบ");
            entity.Property(e => e.IsRequiredDesc).HasComment("true = StandProcess , false = OtherProcess");
            entity.Property(e => e.dCreate)
                .HasComment("วันที่สร้างรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dDelete)
                .HasComment("วันที่ลบรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.dUpdate)
                .HasComment("วันที่แก้ไขรายการ")
                .HasColumnType("datetime");
            entity.Property(e => e.nCreateBy).HasComment("ผู้สร้างรายการ");
            entity.Property(e => e.nDeleteBy).HasComment("ผู้ที่ลบรายการ");
            entity.Property(e => e.nUpdateBy).HasComment("ผู้แก้ไขรายการ");
            entity.Property(e => e.sDescription)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.sProgressName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TM_UserType>(entity =>
        {
            entity.HasKey(e => e.nUserTypeID);

            entity.Property(e => e.nUserTypeID)
                .ValueGeneratedNever()
                .HasComment("User Type ID");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("1 = Active, 0 = Inactive");
            entity.Property(e => e.nSortOrder).HasComment("Sort Order");
            entity.Property(e => e.sUserTypeName)
                .HasMaxLength(50)
                .HasComment("User Type Name");
        });

        modelBuilder.Entity<TM_WFHFlowProcess>(entity =>
        {
            entity.HasKey(e => e.nFlowProcessID);

            entity.ToTable(tb => tb.HasComment("ตารางเก็บ WorkFlow WFH"));

            entity.Property(e => e.nFlowProcessID)
                .ValueGeneratedNever()
                .HasComment("รหัสตาราง");
            entity.Property(e => e.IsActive).HasComment("true = มีการใช้งาน , false ไม่มีการใช้งาน");
            entity.Property(e => e.nOrder).HasComment("ลำดับรายการ");
            entity.Property(e => e.sAction)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("ชื่อ Action");
            entity.Property(e => e.sProcess)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("ชื่อ Process Flow");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
