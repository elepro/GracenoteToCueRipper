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

                    playList = playLists[1];    //1スタート

                    tracks = playList.Tracks;

                    trackCount = tracks.Count;

                    cd[foundDiscCount].AlbumTitle = playList.Name;

                    //アルバムアーティスト
                    //dynamicでキャストするとArtist（アルバムアーティスト）を読み込める。
                    dynamic pl = playList;
                    cd[foundDiscCount].AlbumArtist = pl.Artist;

                    cd[foundDiscCount].Compilation = pl.Compilation;
                    cd[foundDiscCount].DiscCount = (uint)pl.DiscCount;
                    cd[foundDiscCount].DiscNumber = (uint)pl.DiscNumber;
                    cd[foundDiscCount].TrackCount = pl.Tracks.Count;
                    cd[foundDiscCount].Year = (uint)pl.Year;

                    //各トラックの情報取得
                    for (int t = 1; t <= trackCount; t++)   //1から始める
                    {
                        IITTrack track;
                        track = tracks[t];

                        cd[foundDiscCount].TrackArtist[t] = track.Artist;
                        cd[foundDiscCount].TrackTitle[t] = track.Name;

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
