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

