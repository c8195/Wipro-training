SSMS 
sql server management studio 

sql -structured query language 
commands 

query window 

.sql --- open with sql server / notepad 
system databases : master, model msdb, tempdb
any tables inside these 

.ldf
.mdf

sql :  admin track  DBA , developer track

databases
tables
  records 
     row - tuple
	 column -attribute 

databases : collection of tables
table : collection of records
record --collection of information 

wiprojuly2025 --database 
Employee1 : 
 empid1   empname  city    companyid
 101      anita    mysore    wip
 102      sunita   kolkata   ibm
 103      laksh    bangalore  cap

Employee2 : 
 empid2    empname  city    companyid
 101      anita    mysore    ibm
 102      sunita   kolkata   ibm
 103      laksh    bangalore cap

Employee3 : 
 empid    empname  city
 101      anita    mysore
 102      sunita   kolkata
 103      laksh    bangalore 

 Normalization : 1NF, 2NF, 3NF, BCNF, 5NF
 design database 

 DBMS  database management system
 RDBMS  relation database management system
 
 RDBMS market : 
 pl sql
 my sql
 MS sql server
 mongodb 
 db2
 oracle
 ...........................................
 Data types : 
 int, decimal, varchar, nvarchar , money, geography, xml , date, time, datetime, datetimeoffset
 real, smallint........................

 use wiprojuly2025

 create database wiprojuly2025_1

 use wiprojuly2025_1

 -- create a table 
 create table customer ( custid int, name varchar(50) , salary int)

 -- comment lines in sql 
 -- display all records 
 select * from customer 
 
 -- insert a record 
 insert into customer(custid, name,salary) values(101,'pooja',7654)

 -- multiple records--insertion 
  insert into customer(custid, name,salary) values(102,'poojitha',8654),
  (103,'kavitha',5432),(104,'kavisha',8658),(105,'akash',9654)

 -- delete a record 
 delete from customer where custid=101

 -- delete all records 
 delete from customer 

 -- update a record
 update customer set name='john' where custid=12

 -- update with multiple set 
  update customer set name='Mary', salary=8888 where custid=12

 types : 
 1. DDL   data definition language
     create database
	 create table
	 truncate
	    -- table structure will be there, all records will be deleted
		-- during insertion, again records will be added from 1st record only

	-- truncate the table 
		truncate table employee

		select * from employee

		insert into employee(empid, empname, city) values(1000,'lalita', 'pune')
	 alter
	 -- altering the table
	 alter table employee add salary int


	 -- dropping the table
	 drop table employee  
	 -- deletes the records , table structure is also deleted
	 	 	
 2. DML   data manipulation language
     insert
	 delete 
	 update 

Keys :
table should have keys --RDBMS concepts 

	1. primary key  :  
	          key combination of 1 or more columns 
	          unique key which identifies the row 
			  no repetative records 

	2. foreign key
	     
		 table1                    table2
		                           PK                          FK
		 PK                        id1   firstname   country   id
		 id  name city


		 table 
		     empid   empname   city  country  phone ...............


	3. alternate key : 
	   many column combinations can be PK 
	   left out combinations of PK columns 

	4. candidate key :
	   the PK combinations --- which can still be considered as PK 
	
	5. composite key : 
	  combinations of 1 or more columns 
	  composite will be for multiple columns 

=========================================================================
Later : 
constraints : unique, not null, PK,  FK, default, check 
=============================================================================
Operators : 

1. Arithemetic operators : 
   + - * / % 
   select 10+3, 10-3,10*3, 10/3

   --alias :alternate name 
   select 10+3 as 'addition result', 10-3 'subtraction result',10*3, 10/3

2. Relational operators 
   >   <   <=   >=  !=   <>

3. comparison operator ==
4. ternary operator   ? :
  (a>b)?a:b;


5. logical operators :  and , or , not (!)

and ===>  truth table --> both the conditions are ---true 
or  ===>                   either of the condition should be true
not ===>                   neither of the condition is true 


DCL  :
Data control language 

Data Control Language (DCL) in SQL Server, as part of the broader Structured Query Language 
(SQL), is used to manage access and permissions within a database system. It allows database
administrators to control who can access specific data and what actions they are authorized 
to perform on that data.

