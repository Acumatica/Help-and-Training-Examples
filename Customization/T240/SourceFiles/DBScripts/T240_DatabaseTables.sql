USE [PhoneRepairShop]
GO

/****** Object:  Table [dbo].[RSSVEmployeeWorkOrderQty]    Script Date: 25.04.2019 10:11:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVEmployeeWorkOrderQty](
	[CompanyID] [int] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[NbrOfAssignedOrders] [int] NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVEmployeeWorkOrderQty_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVEmployeeWorkOrderQty] ADD  DEFAULT ((0)) FOR [CompanyID]
GO


