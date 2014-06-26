use SutekiShop
ALTER TABLE dbo.[Order] ADD
	Problem bit NOT NULL CONSTRAINT DF_Order_Problem DEFAULT 0