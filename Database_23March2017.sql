USE [master]
GO
/****** Object:  Database [Papaya]    Script Date: 03/24/2017 02:40:30 ******/
CREATE DATABASE [Papaya] ON  PRIMARY 
( NAME = N'PapayaDb', FILENAME = N'C:\\PAPAYADB\PapayaDb.mdf' , SIZE = 6144KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PapayaDb_log', FILENAME = N'C:\\PAPAYADB\PapayaDb_1.ldf' , SIZE = 8384KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Papaya] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Papaya].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Papaya] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Papaya] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Papaya] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Papaya] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Papaya] SET ARITHABORT OFF
GO
ALTER DATABASE [Papaya] SET AUTO_CLOSE ON
GO
ALTER DATABASE [Papaya] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [Papaya] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Papaya] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Papaya] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Papaya] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Papaya] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Papaya] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Papaya] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Papaya] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Papaya] SET  DISABLE_BROKER
GO
ALTER DATABASE [Papaya] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Papaya] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Papaya] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Papaya] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Papaya] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Papaya] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Papaya] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [Papaya] SET  READ_WRITE
GO
ALTER DATABASE [Papaya] SET RECOVERY SIMPLE
GO
ALTER DATABASE [Papaya] SET  MULTI_USER
GO
ALTER DATABASE [Papaya] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Papaya] SET DB_CHAINING OFF
GO
ALTER DATABASE [Papaya] SET CHANGE_TRACKING = ON (CHANGE_RETENTION = 30 DAYS,AUTO_CLEANUP = ON)
GO
USE [Papaya]
GO
/****** Object:  User [admin]    Script Date: 03/24/2017 02:40:30 ******/
CREATE USER [admin] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[rs_syslog]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_syslog](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[ClientIP] [varchar](20) NULL,
	[Controller] [varchar](60) NULL,
	[Action] [varchar](60) NULL,
	[Operation] [varchar](20) NULL,
	[Description] [varchar](100) NULL,
	[User] [varchar](20) NULL,
	[LogTime] [datetime] NULL,
 CONSTRAINT [PK_se_syslog] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_syslog] ON
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9737, N'::1', N'Home', N'Login', N'Login', N'Failed to login, user [papaya]', N'papaya', CAST(0x0000A71D00EA8E6A AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9738, N'::1', N'Home', N'Login', N'Login', N'Failed to login, user [papaya]', N'papaya', CAST(0x0000A71D00EA98E4 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9739, N'::1', N'Home', N'Login', N'Login', N'Failed to login, user [papaya]', N'papaya', CAST(0x0000A71D00EA9D9A AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9740, N'::1', N'Home', N'Login', N'Login', N'Failed to login, user [papaya]', N'papaya', CAST(0x0000A71D00EAEA52 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9741, N'::1', N'Home', N'Login', N'Login', N'Failed to login, user [papaya]', N'papaya', CAST(0x0000A71D00EAEF68 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9742, N'::1', N'Home', N'Login', N'Login', N'Failed to login, user [papaya]', N'papaya', CAST(0x0000A71D00EC2D45 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9743, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A71D00EC9820 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9744, N'::1', N'Module', N'Create', N'Add', N'New Module [Name:Assets]', N'papaya', CAST(0x0000A71D00EFDE66 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9745, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A72000CFE21C AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9746, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A72200C9F92C AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9747, N'::1', N'Action', N'Delete', N'Delete', N'Delete Action - [ID:94, Name:Index]', N'papaya', CAST(0x0000A72200F38ED9 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9748, N'::1', N'Action', N'Delete', N'Delete', N'Delete Action - [ID:525, Name:Upload]', N'papaya', CAST(0x0000A72200F39763 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9749, N'::1', N'Action', N'Delete', N'Delete', N'Delete Action - [ID:524, Name:Index]', N'papaya', CAST(0x0000A72200F39E0A AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9750, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A72F00D88094 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9751, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A73001233C5E AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9752, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A731011F1F1B AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9753, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A73200A476C3 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9754, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A73300096A24 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9755, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A73300096AD2 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9756, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A73500121EFE AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9757, N'::1', N'Module', N'Create', N'Add', N'New Module [Name:Booking]', N'papaya', CAST(0x0000A73500125B46 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9758, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A73C00FF60C8 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9759, N'::1', N'Home', N'Login', N'Login', N'Failed to login, user [papaya]', N'papaya', CAST(0x0000A73E00FBF187 AS DateTime))
INSERT [dbo].[rs_syslog] ([LogId], [ClientIP], [Controller], [Action], [Operation], [Description], [User], [LogTime]) VALUES (9760, N'::1', N'Home', N'Login', N'Login', N'Logged in with user [papaya]', N'papaya', CAST(0x0000A73E00FBFE0D AS DateTime))
SET IDENTITY_INSERT [dbo].[rs_syslog] OFF
/****** Object:  Table [dbo].[rs_ownership]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_ownership](
	[OwnerShipId] [int] IDENTITY(1,1) NOT NULL,
	[OwnerType] [varchar](250) NOT NULL,
	[Description] [varchar](250) NOT NULL,
 CONSTRAINT [PK_rs_ownership] PRIMARY KEY CLUSTERED 
(
	[OwnerShipId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_ownership] ON
INSERT [dbo].[rs_ownership] ([OwnerShipId], [OwnerType], [Description]) VALUES (1, N'RSMY', N'RSMY Owned')
SET IDENTITY_INSERT [dbo].[rs_ownership] OFF
/****** Object:  Table [dbo].[rs_module]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_module](
	[ModuleId] [int] IDENTITY(1,1) NOT NULL,
	[ParentModuleId] [int] NULL,
	[Name] [varchar](100) NULL,
	[Controller] [varchar](100) NULL,
	[HaveChild] [bit] NULL,
	[MenuOrder] [int] NULL,
	[ModuleOrder] [int] NULL,
	[IsBackEnd] [bit] NOT NULL,
	[FlagActive] [bit] NOT NULL,
	[UserEntry] [varchar](30) NULL,
	[DateEntry] [datetime] NULL,
	[UserUpdate] [varchar](30) NULL,
	[DateUpdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_module] ON
INSERT [dbo].[rs_module] ([ModuleId], [ParentModuleId], [Name], [Controller], [HaveChild], [MenuOrder], [ModuleOrder], [IsBackEnd], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (8, 106, N'Module', N'Module', 0, 1, 6, 1, 1, N'schmidt', CAST(0x0000A16500C04C4E AS DateTime), N'schmidt', CAST(0x0000A16500C04C4E AS DateTime))
INSERT [dbo].[rs_module] ([ModuleId], [ParentModuleId], [Name], [Controller], [HaveChild], [MenuOrder], [ModuleOrder], [IsBackEnd], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (9, 8, N'Action', N'Action', 0, 1, 1, 1, 1, N'schmidt', CAST(0x0000A16500C04C4F AS DateTime), N'schmidt', CAST(0x0000A16500C04C4F AS DateTime))
INSERT [dbo].[rs_module] ([ModuleId], [ParentModuleId], [Name], [Controller], [HaveChild], [MenuOrder], [ModuleOrder], [IsBackEnd], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (106, NULL, N'System Management', N'Header', 0, 0, 998, 1, 1, N'schmidt', CAST(0x0000A16500C04C61 AS DateTime), N'schmidt', CAST(0x0000A16500C04C61 AS DateTime))
INSERT [dbo].[rs_module] ([ModuleId], [ParentModuleId], [Name], [Controller], [HaveChild], [MenuOrder], [ModuleOrder], [IsBackEnd], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (171, NULL, N'Access', N'Header', NULL, NULL, 4, 1, 1, N'snyder', CAST(0x0000A66A00D99728 AS DateTime), NULL, NULL)
INSERT [dbo].[rs_module] ([ModuleId], [ParentModuleId], [Name], [Controller], [HaveChild], [MenuOrder], [ModuleOrder], [IsBackEnd], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (172, 171, N'User', N'User', NULL, NULL, 1, 1, 1, N'snyder', CAST(0x0000A66A00D9C1B9 AS DateTime), NULL, NULL)
INSERT [dbo].[rs_module] ([ModuleId], [ParentModuleId], [Name], [Controller], [HaveChild], [MenuOrder], [ModuleOrder], [IsBackEnd], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (173, 171, N'User Group', N'UserGroup', NULL, NULL, 1, 1, 1, N'snyder', CAST(0x0000A66A00D9D8F2 AS DateTime), NULL, NULL)
INSERT [dbo].[rs_module] ([ModuleId], [ParentModuleId], [Name], [Controller], [HaveChild], [MenuOrder], [ModuleOrder], [IsBackEnd], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (182, NULL, N'Assets', N'Assets', NULL, NULL, NULL, 1, 1, N'papaya', CAST(0x0000A71D00EFDE5F AS DateTime), NULL, NULL)
INSERT [dbo].[rs_module] ([ModuleId], [ParentModuleId], [Name], [Controller], [HaveChild], [MenuOrder], [ModuleOrder], [IsBackEnd], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (183, NULL, N'Booking', N'Bookings', NULL, NULL, NULL, 1, 1, N'papaya', CAST(0x0000A73500125B37 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[rs_module] OFF
/****** Object:  Table [dbo].[rs_locations]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_locations](
	[LocationId] [int] IDENTITY(1,1) NOT NULL,
	[LocationName] [varchar](250) NOT NULL,
	[InternalName] [varchar](250) NOT NULL,
	[LocationCode] [varchar](250) NOT NULL,
 CONSTRAINT [PK_se_locations] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_locations] ON
INSERT [dbo].[rs_locations] ([LocationId], [LocationName], [InternalName], [LocationCode]) VALUES (1, N'Test21', N'Test', N'12312')
INSERT [dbo].[rs_locations] ([LocationId], [LocationName], [InternalName], [LocationCode]) VALUES (2, N'EEE', N'EEE', N'EEE')
SET IDENTITY_INSERT [dbo].[rs_locations] OFF
/****** Object:  Table [dbo].[rs_user_group]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_user_group](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](50) NULL,
	[FlagActive] [bit] NOT NULL,
	[UserEntry] [varchar](30) NULL,
	[DateEntry] [datetime] NULL,
	[UserUpdate] [varchar](30) NULL,
	[DateUpdate] [datetime] NULL,
 CONSTRAINT [PK_se_user_group] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_user_group] ON
INSERT [dbo].[rs_user_group] ([GroupId], [Name], [Description], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (1, N'System Administrators', NULL, 1, NULL, NULL, N'schmidt', CAST(0x0000A58901554400 AS DateTime))
INSERT [dbo].[rs_user_group] ([GroupId], [Name], [Description], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate]) VALUES (18, N'Generic Users', N'Generic Users', 1, N'snyder', CAST(0x0000A6A500E7FD31 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[rs_user_group] OFF
/****** Object:  Table [dbo].[rs_division]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_division](
	[DivId] [int] IDENTITY(1,1) NOT NULL,
	[DivisionNo] [int] NOT NULL,
	[Description] [varchar](250) NOT NULL,
	[MarketSegments] [varchar](250) NULL,
 CONSTRAINT [PK_se_division] PRIMARY KEY CLUSTERED 
(
	[DivId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_division] ON
INSERT [dbo].[rs_division] ([DivId], [DivisionNo], [Description], [MarketSegments]) VALUES (1, 1, N'Test & Measurement', NULL)
SET IDENTITY_INSERT [dbo].[rs_division] OFF
/****** Object:  Table [dbo].[rs_dates]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_dates](
	[DateId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Day] [int] NOT NULL,
	[DayOfWeek] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[MonthName] [varchar](50) NOT NULL,
	[Quarter] [int] NOT NULL,
	[QuarterName] [varchar](50) NOT NULL,
	[Year] [int] NOT NULL,
 CONSTRAINT [PK_se_dates] PRIMARY KEY CLUSTERED 
(
	[DateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rs_company]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_company](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[Address] [varchar](200) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[FaxNumber] [varchar](20) NULL,
	[CompanyUrl] [varchar](50) NULL,
	[FlagActive] [bit] NOT NULL,
	[UserEntry] [varchar](30) NULL,
	[DateEntry] [datetime] NULL,
	[UserUpdate] [varchar](30) NULL,
	[DateUpdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rs_assetstatus]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_assetstatus](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[Status] [varchar](50) NOT NULL,
 CONSTRAINT [PK_rs_assetstatus] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_assetstatus] ON
INSERT [dbo].[rs_assetstatus] ([StatusId], [Status]) VALUES (1, N'Available')
INSERT [dbo].[rs_assetstatus] ([StatusId], [Status]) VALUES (2, N'Damaged')
INSERT [dbo].[rs_assetstatus] ([StatusId], [Status]) VALUES (3, N'Blocked')
INSERT [dbo].[rs_assetstatus] ([StatusId], [Status]) VALUES (4, N'Repair')
INSERT [dbo].[rs_assetstatus] ([StatusId], [Status]) VALUES (5, N'Calibration')
SET IDENTITY_INSERT [dbo].[rs_assetstatus] OFF
/****** Object:  Table [dbo].[rs_action]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_action](
	[ActionId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NULL,
	[Name] [varchar](100) NULL,
	[FlagActive] [bit] NOT NULL,
	[UserEntry] [varchar](30) NULL,
	[DateEntry] [datetime] NULL,
	[UserUpdate] [varchar](30) NULL,
	[DateUpdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rs_user]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_user](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](20) NOT NULL,
	[Password] [varchar](32) NOT NULL,
	[FullName] [varchar](50) NOT NULL,
	[MobileNumber] [varchar](20) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[UserType] [varchar](50) NOT NULL,
	[IsBackEnd] [bit] NOT NULL,
	[FlagActive] [bit] NOT NULL,
	[UserEntry] [varchar](30) NULL,
	[DateEntry] [datetime] NULL,
	[UserUpdate] [varchar](30) NULL,
	[DateUpdate] [datetime] NULL,
	[DivId] [int] NOT NULL,
 CONSTRAINT [PK_se_user] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[rs_user] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_user] ON
INSERT [dbo].[rs_user] ([UserId], [Username], [Password], [FullName], [MobileNumber], [Email], [EmployeeId], [GroupId], [UserType], [IsBackEnd], [FlagActive], [UserEntry], [DateEntry], [UserUpdate], [DateUpdate], [DivId]) VALUES (1, N'papaya', N'mqNccON9agJBU3FRKJDA+A==', N'Administrator', N'0123', N'abc', 11, 1, N'User', 1, 1, NULL, NULL, N'snyder', CAST(0x0000A6A500EB7A44 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[rs_user] OFF
/****** Object:  Table [dbo].[rs_useracl]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_useracl](
	[UserAclId] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NULL,
	[ActionId] [int] NULL,
	[FlagActive] [bit] NOT NULL,
	[UserEntry] [varchar](30) NULL,
	[DateEntry] [datetime] NULL,
	[UserUpdate] [varchar](30) NULL,
	[DateUpdate] [datetime] NULL,
 CONSTRAINT [PK__se_usera__CBB7AA8F276EDEB3] PRIMARY KEY CLUSTERED 
(
	[UserAclId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rs_smslogs]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_smslogs](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[ReceiverId] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Action] [varchar](50) NOT NULL,
	[Message] [varchar](150) NOT NULL,
 CONSTRAINT [PK_se_smslogs] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rs_loan_form]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_loan_form](
	[LoanId] [int] IDENTITY(1,1) NOT NULL,
	[RequestNo] [int] NOT NULL,
	[RequestorId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Company] [varchar](250) NOT NULL,
	[ContactPersion] [varchar](250) NOT NULL,
	[Telephone] [varchar](250) NULL,
	[Email] [varchar](50) NOT NULL,
	[Address] [varchar](250) NOT NULL,
	[Purpose] [varchar](250) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[Invalid] [bit] NOT NULL,
	[Returned] [bit] NOT NULL,
	[Signed] [bit] NOT NULL,
	[UploadedFile] [varchar](255) NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_se_loan_form] PRIMARY KEY CLUSTERED 
(
	[LoanId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rs_assets]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_assets](
	[AssetId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NOT NULL,
	[Brand] [varchar](50) NOT NULL,
	[Model] [varchar](50) NOT NULL,
	[Desciption] [varchar](150) NOT NULL,
	[SerialNumber] [varchar](50) NOT NULL,
	[MaterialNo] [varchar](50) NOT NULL,
	[HardwareOpt] [varchar](255) NOT NULL,
	[SoftwareOpt] [varchar](255) NOT NULL,
	[HardwareVer] [varchar](255) NOT NULL,
	[SoftwareVer] [varchar](255) NOT NULL,
	[LicenseExpiry] [date] NULL,
	[Accessories] [varchar](255) NOT NULL,
	[Remarks] [varchar](255) NOT NULL,
	[PurchaseDate] [date] NOT NULL,
	[PurchasePrice] [decimal](18, 0) NULL,
	[DecomDate] [date] NULL,
	[DecomReason] [varchar](255) NULL,
	[Tagged] [bit] NOT NULL,
	[Damaged] [bit] NOT NULL,
	[LastCalibrated] [date] NULL,
	[CalibrationCycle] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[OwnerShipId] [int] NOT NULL,
	[OwnedBy] [int] NOT NULL,
	[TrackingNo] [varchar](50) NOT NULL,
	[ReadyToSell] [bit] NOT NULL,
	[Availability] [int] NOT NULL,
	[OriginLocId] [int] NOT NULL,
	[CurrentLocId] [int] NOT NULL,
	[PurchasePO] [varchar](50) NULL,
	[DepreciationFormula] [varchar](255) NOT NULL,
	[ImageLink] [varchar](255) NULL,
	[ViewedStats] [bigint] NOT NULL,
	[BookingStats] [bigint] NOT NULL,
	[FeaturedOrder] [int] NOT NULL,
	[Featured] [bit] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[DivId] [int] NOT NULL,
 CONSTRAINT [PK_se_assets] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_assets] ON
INSERT [dbo].[rs_assets] ([AssetId], [ParentId], [Brand], [Model], [Desciption], [SerialNumber], [MaterialNo], [HardwareOpt], [SoftwareOpt], [HardwareVer], [SoftwareVer], [LicenseExpiry], [Accessories], [Remarks], [PurchaseDate], [PurchasePrice], [DecomDate], [DecomReason], [Tagged], [Damaged], [LastCalibrated], [CalibrationCycle], [CreatedDate], [UpdatedDate], [CreatedBy], [OwnerShipId], [OwnedBy], [TrackingNo], [ReadyToSell], [Availability], [OriginLocId], [CurrentLocId], [PurchasePO], [DepreciationFormula], [ImageLink], [ViewedStats], [BookingStats], [FeaturedOrder], [Featured], [IsSystem], [DivId]) VALUES (7, 0, N'rfg', N'fd', N'df', N'fd', N'df', N'fddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd', N'h', N'dfgdf', N'dfhdf', CAST(0x683C0B00 AS Date), N'fg', N'e5tetee', CAST(0x75250B00 AS Date), CAST(233 AS Decimal(18, 0)), CAST(0x033B0B00 AS Date), N'tyhhrthrhr', 0, 0, CAST(0x693C0B00 AS Date), 1, CAST(0x0000A72000FC204E AS DateTime), CAST(0x0000A72000FEA9CC AS DateTime), 0, 1, 1, N'3rwe', 0, 1, 1, 1, N'314', N'12', N'14', 0, 0, 0, 0, 0, 1)
INSERT [dbo].[rs_assets] ([AssetId], [ParentId], [Brand], [Model], [Desciption], [SerialNumber], [MaterialNo], [HardwareOpt], [SoftwareOpt], [HardwareVer], [SoftwareVer], [LicenseExpiry], [Accessories], [Remarks], [PurchaseDate], [PurchasePrice], [DecomDate], [DecomReason], [Tagged], [Damaged], [LastCalibrated], [CalibrationCycle], [CreatedDate], [UpdatedDate], [CreatedBy], [OwnerShipId], [OwnedBy], [TrackingNo], [ReadyToSell], [Availability], [OriginLocId], [CurrentLocId], [PurchasePO], [DepreciationFormula], [ImageLink], [ViewedStats], [BookingStats], [FeaturedOrder], [Featured], [IsSystem], [DivId]) VALUES (11, 0, N'fgsd', N'fsdasdfs', N'fasfasd', N'dasdas', N'dasdasdas', N'dasd', N'12', N'21', N'2', CAST(0x683C0B00 AS Date), N'12', N'21', CAST(0x75250B00 AS Date), CAST(12 AS Decimal(18, 0)), CAST(0x683C0B00 AS Date), N'23123', 0, 0, CAST(0x683C0B00 AS Date), 123, CAST(0x0000A7200108D752 AS DateTime), CAST(0x0000A7200108D752 AS DateTime), 0, 1, 1, N'34123', 0, 1, 1, 1, N'132', N'213', N'123', 0, 0, 0, 0, 0, 1)
INSERT [dbo].[rs_assets] ([AssetId], [ParentId], [Brand], [Model], [Desciption], [SerialNumber], [MaterialNo], [HardwareOpt], [SoftwareOpt], [HardwareVer], [SoftwareVer], [LicenseExpiry], [Accessories], [Remarks], [PurchaseDate], [PurchasePrice], [DecomDate], [DecomReason], [Tagged], [Damaged], [LastCalibrated], [CalibrationCycle], [CreatedDate], [UpdatedDate], [CreatedBy], [OwnerShipId], [OwnedBy], [TrackingNo], [ReadyToSell], [Availability], [OriginLocId], [CurrentLocId], [PurchasePO], [DepreciationFormula], [ImageLink], [ViewedStats], [BookingStats], [FeaturedOrder], [Featured], [IsSystem], [DivId]) VALUES (12, 0, N'abc', N'123', N'123', N'123', N'123', N'123', N'123', N'123', N'123', CAST(0x683C0B00 AS Date), N'123', N'123', CAST(0x76250B00 AS Date), CAST(123 AS Decimal(18, 0)), CAST(0x693C0B00 AS Date), N'123', 0, 0, CAST(0x693C0B00 AS Date), 123, CAST(0x0000A72200000000 AS DateTime), CAST(0x0000A72200000000 AS DateTime), 0, 1, 1, N'123', 0, 1, 1, 1, N'123', N'123', N'123', 0, 0, 0, 0, 1, 1)
INSERT [dbo].[rs_assets] ([AssetId], [ParentId], [Brand], [Model], [Desciption], [SerialNumber], [MaterialNo], [HardwareOpt], [SoftwareOpt], [HardwareVer], [SoftwareVer], [LicenseExpiry], [Accessories], [Remarks], [PurchaseDate], [PurchasePrice], [DecomDate], [DecomReason], [Tagged], [Damaged], [LastCalibrated], [CalibrationCycle], [CreatedDate], [UpdatedDate], [CreatedBy], [OwnerShipId], [OwnedBy], [TrackingNo], [ReadyToSell], [Availability], [OriginLocId], [CurrentLocId], [PurchasePO], [DepreciationFormula], [ImageLink], [ViewedStats], [BookingStats], [FeaturedOrder], [Featured], [IsSystem], [DivId]) VALUES (13, 0, N'123123', N'123123', N'123123', N'123123', N'123123', N'123123', N'123123', N'123123', N'123123', CAST(0x7D3C0B00 AS Date), N'123123', N'123123', CAST(0x00000000 AS Date), CAST(123123 AS Decimal(18, 0)), CAST(0x7D3C0B00 AS Date), N'123123', 0, 0, CAST(0x7D3C0B00 AS Date), 123123, CAST(0x0000A72200E17335 AS DateTime), CAST(0x0000A72200E17335 AS DateTime), 0, 1, 1, N'123123', 0, 1, 1, 1, N'123123', N'123123', N'new  2.txt', 0, 0, 0, 0, 0, 1)
INSERT [dbo].[rs_assets] ([AssetId], [ParentId], [Brand], [Model], [Desciption], [SerialNumber], [MaterialNo], [HardwareOpt], [SoftwareOpt], [HardwareVer], [SoftwareVer], [LicenseExpiry], [Accessories], [Remarks], [PurchaseDate], [PurchasePrice], [DecomDate], [DecomReason], [Tagged], [Damaged], [LastCalibrated], [CalibrationCycle], [CreatedDate], [UpdatedDate], [CreatedBy], [OwnerShipId], [OwnedBy], [TrackingNo], [ReadyToSell], [Availability], [OriginLocId], [CurrentLocId], [PurchasePO], [DepreciationFormula], [ImageLink], [ViewedStats], [BookingStats], [FeaturedOrder], [Featured], [IsSystem], [DivId]) VALUES (15, 0, N'13', N'23', N'213', N'213', N'213', N'23', N'213', N'213', N'23', CAST(0x7D3C0B00 AS Date), N'213', N'213', CAST(0x00000000 AS Date), CAST(213 AS Decimal(18, 0)), CAST(0x7D3C0B00 AS Date), N'213', 0, 0, CAST(0x7D3C0B00 AS Date), 213, CAST(0x0000A72200F24521 AS DateTime), CAST(0x0000A72200F24521 AS DateTime), 0, 1, 1, N'213', 0, 1, 1, 1, N'213', N'213', N'213', 0, 0, 0, 0, 0, 1)
INSERT [dbo].[rs_assets] ([AssetId], [ParentId], [Brand], [Model], [Desciption], [SerialNumber], [MaterialNo], [HardwareOpt], [SoftwareOpt], [HardwareVer], [SoftwareVer], [LicenseExpiry], [Accessories], [Remarks], [PurchaseDate], [PurchasePrice], [DecomDate], [DecomReason], [Tagged], [Damaged], [LastCalibrated], [CalibrationCycle], [CreatedDate], [UpdatedDate], [CreatedBy], [OwnerShipId], [OwnedBy], [TrackingNo], [ReadyToSell], [Availability], [OriginLocId], [CurrentLocId], [PurchasePO], [DepreciationFormula], [ImageLink], [ViewedStats], [BookingStats], [FeaturedOrder], [Featured], [IsSystem], [DivId]) VALUES (16, 0, N'sad22sdfd', N'saddsfgfvb', N'12', N'123', N'213', N'123', N'123', N'213', N'213', CAST(0x7D3C0B00 AS Date), N'213', N'213', CAST(0x00000000 AS Date), CAST(213 AS Decimal(18, 0)), CAST(0x7D3C0B00 AS Date), N'213', 0, 0, CAST(0x7D3C0B00 AS Date), 213, CAST(0x0000A72200F64691 AS DateTime), CAST(0x0000A72200F64691 AS DateTime), 0, 1, 1, N'213', 0, 1, 1, 1, N'213', N'213', N'unnamed.jpg', 0, 0, 0, 0, 0, 1)
INSERT [dbo].[rs_assets] ([AssetId], [ParentId], [Brand], [Model], [Desciption], [SerialNumber], [MaterialNo], [HardwareOpt], [SoftwareOpt], [HardwareVer], [SoftwareVer], [LicenseExpiry], [Accessories], [Remarks], [PurchaseDate], [PurchasePrice], [DecomDate], [DecomReason], [Tagged], [Damaged], [LastCalibrated], [CalibrationCycle], [CreatedDate], [UpdatedDate], [CreatedBy], [OwnerShipId], [OwnedBy], [TrackingNo], [ReadyToSell], [Availability], [OriginLocId], [CurrentLocId], [PurchasePO], [DepreciationFormula], [ImageLink], [ViewedStats], [BookingStats], [FeaturedOrder], [Featured], [IsSystem], [DivId]) VALUES (17, 0, N'wef', N'dwe', N'qwe', N'wedf', N'wvd', N'fqw', N'fq', N'wf', N'wfeq', CAST(0x8A3C0B00 AS Date), N'e', N'eqe', CAST(0x78250B00 AS Date), CAST(111 AS Decimal(18, 0)), CAST(0x8A3C0B00 AS Date), N'w', 0, 0, CAST(0x8A3C0B00 AS Date), 23, CAST(0x0000A72F01205C37 AS DateTime), CAST(0x0000A72F01205C37 AS DateTime), 0, 1, 1, N'R1N-0101-0001', 0, 1, 1, 1, N'wewe', N'12', NULL, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[rs_assets] ([AssetId], [ParentId], [Brand], [Model], [Desciption], [SerialNumber], [MaterialNo], [HardwareOpt], [SoftwareOpt], [HardwareVer], [SoftwareVer], [LicenseExpiry], [Accessories], [Remarks], [PurchaseDate], [PurchasePrice], [DecomDate], [DecomReason], [Tagged], [Damaged], [LastCalibrated], [CalibrationCycle], [CreatedDate], [UpdatedDate], [CreatedBy], [OwnerShipId], [OwnedBy], [TrackingNo], [ReadyToSell], [Availability], [OriginLocId], [CurrentLocId], [PurchasePO], [DepreciationFormula], [ImageLink], [ViewedStats], [BookingStats], [FeaturedOrder], [Featured], [IsSystem], [DivId]) VALUES (18, 0, N'12wqeqw', N'123123', N'qwe', N'wqe', N'123', N'213', N'312', N'21', N'123', CAST(0x8C3C0B00 AS Date), N'123', N'213', CAST(0x7F250B00 AS Date), CAST(213 AS Decimal(18, 0)), CAST(0x8C3C0B00 AS Date), N'213', 0, 0, CAST(0x8C3C0B00 AS Date), 123, CAST(0x0000A731012B889C AS DateTime), CAST(0x0000A731012B889C AS DateTime), 0, 1, 1, N'R1N-0101-0001', 0, 1, 1, 1, N'213', N'213', NULL, 0, 0, 0, 0, 0, 1)
INSERT [dbo].[rs_assets] ([AssetId], [ParentId], [Brand], [Model], [Desciption], [SerialNumber], [MaterialNo], [HardwareOpt], [SoftwareOpt], [HardwareVer], [SoftwareVer], [LicenseExpiry], [Accessories], [Remarks], [PurchaseDate], [PurchasePrice], [DecomDate], [DecomReason], [Tagged], [Damaged], [LastCalibrated], [CalibrationCycle], [CreatedDate], [UpdatedDate], [CreatedBy], [OwnerShipId], [OwnedBy], [TrackingNo], [ReadyToSell], [Availability], [OriginLocId], [CurrentLocId], [PurchasePO], [DepreciationFormula], [ImageLink], [ViewedStats], [BookingStats], [FeaturedOrder], [Featured], [IsSystem], [DivId]) VALUES (19, 0, N'21123', N'12213', N'12', N'12', N'12', N'123', N'123', N'213', N'213', CAST(0x8D3C0B00 AS Date), N'123', N'213', CAST(0x00000000 AS Date), CAST(123 AS Decimal(18, 0)), CAST(0x8D3C0B00 AS Date), N'213', 0, 0, CAST(0x8D3C0B00 AS Date), 213, CAST(0x0000A73200A4B8EF AS DateTime), CAST(0x0000A73200A4B8EF AS DateTime), 0, 1, 1, N'R1N-0101-0004', 0, 1, 1, 1, N'123', N'123', NULL, 0, 0, 0, 0, 0, 1)
SET IDENTITY_INSERT [dbo].[rs_assets] OFF
/****** Object:  Table [dbo].[rs_bookstats]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_bookstats](
	[StatsId] [int] NOT NULL,
	[DateId] [int] NOT NULL,
	[AssetId] [int] NOT NULL,
	[Actions] [varchar](50) NOT NULL,
	[BookingCounts] [bigint] NOT NULL,
 CONSTRAINT [PK_se_bookstats] PRIMARY KEY CLUSTERED 
(
	[StatsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rs_bookings]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_bookings](
	[BookId] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[ExtendedDate] [date] NULL,
	[Extended] [bit] NOT NULL,
	[Approved] [bit] NOT NULL,
	[BookAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
	[ResquestorId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ApproverId] [int] NOT NULL,
	[AssetId] [int] NOT NULL,
	[Remarks] [varchar](250) NOT NULL,
	[Purpose] [varchar](250) NOT NULL,
	[Returned] [bit] NOT NULL,
	[ReturnDate] [date] NULL,
	[LoanLocationId] [int] NULL,
	[ReturnLocationId] [int] NOT NULL,
	[Damaged] [bit] NOT NULL,
	[LoanFormId] [int] NULL,
	[VerifyBy] [int] NOT NULL,
	[ReturnRemark] [text] NULL,
	[ReturnBy] [int] NULL,
 CONSTRAINT [PK_se_bookings] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rs_assets_rel]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rs_assets_rel](
	[RelationId] [int] IDENTITY(1,1) NOT NULL,
	[SysId] [int] NOT NULL,
	[AssetId] [int] NOT NULL,
 CONSTRAINT [PK_AssetRelation] PRIMARY KEY CLUSTERED 
(
	[RelationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[rs_assets_rel] ON
INSERT [dbo].[rs_assets_rel] ([RelationId], [SysId], [AssetId]) VALUES (0, 7, 11)
INSERT [dbo].[rs_assets_rel] ([RelationId], [SysId], [AssetId]) VALUES (1, 12, 15)
INSERT [dbo].[rs_assets_rel] ([RelationId], [SysId], [AssetId]) VALUES (2, 12, 17)
INSERT [dbo].[rs_assets_rel] ([RelationId], [SysId], [AssetId]) VALUES (3, 12, 18)
INSERT [dbo].[rs_assets_rel] ([RelationId], [SysId], [AssetId]) VALUES (4, 12, 19)
INSERT [dbo].[rs_assets_rel] ([RelationId], [SysId], [AssetId]) VALUES (5, 12, 16)
INSERT [dbo].[rs_assets_rel] ([RelationId], [SysId], [AssetId]) VALUES (6, 12, 17)
INSERT [dbo].[rs_assets_rel] ([RelationId], [SysId], [AssetId]) VALUES (7, 12, 13)
SET IDENTITY_INSERT [dbo].[rs_assets_rel] OFF
/****** Object:  Table [dbo].[rs_accessories]    Script Date: 03/24/2017 02:40:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rs_accessories](
	[AccId] [int] IDENTITY(1,1) NOT NULL,
	[AssetId] [int] NOT NULL,
	[Accessories] [varchar](250) NOT NULL,
	[Description] [varchar](250) NOT NULL,
	[Remarks] [varchar](250) NULL,
 CONSTRAINT [PK_rs_accessories] PRIMARY KEY CLUSTERED 
(
	[AccId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[rs_accessories] ON
INSERT [dbo].[rs_accessories] ([AccId], [AssetId], [Accessories], [Description], [Remarks]) VALUES (1, 12, N'SSS', N'ASD', N'AS')
INSERT [dbo].[rs_accessories] ([AccId], [AssetId], [Accessories], [Description], [Remarks]) VALUES (2, 12, N'as', N'df', N'dwd')
SET IDENTITY_INSERT [dbo].[rs_accessories] OFF
/****** Object:  Default [DF_se_user_UserType]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_user] ADD  CONSTRAINT [DF_se_user_UserType]  DEFAULT ('User') FOR [UserType]
GO
/****** Object:  Default [DF_rs_bookings_LoanLocationId]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookings] ADD  CONSTRAINT [DF_rs_bookings_LoanLocationId]  DEFAULT ((0)) FOR [LoanLocationId]
GO
/****** Object:  ForeignKey [FK__rs_module__Paren__33D4B598]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_module]  WITH CHECK ADD FOREIGN KEY([ParentModuleId])
REFERENCES [dbo].[rs_module] ([ModuleId])
GO
/****** Object:  ForeignKey [FK__rs_action__Modul__267ABA7A]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_action]  WITH CHECK ADD FOREIGN KEY([ModuleId])
REFERENCES [dbo].[rs_module] ([ModuleId])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
/****** Object:  ForeignKey [FK_se_user_se_division]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_user]  WITH CHECK ADD  CONSTRAINT [FK_se_user_se_division] FOREIGN KEY([DivId])
REFERENCES [dbo].[rs_division] ([DivId])
GO
ALTER TABLE [dbo].[rs_user] CHECK CONSTRAINT [FK_se_user_se_division]
GO
/****** Object:  ForeignKey [FK_se_user_se_user_group]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_user]  WITH CHECK ADD  CONSTRAINT [FK_se_user_se_user_group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[rs_user_group] ([GroupId])
GO
ALTER TABLE [dbo].[rs_user] CHECK CONSTRAINT [FK_se_user_se_user_group]
GO
/****** Object:  ForeignKey [FK__se_userac__Actio__3C69FB99]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_useracl]  WITH CHECK ADD  CONSTRAINT [FK__se_userac__Actio__3C69FB99] FOREIGN KEY([ActionId])
REFERENCES [dbo].[rs_action] ([ActionId])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[rs_useracl] CHECK CONSTRAINT [FK__se_userac__Actio__3C69FB99]
GO
/****** Object:  ForeignKey [FK_se_useracl_se_user_group]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_useracl]  WITH CHECK ADD  CONSTRAINT [FK_se_useracl_se_user_group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[rs_user_group] ([GroupId])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[rs_useracl] CHECK CONSTRAINT [FK_se_useracl_se_user_group]
GO
/****** Object:  ForeignKey [FK_se_smslogs_se_user]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_smslogs]  WITH CHECK ADD  CONSTRAINT [FK_se_smslogs_se_user] FOREIGN KEY([ReceiverId])
REFERENCES [dbo].[rs_user] ([UserId])
GO
ALTER TABLE [dbo].[rs_smslogs] CHECK CONSTRAINT [FK_se_smslogs_se_user]
GO
/****** Object:  ForeignKey [FK_rs_loan_form_rs_user]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_loan_form]  WITH CHECK ADD  CONSTRAINT [FK_rs_loan_form_rs_user] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[rs_user] ([UserId])
GO
ALTER TABLE [dbo].[rs_loan_form] CHECK CONSTRAINT [FK_rs_loan_form_rs_user]
GO
/****** Object:  ForeignKey [FK_rs_loan_form_rs_user1]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_loan_form]  WITH CHECK ADD  CONSTRAINT [FK_rs_loan_form_rs_user1] FOREIGN KEY([RequestorId])
REFERENCES [dbo].[rs_user] ([UserId])
GO
ALTER TABLE [dbo].[rs_loan_form] CHECK CONSTRAINT [FK_rs_loan_form_rs_user1]
GO
/****** Object:  ForeignKey [FK_rs_assets_rs_assetstatus]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_assets]  WITH CHECK ADD  CONSTRAINT [FK_rs_assets_rs_assetstatus] FOREIGN KEY([Availability])
REFERENCES [dbo].[rs_assetstatus] ([StatusId])
GO
ALTER TABLE [dbo].[rs_assets] CHECK CONSTRAINT [FK_rs_assets_rs_assetstatus]
GO
/****** Object:  ForeignKey [FK_rs_assets_rs_division]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_assets]  WITH CHECK ADD  CONSTRAINT [FK_rs_assets_rs_division] FOREIGN KEY([DivId])
REFERENCES [dbo].[rs_division] ([DivId])
GO
ALTER TABLE [dbo].[rs_assets] CHECK CONSTRAINT [FK_rs_assets_rs_division]
GO
/****** Object:  ForeignKey [FK_rs_assets_rs_ownership]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_assets]  WITH CHECK ADD  CONSTRAINT [FK_rs_assets_rs_ownership] FOREIGN KEY([OwnerShipId])
REFERENCES [dbo].[rs_ownership] ([OwnerShipId])
GO
ALTER TABLE [dbo].[rs_assets] CHECK CONSTRAINT [FK_rs_assets_rs_ownership]
GO
/****** Object:  ForeignKey [FK_rs_assets_rs_user]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_assets]  WITH CHECK ADD  CONSTRAINT [FK_rs_assets_rs_user] FOREIGN KEY([OwnedBy])
REFERENCES [dbo].[rs_user] ([UserId])
GO
ALTER TABLE [dbo].[rs_assets] CHECK CONSTRAINT [FK_rs_assets_rs_user]
GO
/****** Object:  ForeignKey [FK_se_assets_se_locations]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_assets]  WITH CHECK ADD  CONSTRAINT [FK_se_assets_se_locations] FOREIGN KEY([OriginLocId])
REFERENCES [dbo].[rs_locations] ([LocationId])
GO
ALTER TABLE [dbo].[rs_assets] CHECK CONSTRAINT [FK_se_assets_se_locations]
GO
/****** Object:  ForeignKey [FK_se_assets_se_locations1]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_assets]  WITH CHECK ADD  CONSTRAINT [FK_se_assets_se_locations1] FOREIGN KEY([CurrentLocId])
REFERENCES [dbo].[rs_locations] ([LocationId])
GO
ALTER TABLE [dbo].[rs_assets] CHECK CONSTRAINT [FK_se_assets_se_locations1]
GO
/****** Object:  ForeignKey [FK_se_bookstats_se_assets]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookstats]  WITH CHECK ADD  CONSTRAINT [FK_se_bookstats_se_assets] FOREIGN KEY([AssetId])
REFERENCES [dbo].[rs_assets] ([AssetId])
GO
ALTER TABLE [dbo].[rs_bookstats] CHECK CONSTRAINT [FK_se_bookstats_se_assets]
GO
/****** Object:  ForeignKey [FK_se_bookstats_se_dates]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookstats]  WITH CHECK ADD  CONSTRAINT [FK_se_bookstats_se_dates] FOREIGN KEY([DateId])
REFERENCES [dbo].[rs_dates] ([DateId])
GO
ALTER TABLE [dbo].[rs_bookstats] CHECK CONSTRAINT [FK_se_bookstats_se_dates]
GO
/****** Object:  ForeignKey [FK_rs_bookings_rs_locations]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookings]  WITH CHECK ADD  CONSTRAINT [FK_rs_bookings_rs_locations] FOREIGN KEY([LoanLocationId])
REFERENCES [dbo].[rs_locations] ([LocationId])
GO
ALTER TABLE [dbo].[rs_bookings] CHECK CONSTRAINT [FK_rs_bookings_rs_locations]
GO
/****** Object:  ForeignKey [FK_rs_bookings_rs_user]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookings]  WITH CHECK ADD  CONSTRAINT [FK_rs_bookings_rs_user] FOREIGN KEY([ReturnBy])
REFERENCES [dbo].[rs_user] ([UserId])
GO
ALTER TABLE [dbo].[rs_bookings] CHECK CONSTRAINT [FK_rs_bookings_rs_user]
GO
/****** Object:  ForeignKey [FK_se_bookings_se_assets]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookings]  WITH CHECK ADD  CONSTRAINT [FK_se_bookings_se_assets] FOREIGN KEY([AssetId])
REFERENCES [dbo].[rs_assets] ([AssetId])
GO
ALTER TABLE [dbo].[rs_bookings] CHECK CONSTRAINT [FK_se_bookings_se_assets]
GO
/****** Object:  ForeignKey [FK_se_bookings_se_loan_form]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookings]  WITH CHECK ADD  CONSTRAINT [FK_se_bookings_se_loan_form] FOREIGN KEY([LoanFormId])
REFERENCES [dbo].[rs_loan_form] ([LoanId])
GO
ALTER TABLE [dbo].[rs_bookings] CHECK CONSTRAINT [FK_se_bookings_se_loan_form]
GO
/****** Object:  ForeignKey [FK_se_bookings_se_user]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookings]  WITH CHECK ADD  CONSTRAINT [FK_se_bookings_se_user] FOREIGN KEY([ApproverId])
REFERENCES [dbo].[rs_user] ([UserId])
GO
ALTER TABLE [dbo].[rs_bookings] CHECK CONSTRAINT [FK_se_bookings_se_user]
GO
/****** Object:  ForeignKey [FK_se_bookings_se_user1]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookings]  WITH CHECK ADD  CONSTRAINT [FK_se_bookings_se_user1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[rs_user] ([UserId])
GO
ALTER TABLE [dbo].[rs_bookings] CHECK CONSTRAINT [FK_se_bookings_se_user1]
GO
/****** Object:  ForeignKey [FK_se_bookings_se_user2]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookings]  WITH CHECK ADD  CONSTRAINT [FK_se_bookings_se_user2] FOREIGN KEY([ResquestorId])
REFERENCES [dbo].[rs_user] ([UserId])
GO
ALTER TABLE [dbo].[rs_bookings] CHECK CONSTRAINT [FK_se_bookings_se_user2]
GO
/****** Object:  ForeignKey [FK_se_bookings_se_user3]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_bookings]  WITH CHECK ADD  CONSTRAINT [FK_se_bookings_se_user3] FOREIGN KEY([VerifyBy])
REFERENCES [dbo].[rs_user] ([UserId])
GO
ALTER TABLE [dbo].[rs_bookings] CHECK CONSTRAINT [FK_se_bookings_se_user3]
GO
/****** Object:  ForeignKey [FK_AssetRelation_Asset]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_assets_rel]  WITH CHECK ADD  CONSTRAINT [FK_AssetRelation_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[rs_assets] ([AssetId])
GO
ALTER TABLE [dbo].[rs_assets_rel] CHECK CONSTRAINT [FK_AssetRelation_Asset]
GO
/****** Object:  ForeignKey [FK_AssetRelation_System]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_assets_rel]  WITH CHECK ADD  CONSTRAINT [FK_AssetRelation_System] FOREIGN KEY([SysId])
REFERENCES [dbo].[rs_assets] ([AssetId])
GO
ALTER TABLE [dbo].[rs_assets_rel] CHECK CONSTRAINT [FK_AssetRelation_System]
GO
/****** Object:  ForeignKey [FK_rs_accessories_rs_assets]    Script Date: 03/24/2017 02:40:32 ******/
ALTER TABLE [dbo].[rs_accessories]  WITH CHECK ADD  CONSTRAINT [FK_rs_accessories_rs_assets] FOREIGN KEY([AssetId])
REFERENCES [dbo].[rs_assets] ([AssetId])
GO
ALTER TABLE [dbo].[rs_accessories] CHECK CONSTRAINT [FK_rs_accessories_rs_assets]
GO
