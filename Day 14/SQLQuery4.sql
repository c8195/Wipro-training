select * from Departments
create view Departview
as
select DeptID,DeptName from Departments where DeptID=10 and DeptName='HR'

alter view Departview
as 
select * from Departments where DeptName='Marketing

select * from customer
select * from Departments
select * from Departview
select * from EmpDetail


CREATE INDEX index
On table column;

create index idx_product_id
on sales (product_id);


-- Create a table
CREATE TABLE sales (
    sale_id INT,
    product_id INT,
    quantity INT
);

-- Now create an index on product_id
CREATE INDEX idx_product_id
ON sales (product_id);


CREATE INDEX idx_product_id_2
ON sales (product_id);

EXEC sp_helpindex 'sales';


EXEC sp_rename 'sales.idx_product_id', 'idx_prod_id_new', 'INDEX';
  SELECT * FROM idx_prod_id_new
  EXEC sp_helpindex 'sales';

CREATE TABLE Category (
    category_id INT PRIMARY KEY IDENTITY(1,1),
    category_name VARCHAR(100) NOT NULL
);

CREATE TABLE Manufacturer (
    manufacturer_id INT PRIMARY KEY IDENTITY(1,1),
    manufacturer_name VARCHAR(100) NOT NULL
);


CREATE TABLE Product (
    product_id INT PRIMARY KEY IDENTITY(1,1),
    product_name VARCHAR(100) NOT NULL,
    usage VARCHAR(100),
    category_id INT,
    manufacturer_id INT,
    color VARCHAR(50),
    price DECIMAL(10, 2),
    size VARCHAR(50),
    description TEXT,
    FOREIGN KEY (category_id) REFERENCES Category(category_id),
    FOREIGN KEY (manufacturer_id) REFERENCES Manufacturer(manufacturer_id)
);

-- Category table
CREATE TABLE Category1 (
    category_id INT PRIMARY KEY IDENTITY(1,1),
    category_name VARCHAR(100)
);

-- Manufacturer table
CREATE TABLE Manufacturer1 (
    manufacturer_id INT PRIMARY KEY IDENTITY(1,1),
    manufacturer_name VARCHAR(100)
);
