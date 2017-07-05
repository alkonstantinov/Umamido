use Umamido
go


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
  , IsActive bit not null default 1
  , constraint pk_UserId primary key(UserId)   
)
go
exec p_ak_create_fk_indeces 'User'
go

insert into [User] (UserName, Password,IsActive) values ('alex', '202cb962ac59075b964b07152d234b70', 1)
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
