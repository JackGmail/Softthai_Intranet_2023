using ST.INFRA;

namespace Backend.Enum
{
    public class EnumGlobal
    {
        public enum LoginMode
        {
            Guest = 1,
            Local = 2,
            AD = 3,
            LDAP = 4
        }
        public enum Config
        {
            IsLoginBypass = 10103,
            ByPass = 10101,
            DefaultPath = 20102,
            CheckIn = 20103,
            CheckOUt = 20104,
            SenddingNoti = 20209,
            EmailSendding = 20210,
            CheckPermission = 30104,
            MailMode = 20207,
            WorkYear_ThreeYear = 20106,
            WorkYear_UnderThreeYear = 20107

        }
        public enum UserPermission
        {
            User = 1,
            Group = 2,
            Full = 3
        }
        public enum MasterDataType
        {
            ProcessType = 1,
            ReasonType = 2,
            ProjectType = 3,
            Sex = 10,
            AllowanceType = 33,
            VehicleType = 34,
            TravelStartType = 35,
            TaskType = 32,
            ProgressType = 37,
            TaskStatus = 27,
            DentalType = 36
        }
        public enum MasterData
        {
            ImageProfile = 88,
            Plan = 111,
            OtherDental = 139
        }
        public enum Culture
        {
            [StringValue("th-TH", "")]
            th_TH = 1,
            [StringValue("en-US", "")]
            en_US = 2,
        }
        public enum RequestFormType
        {
            Leave = 1,
            OT = 2,
            Allowance = 3,
            Travel = 4,
            Dental = 5,
        }
        public enum StatusRequestOT
        {
            Canceled = 0,
            Draft = 1,
            WaitingApprove = 2,
            Reject = 3,
            WaitingResult = 4,
            WaitingClosed = 5,
            Closed = 6,
        }
        public enum StatusRequestWelfare
        {
            Canceled = 0,
            Draft = 1,
            WaitingApprove = 2,
            Reject = 3,
            Completed = 4,
        }
    }

    #region Http Status Code
    /// <summary>
    /// Http Status Code
    /// </summary>
    public enum APIStatusCode
    {
        /// <summary>
        /// Duplicate / Warning
        /// </summary>
        Warning = (int)StatusCodes.Status203NonAuthoritative,
        /// <summary>
        /// Success
        /// </summary>
        Success = (int)StatusCodes.Status200OK,
        /// <summary>
        /// Confirm data
        /// </summary>
        Confirm = (int)StatusCodes.Status202Accepted,
        /// <summary>
        /// Error
        /// </summary>
        Error = (int)StatusCodes.Status500InternalServerError,
        /// <summary>
        /// �դ�������͹�Ѻ 403 Forbidden (�ǧ����) �� 401 Unauthorized ����੾������ͨ��繵�ͧ�ա�õ�Ǩ�ͺ�Է��� (Authentication) ������������ �����ѧ������Ѻ����׹�ѹ
        /// </summary>
        Unauthorized = (int)StatusCodes.Status401Unauthorized,
    }
    #endregion
}
