alter table OrderLine
add ProductUrlName nvarchar(255) null

update OrderLine set ProductUrlName = ''

update OrderLine set ProductUrlName = p.UrlName
from OrderLine l
join Product p on SUBSTRING(l.ProductName, 0, CHARINDEX(' - ', l.ProductName, 0)) = p.Name

update OrderLine set ProductUrlName = p.UrlName
from OrderLine l
join Product p on l.ProductName = p.Name

update OrderLine set ProductUrlName = p.UrlName
from OrderLine l
join Product p on SUBSTRING(l.ProductName, 0, CHARINDEX(' - ', l.ProductName, CHARINDEX(' - ', l.ProductName, 0)+1)) = p.Name
