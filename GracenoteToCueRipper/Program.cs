// See https://aka.ms/new-console-template for more information
using GracenoteToCueRipper;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml;

string drive = "F:";

int totalDiscs = 0;
int discNumber = 0;
int numtracks = 0;

if (args.Length == 0)
{
    //ドライブ名を入力させる
    Console.Write("Select Drive:");
    char[] buf = new char[1];
    Console.In.Read(buf, 0, 1);
    if (buf[0] != '\r')
    {
        char firstChar = new string(buf).ToUpper()[0];
        if ((firstChar >= 'D') && (firstChar <= 'Z'))
        {
            drive = firstChar + ":";
        }
    }
}
else if (args.Length == 1)
{
    //引数1番目がドライブ名とする
    char firstChar = new string(args[0]).ToUpper()[0];
    if ((firstChar >= 'D') && (firstChar <= 'Z'))
    {
        drive = firstChar + ":";
    }
}

MetaBrainz.MusicBrainz.DiscId.TableOfContents toc = MetaBrainz.MusicBrainz.DiscId.TableOfContents.ReadDisc(drive, MetaBrainz.MusicBrainz.DiscId.DiscReadFeature.All);


string result;
result = LocalIni.ReadLocalIni(out totalDiscs, out int year, out numtracks, out string artist, out string title, out string[] trackInfo, drive);

string ctdbid = CTDB.GetCTDBTocId(toc);
CTDB.WriteCuetoolsXML(totalDiscs, discNumber, year, artist, title, trackInfo, ctdbid);
