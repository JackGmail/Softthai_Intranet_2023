using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Net;
using Extensions.Common.STFunction;

public class NetworkConnection : IDisposable
{
    readonly string _networkName;
    public static bool Logon()
    {
        try
        {
            string networkPath = STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathUpFile") + "";
            bool IsLogonSharePath = STFunction.GetAppSettingJson("AppSetting:SharePath:IsLogonSharePath") + "" == "Y";
            if (IsLogonSharePath)
            {
                //var credentials = new UserCredentials(domain, username, password);
                //var result = Impersonation.RunAsUser(credentials, logonType, () =>
                //{
                //    return System.IO.Directory.GetFiles(@"\\server\share");
                //});

                bool IsReturn = false;

                //var networkPath = @"//"+ Systemfunction.sSharePath + "/";
                var credentials = new NetworkCredential(STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathUser").ToString(), STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathPassword").ToString(), STFunction.GetAppSettingJson("AppSetting:SharePath:SharePathDomain").ToString());

                //IntPtr token = IntPtr.Zero;
                //IntPtr tokenDuplicate = IntPtr.Zero;

                if (RevertToSelf())
                {
                    //if (LogonUserA(credentials.UserName, credentials.Domain, credentials.Password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                    //{
                    //    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    //    {
                            using (new NetworkConnection(networkPath, credentials))
                            {
                                var fileList = Directory.GetFiles(networkPath);

                                //foreach (var file in fileList)
                                //{
                                //    //Console.WriteLine("{0}", Path.GetFileName(file));
                                //}

                                IsReturn = true;
                                //if (fileList.Length > 0) {
                                //    IsReturn = true;
                                //} else {
                                //    IsReturn = false;
                                //}
                            }
                    //    }
                    //}
                }

                //if (token != IntPtr.Zero) { 
                //    CloseHandle(token);
                //    IsReturn = false; 
                //}
                //if (tokenDuplicate != IntPtr.Zero) { 
                //    CloseHandle(tokenDuplicate);
                //    IsReturn = false; 
                //}
                return IsReturn;
            }
            else
            {
                return false; //true;
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public NetworkConnection(string networkName, NetworkCredential credentials)
    {
        _networkName = networkName;
       
        var netResource = new NetResource
        {
            Scope = ResourceScope.GlobalNetwork,
            ResourceType = ResourceType.Disk,
            DisplayType = ResourceDisplaytype.Share,
            RemoteName = networkName
        };

        var userName = string.IsNullOrEmpty(credentials.Domain) ? credentials.UserName : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);
        var result = WNetAddConnection2(netResource, credentials.Password, userName, 0);
        //if (result != 0)
        //{
        //    throw new Win32Exception(result, "Error connecting to remote share");
        //}             
    }

    ~NetworkConnection()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        WNetCancelConnection2(_networkName, 0, true);
    }

    #region Logon To Share Path
    public const int LOGON32_LOGON_INTERACTIVE = 2;
    public const int LOGON32_PROVIDER_DEFAULT = 0;

    [DllImport("advapi32.dll")]
    public static extern int LogonUserA(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool RevertToSelf();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool CloseHandle(IntPtr handle);
    #endregion

    [DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(NetResource netResource, string password, string username, int flags);

    [DllImport("mpr.dll")]
    private static extern int WNetCancelConnection2(string name, int flags, bool force);

    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    public enum ResourceScope : int
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

    public enum ResourceType : int
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    public enum ResourceDisplaytype : int
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }
}