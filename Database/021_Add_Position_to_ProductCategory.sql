USE SutekiShop

ALTER TABLE dbo.[ProductCategory] ADD
	Position int NOT NULL default 0
GO

update dbo.[ProductCategory] set Position = p.Position
from dbo.[ProductCategory] as pc
join dbo.[Product] as p on pc.ProductId = p.ProductId
GO
