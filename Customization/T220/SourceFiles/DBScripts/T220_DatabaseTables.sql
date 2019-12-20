USE [PhoneRepairShop]
GO

/****** Object:  Table [dbo].[RSSVWorkOrder]    Script Date: 25.04.2019 10:11:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVWorkOrder](
	[CompanyID] [int] NOT NULL,
	[OrderNbr] [nvarchar](15) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[DateCreated] [datetime2](0) NOT NULL,
	[DateCompleted] [datetime2](0),
	[Status] [nvarchar](2) NOT NULL,
	[Hold] [bit] NOT NULL,
	[Description] [nvarchar](60),
	[DeviceID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
	[OrderTotal] [decimal](19, 4) NOT NULL,
	[RepairItemLineCntr] [int] NOT NULL,
	[Assignee] [uniqueidentifier],
	[Priority] [nvarchar](1),
	[InvoiceNbr] [nvarchar](15) NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVWorkOrder_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[OrderNbr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVWorkOrder] ADD  DEFAULT ((0)) FOR [CompanyID]
GO

/****** Object:  Table [dbo].[RSSVWorkOrderItem]    Script Date: 25.04.2019 10:10:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVWorkOrderItem](
	[CompanyID] [int] NOT NULL,
	[OrderNbr] [nvarchar](15) NOT NULL,
	[ServiceID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[LineNbr] [int] NOT NULL,
	[RepairItemType] [nvarchar](2) NULL,
	[InventoryID] [int] NOT NULL,
	[BasePrice] [decimal](19, 6) NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVWorkOrderItem_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[OrderNbr] ASC,
	[LineNbr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVWorkOrderItem] ADD  DEFAULT ((0)) FOR [CompanyID]
GO

/****** Object:  Table [dbo].[RSSVWorkOrderLabor]    Script Date: 25.04.2019 10:10:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVWorkOrderLabor](
	[CompanyID] [int] NOT NULL,
	[InventoryID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
	[OrderNbr] [nvarchar](15) NOT NULL,
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
 CONSTRAINT [RSSVWorkOrderLabor_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[InventoryID] ASC,
	[OrderNbr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVWorkOrderLabor] ADD  DEFAULT ((0)) FOR [CompanyID]
GO

/****** Object:  Table [dbo].[RSSVSetup]    Script Date: 25.04.2019 10:11:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVSetup](
	[CompanyID] [int] NOT NULL,
	[NumberingID] [nvarchar](10) NOT NULL,
	[WalkInCustomerID] [int] NOT NULL,
	[DefaultEmployee] [uniqueidentifier] NOT NULL,
	[PrepaymentPercent] [decimal](9, 6) NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVSetup_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVSetup] ADD  DEFAULT ((0)) FOR [CompanyID]
GO



