using iTunesLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GracenoteToCueRipper
{
    internal class ITunes
    {
        ///<summery>
        //iTunesでCDの情報を取得する
        //戻り値は見つかったCDの枚数
        /// </summery>
        public static int GetCDInfofromItunes(ref CDINFO[] cd)
        {
            iTunesLib.iTunesApp itunes = new();

            int i;
            int foundDiscCount = 0;  //見つかったディスクの枚数

            //取得処理開始
            IITSourceCollection sources = itunes.Sources;
            int srcCount = sources.Count;
            for (i = 1; i <= srcCount; i++) //1からはじめる
            {
                //オーディオCDかどうか
                IITSource src;
                src = sources[i];

                ITSourceKind srcKind;
                srcKind = src.Kind;

                //オーディオCD
                if (srcKind == ITSourceKind.ITSourceKindAudioCD)
                {
                    IITPlaylistCollection playLists;
                    IITPlaylist playList;
                    IITTrackCollection tracks;
                    int trackCount;    //トラック数

                    playLists = src.Playlists;

                    playList = playLists[1];

                    tracks = playList.Tracks;

                    //アルバムアーティスト情報はなさそう
                    //Debug.WriteLine(playList.trackID);
                    //Debug.WriteLine(playList.trackID);
                    //Debug.WriteLine(playList.playlistID);
                    //Debug.WriteLine(playList.Duration);
                    //Debug.WriteLine(playList.Kind);
                    //Debug.WriteLine(playList.Size);
                    //Debug.WriteLine(playList.Name);
                    //Debug.WriteLine(playList.Time);
                    //Debug.WriteLine(playList.Shuffle);
                    //Debug.WriteLine(playList.Source.Name);

                    trackCount = tracks.Count;

                    cd[foundDiscCount].AlbumTitle = playList.Name;

                    //各トラックの情報取得
                    for (int t = 1; t <= trackCount; t++)   //1から始める
                    {
                        IITTrack track;
                        track = tracks[t];

                        bool isCompilation;
                        isCompilation = track.Compilation;

                        int discCount;
                        discCount = track.DiscCount;

                        int discNumber;
                        discNumber = track.DiscNumber;

                        string trackArtist;
                        trackArtist = track.Artist;

                        string trackTitle;
                        trackTitle = track.Name;

                        int year;
                        year = track.Year;

                        cd[foundDiscCount].Compilation = isCompilation;

                        if (t == 1)
                        {
                            //1曲目のアーティストをアルバムアーティストとする。
                            cd[foundDiscCount].AlbumArtist = trackArtist;
                        }

                        cd[foundDiscCount].TrackArtist[t] = trackArtist;
                        cd[foundDiscCount].TrackTitle[t] = trackTitle;
                        cd[foundDiscCount].TrackCount = trackCount;
                        cd[foundDiscCount].DiscCount = (uint)discCount;
                        cd[foundDiscCount].DiscNumber = (uint)discNumber;
                        cd[foundDiscCount].Year = (uint)year;

                        //MessageBox(NULL,Title ,L"",NULL);
                    }
                    foundDiscCount++;
                }
            }
            itunes.Quit();
            return foundDiscCount;
        }
    }
}
