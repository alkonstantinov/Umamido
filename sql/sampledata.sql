insert into Req (PaymentTypeId, Receiver, Address, Paid, LatLong, ClientId)
values (1, 'Alexander', 'Tsanko Tserkovski 1', 0, '42.6822368,23.3349951', 1)


insert into Req2Status (ReqId, StatusId, OnDate, UserId, Note)
values 
(1,1,getdate(),null,'')

insert into Req2Good (ReqId, GoodId, Price, Quantity)
values (1,1,12,1),
(1,2,12,3)


