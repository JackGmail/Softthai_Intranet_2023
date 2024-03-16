
using ST.INFRA;

namespace Backend.Enum
{ /// <summary>
  /// </summary>
    public class EnumStandard
    {
            /// <summary>
            /// </summary>
        public enum Status
        {
           /// <summary>
            /// ใช้งาน
            /// </summary>
            [StringValue("ใช้งาน", "")]
            Active = 1,
             /// <summary>
            /// ไม่ใช้งาน
            /// </summary>
            [StringValue("ไม่ใช้งาน", "")]
            InActive = 2,
        }
        

    }
}