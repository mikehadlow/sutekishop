USE SutekiShop

CREATE TABLE dbo.Referer (
	RefererId int NOT NULL IDENTITY (1, 1),
	Name varchar(1024) NOT NULL,
	Position int NOT NULL,
	IsActive bit NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE dbo.Referer ADD CONSTRAINT PK_Referer PRIMARY KEY CLUSTERED (
	RefererId
) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE dbo.[Order] ADD
	RefererId int NULL
GO

ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Referer_RefererId] FOREIGN KEY([RefererId])
REFERENCES [dbo].[Referer] ([RefererId])
GO