use Umamido
go




if object_id('UserLevel') is not null
begin
  exec p_ak_drop_all_foreign_keys 'UserLevel'
  drop table UserLevel
end
go

create table UserLevel
( 
  UserLevelId int not null
  , UserLevelName nvarchar (100) not null 
  , constraint pk_UserLevelId primary key(UserLevelId)   
)
go
exec p_ak_create_fk_indeces 'UserLevel'
go
insert into UserLevel (UserLevelId, UserLevelName)
values (1,'Administrator'), (2,'Delivery')



if object_id('User') is not null
begin
  exec p_ak_drop_all_foreign_keys 'User'
  drop table [User]
end
go

create table [User]
( 
  UserId int not null identity(1,1)
  , UserName nvarchar (100) not null 
  , Password nvarchar (64) not null
  , UserLevelId int not null
  , IsActive bit not null default 1
  , constraint pk_UserId primary key(UserId)   
)
go
exec p_ak_create_fk_indeces 'User'
go

insert into [User] (UserName, Password, UserLevelId, IsActive) values ('alex', '202cb962ac59075b964b07152d234b70', 1, 1)
go

if object_id('Client') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Client'
  drop table Client
end
go

create table Client
( 
  ClientId int not null identity(1,1)
  , Name nvarchar(200) not null 
  , Password nvarchar(64) not null
  , Firstname nvarchar(200)
  , Familyname nvarchar(200)
  , eMail nvarchar(200)
  , constraint pk_ClientId primary key(ClientId)   
)
go
exec p_ak_create_fk_indeces 'Client'
go
insert into Client (Name, Password) values ('a','202cb962ac59075b964b07152d234b70')
go

if object_id('Lang') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Lang'
  drop table Lang
end
go

create table Lang
( 
  LangId int not null identity(1,1)
  , LangName nvarchar (5) not null 
  , IsActive bit not null default 1
  , constraint pk_LangId primary key(LangId)   
)
go
exec p_ak_create_fk_indeces 'Lang'
go


insert into Lang (LangName, IsActive) values ('bg-BG', 1)
insert into Lang (LangName, IsActive) values ('en-EN', 2)
go

if object_id('Image') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Image'
  drop table [Image]
end
go

create table [Image]
( 
  ImageId int not null identity(1,1)
  , ImageName nvarchar (100) not null 
  , Content varbinary (max)
  , Filename nvarchar (max)
  , IsActive bit not null default 1
  , constraint pk_ImageId primary key(ImageId)   
)
go
exec p_ak_create_fk_indeces 'Image'
go

if object_id('Restaurant') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Restaurant'
  drop table Restaurant
end
go

create table Restaurant
( 
  RestaurantId int not null identity(1,1)
  , ImageId int not null
  , LogoImageId int  not null
  , BigImageId int not null
  , IsActive bit not null default 1
  , constraint pk_RestaurantId primary key(RestaurantId)   
  , constraint fk_Restaurant_ImageId foreign key (ImageId) references [Image](ImageId)
  , constraint fk_Restaurant_BigImageId foreign key (BigImageId) references [Image](ImageId)
  , constraint fk_Restaurant_LogoImageId foreign key (LogoImageId) references [Image](ImageId)
)
go
exec p_ak_create_fk_indeces 'Restaurant'
go



if object_id('RestaurantTitle') is not null
begin
  exec p_ak_drop_all_foreign_keys 'RestaurantTitle'
  drop table RestaurantTitle
end
go

create table RestaurantTitle
( 
  RestaurantTitleId int not null identity(1,1)
  , RestaurantId int not null 
  , LangId int not null
  , Text nvarchar(max)
  , constraint pk_RestaurantTitleId primary key(RestaurantTitleId)   
  , constraint fk_RestaurantTitle_RestaurantId foreign key (RestaurantId) references Restaurant(RestaurantId)
  , constraint fk_RestaurantTitle_LangId foreign key (LangId) references Lang(LangId)
)
go
exec p_ak_create_fk_indeces 'RestaurantTitle'
go

