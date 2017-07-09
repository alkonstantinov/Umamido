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

if object_id('Lang') is not null
begin
  exec p_ak_drop_all_foreign_keys 'Lang'
  drop table Lang
end
go

create table Lang
( 
  LangId int not null identity(1,1)
  , LangName nvarchar (2) not null 
  , IsActive bit not null default 1
  , constraint pk_LangId primary key(LangId)   
)
go
exec p_ak_create_fk_indeces 'Lang'
go

insert into Lang (LangName, IsActive) values ('EN', 1)
insert into Lang (LangName, IsActive) values ('BG', 2)
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
  , IsActive bit not null default 1
  , constraint pk_RestaurantId primary key(RestaurantId)   
  , constraint fk_Restaurant_ImageId foreign key (ImageId) references [Image](ImageId)
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
  , Receiver nvarchar(max) not null
  , Address nvarchar(max) not null
  , Paid bit not null default 0 
  , constraint pk_ReqId primary key(ReqId)   
  , constraint fk_Req_PaymentTypeId foreign key (PaymentTypeId) references PaymentType(PaymentTypeId)  
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