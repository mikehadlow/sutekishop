Alter table [SutekiShop].[dbo].[Category]
Add UrlName varchar(255) null

ALTER TABLE category ADD CONSTRAINT
            category_UrlName_unique UNIQUE NONCLUSTERED
    (
                UrlName
    )

go

update [dbo].[Category] set UrlName = REPlACE([Name], ' ', '_')
update [dbo].[Category] set UrlName = REPlACE(UrlName, ',', '_')
update [dbo].[Category] set UrlName = REPlACE(UrlName, '.', '_')
update [dbo].[Category] set UrlName = REPlACE(UrlName, '''', '_')
update [dbo].[Category] set UrlName = REPlACE(UrlName, '"', '_')
update [dbo].[Category] set UrlName = REPlACE(UrlName, '&', 'and')
update [dbo].[Category] set UrlName = REPlACE(UrlName, '/', '_')
update [dbo].[Category] set UrlName = REPlACE(UrlName, '\', '_')
update [dbo].[Category] set UrlName = REPlACE(UrlName, '-', '_')

go