if object_id('RestaurantDesc') is not null
begin
  exec p_ak_drop_all_foreign_keys 'RestaurantDesc'
  drop table RestaurantDesc
end
go

create table RestaurantDesc
( 
  RestaurantDescId int not null identity(1,1)
  , RestaurantId int not null 
  , LangId int not null
  , Text nvarchar(max)
  , constraint pk_RestaurantDescId primary key(RestaurantDescId)   
  , constraint fk_RestaurantDesc_RestaurantId foreign key (RestaurantId) references Restaurant(RestaurantId)
  , constraint fk_RestaurantDesc_LangId foreign key (LangId) references Lang(LangId)
)
go
exec p_ak_create_fk_indeces 'RestaurantDesc'
go


if object_id('Good') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Good'
  drop table Good
end
go

create table Good
( 
  GoodId int not null identity(1,1)
  , RestaurantId int not null 
  , ImageId int not null
  , Price decimal (10,2) not null
  , CookMinutes int not null
  , IsActive bit not null default 1
  , constraint pk_GoodId primary key(GoodId)   
  , constraint fk_Good_RestaurantId foreign key (RestaurantId) references Restaurant(RestaurantId)
  , constraint fk_Good_ImageId foreign key (ImageId) references [Image](ImageId)

)
go
exec p_ak_create_fk_indeces 'Good'
go



if object_id('GoodTitle') is not null
begin
  exec p_ak_drop_all_foreign_keys 'GoodTitle'
  drop table GoodTitle
end
go

create table GoodTitle
( 
  GoodTitleId int not null identity(1,1)
  , GoodId int not null 
  , LangId int not null
  , Text nvarchar(max)
  , constraint pk_GoodTitleId primary key(GoodTitleId)   
  , constraint fk_GoodTitle_GoodId foreign key (GoodId) references Good(GoodId)
  , constraint fk_GoodTitle_LangId foreign key (LangId) references Lang(LangId)
)
go
exec p_ak_create_fk_indeces 'GoodTitle'
go

if object_id('GoodDesc') is not null
begin
  exec p_ak_drop_all_foreign_keys 'GoodDesc'
  drop table GoodDesc
end
go

create table GoodDesc
( 
  GoodDescId int not null identity(1,1)
  , GoodId int not null 
  , LangId int not null
  , Text nvarchar(max)
  , constraint pk_GoodDescId primary key(GoodDescId)   
  , constraint fk_GoodDesc_GoodId foreign key (GoodId) references Good(GoodId)
  , constraint fk_GoodDesc_LangId foreign key (LangId) references Lang(LangId)
)
go
exec p_ak_create_fk_indeces 'GoodDesc'
go



if object_id('Slider') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Slider'
  drop table Slider
end
go

create table Slider
( 
  SliderId int not null identity(1,1)
  , ImageId int not null
  , ButtonUrl nvarchar(max)
  , IsActive bit not null default 1
  , constraint pk_SliderId primary key(SliderId)   
  , constraint fk_Slider_ImageId foreign key (ImageId) references [Image](ImageId)
)
go
exec p_ak_create_fk_indeces 'Slider'
go



if object_id('SliderTitle') is not null
begin
  exec p_ak_drop_all_foreign_keys 'SliderTitle'
  drop table SliderTitle
end
go

create table SliderTitle
( 
  SliderTitleId int not null identity(1,1)
  , SliderId int not null 
  , LangId int not null
  , Text nvarchar(max)
  , constraint pk_SliderTitleId primary key(SliderTitleId)   
  , constraint fk_SliderTitle_SliderId foreign key (SliderId) references Slider(SliderId)
  , constraint fk_SliderTitle_LangId foreign key (LangId) references Lang(LangId)
)
go
exec p_ak_create_fk_indeces 'SliderTitle'
go

