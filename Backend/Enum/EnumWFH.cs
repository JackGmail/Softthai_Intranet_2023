namespace Backend.Enum
{
    /// <summary>
    /// </summary>
    public class EnumWFH
    {
        /// <summary>
        /// </summary>
        public enum WFHRequestMode
        {
            /// <summary>
            /// </summary>
            Draft = 1,
            /// <summary>
            /// Approve / save submit
            /// </summary>
            Save = 2,
            /// <summary>
            /// </summary>
            Reject = 3,
            /// <summary>
            /// </summary>
            Recall = 4,
            /// <summary>
            /// </summary>
            Cancel = 5
        }
        /// <summary>
        /// </summary>
        public enum WFHStatus
        {
            /// <summary>
            /// </summary>
            Draft = 1,
            /// <summary>
            /// </summary>
            Submit = 2,
            /// <summary>
            /// </summary>
            Approve = 3,
            /// <summary>
            /// </summary>
            Completed = 4,
            /// <summary>
            /// </summary>
            Recall = 5,
            /// <summary>
            /// </summary>
            Reject= 6,
            /// <summary>
            /// </summary>
            Cancel = 7,
        }

        /// <summary>
        /// </summary>
        public enum LineApprover
        {
            /// <summary>
            /// </summary>
            HR = 9,
        }

        public enum WFHType
        {
            /// <summary>
            /// </summary>
            RequestDate = 1,
            /// <summary>
            /// </summary>
            WFHDate = 2
        }
    }
}