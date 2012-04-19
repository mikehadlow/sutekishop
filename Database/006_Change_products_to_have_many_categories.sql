use SutekiShop
create table ProductCategory (
	Id int not null identity(1, 1),
	ProductId int not null,
	CategoryId int not null
) ON [PRIMARY]

ALTER TABLE ProductCategory ADD CONSTRAINT PK_ProductCategory PRIMARY KEY CLUSTERED (
	Id
) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

insert into ProductCategory (ProductId, CategoryId)
select ProductId, CategoryId from Product 

ALTER TABLE dbo.ProductCategory ADD CONSTRAINT
	FK_ProductCategory_Category FOREIGN KEY
	(
	CategoryId
	) REFERENCES dbo.Category
	(
	CategoryId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.ProductCategory ADD CONSTRAINT
	FK_ProductCategory_Product FOREIGN KEY
	(
	ProductId
	) REFERENCES dbo.Product
	(
	ProductId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 


alter table Product
	drop constraint FK_Product_Category

alter table Product
	drop column CategoryId