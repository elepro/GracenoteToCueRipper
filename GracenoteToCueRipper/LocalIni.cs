using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GracenoteToCueRipper
{
    internal class LocalIni
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpDefault"></param>
        /// <param name="lpReturnedString"></param>
        /// <param name="nSize"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            uint nSize,
            string lpFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="nDefault"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileInt(
            string lpAppName,
            string lpKeyName,
            int nDefault,
            string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern int GetPrivateProfileSection(
             [In][MarshalAs(UnmanagedType.LPStr)] string lpAppName,
            // Note that because the key/value pars are returned as null-terminated
            // strings with the last string followed by 2 null-characters, we cannot
            // use StringBuilder.
            [In] IntPtr lpReturnedString,
            [In] UInt32 nSize,
            [In][MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        public static string GetLocalId(string driveName)
        {
            System.Management.ManagementObject _mo = new System.Management.ManagementObject("Win32_LogicalDisk=\"" + driveName + "\"");
            return ((string)_mo.Properties["VolumeSerialNumber"].Value).TrimStart(['0']);
        }
    }
}
