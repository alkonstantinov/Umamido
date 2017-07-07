insert into Req (PaymentTypeId, Receiver, Address, Paid)
values (1, 'Alexander', 'Tsanko Tserkovski 1', 0),
(2, 'Stoyan', 'Tsanko Tserkovski 2', 1)


insert into Req2Status (ReqId, StatusId, OnDate, UserId, Note)
values 
(1,1,getdate(),1,''),
(1,1,getdate(),2,''),
(2,1,getdate(),1,'')

insert into Req2Good (ReqId, GoodId, Price, Quantity)
values (1,1,12,1),
(1,1,12,2),
(2,1,12,1)