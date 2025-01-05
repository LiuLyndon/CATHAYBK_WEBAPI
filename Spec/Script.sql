CREATE TABLE tblBitcoin
(
  id          int            NOT NULL IDENTITY(1,1),
  code        varchar(10)    NOT NULL,
  symbol      varchar(10)    NOT NULL,
  rate        DECIMAL(15, 6) NOT NULL,
  description varchar(100)   NOT NULL,
  rate_float  DECIMAL(15, 6) NOT NULL,
  CreatedaAt  datetime       NOT NULL,
  CONSTRAINT PK_tblBitcoin PRIMARY KEY (id)
)
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '用來記錄比特幣的價格詳細數據（不同貨幣）', 'user', dbo, 'table', 'tblBitcoin'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '唯一識別碼', 'user', dbo, 'table', 'tblBitcoin', 'column', 'id'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '貨幣代碼，例如 USD、GBP、EUR', 'user', dbo, 'table', 'tblBitcoin', 'column', 'code'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '貨幣符號，例如 $、£、€', 'user', dbo, 'table', 'tblBitcoin', 'column', 'symbol'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '格式化顯示的價格', 'user', dbo, 'table', 'tblBitcoin', 'column', 'rate'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '貨幣描述', 'user', dbo, 'table', 'tblBitcoin', 'column', 'description'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '浮點數格式的價格', 'user', dbo, 'table', 'tblBitcoin', 'column', 'rate_float'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '資料建立時間', 'user', dbo, 'table', 'tblBitcoin', 'column', 'CreatedaAt'
GO

CREATE TABLE tblCurrency
(
  id         int          NOT NULL IDENTITY(1,1),
  code       varchar(10)  NOT NULL,
  name       nvarchar(10),
  CreatedaAt datetime    ,
  UpdatedAt  datetime    ,
  CONSTRAINT PK_tblCurrency PRIMARY KEY (id)
)
GO

ALTER TABLE tblCurrency
  ADD CONSTRAINT UQ_code UNIQUE (code)
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '幣別', 'user', dbo, 'table', 'tblCurrency'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '幣別代碼', 'user', dbo, 'table', 'tblCurrency', 'column', 'code'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '幣別中文名稱', 'user', dbo, 'table', 'tblCurrency', 'column', 'name'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '資料建立時間', 'user', dbo, 'table', 'tblCurrency', 'column', 'CreatedaAt'
GO

EXECUTE sys.sp_addextendedproperty 'MS_Description',
  '資料更新時間', 'user', dbo, 'table', 'tblCurrency', 'column', 'UpdatedAt'
GO