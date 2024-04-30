// See https://aka.ms/new-console-template for more information
using GracenoteToCueRipper;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml;

string drive = "F:";

int totalDiscs = 0;
int discNumber = 0;
int year = 2024;
int numtracks = 0;   
string artist = "";
string title = "";
string[] track = new string[0];

MetaBrainz.MusicBrainz.DiscId.TableOfContents toc = MetaBrainz.MusicBrainz.DiscId.TableOfContents.ReadDisc(drive,  MetaBrainz.MusicBrainz.DiscId.DiscReadFeature.All);

string ctdbid = CTDB.GetCTDBTocId(toc);
string localId = LocalIni.GetLocalId(drive);

string file = Environment.GetEnvironmentVariable("APPDATA") + @"\CUE Tools\MetadataCache\" + ctdbid + ".xml0";

IntPtr iniString = Marshal.AllocHGlobal(32767);
String result;

string initFileName = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\VirtualStore\\Windows" + @"\cdplayer.ini";
if (System.IO.File.Exists(initFileName))
{
    int length = LocalIni.GetPrivateProfileSection(localId, iniString, 32767, initFileName);
    result = Marshal.PtrToStringAnsi(iniString, length);
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
                track = new string[numtracks];
                break;
            case "totaldiscs":
                totalDiscs = Convert.ToInt32(dic[1]);
                break;
            case "year":
                year = Convert.ToInt32(dic[1]);
                break;
        }

        if (int.TryParse(dic[0], out int trackNum))
        {

            if ((trackNum >= 0) && (trackNum <= 99))
            {
                track[trackNum] = dic[1];
            }


        }

    }
}
XmlWriterSettings setting = new XmlWriterSettings{ Indent = true, };
XmlWriter writer = XmlWriter.Create(file, setting);
writer.WriteStartDocument();
writer.WriteStartElement("CUEMetadata");
writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
{
    writer.WriteStartElement("Id");
    writer.WriteString(ctdbid);
    writer.WriteEndElement();
    writer.WriteStartElement("TotalDiscs");
    writer.WriteString(totalDiscs.ToString());
    writer.WriteEndElement();
    writer.WriteStartElement("DiscNumber");
    writer.WriteString(discNumber.ToString());
    writer.WriteEndElement();
    writer.WriteStartElement("Year");
    writer.WriteString(year.ToString());
    writer.WriteEndElement();
    writer.WriteStartElement("Artist");
    writer.WriteString(artist);
    writer.WriteEndElement();
    writer.WriteStartElement("Title");
    writer.WriteString(title);
    writer.WriteEndElement();
    writer.WriteStartElement("Tracks");
        foreach (var tr in track)
        {
            writer.WriteStartElement("CUETrackMetadata");
            writer.WriteStartElement("Artist");
            writer.WriteString(artist);
            writer.WriteEndElement();
            writer.WriteStartElement("Title");
        writer.WriteString(tr);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    writer.WriteEndElement();
}

writer.WriteStartElement("AlbumArt");
writer.WriteEndElement();
writer.WriteEndElement();
writer.Close();
