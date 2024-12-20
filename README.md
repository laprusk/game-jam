## コミット
変更内容をgitで追跡し、確定する。
```
git add .
git commit -m "コミットメッセージをここに書く"
```

## プッシュ
変更内容をリモートのGitHubへ反映させる。
```
git push origin main:{ブランチ名}
```
例: hogeブランチへプッシュ
```
git push origin main:hoge
```
プッシュした後はプルリクエストを送信してマージする。

## プル
リモートのGitHubの更新内容をローカル環境へ反映させる。
こまめにプルしておくとコンフリクトしづらくなる。
```
git pull
```
