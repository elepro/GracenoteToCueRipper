// See https://aka.ms/new-console-template for more information
using GracenoteToCueRipper;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml;

string drive = "F:";

int totalDiscs = 0;
int discNumber = 0;
int numtracks = 0;

MetaBrainz.MusicBrainz.DiscId.TableOfContents toc = MetaBrainz.MusicBrainz.DiscId.TableOfContents.ReadDisc(drive, MetaBrainz.MusicBrainz.DiscId.DiscReadFeature.All);


string result;
result = LocalIni.ReadLocalIni(out totalDiscs, out int year, out numtracks, out string artist, out string title, out string[] trackInfo, drive);

string ctdbid = CTDB.GetCTDBTocId(toc);
CTDB.WriteCuetoolsXML(totalDiscs, discNumber, year, artist, title, trackInfo, ctdbid);
