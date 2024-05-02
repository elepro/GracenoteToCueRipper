// See https://aka.ms/new-console-template for more information
using GracenoteToCueRipper;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml;


CDINFO[] cdInfo = new CDINFO[26];   //"A" to "Z"

for (int i = 0; i < 26; i++)
{
    cdInfo[i] = new CDINFO();
}

//光学ドライブ名列挙
DriveInfo[] allDrives = DriveInfo.GetDrives();
for (int i = 0, j = 0; i < allDrives.Length; i++)
{
    if (allDrives[i].DriveType == DriveType.CDRom)
    {
        //見つかった順に割り当て
        cdInfo[j++].Drive = allDrives[i].Name;
    }
}

int count = ITunes.GetCDInfofromItunes(ref cdInfo);

for (int i = 0; i < count; i++)
{
    //TOC読み取り
    MetaBrainz.MusicBrainz.DiscId.TableOfContents toc = MetaBrainz.MusicBrainz.DiscId.TableOfContents.ReadDisc(cdInfo[i].Drive, MetaBrainz.MusicBrainz.DiscId.DiscReadFeature.All);

    //string result;
    //result = LocalIni.ReadLocalIni(ref cdInfo[i],  drive);

    //Cuetools用Disc ID算出
    string ctdbid = CTDB.GetCTDBTocId(toc);
    //XMLに出力
    CTDB.WriteXML(ref cdInfo[i], ctdbid);
}
//end


/// <summary>
/// CD情報
/// </summary>
class CDINFO
{
    /// <summary>
    /// ドライブ名
    /// </summary>
    public string Drive = string.Empty;
    /// <summary>
    /// CDID
    /// </summary>
    //public uint id;
    /// <summary>
    /// コンピレーションか
    /// </summary>
    public bool Compilation;
    /// <summary>
    /// アルバムタイトル
    /// </summary>
    public string AlbumTitle = string.Empty;
    /// <summary>
    /// アーティスト
    /// </summary>
    public string AlbumArtist = string.Empty;
    /// <summary>
    /// トラック数
    /// </summary>
    public int TrackCount;
    /// <summary>
    /// タイトル(1から使う)
    /// </summary>
    public string[] TrackTitle = new string[100];
    /// <summary>
    /// トラックアーティスト(1から使う)
    /// </summary>
    public string[] TrackArtist = new string[100];
    /// <summary>
    /// 販売年
    /// </summary>
    public uint Year;
    /// <summary>
    /// ディスク数
    /// </summary>
    public uint DiscCount;
    /// <summary>
    /// ディスク番号
    /// </summary>
    public uint DiscNumber;
}