Grant
    GRANT INSERT, UPDATE, DELETE ON dbo.employee TO guest;

Revoke
   revoke INSERT, UPDATE, DELETE ON dbo.employee TO guest;

TCL
transact control language 

In SQL, TCL stands for Transaction Control Language. TCL commands are used to manage 
transactions within a database, ensuring data integrity and consistency. 

A transaction is a single logical unit of work that either completes entirely or not at all,
adhering to the ACID properties (Atomicity, Consistency, Isolation, Durability).


rollback
commit
savepoint 


select * from student

update student set marks=80 where name='mary'

begin transaction
    update student set marks=800 where name='mary'
rollback 

===================================================================================

Day 12 discussion 

SQL constraints : 
1. Primary key
2. Foreign key
3. not null
4. unique
5. check 
6. default


auto increment 

1. primary key

create table person
(
   id int not null primary key,
   lastname varchar(100) not null,
   firstname varchar(100),
   age int
)

primary key ===> unique, not null 

insert into person(id, lastname, firstname, age) values(100,'gupta','swati',25)

select * from person

2. check  constraint

create table person1
(
   id int not null,
   lastname varchar(100) not null,
   firstname varchar(100),
   age int check (age >=18)
)

insert into person(id, lastname, firstname, age) values(100,'gupta','swati',12)

sp_databases

3. default constraint :

create table person2
(
   id int not null,
   lastname varchar(100) not null,
   firstname varchar(100),
   city varchar(100) default 'Bangalore'
)

insert into person2(id, lastname, firstname) values(100,'gupta','swati')

select * from person2

4. not null constraint

create table person3
(
   id int not null,
   lastname varchar(100) not null,
   firstname varchar(100) not null ,
   age int
)

insert into person3(id, lastname, firstname,age) values(100,null,null,25)

5. unique constraint 

create table person4
(
   id int not null unique,
   lastname varchar(100) not null,
   firstname varchar(100),
   age int
)

insert into person4(id, lastname, firstname,age) values(100,'agarval','arati',25)
select * from person4

not null + unique ====> primary key

6. Foreign key 

create table person                -- table is already present 
(
   id int not null primary key,
   lastname varchar(100) not null,
   firstname varchar(100),
   age int
)

create table orders
(
   orderid int, 
   number int, 
   person_id int,

   primary key (orderid),
   foreign key(person_id) references Person(id) 
)
==========================================================================
built in functions : 
built in functions : 
1. date and time functions
   select getdate()
   select dateadd(day, 2,getdate())
      select dateadd(month, 2,getdate())
	     select dateadd(year, 2,getdate())

  select datepart(day,getdate())
    select datepart(month,getdate())
	  select datepart(year,getdate())

  select datepart(day,'2025-08-1')
    select datepart(month,'2025-08-1')
	  select datepart(year,'2025-08-1')
	   select datepart(dayofweek,'2025-08-01')

-- displays name of the day
  select datename(dw, '1/8/2025')

  -- displays name of the month
  SELECT DATENAME(MONTH, 2025/8/9) AS MonthName
  
  select datediff(day, '2025/8/1','2025/8/25')
  select datediff(month, '2025/8/1','2025/8/25')
  select datediff(year, '2025/8/1','2025/8/25')
 
 select isdate('2025/9/1')  -- returns 0 -- false if date format is wrong
                            -- returns 1 -- true if date format is correct

2. mathematical functions
    select sin(0.3), cos(0.4), tan(0.5), asin(0.2), acos(0.3), atan(0.4), cot(0.5)
	select sqrt(64), square(4)
	select log(0.5), log10(0.6), power(2,3), PI()
	select round(3423.23423,2)
	select abs(15), abs(-15), abs(0)
	select ceiling(3.142833)
	select floor(2.76767676)

3. string functions
    select ascii('a')      -- 97 a   65  A
	select replace('india is a big country', 'india', 'US')
	select replicate('india',10)
	select ltrim('    india country')
	select rtrim('india country      ')
	select trim('    india           country     ');
	select ltrim(rtrim('   india....country    ')) 
	select lower('INDIA'), upper('india')
	select len('india is a country')
	select substring('india is a country', 0, 6)
	select reverse('india')
	select left('india is a country',5) 
	select right('india is a country', 7)
	
