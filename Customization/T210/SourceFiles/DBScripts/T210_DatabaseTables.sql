USE [PhoneRepairShop]
GO

/****** Object:  Table [dbo].[RSSVRepairPrice]    Script Date: 25.04.2019 10:11:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVRepairPrice](
	[CompanyID] [int] NOT NULL,
	[PriceID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
	[Price] [decimal](19, 6) NOT NULL,
	[RepairItemLineCntr] [int] NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVRepairPrice_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[PriceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVRepairPrice] ADD  DEFAULT ((0)) FOR [CompanyID]
GO

/****** Object:  Table [dbo].[RSSVRepairItem]    Script Date: 25.04.2019 10:10:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVRepairItem](
	[CompanyID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[LineNbr] [int] NOT NULL,
	[RepairItemType] [nvarchar](2) NULL,
	[InventoryID] [int] NOT NULL,
	[Required] [bit] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[BasePrice] [decimal](19, 6) NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVRepairItem_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[ServiceID] ASC,
	[DeviceID] ASC,
	[LineNbr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVRepairItem] ADD  DEFAULT ((0)) FOR [CompanyID]
GO

/****** Object:  Table [dbo].[RSSVLabor]    Script Date: 25.04.2019 10:10:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVLabor](
	[CompanyID] [int] NOT NULL,
	[InventoryID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
	[DefaultPrice] [decimal](19, 6) NOT NULL,
	[Quantity] [decimal](19, 6) NOT NULL,
	[ExtPrice] [decimal](19, 6) NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVLabor_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[InventoryID] ASC,
	[DeviceID] ASC,
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVLabor] ADD  DEFAULT ((0)) FOR [CompanyID]
GO

/****** Object:  Table [dbo].[RSSVWarranty]    Script Date: 25.04.2019 10:12:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVWarranty](
	[CompanyID] [int] NOT NULL,
	[ContractID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
	[DefaultWarranty] [bit] NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVWarranty_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[ContractID] ASC,
	[DeviceID] ASC,
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVWarranty] ADD  DEFAULT ((0)) FOR [CompanyID]
GO

/****** Object:  Table [dbo].[RSSVStockItemDevice]    Script Date: 25.04.2019 10:11:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVStockItemDevice](
	[CompanyID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[InventoryID] [int] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVStockItemDevice_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[DeviceID] ASC,
	[InventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVStockItemDevice] ADD  DEFAULT ((0)) FOR [CompanyID]
GO



