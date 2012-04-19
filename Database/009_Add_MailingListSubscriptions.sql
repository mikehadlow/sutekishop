use SutekiShop
GO
CREATE TABLE dbo.MailingListSubscription
(
	Id int NOT NULL IDENTITY (1, 1),
	ContactId int NOT NULL,
	Email nvarchar(250) NOT NULL,
	DateSubscribed datetime NOT NULL CONSTRAINT DF_MailingListSubscription_DateSubscribed DEFAULT getdate()
)  ON [PRIMARY]
GO
ALTER TABLE dbo.MailingListSubscription ADD CONSTRAINT
	PK_MailingListSubscription PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

ALTER TABLE dbo.MailingListSubscription ADD CONSTRAINT
FK_MailingListSubscription_Contact FOREIGN KEY
(
	ContactId
) REFERENCES dbo.Contact
(
	ContactId
) ON UPDATE  NO ACTION 
ON DELETE  NO ACTION 
	
GO

IF NOT EXISTS(select * from Content where Name = 'Mailing List')
	INSERT INTO Content (ParentContentId, ContentTypeId, Name, UrlName, Controller, Action, IsActive, Position) 
	VALUES (1, 3, 'Mailing List', 'Mailing_List', 'MailingList', 'Index', 1, 20)
GO
