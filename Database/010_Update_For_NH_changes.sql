
-- update db for NH changes
use SutekiShop

alter table [Content]
	add ContentType varchar(50)
go
update [Content] set ContentType = 'Menu' where ContentTypeId = 1
update [Content] set ContentType = 'TextContent' where ContentTypeId = 2
update [Content] set ContentType = 'ActionContent' where ContentTypeId = 3
update [Content] set ContentType = 'TopContent' where ContentTypeId = 4

exec sp_rename 'ProductCategory.Id', 'ProductCategoryId'
go
exec sp_rename 'Review.Id', 'ReviewId'
go
exec sp_rename 'MailingListSubscription.Id', 'MailingListSubscriptionId'
go


