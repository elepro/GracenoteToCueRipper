using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GracenoteToCueRipper
{
    internal class CTDB
    {

        public static string GetCTDBTocId(MetaBrainz.MusicBrainz.DiscId.TableOfContents toc)
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
            byte[] hashBytes = new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(mbSB.ToString()));
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
    }
}
