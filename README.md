## 使い方
### ExecSelectQuery<br>
#### List<Dictionary<string, string>> ExecSelectQuery(string sql, Dictionary<string,string> parameter)
select文用<br>
すべての結果をListで返す第一引数にsql、第二引数に連想配列でパラメーターをセット<br>
戻り値はselect文の結果が入ったList<br>
{0 : {“商品コード” : "A0001",”商品名”: “フガー”},1:{“商品コード” : "A0002",”商品名”: “ホゲェェ”}}　←こんな感じ<br>
商品コードとか商品名はSELECT文で指定した名前になるので、取得したものを続けて加工したいならas句を使って元の名前に戻してね<br>
<br>

### FetcDGVData
#### DataTable FetcDGVData(string sql, Dictionary<string, string> parameter) 
select文用<br>
datagridviewの値を取得する<br>
第一引数にsql、第二引数に連想配列でパラメーターをセット<br>
戻り値はselect文の結果のデータテーブル<br>
dgv_hoge.DataSource = FetcDGVData でdatagridviewに結果が入るよ<br>
<br>

### ExecQueryForChangeTable 
##### int ExecQueryForChangeTable(string sql, Dictionary<string, string> parameter)
INSERT  UPDATE  DELETE文用<br>
第一引数にsql、第二引数に連想配列でパラメーターをセット<br>
戻り値は更新されたレコードの数<br>
1以上ならみたいなif文書いて成功してるか確認してね<br>
<br>

## 注意事項
それぞれの第二引数の連想配列(Dictionary)は{“@param”: value}の形で作ってください<br>
sqlのパラメーター(@～)と同じ順番で連想配列を作ってください<br>
連想配列のkeyとvalueは文字列(string)にしてください<br>
変数名や関数名に文句つけないでください、貧弱英語力を振り絞ってつけてるんです
