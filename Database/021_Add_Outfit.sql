use SutekiShop
GO

create table dbo.Outfit (
	OutfitId	int not null identity(1,1),
	Name varchar(1024) NOT NULL,
	Position int NOT NULL,
	IsActive bit NOT NULL,
	[Description] varchar(max) not null,
	UrlName varchar(1024) not null)
GO

ALTER TABLE dbo.Outfit ADD CONSTRAINT PK_Outfit PRIMARY KEY CLUSTERED (
	OutfitId
) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

create table dbo.OutfitImage (
	OutfitImageId	int not null identity(1,1),
	Position int not null,
	OutfitId int not null,
	ImageId int not null)
GO

ALTER TABLE dbo.OutfitImage ADD CONSTRAINT PK_OutfitImage PRIMARY KEY CLUSTERED (
	OutfitImageId
) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

create table dbo.OutfitProduct (
	OutfitProductId int not null identity(1,1),
	Position int not null,
	OutfitId int not null,
	ProductId int not null)
GO

ALTER TABLE dbo.OutfitProduct ADD CONSTRAINT PK_OutfitProduct PRIMARY KEY CLUSTERED (
	OutfitProductId
) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].OutfitImage  WITH CHECK ADD  CONSTRAINT [FK_OutfitImage_Outfit_OutfitId] FOREIGN KEY([OutfitId])
REFERENCES [dbo].Outfit ([OutfitId])
GO

ALTER TABLE [dbo].OutfitImage  WITH CHECK ADD  CONSTRAINT [FK_OutfitImage_Image_ImageId] FOREIGN KEY([ImageId])
REFERENCES [dbo].[Image] ([ImageId])
GO

ALTER TABLE [dbo].OutfitProduct  WITH CHECK ADD  CONSTRAINT [FK_OutfitProduct_Outfit_OutfitId] FOREIGN KEY([OutfitId])
REFERENCES [dbo].Outfit ([OutfitId])
GO

ALTER TABLE [dbo].OutfitProduct  WITH CHECK ADD  CONSTRAINT [FK_OutfitProduct_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].Product ([ProductId])
GO
