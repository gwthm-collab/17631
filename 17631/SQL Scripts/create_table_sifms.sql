USE [sifms]
GO

/****** Object:  Table [dbo].[emp_sal]    Script Date: 31/03/2018 16:05:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[emp_sal](
	[emp_id] [int] IDENTITY(1,1) NOT NULL,
	[emp_name] [nvarchar](max) NOT NULL,
	[desig] [nvarchar](50) NOT NULL,
	[dept] [nvarchar](50) NULL,
	[position] [nvarchar](50) NULL,
	[off_name] [nvarchar](max) NULL,
	[dist] [nvarchar](max) NOT NULL,
	[salary] [numeric](18, 2) NOT NULL,
	[dob] [date] NOT NULL,
	[doj] [date] NOT NULL,
	[flag] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[emp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

