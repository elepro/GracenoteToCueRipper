# GracenoteToCueRipper
Gracenote CDDBから情報を取得し、CueToolsで使用するXMLファイルに出力します。  
Gracenote CDDBへのアクセスはiTunesを利用します。  
iTunesにはアルバムアーティスト名を出力するAPIが存在しないため、1曲目のアーティスト名を代用します。  
XMLの出力先は"%%APPDATA%%\CUE Tools\MetadataCache\"フォルダです。

# 使い方
.NET8が必要です。  
実行すると、CDドライブを探して、先頭のドライブから順番にXMLを出力します。  
処理が終われば自動的に終了します。

# 開発動機
CDリッパーはCueRipperを使いたいがCTDBが貧弱で、充実しているGracenote CDDBから情報を取得したかったから。