4. aggregate functions
 
 select * from employee

 select sum(salary) 'sum of salaries' from employee
  select avg(salary)  'avg of salaries' from employee
   select min(salary) 'min of salaries' from employee
    select max(salary)  'max of salaries'from employee
	 select count(salary)  'count of salaries'from employee

DCL and TCL commands 

Data control language DCL
  grant , revoke 

  grant select,update on employee to WiproUser1

  revoke select, update on employee from WiproUser1


Transaction control language  TCL
  commit, rollback 

begin
   delete from employee where empid=1000
commit

begin
   delete from employee where empid=1000
   rollback transaction
end


   select * from employee
===================================================================
Day 13 discussion 

constraints ? types ? check ? default ? 
DDL ? DML ? DCL ? TCL
builtin functions ?

functions : 
  system defined functions
  user defined functions 
    
select * from employee

-- creating table valued functions 
create function dbo.empcheck()
RETURNS table
as
RETURN 
     select * from employee where empid =1001

--displaying function result 
select * from dbo.empcheck()

Exercise : 
create a table valued function to check empid >=1000
and empname='anita'

Table valued functions :
======================================================
CREATE FUNCTION dbo.empcheck()
RETURNS TABLE
AS
RETURN
    SELECT * FROM employee WHERE empid = 1001
--------------------------------------------------------------------
Scalar valued functions : 

