USE [master]
GO
/****** Object:  Database [Gallery]    Script Date: 09/06/2011 12:08:32 ******/
CREATE DATABASE [Gallery]
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'Gallery', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Gallery].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Gallery] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Gallery] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Gallery] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Gallery] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Gallery] SET ARITHABORT OFF
GO
ALTER DATABASE [Gallery] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [Gallery] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [Gallery] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Gallery] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Gallery] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Gallery] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Gallery] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Gallery] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Gallery] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Gallery] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Gallery] SET  DISABLE_BROKER
GO
ALTER DATABASE [Gallery] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Gallery] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Gallery] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Gallery] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Gallery] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Gallery] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Gallery] SET  READ_WRITE
GO
ALTER DATABASE [Gallery] SET RECOVERY SIMPLE
GO
ALTER DATABASE [Gallery] SET  MULTI_USER
GO
ALTER DATABASE [Gallery] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Gallery] SET DB_CHAINING OFF
GO
USE [Gallery]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 09/06/2011 12:08:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[dateRegistered] [datetime] NOT NULL,
	[name] [nvarchar](300) NULL,
	[city] [nvarchar](300) NULL,
	[birthdate] [datetime] NULL,
	[email] [nvarchar](300) NULL,
	[moreInfo] [nvarchar](max) NULL,
	[avatar] [image] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([ID], [username], [password], [dateRegistered], [name], [city], [birthdate], [email], [moreInfo], [avatar]) VALUES (1, N'system', N'asdas213124gtag gfASD 423423 fsdfaf qwer', CAST(0x00009D3700000000 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([ID], [username], [password], [dateRegistered], [name], [city], [birthdate], [email], [moreInfo], [avatar]) VALUES (4, N'user', N'user', CAST(0x00009D3700D4315A AS DateTime), N'Georgi Kolev', N'Veliko Turnovo', CAST(0x0000402D00000000 AS DateTime), N'email@mail.com', N'random info about myself', NULL)
INSERT [dbo].[Users] ([ID], [username], [password], [dateRegistered], [name], [city], [birthdate], [email], [moreInfo], [avatar]) VALUES (5, N'grim', N'grim', CAST(0x00009D38013EF01B AS DateTime), N'', N'', NULL, N'', N'', NULL)
INSERT [dbo].[Users] ([ID], [username], [password], [dateRegistered], [name], [city], [birthdate], [email], [moreInfo], [avatar]) VALUES (6, N'arcanis', N'arcanis', CAST(0x00009D3B00DC6776 AS DateTime), N'some1', N'varna', CAST(0x0000901A00000000 AS DateTime), N'asd@asd.com', N'', NULL)
INSERT [dbo].[Users] ([ID], [username], [password], [dateRegistered], [name], [city], [birthdate], [email], [moreInfo], [avatar]) VALUES (7, N'astarte', N'astarte', CAST(0x00009D3B00DD2BD7 AS DateTime), N'', N'', NULL, N'', N'', NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Table [dbo].[ImageVisits]    Script Date: 09/06/2011 12:08:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageVisits](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[User] [bigint] NOT NULL,
	[Image] [bigint] NOT NULL,
 CONSTRAINT [PK_ImageVisits] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImageRatings]    Script Date: 09/06/2011 12:08:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageRatings](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[User] [bigint] NOT NULL,
	[Image] [bigint] NOT NULL,
	[rating] [int] NOT NULL,
 CONSTRAINT [PK_ImageRatings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImageComments]    Script Date: 09/06/2011 12:08:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageComments](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[User] [bigint] NOT NULL,
	[Image] [bigint] NOT NULL,
	[dateWritten] [datetime] NOT NULL,
	[description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ImageComments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Images]    Script Date: 09/06/2011 12:08:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[User] [bigint] NOT NULL,
	[image] [image] NOT NULL,
	[InCategory] [bigint] NOT NULL,
	[dateUploaded] [datetime] NOT NULL,
	[liked] [bigint] NOT NULL,
	[disliked] [bigint] NOT NULL,
	[visits] [bigint] NOT NULL,
	[commens] [bigint] NOT NULL,
	[description] [nvarchar](max) NULL,
	[thumbnail] [image] NOT NULL,
 CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 09/06/2011 12:08:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ForUser] [bigint] NOT NULL,
	[name] [nvarchar](300) NULL,
	[IsPrimary] [bit] NOT NULL,
	[description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (1, 1, N'всички', 1, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (3, 4, N'лято', 0, N'снимани през лятото !!')
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (4, 4, N'зима', 0, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (5, 4, N'есен', 0, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (6, 4, N'пролет', 0, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (16, 1, N'природа', 1, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (17, 1, N'животни', 1, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (18, 1, N'изкуство', 1, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (19, 1, N'хора', 1, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (20, 1, N'космос', 1, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (21, 1, N'абстрактни', 1, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (22, 1, N'спорт', 1, NULL)
INSERT [dbo].[Categories] ([ID], [ForUser], [name], [IsPrimary], [description]) VALUES (23, 1, N'коли', 1, NULL)
SET IDENTITY_INSERT [dbo].[Categories] OFF
/****** Object:  ForeignKey [FK_ImageVisits_Image]    Script Date: 09/06/2011 12:08:33 ******/
ALTER TABLE [dbo].[ImageVisits]  WITH CHECK ADD  CONSTRAINT [FK_ImageVisits_Image] FOREIGN KEY([Image])
REFERENCES [dbo].[Images] ([ID])
GO
ALTER TABLE [dbo].[ImageVisits] CHECK CONSTRAINT [FK_ImageVisits_Image]
GO
/****** Object:  ForeignKey [FK_ImageVisits_User]    Script Date: 09/06/2011 12:08:33 ******/
ALTER TABLE [dbo].[ImageVisits]  WITH CHECK ADD  CONSTRAINT [FK_ImageVisits_User] FOREIGN KEY([User])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ImageVisits] CHECK CONSTRAINT [FK_ImageVisits_User]
GO
/****** Object:  ForeignKey [FK_ImageRatings_Image]    Script Date: 09/06/2011 12:08:33 ******/
ALTER TABLE [dbo].[ImageRatings]  WITH CHECK ADD  CONSTRAINT [FK_ImageRatings_Image] FOREIGN KEY([Image])
REFERENCES [dbo].[Images] ([ID])
GO
ALTER TABLE [dbo].[ImageRatings] CHECK CONSTRAINT [FK_ImageRatings_Image]
GO
/****** Object:  ForeignKey [FK_ImageRatings_User]    Script Date: 09/06/2011 12:08:33 ******/
ALTER TABLE [dbo].[ImageRatings]  WITH CHECK ADD  CONSTRAINT [FK_ImageRatings_User] FOREIGN KEY([User])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ImageRatings] CHECK CONSTRAINT [FK_ImageRatings_User]
GO
/****** Object:  ForeignKey [FK_ImageComments_Image]    Script Date: 09/06/2011 12:08:33 ******/
ALTER TABLE [dbo].[ImageComments]  WITH CHECK ADD  CONSTRAINT [FK_ImageComments_Image] FOREIGN KEY([Image])
REFERENCES [dbo].[Images] ([ID])
GO
ALTER TABLE [dbo].[ImageComments] CHECK CONSTRAINT [FK_ImageComments_Image]
GO
/****** Object:  ForeignKey [FK_ImageComments_User]    Script Date: 09/06/2011 12:08:33 ******/
ALTER TABLE [dbo].[ImageComments]  WITH CHECK ADD  CONSTRAINT [FK_ImageComments_User] FOREIGN KEY([User])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ImageComments] CHECK CONSTRAINT [FK_ImageComments_User]
GO
/****** Object:  ForeignKey [FK_Images_Category]    Script Date: 09/06/2011 12:08:33 ******/
ALTER TABLE [dbo].[Images]  WITH CHECK ADD  CONSTRAINT [FK_Images_Category] FOREIGN KEY([InCategory])
REFERENCES [dbo].[Categories] ([ID])
GO
ALTER TABLE [dbo].[Images] CHECK CONSTRAINT [FK_Images_Category]
GO
/****** Object:  ForeignKey [FK_Images_User]    Script Date: 09/06/2011 12:08:33 ******/
ALTER TABLE [dbo].[Images]  WITH CHECK ADD  CONSTRAINT [FK_Images_User] FOREIGN KEY([User])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Images] CHECK CONSTRAINT [FK_Images_User]
GO
/****** Object:  ForeignKey [FK_Categories_Users]    Script Date: 09/06/2011 12:08:33 ******/
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_Users] FOREIGN KEY([ForUser])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_Users]
GO
