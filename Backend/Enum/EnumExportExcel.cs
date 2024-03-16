using ST.INFRA;

namespace Backend.Enum
{
    public class EnumExportExcel
    {
        public enum ExportExcelFont
        {
            nFontSize = 14,
            nFontSizeHead = 14,

            [StringValue("TH Sarabun PSK", "1")]
            FontName = 1,
        }
    }
}