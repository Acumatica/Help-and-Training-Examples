USE [SmartFix_T200]
GO

/****** Object:  Table [dbo].[RSSVRepairService]    Script Date: 7/3/2019 4:08:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVRepairService](
	[CompanyID] [int] NOT NULL,
	[ServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceCD] [nvarchar](15) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
	[WalkInService] [bit] NOT NULL,
	[PreliminaryCheck] [bit] NOT NULL,
	[Prepayment] [bit] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RSSVRepairService] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVRepairService] ADD  CONSTRAINT [DF_RSSVRepairService_CompanyID]  DEFAULT ((0)) FOR [CompanyID]
GO

CREATE UNIQUE NONCLUSTERED INDEX [RSSVRepairService_ServiceCD_Uindex] ON [dbo].[RSSVRepairService]
(
	[CompanyID] ASC,
	[ServiceCD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[RSSVDevice]    Script Date: 25.04.2019 10:09:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RSSVDevice](
	[CompanyID] [int] NOT NULL,
	[DeviceID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceCD] [nvarchar](15) NOT NULL,
	[Description] [nvarchar](256) NULL,
	[Active] [bit] NOT NULL,
	[AvgComplexityOfRepair] [char](1) NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NULL,
 CONSTRAINT [RSSVDevice_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[DeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RSSVDevice] ADD  DEFAULT ((0)) FOR [CompanyID]
GO

CREATE UNIQUE NONCLUSTERED INDEX [RSSVDevice_DeviceCD_Uindex] ON [dbo].[RSSVDevice]
(
	[CompanyID] ASC,
	[DeviceCD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
