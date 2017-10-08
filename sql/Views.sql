if OBJECT_ID('vwReq') is not null
  drop view vwReq;
go


create view vwReq as
select r.ReqId, r2s.OnDate, r2s.StatusId, R.ClientId
from Req r
join Req2Status r2s on r2s.ReqId = r.ReqId
left join Req2Status r2s2 on r2s2.ReqId = r.ReqId and r2s2.Req2StatusId > r2s.Req2StatusId
where r2s2.Req2StatusId is null
go