if object_id('SliderDesc') is not null
begin
  exec p_ak_drop_all_foreign_keys 'SliderDesc'
  drop table SliderDesc
end
go

create table SliderDesc
( 
  SliderDescId int not null identity(1,1)
  , SliderId int not null 
  , LangId int not null
  , Text nvarchar(max)
  , constraint pk_SliderDescId primary key(SliderDescId)   
  , constraint fk_SliderDesc_SliderId foreign key (SliderId) references Slider(SliderId)
  , constraint fk_SliderDesc_LangId foreign key (LangId) references Lang(LangId)
)
go
exec p_ak_create_fk_indeces 'SliderDesc'
go

if object_id('Text') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Text'
  drop table Text
end
go

create table Text
( 
  TextId int not null identity(1,1)
  , TextName nvarchar(100) not null
  , constraint pk_TextId primary key(TextId)   
  
)
go
exec p_ak_create_fk_indeces 'Text'
go


if object_id('TextDesc') is not null
begin
  exec p_ak_drop_all_foreign_keys 'TextDesc'
  drop table TextDesc
end
go

create table TextDesc
( 
  TextDescId int not null identity(1,1)
  , TextId int not null 
  , LangId int not null
  , Text nvarchar(max)
  , constraint pk_TextDescId primary key(TextDescId)   
  , constraint fk_TextDesc_RestaurantId foreign key (TextId) references Text(TextId)
  , constraint fk_TextDesc_LangId foreign key (LangId) references Lang(LangId)
)
go
exec p_ak_create_fk_indeces 'TextDesc'
go


if object_id('Status') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Status'
  drop table Status
end
go

create table Status
( 
  StatusId int not null
  , StatusName nvarchar (100) not null 
  , constraint pk_StatusId primary key(StatusId)   
)
go
exec p_ak_create_fk_indeces 'Status'
go

insert into Status (StatusId, StatusName) values (1, 'New order')
insert into Status (StatusId, StatusName) values (2, 'Started cooking')
insert into Status (StatusId, StatusName) values (3, 'Dispatched for delivery')
insert into Status (StatusId, StatusName) values (4, 'Started delivering')
insert into Status (StatusId, StatusName) values (5, 'Delivered')
insert into Status (StatusId, StatusName) values (6, 'Not delivered')
go





if object_id('PaymentType') is not null
begin
  exec p_ak_drop_all_foreign_keys 'PaymentType'
  drop table PaymentType
end
go

create table PaymentType
( 
  PaymentTypeId int not null
  , PaymentTypeName nvarchar (100) not null 
  , constraint pk_PaymentTypeId primary key(PaymentTypeId)   
)
go
exec p_ak_create_fk_indeces 'PaymentType'
go

insert into PaymentType (PaymentTypeId, PaymentTypeName) values (1, 'Credit card')
insert into PaymentType (PaymentTypeId, PaymentTypeName) values (2, 'Cash on delivery')
go


if object_id('Req') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Req'
  drop table Req
end
go

create table Req
( 
  ReqId int not null identity(1,1)
  , PaymentTypeId int not null 
  , ClientId int not null
  , Receiver nvarchar(max) not null
  , Address nvarchar(max) not null
  , LatLong nvarchar(100) not null
  , Paid bit not null default 0 
  , constraint pk_ReqId primary key(ReqId)   
  , constraint fk_Req_PaymentTypeId foreign key (PaymentTypeId) references PaymentType(PaymentTypeId)  
  , constraint fk_Req_ClientId foreign key (ClientId) references Client(ClientId)  
)
go
exec p_ak_create_fk_indeces 'Req'
go

if object_id('Req2Good') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Req2Good'
  drop table Req2Good
end
go

