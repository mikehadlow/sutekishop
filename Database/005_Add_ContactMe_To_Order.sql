use SutekiShop
ALTER TABLE dbo.[Order] ADD
	ContactMe bit NOT NULL CONSTRAINT DF_Order_ContactMe DEFAULT 0