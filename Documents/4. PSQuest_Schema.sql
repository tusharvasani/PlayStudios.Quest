USE [PSQuests]
GO
/****** Object:  Table [dbo].[PlayerQuestProgress]    Script Date: 12/12/2021 1:39:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerQuestProgress](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [varchar](50) NOT NULL,
	[QuestId] [varchar](10) NULL,
	[PlayerLevel] [int] NOT NULL,
	[ChipAmountBet] [int] NOT NULL,
	[QuestPointsEarned] [int] NOT NULL,
	[InsertDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_PlayerQuestProgress] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayerQuestState]    Script Date: 12/12/2021 1:39:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerQuestState](
	[PlayerId] [varchar](50) NOT NULL,
	[QuestId] [varchar](10) NOT NULL,
	[TotalQuestPercentCompleted] [decimal](18, 2) NOT NULL,
	[LastMilestoneIndexCompleted] [int] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_PlayerQuestState] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[QuestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PlayerQuestProgress] ADD  CONSTRAINT [DF__PlayerQue__Playe__4316F928]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [PlayerId]
GO
ALTER TABLE [dbo].[PlayerQuestProgress] ADD  CONSTRAINT [DF_PlayerQuestProgress_QuestPointsEarned]  DEFAULT ((0)) FOR [QuestPointsEarned]
GO
