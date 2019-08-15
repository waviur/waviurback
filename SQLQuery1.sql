select * from members

select * from members where Id = 1

insert into members(name,position,tag,photo) values('Кристиа Землянова','Скрам-мастер','QA','1.png')
insert into members(name,position,tag,photo) values('Alex lebedev','developer','c#','56.png')

update members set photo = '2.png' where ID = 1

delete members where Id = 1




select * from contacts

insert into contacts(skype,mail,memberID) values('imofka','1@mail.ru',2)

select * from members
	join contacts on members.ID = contacts.memberID
where members.Id = 1

select count(1) from members

select sum(ID) from members