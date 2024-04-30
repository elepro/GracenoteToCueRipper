// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Xml;
using static MetaBrainz.MusicBrainz.DiscId.TableOfContents;

int totalDiscs=0;
int discNumber=0;
int year=2024;
string artist="";
string title="";

MetaBrainz.MusicBrainz.DiscId.TableOfContents toc = MetaBrainz.MusicBrainz.DiscId.TableOfContents.ReadDisc("F:",  MetaBrainz.MusicBrainz.DiscId.DiscReadFeature.All);
Console.WriteLine(toc.Length);
string ctdbid = TOCID(toc);
string file = Environment.GetEnvironmentVariable("APPDATA") + @"\CUE Tools\MetadataCache\" + ctdbid + ".xml0";
XmlWriterSettings setting = new XmlWriterSettings
{ Indent = true,};
XmlWriter writer = XmlWriter.Create(file,setting);
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
}

writer.WriteEndElement();
writer.Close();

string TOCID(MetaBrainz.MusicBrainz.DiscId.TableOfContents toc)
{
    byte AudioTracks = toc.LastTrack;
    StringBuilder mbSB = new StringBuilder();
    int totalLength = 0;
    for (int iTrack = 1; iTrack < AudioTracks + 1; iTrack++)
    {
        totalLength += toc.Tracks[iTrack].Length;
        mbSB.AppendFormat("{0:X8}", totalLength);
    }
    //        mbSB.AppendFormat("{0:X8}", toc.Tracks[(int)AudioTracks].StartTime.Milliseconds - 1 - 0);
    // Use Math.Max() to avoid negative count number in case of non-standard CUE sheet with more than 99 tracks.
    mbSB.Append(new string('0', Math.Max(0, (100 - (int)AudioTracks) * 8)));
    byte[] hashBytes = (new System.Security.Cryptography.SHA1CryptoServiceProvider()).ComputeHash(Encoding.ASCII.GetBytes(mbSB.ToString()));
    return Convert.ToBase64String(hashBytes).Replace('+', '.').Replace('/', '_').Replace('=', '-');
}


//public string TOCID
//{
//    get
//    {
//        StringBuilder mbSB = new StringBuilder();
//        for (int iTrack = 1; iTrack < AudioTracks; iTrack++)
//            mbSB.AppendFormat("{0:X8}", _tracks[_firstAudio + iTrack].Start - _tracks[_firstAudio].Start);
//        mbSB.AppendFormat("{0:X8}", _tracks[_firstAudio + (int)AudioTracks - 1].End + 1 - _tracks[_firstAudio].Start);
//        // Use Math.Max() to avoid negative count number in case of non-standard CUE sheet with more than 99 tracks.
//        mbSB.Append(new string('0', Math.Max(0, (100 - (int)AudioTracks) * 8)));
//        byte[] hashBytes = (new SHA1CryptoServiceProvider()).ComputeHash(Encoding.ASCII.GetBytes(mbSB.ToString()));
//        return Convert.ToBase64String(hashBytes).Replace('+', '.').Replace('/', '_').Replace('=', '-');
//    }
//}
