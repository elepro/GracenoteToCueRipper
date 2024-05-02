using iTunesLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

                    tracks= playList.Tracks;

                    trackCount = tracks.Count;

                    //各トラックの情報取得
                    for (int t = 1; t <= trackCount; t++)   //1から始める
                    {
                        IITTrack track;
                        track = tracks[t];

                        bool isCompilation;
                        isCompilation = track.Compilation;

                        string album;
                        album= track.Album;

                        int discCount;
                        discCount = track.DiscCount;

                        int discNumber;
                        discNumber = track.DiscNumber;

                        string artist;
                        artist = track.Artist;

                        string title;
                        title = track.Name;

                        int year;
                        year = track.Year;

                        cd[discCount].Compilation = isCompilation;
                        cd[discCount].AlbumTitle = album;

                        cd[discCount].TrackArtist[t] = artist;
                        cd[discCount].TrackTitle[t] = title;
                        cd[discCount].TrackCount = trackCount;
                        cd[discCount].DiscCount = (uint)discCount;
                        cd[discCount].DiscNumber = (uint)discNumber;
                        cd[discCount].Year = (uint)year;

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
