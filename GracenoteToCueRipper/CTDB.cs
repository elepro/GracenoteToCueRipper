using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GracenoteToCueRipper
{
    internal class CTDB
    {
        /// <summary>
        /// CuetoolsDB用のTOCIDを取得する
        /// </summary>
        /// <param name="toc"></param>
        /// <returns>TOCID</returns>
        public static string GetCTDBTocId(MetaBrainz.MusicBrainz.DiscId.TableOfContents toc)
        {
            byte AudioTracks = toc.LastTrack;
            StringBuilder mbSB = new();
            int totalLength = 0;
            //Tracks[]へのインデックスは1ベースになっていてOK
            for (int iTrack = 1; iTrack < AudioTracks + 1; iTrack++)
            {
                totalLength += toc.Tracks[iTrack].Length;
                mbSB.AppendFormat("{0:X8}", totalLength);
            }
            // Use Math.Max() to avoid negative count number in case of non-standard CUE sheet with more than 99 tracks.
            mbSB.Append(new string('0', Math.Max(0, (100 - (int)AudioTracks) * 8)));
            byte[] hashBytes = System.Security.Cryptography.SHA1.HashData(Encoding.ASCII.GetBytes(mbSB.ToString()));
            return Convert.ToBase64String(hashBytes).Replace('+', '.').Replace('/', '_').Replace('=', '-');
        }

        public static void WriteCuetoolsXML(int totalDiscs, int discNumber, int year, string artist, string title, string[] track, string ctdbid)
        {
            string file = Environment.GetEnvironmentVariable("APPDATA") + @"\CUE Tools\MetadataCache\" + ctdbid + ".xml0";

            XmlWriterSettings setting = new XmlWriterSettings { Indent = true, };
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
        }
    }
}
