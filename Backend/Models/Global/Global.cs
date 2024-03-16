

using ST.INFRA.Common;
namespace Backend.Models
{
    public class cSelectOption
    {
        public string? label { get; set; }

        public string? value { get; set; }
    }
    public class cRemoveTable : STGrid.PaginationData
    {
        public List<string>? lstID { get; set; } = new List<string>();
    }

}