CREATE FUNCTION dbo.CalculateTotal
(
    @Price DECIMAL(10,2),
    @Quantity INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @Price * @Quantity;
END

SELECT dbo.CalculateTotal(100.50, 3) AS TotalAmount;

================================================================
Exercise : 
1. create a scalar valued function to calculate and display
   area of a circle. 

create function dbo.area(	@Radius Decimal(5,2))returns decimal(5,2)as beginreturn (PI() * @Radius*@Radius);Endselect dbo.area(2.4) as Area_of_Circle;
=================================================================

-- another example on scalar valued functions
CREATE FUNCTION dbo.GetDiscountedPrice
(
    @Price DECIMAL(10,2),
    @DiscountRate DECIMAL(5,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @Price - (@Price * @DiscountRate / 100);
END
==============================================================================

Joins : 

connecting / combining more than 1 table 

types : 
1. equi join
2. non equi join
3. outer joins : left outer join, right outer join, full outer join
                 left join,       right join,       full join
4. self join
5. cross join
6. inner join

select * from person p join orders o on  p.id>=o.orderid
select * from employee

-- inner join / join 
select * from employee e join company c on  e.empid=c.empid

-- outer joins -left, right, full
select * from employee e left outer join company c on  e.empid=c.empid
select * from employee e left join  company c on  e.empid=c.empid

select * from employee e right outer join  company c on  e.empid=c.empid
select * from employee e right join  company c on  e.empid=c.empid

select * from employee e full outer join company c on  e.empid=c.empid
select * from employee e full join  company c on  e.empid=c.empid

--cross join --cartesian product
select * from employee cross join company 


-- check for null to avoid null values

self join : 
-table is joined with itself 
select * from employee

select * from employee e1, employee e2 
where e1.empid > e2.empid 


==========================================================================
SELECT A.CustomerName AS CustomerName1, B.CustomerName AS CustomerName2, A.City
FROM Customers A, Customers B
WHERE A.CustomerID <> B.CustomerID
AND A.City = B.City
ORDER BY A.City;

-------------------------------------------------------------
set operators : 
1. union
   --combines one table with another table

   select * from employee
   union
   select * from company        --6 records

   select * from employee
   union
   select * from orders

2. intersect
   select empid  from employee where city='mumbai'
   intersect
   select empid from company where city='mumbai'

3. except
   select empid  from employee where city='mysore'
   except
   select empid from company where city='mysore'

   select * from employee
   select * from company
   	 

4. cartesian product 

   select * from employee cross join company

=============================================================================================

stored procedure :
- collection of T SQL statements
types :
1. with parameters
        create proc delemp
		(@eid int)
		as
		begin
		   delete from employee where empid=@eid
		end

		exec delemp 1001

-- insertion for employee table--sp
-- updation for employee table --sp

--update  
CREATE PROCEDURE UpdateSalaryByEmpID    @EmpID INT,    @NewSalary DECIMAL(10, 2)ASBEGIN    UPDATE Employees    SET Salary = @NewSalary    WHERE EmpID = @EmpID;END;

EXEC UpdateSalaryByEmpID   @EmpID = 101, @NewSalary = 72000.00;

--insertion
create proc insertemp (@eid int, @ename varchar(50), @city varchar(50), @salary int) as begin     insert into employee(empid, empname, city, salary)     values(@eid, @ename, @city, @salary) end exec insertemp 1006, 'Pratyush', 'Mumbai', 5000;


2. without parameters

create proc displaallempdetails
as
  begin
     select * from employee
  end

exec displaallempdetails


create proc greaterempids
as
  begin
     select * from employee where empid >1001
  end

exec greaterempids


create proc lesserempids
as
  begin
     select * from employee where empid <1005
  end

exec lesserempids

---------------------------------------
input and output parameters : 
input
output keywords which you add it inside the stored procedure

-----------------------------------------------------------------
CREATE PROCEDURE SimpleInOutProcedure
    @input INT,
    @inOutParam INT OUTPUT
AS
BEGIN
    SET @inOutParam = @inOutParam + @input
END

DECLARE @val INT = 5
EXEC SimpleInOutProcedure @input = 3, @inOutParam = @val OUTPUT
PRINT @val  -- Output will be 8
===========================================================================
create proc SimpleIOProc	@input int output,	@inout int as begin	set @inout= @inout + @input Enddeclare @val int =5exec SimpleIOProc @input=@val output, @inout=3 print @val;
=================================================================
CREATE PROCEDURE uspGetManagerID  
   @empId int,  
   @managerId int OUTPUT  
AS  
BEGIN  
   SELECT @managerId = ManagerID
   FROM dbo.Employee
   WHERE EmployeeID = @empId  
END


DECLARE @managerID int

EXECUTE uspGetManagerID @empId = 2, @managerId OUTPUT

PRINT @managerId

===========================================================================

if...else , if(),  if...else if....



DECLARE @SalesAmount INT = 6000;

IF @SalesAmount > 5000
BEGIN
    PRINT 'Sales target achieved!';
END

ELSE

BEGIN
    PRINT 'Sales target not yet achieved.';
END;


Declare @val4 int;
Declare @val5 int;

BEGIN TRY
   Set @val4=8;

BEGIN TRY
    Set @val4=8;
    Set @val5=@val4/0; /* Error Occur Here */
END TRY

BEGIN CATCH
    Print 'Error Occur that is:'
    Print Error_Message()
END CATCH

Set @val5=@val4/0; /* Error Occur Here */
END TRY

BEGIN CATCH
    Print 'Error Occur that is:'
    Print Error_Message()
END CATCH
===================================================================================

user defined exceptions :

Declare @val1 int;
Declare @val2 int;

BEGIN TRY
Set @val1=8;
Set @val2=@val1%2;

if @val1=1
    Print ' Error Not Occur'
else
Begin
    Print 'Error Occur';
    Throw 60000,'Number Is Even',5
End

END TRY
BEGIN CATCH
Print 'Error Occur that is:'
Print Error_Message()
END CATCH

=========================================================


Exception handling in sql :

Declare @val4 int;
Declare @val5 int;

BEGIN TRY
   Set @val4=8;
   Declare @val4 int;
   Declare @val5 int;

BEGIN TRY
    Set @val4=8;
    Set @val5=@val4/0; /* Error Occur Here */
END TRY

BEGIN CATCH
    Print 'Error Occur that is:'
    Print Error_Message()
END CATCH

Set @val5=@val4/0; /* Error Occur Here */
END TRY

BEGIN CATCH
    Print 'Error Occur that is:'
    Print Error_Message()
END CATCH
