create table Req2Good
( 
  Req2GoodId int not null identity(1,1)
  , ReqId int not null
  , GoodId int not null
  , Price decimal (10,2) not null
  , Quantity int not null  
  , constraint pk_Req2GoodId primary key(Req2GoodId)   
  , constraint fk_Req2Good_ReqId foreign key (ReqId) references Req(ReqId)  
  , constraint fk_Req2Good_GoodId foreign key (GoodId) references Good(GoodId)  
)
go
exec p_ak_create_fk_indeces 'Req2Good'
go

if object_id('Req2Status') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Req2Status'
  drop table Req2Status
end
go

create table Req2Status
( 
  Req2StatusId int not null identity(1,1)
  , ReqId int not null 
  , StatusId int not null 
  , OnDate datetime2 not null
  , UserId  int null
  , Note nvarchar(max)
  , constraint pk_Req2StatusId primary key(Req2StatusId)   
)
go
exec p_ak_create_fk_indeces 'Req2Status'
go

if object_id('Message') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Message'
  drop table Message
end
go

create table Message
( 
  MessageId int not null identity(1,1)
  , OnDate date not null default getdate()
  , FromName nvarchar(max) 
  , EMail nvarchar(max) 
  , Subject nvarchar(max) 
  , MessageText nvarchar(max) 
  , constraint pk_MessageId primary key(MessageId)   
  
)
go
exec p_ak_create_fk_indeces 'Message'
go



if object_id('Blog') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Blog'
  drop table Blog
end
go

create table Blog
( 
  BlogId int not null identity(1,1)
  , ImageId int not null
  , OnDate date
  , IsActive bit not null default 1
  , constraint pk_BlogId primary key(BlogId)   
  , constraint fk_Blog_ImageId foreign key (ImageId) references [Image](ImageId)

)
go
exec p_ak_create_fk_indeces 'Blog'
go



if object_id('BlogTitle') is not null
begin
  exec p_ak_drop_all_foreign_keys 'BlogTitle'
  drop table BlogTitle
end
go

create table BlogTitle
( 
  BlogTitleId int not null identity(1,1)
  , BlogId int not null 
  , LangId int not null
  , Text nvarchar(max)
  , constraint pk_BlogTitleId primary key(BlogTitleId)   
  , constraint fk_BlogTitle_BlogId foreign key (BlogId) references Blog(BlogId)
  , constraint fk_BlogTitle_LangId foreign key (LangId) references Lang(LangId)
)
go
exec p_ak_create_fk_indeces 'BlogTitle'
go

if object_id('BlogDesc') is not null
begin
  exec p_ak_drop_all_foreign_keys 'BlogDesc'
  drop table BlogDesc
end
go

create table BlogDesc
( 
  BlogDescId int not null identity(1,1)
  , BlogId int not null 
  , LangId int not null
  , Text nvarchar(max)
  , constraint pk_BlogDescId primary key(BlogDescId)   
  , constraint fk_BlogDesc_BlogId foreign key (BlogId) references Blog(BlogId)
  , constraint fk_BlogDesc_LangId foreign key (LangId) references Lang(LangId)
)
go
exec p_ak_create_fk_indeces 'BlogDesc'
go




--if object_id('AddressCheck') is not null
--begin
--  exec p_ak_drop_all_foreign_keys 'AddressCheck'
--  drop table AddressCheck
--end
--go

--create table AddressCheck
--( 
--  AddressCheckId int not null identity(1,1)
--  , Address nvarchar(200) not null 
--  , Km int not null
--  , constraint pk_AddressCheckId primary key(AddressCheckId)   
--)
--go
--exec p_ak_create_fk_indeces 'AddressCheck'
--go



--if object_id('DistantAddress') is not null
--begin
--  exec p_ak_drop_all_foreign_keys 'DistantAddress'
--  drop table DistantAddress
--end
--go

--create table DistantAddress
--( 
--  DistantAddressId int not null identity(1,1)
--  , eMail nvarchar(200) 
--  , Address nvarchar(300)
--  , constraint pk_DistantAddressId primary key(DistantAddressId)   
--)
--go
--exec p_ak_create_fk_indeces 'DistantAddress'
--go
