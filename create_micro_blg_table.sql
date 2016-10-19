USE [YOUR DATABASE]
GO

/****** Object:  Table [dbo].[MicroBlog]    Script Date: 10/18/2016 11:22:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MicroBlog](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[CommentUserName] [nvarchar](50) NOT NULL,
	[CommentText] [nvarchar](140) NOT NULL,
	[CommentPostDate] [datetime] NOT NULL,
 CONSTRAINT [PK_MicroBlog] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


