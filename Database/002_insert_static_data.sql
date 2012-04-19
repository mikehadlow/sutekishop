use SutekiShop


insert [role] (RoleId, [Name]) values(1, 'Administrator')
insert [role] (RoleId, [Name]) values(2, 'Order Processor')
insert [role] (RoleId, [Name]) values(3, 'Customer')
insert [role] (RoleId, [Name]) values(4, 'Guest')

insert [user] (Email, [Password], RoleId, IsEnabled) 
	values('admin@sutekishop.co.uk', 'D033E22AE348AEB5660FC2140AEC35850C4DA997', 1, 1)

set identity_insert category on
insert category (CategoryId, [Name], ParentId, Position, IsActive) values(1, '- Root', null, 1, 1)
set identity_insert category off

insert CardType values(1,	'Visa / Delta / Electron',	0)
insert CardType values(2,	'Master Card / Euro Card',	0)
insert CardType values(3,	'American Express',	0)
insert CardType values(4,	'Switch / Solo / Maestro',	1)

insert OrderStatus values(1, 'Created')
insert OrderStatus values(2, 'Dispatched')
insert OrderStatus values(3, 'Rejected')

insert ContentType values(1, 'Menu')
insert ContentType values(2, 'Text')
insert ContentType values(3, 'Action')
insert ContentType values(4, 'Top')

set identity_insert [content] on

insert [content](contentId, parentContentId, contentTypeId, [name], UrlName, [text], controller, [action], position, isActive)
values(1, null, 1, 'Main Menu', 'Main_menu', null, null, null, 1, 1)

insert [content](contentId, parentContentId, contentTypeId, [name], UrlName, [text], controller, [action], position, isActive)
values(2, 1, 4, 'Home', 'Home', 'Homepage Content', null, null, 2, 1)

insert [content](contentId, parentContentId, contentTypeId, [name], UrlName, [text], controller, [action], position, isActive)
values(3, 1, 3, 'Online Shop', 'Online_Shop', null, 'Home', 'Index', 3, 1)

insert [content](contentId, parentContentId, contentTypeId, [name], UrlName, [text], controller, [action], position, isActive)
values(4, null, 2, 'Shopfront', 'Shopfront', '<h1>Wecome to our online shop</h1>', null, null, 4, 1)

set identity_insert [content] off

set identity_insert PostZone on

insert PostZone (PostZoneId, [Name], Multiplier, AskIfMaxWeight, Position, IsActive, FlatRate)
values(1, 'United Kingdom', 1, 0, 1, 1, 10)

set identity_insert PostZone off

set identity_insert [Country] on

insert [Country] (CountryId, [Name], Position, IsActive, PostZoneId)
values(1, 'United Kingdom', 1, 1, 1)

set identity_insert [Country] off