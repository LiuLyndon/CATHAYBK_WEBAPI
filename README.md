# CATHAYBK 系統專案 (Lyndon)
此專案包含多個模組與功能，旨在提供高效且易於維護的 WebAPI 架構，並包含測試、規格文件與範本配置。

PS: global.json 為 mac 上的設定檔，win 需要將此檔案進行刪除

## 功能介紹
### 1. **BasicEIP_Core**
- 負責系統的基礎功能與核心邏輯。
- 包含通用邏輯、工具類別及跨層共用的模組。

### 2. **CATHAYBK_Model**
- 定義資料模型 (Entity) 與 DTO (Data Transfer Object)。
- 集中處理資料的結構化設計。

### 3. **CATHAYBK_Service**
- 提供業務邏輯層，處理資料的操作與邏輯。
- 確保業務層與資料層的分離，提升系統維護性。

### 4. **CATHAYBK_WEBAPI**
- 系統的 API 入口點。
- 使用 ASP.NET Core WebAPI 開發，支援 JWT 驗證。
- 遵循 RESTful API 設計原則。

### 5. **CATHAYBK_WEBAPI.Tests**
- 針對 `CATHAYBK_WEBAPI` 的單元測試與整合測試。
- 使用 xUnit 框架進行測試。

### 6. **Postmen**
- 包含 Postman 測試範本 (Collection)。
- 方便快速驗證 API 功能。

### 7. **Spec**
- 包含系統規格文件與資料庫設定。
- 提供詳細的資料表結構與關聯設計。

--------------------
## 完成項目如下

### 資料庫 
- SQL Server Express LocalDB（Entity Framework Core） 
- SQL 處理都會在 /logs/*.log
- Microsoft.EntityFrameworkCore.Database.Command

### 功能簡述
- 1. 呼叫 coindesk API，解析其下行內容與資料轉換，並實作新的 API。(已完成)
- API 路徑為 api/Coindesk/FetchAndSaveBitcoinData 
- 2. 建立一張幣別與其對應中文名稱的資料表（需附建立SQL語法），並提供查詢/新增/修改/刪除功能 API。(已完成)
- tblCurrency 幣別 (API/Coindesk)
- tblBitcoin 用來記錄比特幣的價格詳細數據 (API/Bitcoin)
- 3. 查詢幣別請依照幣別代碼排序。(已完成)
- OrderBy(c => c.Code)

### 所有功能均須包含單元測試。 
- CATHAYBK_WEBAPI.Tests (未完全)
- Postmen 有包含每支 API 的範例 (已完成)

### 印出所有 API 被呼叫以及呼叫外部 API 的 request and response body log (已完成)
- /logs/*.log
- 範例：BasicEIP_Core.Middleware.RequestLoggingMiddleware|HTTP Response|****

### Error handling 處理 API response (已完成)
- BasicEIP_Core/Middleware/ExceptionHandlingMiddleware
- 錯誤問題會統一在這邊進行處理

### swagger-ui (已完成)
- CATHAYBK_WEBAPI/Program.cs
- builder.Services.AddSwaggerGen 加上 Swagger

### 多語系設計 (未處理)

### design pattern 實作 (使用)
- 1. Repository Pattern (資料存取層)
- var bitcoins = await _bitcoinService.GetAllAsync();
- 2. Dependency Injection (依賴注入)
- public BitcoinController(
-     BitcoinService bitcoinService,
-     IAppLogger<BitcoinController> logger) : base(logger)
- {
-     _bitcoinService = bitcoinService;
- }
- 3. Singleton Pattern (單例模式)
- builder.Services.AddSingleton<AESService>(...);
- 4. Factory Method Pattern (工廠方法模式)
- var loggerFactory = LoggerFactory.Create(logging =>
- {
-      logging.ClearProviders();
-      logging.AddNLog();
- });

### 能夠運行在 Docker (未完成)
- docker-compose
- 有試著實作，但未完成，還沒有進行部署與測試

### 加解密技術應用 AES (已完成)
- CATHAYBK_WEBAPI/appsettings.json : AESConfig
- BitcoinAESController 進行簡單的 AES 只實作 GetBitcoins & GetBitcoinById
