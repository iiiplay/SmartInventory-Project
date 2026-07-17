# SmartInventory 智慧庫存管理系統

以 C# WinForms 開發的桌面庫存管理系統，串接雲端 MySQL（TiDB Cloud）資料庫，
提供商品 CRUD、搜尋篩選、庫存警示、統計圖表與 CSV 匯出等功能。

![登入畫面與主畫面截圖](screenshot/螢幕擷取畫面%202026-07-17%20110948.png)

## 功能特色

- **帳號登入**：登入畫面驗證帳號密碼，通過後才能進入主畫面（`LoginForm` → `MainForm`）。
- **商品 CRUD**：新增／修改／刪除商品，欄位包含名稱、分類、數量、單價，並自動計算小計（`TotalValue`）。
- **搜尋與分類篩選**：依商品名稱關鍵字、分類（電子／生活／文具／食品）即時篩選清單。
- **庫存警示**：一鍵切換顯示數量低於門檻（預設 10）的商品。
- **統計圖表**：依分類統計數量與總金額，繪製「庫存量直條圖」＋「總金額環形圖」，可在主畫面或獨立視窗（`ChartForm`）檢視。
- **CSV 匯出**：將目前所有商品資料匯出成 CSV 檔（UTF-16 編碼），檔名自動帶入時間戳記。
- **雲端資料庫**：透過 `appsettings.json` 設定連線字串，資料儲存於 TiDB Cloud（相容 MySQL 協定）。

## 技術棧

| 項目 | 說明 |
|---|---|
| 語言／框架 | C# / .NET 8.0 (`net8.0-windows`) |
| UI | Windows Forms |
| 資料庫 | TiDB Cloud（MySQL 相容）／`MySql.Data` |
| 設定讀取 | `Microsoft.Extensions.Configuration` + `Configuration.Json` |
| 圖表 | `WinForms.DataVisualization` |
| 本機資料庫（保留） | `Microsoft.Data.Sqlite`（早期章節示範用，見 `Data/DbHelper.cs`） |

## 專案結構

```
SmartInventory/
├─ Program.cs                 # 進入點：先顯示 LoginForm，登入成功才開 MainForm
├─ MainForm.cs / .Designer.cs # 主畫面：清單、搜尋、CRUD、圖表、匯出
├─ Forms/
│  ├─ LoginForm.cs            # 登入畫面
│  └─ ChartForm.cs            # 獨立統計圖表視窗
├─ Models/
│  └─ Product.cs              # 商品模型（Id/Name/Category/Quantity/Price/TotalValue）
├─ Services/
│  ├─ ProductService.cs       # 搜尋、統計、總價值、低庫存判斷
│  └─ ChartServices.cs        # 圖表繪製（直條圖＋環形圖）
├─ Data/
│  ├─ MySqlDbHelper.cs        # 雲端 MySQL/TiDB 存取（帳號驗證＋商品 CRUD）
│  └─ DbHelper.cs             # 本機 SQLite 存取（早期章節保留）
├─ appsettings.json           # 連線字串設定（未納入版本控制，需自行建立）
└─ screenshot/                # 執行畫面截圖
```

## 資料庫需求

TiDB／MySQL 資料庫需具備以下兩張表：

```sql
CREATE TABLE Accounts (
    Id INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(255),
    Password VARCHAR(255),
    PRIMARY KEY (Id)
);

CREATE TABLE Products (
    Id INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(255),
    Category VARCHAR(100),
    Quantity INT,
    Price DOUBLE,
    PRIMARY KEY (Id)
);
```

`Products` 表會由程式在啟動時自動建立（`MySqlDbHelper.InitDb()`），
但 `Accounts` 表需自行建立並手動新增至少一筆帳號資料，登入才會通過。

> 目前帳號密碼是以明文比對（`CheckAccount` 直接用 SQL 查詢比對 `Password` 欄位），
> 僅適合課程練習情境，正式環境請改用雜湊加鹽儲存密碼。

## 環境設定

1. 在專案根目錄建立 `appsettings.json`（此檔已加入 `.gitignore`，不會被提交）：

    ```json
    {
      "ConnectionStrings": {
        "TiDB": "Server=<你的伺服器位址>;Database=<資料庫名稱>;Uid=<帳號>;Pwd=<密碼>;Port=4000;SslMode=Preferred;"
      }
    }
    ```

2. 確認 `SmartInventory.csproj` 已將 `appsettings.json` 設定為 `CopyToOutputDirectory: Always`（已內建）。

## 建置與執行

需求：安裝 .NET 8 SDK（含 Windows Desktop 開發工作負載）。

```powershell
dotnet build
dotnet run
```

或直接以 Visual Studio 開啟 `SmartInventory.sln` 執行。

啟動後會先看到登入畫面，輸入 `Accounts` 表中已存在的帳號密碼即可進入主畫面。

## 操作說明

- **新增／修改**：填寫名稱、分類、數量、單價後按「新增」；點選清單中的列可將資料帶回輸入欄，修改後按「修改」。
- **刪除**：點選列後按「刪除」，會跳出確認視窗。
- **搜尋／篩選**：於搜尋框輸入關鍵字，或於分類下拉選單切換分類，清單即時更新。
- **庫存警示**：按「庫存警示」切換顯示數量 < 10 的商品，再按一次恢復完整清單。
- **統計圖表**：主畫面右側即時顯示分類統計圖；按「圖表」按鈕可開獨立視窗放大檢視。
- **匯出 CSV**：按「匯出」選擇存檔位置，會將目前所有商品資料輸出成 CSV。
