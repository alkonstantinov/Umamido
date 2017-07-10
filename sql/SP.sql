if OBJECT_ID('SearchReq') is not null
  drop procedure SearchReq
go


create procedure SearchReq @from datetime2 , @to datetime2, @restaurantId int as 
begin
  select r.ReqId, r.Receiver, r.Address, r.Paid, pt.PaymentTypeName, r2s.OnDate, s.StatusName, r2s.Note, 
  (select sum (r2q.Price*r2q.Quantity) from  Req2Good r2q where r2q.ReqId = r.ReqId) Total
  from Req r
  join Req2Status r2s on r2s.ReqId = r.ReqId
  join Status s on s.StatusId = r2s.StatusId  
  join PaymentType pt on pt.PaymentTypeId = r.PaymentTypeId
  left join Req2Status r2s2 on r2s2.ReqId = r.ReqId and r2s2.Req2StatusId > r2s.Req2StatusId
  where 
    r2s2.Req2StatusId is null and 
	r2s.OnDate>=@from and
	r2s.OnDate<=@to and 
	(@restaurantId = -1 or 
	 Exists (select top 1 1 
	 from good g join Req2Good r2g on r2g.ReqId = r.ReqId join Restaurant rs on rs.RestaurantId = g.GoodId)
	)
  order by r.ReqId
end
go

if OBJECT_ID('ForDispatch') is not null
  drop procedure ForDispatch
go


create procedure ForDispatch  as 
begin
  select 
    r.ReqId, r.Receiver, r.Address
  from Req r
  join Req2Status r2s on r2s.ReqId = r.ReqId
  left join Req2Status r2s2 on r2s2.ReqId = r.ReqId and r2s2.Req2StatusId > r2s.Req2StatusId
  where 
    r2s2.Req2StatusId is null and 
	r2s.StatusId = 2
  order by r.ReqId
end
go


if OBJECT_ID('ForDispatch') is not null
  drop procedure ForDispatch
go


create procedure ForDispatch  as 
begin
  select 
    r.ReqId, r.Receiver, r.Address
  from Req r
  join Req2Status r2s on r2s.ReqId = r.ReqId
  left join Req2Status r2s2 on r2s2.ReqId = r.ReqId and r2s2.Req2StatusId > r2s.Req2StatusId
  where 
    r2s2.Req2StatusId is null and 
	r2s.StatusId = 2
  order by r.ReqId
end
go




if OBJECT_ID('ForCollect') is not null
  drop procedure ForCollect
go

-- ForCollect 2
create procedure ForCollect @userId int  as 
begin
  select 
    r.ReqId, r.Receiver, r.Address
  from Req r
  join Req2Status r2s on r2s.ReqId = r.ReqId
  left join Req2Status r2s2 on r2s2.ReqId = r.ReqId and r2s2.Req2StatusId > r2s.Req2StatusId
  where 
    r2s2.Req2StatusId is null and 
	r2s.StatusId = 3 and r2s.UserId = @userId	
  order by r.ReqId
end
go

if OBJECT_ID('CollectDetails') is not null
  drop procedure CollectDetails
go

-- CollectDetails 1
create procedure CollectDetails @reqId int  as 
begin
  select 
    r2g.reqId,
    (select top 1 text from GoodTitle where GoodId = g.GoodId) Good, 
	g.CookMinutes,
	r2g.Quantity,
	(select top 1 text from RestaurantTitle where RestaurantId = g.RestaurantId) Restaurant 
  from Req2Good r2g
  join Good g on g.GoodId = r2g.GoodId
  where r2g.ReqId = @reqId
  order by g.CookMinutes
end
go


if OBJECT_ID('ForDeliver') is not null
  drop procedure ForDeliver
go

-- ForCollect 2
create procedure ForDeliver @userId int  as 
begin
  select 
    r.ReqId, r.Receiver, r.Address
  from Req r
  join Req2Status r2s on r2s.ReqId = r.ReqId
  left join Req2Status r2s2 on r2s2.ReqId = r.ReqId and r2s2.Req2StatusId > r2s.Req2StatusId
  where 
    r2s2.Req2StatusId is null and 
	r2s.StatusId = 4 and r2s.UserId = @userId	
  order by r.ReqId
end
go
