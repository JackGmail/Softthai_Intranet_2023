using ST.INFRA;

namespace Backend.Enum
{
    public class EnumPermission
    {
        #region Enum
        public enum Permission
        {
            [StringValue("", "")]
            Full = 2,
            [StringValue("", "")]
            ReadOnly = 1,
            [StringValue("", "")]
            Disable = 0,
        }
        public enum MenuDisplay
        {
            nLevel3 = 3,
        }
        #endregion

    }
}

