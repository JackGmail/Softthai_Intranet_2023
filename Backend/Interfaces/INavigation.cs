using Backend.Models;

namespace Backend.Interfaces
{
    public interface INavigation
    {
        ResultAPI GetBreadCrumb(string sRoute, string sLanguages);
        ResultAPI GetMenu(int nMenuType);
    }
}