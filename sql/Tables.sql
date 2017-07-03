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
  ,constraint pk_UserId primary key(UserId)   
)
go
exec p_ak_create_fk_indeces 'User'
go