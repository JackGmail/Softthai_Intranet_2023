namespace Backend.Enum
{
    public class EnumLeave
    {
        #region Enum
        public enum SaveMode
        {
            SaveDraft = 0,
            Submit = 1,
            Approve = 2,
            Recall = 3,
            Cancel = 4,
            Reject = 5,
        }
        public enum Status
        {
            Canceled = 0,
            Draft = 1,
            Submit = 2,
            Approved = 3,
            RejectHr = 5,
            RejectLead = 6,
            Recall = 7,
            RecallHr = 8,
            RecallLead = 9,
            Complete = 99,
        }

        public enum EmployeeType
        {
            FullTime = 21,
            Probationary = 22,
            OutSouse = 23,
        }

        public enum DataType
        {
            nSex = 10,
            nEmployeeType = 7

        }

        public static class GlobalText
        {
            public static string Active { get; set; } = "ใช้งาน";
            public static string InActive { get; set; } = "ไม่ใช้งาน";
        }

        public enum LeaveTypeName
        {
            /// <summary>
            /// ลาพักร้อน
            /// </summary>
            Holiday = 81,

            /// <summary>
            /// ลากิจ
            /// </summary>
            Business = 82,

            /// <summary>
            /// วันเกิด
            /// </summary>
            BirthDate = 83,

            /// <summary>
            /// ลาบวช
            /// </summary>
            Ordination = 84,

            /// <summary>
            /// ลาคลอดบุตร
            /// </summary>
            Maternity = 85,

            /// <summary>
            /// ลาไม่รับค่าจ้าง
            /// </summary>
            Unpaid = 86,

            /// <summary>
            /// ทำงานชดเชย
            /// </summary>
            Compensation = 87,

             /// <summary>
            /// ลาป่วย
            /// </summary>
            Sick = 88,
        }


        #endregion

    }
}
