@echo off

@echo on
curl "https://gateway-a.watsonplatform.net/visual-recognition/api/v3/classifiers?api_key=367b29a6797114577d8ac9f8c9335b0ca5a3baa2&version=2016-05-20"
@echo off

rem フリーでコレクションを確認する
rem curl -X POST -F "name=mahjong" "https://gateway-a.watsonplatform.net/visual-recognition/api/v3/collections?api_key=367b29a6797114577d8ac9f8c9335b0ca5a3baa2&version=2016-05-20"

set CLASSIFIER=mahjong_20519221
rem 削除する
rem curl -X DELETE "https://gateway-a.watsonplatform.net/visual-recognition/api/v3/classifiers/%CLASSIFIER%?api_key=367b29a6797114577d8ac9f8c9335b0ca5a3baa2&version=2016-05-20"
@echo off

set TON="C:\Users\seiyu\Documents\ComJanData\ton.zip"
set NAN="C:\Users\seiyu\Documents\ComJanData\nan.zip"
set SHA="C:\Users\seiyu\Documents\ComJanData\sha.zip"
set PEE="C:\Users\seiyu\Documents\ComJanData\pee.zip"
set HAk="C:\Users\seiyu\Documents\ComJanData\haku.zip"
set HAT="C:\Users\seiyu\Documents\ComJanData\hatu.zip"
set CHU="C:\Users\seiyu\Documents\ComJanData\chun.zip"
set MZ1="C:\Users\seiyu\Documents\ComJanData\MANZU1.zip"
set MZ2="C:\Users\seiyu\Documents\ComJanData\MANZU2.zip"
set MZ3="C:\Users\seiyu\Documents\ComJanData\MANZU3.zip"
set MZ4="C:\Users\seiyu\Documents\ComJanData\MANZU4.zip"
set MZ5="C:\Users\seiyu\Documents\ComJanData\MANZU5.zip"
set MZ6="C:\Users\seiyu\Documents\ComJanData\MANZU6.zip"
set MZ7="C:\Users\seiyu\Documents\ComJanData\MANZU7.zip"
set MZ8="C:\Users\seiyu\Documents\ComJanData\MANZU8.zip"
set MZ9="C:\Users\seiyu\Documents\ComJanData\MANZU9.zip"
set PZ1="C:\Users\seiyu\Documents\ComJanData\PINZU1.zip"
set PZ2="C:\Users\seiyu\Documents\ComJanData\PINZU2.zip"
set PZ3="C:\Users\seiyu\Documents\ComJanData\PINZU3.zip"
set PZ4="C:\Users\seiyu\Documents\ComJanData\PINZU4.zip"
set PZ5="C:\Users\seiyu\Documents\ComJanData\PINZU5.zip"
set PZ6="C:\Users\seiyu\Documents\ComJanData\PINZU6.zip"
set PZ7="C:\Users\seiyu\Documents\ComJanData\PINZU7.zip"
set PZ8="C:\Users\seiyu\Documents\ComJanData\PINZU8.zip"
set PZ9="C:\Users\seiyu\Documents\ComJanData\PINZU9.zip"
set SZ1="C:\Users\seiyu\Documents\ComJanData\SOUZU1.zip"
set SZ2="C:\Users\seiyu\Documents\ComJanData\SOUZU2.zip"
set SZ3="C:\Users\seiyu\Documents\ComJanData\SOUZU3.zip"
set SZ4="C:\Users\seiyu\Documents\ComJanData\SOUZU4.zip"
set SZ5="C:\Users\seiyu\Documents\ComJanData\SOUZU5.zip"
set SZ6="C:\Users\seiyu\Documents\ComJanData\SOUZU6.zip"
set SZ7="C:\Users\seiyu\Documents\ComJanData\SOUZU7.zip"
set SZ8="C:\Users\seiyu\Documents\ComJanData\SOUZU8.zip"
set SZ9="C:\Users\seiyu\Documents\ComJanData\SOUZU9.zip"
rem @echo on
rem 学習
rem curl -X POST -F "ton_positive_examples=@%TON%" -F "nan_positive_examples=@%NAN%" -F "sha_positive_examples=@%SHA%" -F "pee_positive_examples=@%PEE%" -F "haku_positive_examples=@%HAK%" -F "hatu_positive_examples=@%HAT%" -F "chun_positive_examples=@%CHU%" -F "manzu1_positive_examples=@%MZ1%" -F "manzu2_positive_examples=@%MZ2%" -F "manzu3_positive_examples=@%MZ3%" -F "manzu4_positive_examples=@%MZ4%" -F "manzu5_positive_examples=@%MZ5%" -F "manzu6_positive_examples=@%MZ6%" -F "manzu7_positive_examples=@%MZ7%" -F "manzu8_positive_examples=@%MZ8%" -F "manzu9_positive_examples=@%MZ9%" -F "pinzu1_positive_examples=@%PZ1%" -F "pinzu2_positive_examples=@%PZ2%" -F "pinzu3_positive_examples=@%PZ3%" -F "pinzu4_positive_examples=@%PZ4%" -F "pinzu5_positive_examples=@%PZ5%" -F "pinzu6_positive_examples=@%PZ6%" -F "pinzu7_positive_examples=@%PZ7%" -F "pinzu8_positive_examples=@%PZ8%" -F "pinzu9_positive_examples=@%PZ9%" -F "souzu1_positive_examples=@%SZ1%" -F "souzu2_positive_examples=@%SZ2%" -F "souzu3_positive_examples=@%SZ3%" -F "souzu4_positive_examples=@%SZ4%" -F "souzu5_positive_examples=@%SZ5%" -F "souzu6_positive_examples=@%SZ6%" -F "souzu7_positive_examples=@%SZ7%" -F "souzu8_positive_examples=@%SZ8%" -F "souzu9_positive_examples=@%SZ9%" -F "name=mahjong" "https://gateway-a.watsonplatform.net/visual-recognition/api/v3/classifiers?api_key=367b29a6797114577d8ac9f8c9335b0ca5a3baa2&version=2016-05-20"
@echo off

rem 評価
rem curl.exe -X POST -F "images_file=@"C:\Users\seiyu\Documents\ComJanData\m150813540.jpg"" -F "parameters=@"C:\Users\seiyu\Documents\ComJanData\myparams.json"" "https://gateway-a.watsonplatform.net/visual-recognition/api/v3/classify?api_key=367b29a6797114577d8ac9f8c9335b0ca5a3baa2&version=2016-05-20" 

rem 追加学習
rem curl -X POST -F "manzu2_positive_examples=@manzu2_1.zip" -F "manzu3_positive_examples=@manzu3_1.zip" "https://gateway-a.watsonplatform.net/visual-recognition/api/v3/classifiers/mahjong_831931708?api_key=367b29a6797114577d8ac9f8c9335b0ca5a3baa2&version=2016-05-20"

