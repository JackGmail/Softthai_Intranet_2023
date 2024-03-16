namespace Backend.Models
{
    public class Breadcrumbs
    {
        public List<ItemBreadcrumbs>? lstBreadcrumbs { get; set; }
    }
    public class ItemBreadcrumbs
    {
        /// <summary>
        /// Key
        /// </summary>
        public int nMenuID { get; set; }
        /// <summary>
        /// Item Name
        /// </summary>
        public string? sMenuName { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public string? sIcon { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string? sRoute { get; set; }
        /// <summary>
        /// Level
        /// </summary>
        public int? nLevel { get; set; }
    }
   
    public class Menu : ResultAPI
    {
        /// <summary>
        /// List Menu
        /// </summary>
        public List<ItemMenu>? lstMenu { get; set; }

    }
    public class ItemMenu
    {
        /// <summary>
        /// Key
        /// </summary>
        public string? sKey { get; set; }
        /// <summary>
        /// Menu ID
        /// </summary>
        public int nMenuID { get; set; }
        /// <summary>
        /// Menu ID
        /// </summary>
        public int? nParentID { get; set; }
        /// <summary>
        /// Menu Name
        /// </summary>
        public string? sMenuName { get; set; }
        /// <summary>
        /// Level
        /// </summary>
        public int? nLevel { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string? sRoute { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public string? sIcon { get; set; }
        /// <summary>
        /// Children
        /// </summary>
        public List<ItemMenu>? lstChildrenMenu { get; set; }
    }
}
