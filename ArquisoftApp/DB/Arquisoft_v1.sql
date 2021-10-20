USE [Arquisoft]
GO
/****** Object:  Table [dbo].[Audit]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Audit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Module] [varchar](50) NULL,
	[Action] [varchar](50) NULL,
	[User] [varchar](50) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[IdClient] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Last_Name] [varchar](100) NULL,
	[Address] [varchar](200) NULL,
	[Phone] [varchar](8) NULL,
	[Email] [varchar](50) NULL,
	[idState] [int] NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[IdClient] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Materials]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Materials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](200) NULL,
	[Price] [varchar](50) NULL,
	[IdState] [int] NULL,
 CONSTRAINT [PK_Materials] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuleOperations]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuleOperations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[IdModule] [int] NULL,
 CONSTRAINT [PK_MuduleOperations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modules]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
 CONSTRAINT [PK_Modules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleOperations]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleOperations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdRole] [int] NULL,
	[IdOperation] [int] NULL,
 CONSTRAINT [PK_RoleOperations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](200) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[State]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[State](
	[idState] [int] NOT NULL,
	[description] [varchar](50) NULL,
 CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED 
(
	[idState] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[Password] [varchar](150) NULL,
	[Name] [varchar](150) NULL,
	[Last_Name] [varchar](150) NULL,
	[IdRole] [int] NULL,
	[IdState] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VendorMaterials]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendorMaterials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](200) NULL,
	[SiteURL] [varchar](400) NULL,
	[ImgURL] [varchar](400) NULL,
	[Price] [varchar](50) NULL,
 CONSTRAINT [PK_VendorMaterials] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Materials]  WITH CHECK ADD  CONSTRAINT [FK_Materials_State] FOREIGN KEY([IdState])
REFERENCES [dbo].[State] ([idState])
GO
ALTER TABLE [dbo].[Materials] CHECK CONSTRAINT [FK_Materials_State]
GO
ALTER TABLE [dbo].[ModuleOperations]  WITH CHECK ADD  CONSTRAINT [FK_ModuleOperations] FOREIGN KEY([IdModule])
REFERENCES [dbo].[Modules] ([Id])
GO
ALTER TABLE [dbo].[ModuleOperations] CHECK CONSTRAINT [FK_ModuleOperations]
GO
ALTER TABLE [dbo].[RoleOperations]  WITH CHECK ADD  CONSTRAINT [FK_RoleOperations_Operations] FOREIGN KEY([IdOperation])
REFERENCES [dbo].[ModuleOperations] ([Id])
GO
ALTER TABLE [dbo].[RoleOperations] CHECK CONSTRAINT [FK_RoleOperations_Operations]
GO
ALTER TABLE [dbo].[RoleOperations]  WITH CHECK ADD  CONSTRAINT [FK_RoleOperations_Roles] FOREIGN KEY([IdRole])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[RoleOperations] CHECK CONSTRAINT [FK_RoleOperations_Roles]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_IdState] FOREIGN KEY([IdState])
REFERENCES [dbo].[State] ([idState])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_IdState]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_UserRole] FOREIGN KEY([IdRole])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_UserRole]
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewUser]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Steven Gonzalez	
-- Create date: June 12, 2021
-- Description:	This SP retrieve all active users
-- =============================================
CREATE PROCEDURE [dbo].[SP_AddNewUser] 
@Name VARCHAR(150),
@Last_Name VARCHAR(150),
@Email VARCHAR(150),
@Username VARCHAR(150),
@Password VARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO Users(Username, Email, Password, Name, Last_Name, Enable)
	VALUES(@Username, @Email, @Password, @Name, @Last_Name, 1)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ChangePassword]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Steven Gonzalez	
-- Create date: June 12, 2021
-- Description:	This SP retrieve all active users
-- =============================================
CREATE PROCEDURE [dbo].[SP_ChangePassword] 
@NewPassword VARCHAR(150),
@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE Users SET Password = @NewPassword WHERE Id = @UserId
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllUsers]    Script Date: 19/10/2021 22:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Steven Gonzalez	
-- Create date: June 12, 2021
-- Description:	This SP retrieve all active users
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetAllUsers] 
AS
BEGIN
	SET NOCOUNT ON;

    SELECT * FROM Users
END
GO
