using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace GracenoteToCueRipper
{
    [SupportedOSPlatform("windows")]
    internal partial class LocalIni
    {
        [LibraryImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileSectionW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial uint GetPrivateProfileSection(string lpAppName, char[] lpReturnedString, uint nSize, string lpFileName);

        /// <summary>
        /// cdplayer.ini用のidを取得する
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        public static string GetLocalId(string driveName)
        {
            System.Management.ManagementObject _mo = new System.Management.ManagementObject("Win32_LogicalDisk=\"" + driveName + "\"");
            return ((string)_mo.Properties["VolumeSerialNumber"].Value).TrimStart(['0']);
        }

        /// <summary>
        /// cdplayer.iniからCD情報を取得する
        /// </summary>
        /// <param name="totalDiscs"></param>
        /// <param name="year"></param>
        /// <param name="numtracks"></param>
        /// <param name="artist"></param>
        /// <param name="title"></param>
        /// <param name="trackInfo"></param>
        /// <param name="drive"></param>
        /// <returns></returns>
        public static string ReadLocalIni(out int totalDiscs, out int year, out int numtracks, out string artist, out string title, out string[] trackInfo, string drive)
        {
            totalDiscs = 0;
            year = 0;
            numtracks = 0;
            artist = "";
            title = "";
            trackInfo = [];
            char[] iniString = new char[32767];
            string localId = GetLocalId(drive);
            string result = string.Empty;
            string initFileName = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\VirtualStore\Windows\cdplayer.ini";
            if (System.IO.File.Exists(initFileName))
            {
                uint length = GetPrivateProfileSection(localId, iniString, 32767, initFileName);
                if (length > 0)
                {
                    result = new string(iniString);
                    string[] keys = result.TrimEnd('\0').Split('\0');
                    foreach (var key in keys)
                    {
                        string[] dic = key.Split('=');
                        switch (dic[0])
                        {
                            case "artist":
                                artist = dic[1];
                                break;
                            case "title":
                                title = dic[1];
                                break;
                            case "numtracks":
                                numtracks = Convert.ToInt32(dic[1]);
                                trackInfo = new string[numtracks];
                                break;
                            case "totaldiscs":
                                totalDiscs = Convert.ToInt32(dic[1]);
                                break;
                            case "year":
                                year = Convert.ToInt32(dic[1]);
                                break;
                        }

                        //数字に変換出来たらトラック番号(0-99)
                        if (int.TryParse(dic[0], out int trackNum))
                        {

                            if ((trackNum >= 0) && (trackNum <= 99))
                            {
                                trackInfo[trackNum] = dic[1];
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
