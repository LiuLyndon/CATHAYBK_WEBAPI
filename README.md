# CATHAYBK 系統專案

此專案包含多個模組與功能，旨在提供高效且易於維護的 WebAPI 架構，並包含測試、規格文件與範本配置。

## 專案結構
CATHAYBK
├── BasicEIP_Core               // 核心功能與基礎邏輯層
├── CATHAYBK_Model              // 資料模型與 DTO 定義
├── CATHAYBK_Service            // 業務邏輯層
├── CATHAYBK_WEBAPI             // WebAPI 入口點
├── CATHAYBK_WEBAPI.Tests       // 測試專案，針對 CATHAYBK_WEBAPI
├── Postmen                     // 測試範本 (Postman Collection)
└── Spec                        // 規格文件與資料庫設定

---

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

---

## 系統需求

- **.NET Core 8.0 或更高版本**
- **SQL Server** (資料庫支援)
- **Postman** (API 測試工具)

---

## 環境設定

### 1. 資料庫配置
1. 確保您的環境已安裝 SQL Server。
2. 從 `Spec/Database/` 資料夾中找到對應的 `SQL Schema`，並執行腳本以初始化資料庫。

### 2. 啟動專案
1. 克隆專案：
   ```bash
   git clone <repo-url>
   cd CATHAYBK