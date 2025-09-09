create database WiproJuly2026
use WiproEmp
create table customer (custid int, name varchar(50),salary int)
select * from customer
insert into customer(custid,name,salary)
values (1,'Rushikesh',20000)
insert into customer(custid,name,salary)
values (2,'Gopi',30000)
delete from customer where custid=2
truncate table customer
drop table customer
alter table customer add City varchar