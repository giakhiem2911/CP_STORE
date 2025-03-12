CREATE DATABASE Project_64131000
USE Project_64131000;

-- 1. Bảng Employees

CREATE TABLE Employees (
    EmployeeId NVARCHAR(20) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) CHECK (Role IN ('Manager', 'Staff')) DEFAULT 'Staff'
);

-- 2. Bảng Customers
CREATE TABLE Customers (
    CustomerId NVARCHAR(20) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    CustomerAddress NVARCHAR(255),
    PasswordHash NVARCHAR(255) NOT NULL
);

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
	PhoneNumber NVARCHAR(20) NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Role NVARCHAR(50) NOT NULL
);

-- Thêm tài khoản cho Employees
INSERT INTO Users (Username, PasswordHash, Email, Role) VALUES
('E001', 'passwordhash1', 'anhnguyen@gmail.com', 'Employee'),
('E002', 'passwordhash2', 'bichtran1@gmail.com', 'Employee'),
('E003', 'passwordhash3', 'cuongpham@gmail.com', 'Employee'),
('E004', 'passwordhash4', 'duyenle@gmail.com', 'Employee'),
('E005', 'passwordhash5', 'emhoang1@gmail.com', 'Employee');

-- Thêm tài khoản cho Customers
DELETE FROM Users
INSERT INTO Users (Username, PasswordHash, Email, PhoneNumber, Role) VALUES
('C001', 'passwordhash1', 'annguyen1@gmail.com', '0901234567', 'Customer'),
('C002', 'passwordhash2', 'bichtran1@gmail.com', '0912345678', 'Customer'),
('C003', 'passwordhash3', 'lanhpham1@gmail.com', '0923456789', 'Customer'),
('C004', 'passwordhash4', 'hoale1@gmail.com', '0934567890', 'Customer'),
('C005', 'passwordhash5', 'taihoang1@gmail.com', '0945678901', 'Customer');




-- 3. Bảng Categories
CREATE TABLE Categories (
    CategoryId NVARCHAR(20) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
);

-- 4. Bảng Brands
CREATE TABLE Brands (
    BrandId NVARCHAR(20) PRIMARY KEY,
    BrandName NVARCHAR(100) NOT NULL UNIQUE,
    LogoUrl NVARCHAR(255),
);

-- 5. Bảng CategoryBrand - Bảng trung gian giữa Categories và Brands
CREATE TABLE CategoryBrand (
    CategoryId NVARCHAR(20),
    BrandId NVARCHAR(20),
    PRIMARY KEY (CategoryId, BrandId),
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE CASCADE,
    FOREIGN KEY (BrandId) REFERENCES Brands(BrandId) ON DELETE CASCADE
);

-- 6. Bảng Products
CREATE TABLE Products (
    ProductId NVARCHAR(20) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    ProductDescription NVARCHAR(500),
    Price DECIMAL(10, 2) NOT NULL,
    Unit NVARCHAR(20) NOT NULL,
    ProductImage NVARCHAR(255),
    CategoryId NVARCHAR(20) NOT NULL,
    BrandId NVARCHAR(20) NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    FOREIGN KEY (BrandId) REFERENCES Brands(BrandId)
);

-- 6. Bảng Orders
CREATE TABLE Orders (
    OrderId NVARCHAR(20) PRIMARY KEY,
	OrderDate DATETIME DEFAULT GETDATE(),
	ShipDate DATETIME NOT NULL,
    OrderStatus NVARCHAR(20) NOT NULL,
	CustomerId NVARCHAR(20) NOT NULL,
	EmployeeId NVARCHAR(20) NOT NULL,
	TotalAmount DECIMAL(10, 2)
);

-- 7. Bảng OrderDetails
CREATE TABLE OrderDetails (
    OrderDetailsId NVARCHAR(20) PRIMARY KEY,
    OrderId NVARCHAR(20) NOT NULL,
    ProductId NVARCHAR(20) NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE ShoppingCart (
    CartId NVARCHAR(20) Primary Key,
    UserId NVARCHAR(20),
    ProductId NVARCHAR(20),
    Quantity INT,
	FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Thêm dữ liệu mẫu vào các bảng chính
-- Thêm dữ liệu vào bảng Categories
INSERT INTO Categories (CategoryId, CategoryName) VALUES
('L01', N'Laptop'),
('DT01', N'Điện thoại'),
('MTB01', N'Máy tính bảng'),
('PK01', N'Phụ kiện');

-- Thêm dữ liệu vào bảng Brands
INSERT INTO Brands (BrandId, BrandName, LogoUrl) VALUES
('MSI01', 'MSI', 'logomsi.png'),
('DELL01', 'Dell', 'logodell.png'),
('APPLE01', 'Macbook', 'logoapple.png'),
('ASUS01', 'Asus', 'logoasus.png'),
('APPLE02', 'Iphone', 'logoapple.png'),
('XM01', 'Xiaomi', 'logoxiaomi.png'),
('SS01', 'Samsung', 'logosamsung.png'),
('OP01', 'Oppo', 'logooppo.png'),
('LE01', 'Lenovo', 'logolenovo.png');

-- Thêm dữ liệu vào bảng Employees
INSERT INTO Employees (EmployeeId, FullName, Email, PhoneNumber, PasswordHash, Role) VALUES
('E001', N'Nguyễn Văn Anh', 'anhnguyen@gmail.com', '0981234567', 'passwordhash1', 'Manager'),
('E002', N'Trần Thị Bích', 'bichtran1@gmail.com', '0982345678', 'passwordhash2', 'Staff'),
('E003', N'Phạm Minh Cường', 'cuongpham@gmail.com', '0983456789', 'passwordhash3', 'Staff'),
('E004', N'Lê Thị Duyên', 'duyenle@gmail.com', '0984567890', 'passwordhash4', 'Staff'),
('E005', N'Hoàng Minh Em', 'emhoang1@gmail.com', '0985678901', 'passwordhash5', 'Manager');

-- Thêm dữ liệu vào bảng Customers
INSERT INTO Customers (CustomerId, FullName, Email, PhoneNumber, CustomerAddress, PasswordHash) VALUES
('C001', N'Nguyễn Văn An', 'annguyen@gmail.com', '0912345678', N'Hà Nội, Việt Nam', 'passwordhash1'),
('C002', N'Trần Thị Bích', 'bichtran@gmail.com', '0912345679', N'TP.HCM, Việt Nam', 'passwordhash2'),
('C003', N'Phạm Thị Lãnh', 'lanhpham@gmail.com', '0912345680', N'Đà Nẵng, Việt Nam', 'passwordhash3'),
('C004', N'Lê Thị Hoa', 'hoale@gmail.com', '0912345681', N'Hải Phòng, Việt Nam', 'passwordhash4'),
('C005', N'Hoàng Minh Tài', 'taihoang.@gmail.com', '0912345682', N'Cần Thơ, Việt Nam', 'passwordhash5'),
('C006', N'Nguyễn Hồng Sơn', 'sonnguyen@gmail.com', '0912345683', N'Quảng Ninh, Việt Nam', 'passwordhash6'),
('C007', N'Phạm Văn Hải', 'haipham@gmail.com', '0912345684', N'Nghệ An, Việt Nam', 'passwordhash7'),
('C008', N'Trần Thị Xuân', 'xuantran@gmail.com', '0912345685', N'Thanh Hóa, Việt Nam', 'passwordhash8'),
('C009', N'Lê Văn Bảo', 'baole@gmail.com', '0912345686', N'Huế, Việt Nam', 'passwordhash9'),
('C010', N'Hoàng Thị Dung', 'dunghoang1@gmail.com', '0912345687', N'Quảng Nam, Việt Nam', 'passwordhash10'),
('C011', N'Vũ Văn Cường', 'cuongvu@gmail.com', '0912345688', N'Hải Dương, Việt Nam', 'passwordhash11'),
('C012', N'Đỗ Hồng Nhung', 'nhungdo@gmail.com', '0912345689', N'Lào Cai, Việt Nam', 'passwordhash12'),
('C013', N'Ngô Văn Phong', 'phongngo@gmail.com', '0912345690', N'Phú Thọ, Việt Nam', 'passwordhash13'),
('C014', N'Bùi Thị Lan', 'lanbui@gmail.com', '0912345691', N'Nam Định, Việt Nam', 'passwordhash14'),
('C015', N'Tạ Văn Đạt', 'datta@gmail.com', '0912345692', N'Thái Bình, Việt Nam', 'passwordhash15'),
('C016', N'Hồ Ngọc Anh', 'anhho@gmail.com', '0912345693', N'Quảng Trị, Việt Nam', 'passwordhash16'),
('C017', N'Nguyễn Văn Huy', 'huynguyen@gmail.com', '0912345694', N'Lâm Đồng, Việt Nam', 'passwordhash17'),
('C018', N'Trương Thị Mai', 'maitruong@gmail.com', '0912345695', N'Bình Định, Việt Nam', 'passwordhash18'),
('C019', N'Phan Văn Nam', 'namphan@gmail.com', '0912345696', N'Khánh Hòa, Việt Nam', 'passwordhash19'),
('C020', N'Lê Thị Quỳnh', 'quynhle@gmail.com', '0912345697', N'Bắc Giang, Việt Nam', 'passwordhash20'),
('C021', N'Trần Văn Tài', 'taitran@gmail.com', '0912345698', N'Đồng Nai, Việt Nam', 'passwordhash21'),
('C022', N'Hoàng Văn Dũng', 'dunghoang@gmail.com', '0912345699', N'Tây Ninh, Việt Nam', 'passwordhash22'),
('C023', N'Phạm Hồng Thảo', 'thaopham@gmail.com', '0912345700', N'Bến Tre, Việt Nam', 'passwordhash23'),
('C024', N'Nguyễn Minh Hà', 'haminh@gmail.com', '0912345701', N'Sóc Trăng, Việt Nam', 'passwordhash24'),
('C025', N'Trần Thị Phương', 'phuongtran@gmail.com', '0912345702', N'Hậu Giang, Việt Nam', 'passwordhash25'),
('C026', N'Lê Văn Khoa', 'khoale@gmail.com', '0912345703', N'Đắk Lắk, Việt Nam', 'passwordhash26'),
('C027', N'Trịnh Văn Minh', 'minhtrinh@gmail.com', '0912345704', N'Bình Dương, Việt Nam', 'passwordhash27'),
('C028', N'Phan Thị Ngọc', 'ngocphan@gmail.com', '0912345705', N'Vĩnh Long, Việt Nam', 'passwordhash28'),
('C029', N'Hà Văn Thắng', 'thangha@gmail.com', '0912345706', N'Đà Lạt, Việt Nam', 'passwordhash29'),
('C030', N'Tô Thị Hằng', 'hangto@gmail.com', '0912345707', N'Bạc Liêu, Việt Nam', 'passwordhash30');

-- Thêm dữ liệu vào bảng Products (liên kết với Brands và Categories)
UPDATE Products SET ProductImage = 'msilaptop.png' WHERE ProductId = 'P001';
UPDATE Products SET ProductImage = 'dellxps.png' WHERE ProductId = 'P002';
UPDATE Products SET ProductImage = 'macbookpro.png' WHERE ProductId = 'P003';
UPDATE Products SET ProductImage = 'asuszenbook.png' WHERE ProductId = 'P004';
UPDATE Products SET ProductImage = 'iphone13.png' WHERE ProductId = 'P005';
UPDATE Products SET ProductImage = 'samsungs21.png' WHERE ProductId = 'P006';
UPDATE Products SET ProductImage = 'xiaomi11.png' WHERE ProductId = 'P007';
UPDATE Products SET ProductImage = 'opporeno6.png' WHERE ProductId = 'P008';
UPDATE Products SET ProductImage = 'ipadpro.png' WHERE ProductId = 'P009';
UPDATE Products SET ProductImage = 'lenovotab.png' WHERE ProductId = 'P010';
UPDATE Products SET ProductImage = 'logitechmouse.png' WHERE ProductId = 'P011';
UPDATE Products SET ProductImage = 'sonyxm4.png' WHERE ProductId = 'P012';
UPDATE Products SET ProductImage = 'corsairkeyboard.png' WHERE ProductId = 'P013';
UPDATE Products SET ProductImage = 'msi3080.png' WHERE ProductId = 'P014';
UPDATE Products SET ProductImage = 'galaxyzflip.png' WHERE ProductId = 'P015';
UPDATE Products SET ProductImage = 'xiaominote10.png' WHERE ProductId = 'P016';
UPDATE Products SET ProductImage = 'oppoa54.png' WHERE ProductId = 'P017';
UPDATE Products SET ProductImage = 'macbookair.png' WHERE ProductId = 'P018';
UPDATE Products SET ProductImage = 'asustuf.png' WHERE ProductId = 'P019';
UPDATE Products SET ProductImage = 'dellinspiron14.png' WHERE ProductId = 'P020';
UPDATE Products SET ProductImage = 'lenovolegion5.png' WHERE ProductId = 'P021';
UPDATE Products SET ProductImage = 'iphone12.png' WHERE ProductId = 'P022';
UPDATE Products SET ProductImage = 'samsungnote20.png' WHERE ProductId = 'P023';
UPDATE Products SET ProductImage = 'miPad5.png' WHERE ProductId = 'P024';
UPDATE Products SET ProductImage = 'oppofindx3.png' WHERE ProductId = 'P025';
UPDATE Products SET ProductImage = 'lenovothinkpad.png' WHERE ProductId = 'P026';
UPDATE Products SET ProductImage = 'asusrog.png' WHERE ProductId = 'P027';
UPDATE Products SET ProductImage = 'iphonese.png' WHERE ProductId = 'P028';
UPDATE Products SET ProductImage = 'samsunga52.png' WHERE ProductId = 'P029';
UPDATE Products SET ProductImage = 'xiaomi10t.png' WHERE ProductId = 'P030';

UPDATE Products
SET ProductName = N'Chuột Logitech'
WHERE ProductId = 'P011';

INSERT INTO Products (ProductId, ProductName, ProductDescription, Price, Unit, ProductImage, CategoryId, BrandId) VALUES
('P001', 'MSI Laptop', N'Laptop gaming MSI', 25000000, N'VND', 'msilaptop.png', 'L01', 'MSI01'),
('P002', 'Dell XPS', N'Laptop cao cấp Dell', 23000000, N'VND', 'dellxps.png', 'L01', 'DELL01'),
('P003', 'MacBook Pro', N'Laptop cao cấp Apple', 35000000, N'VND', 'macbookpro.png', 'L01', 'APPLE01'),
('P004', 'Asus ZenBook', N'Laptop Asus mỏng nhẹ', 15000000, N'VND', 'asuszenbook.png', 'L01', 'ASUS01'),
('P005', 'Iphone 13', N'Điện thoại iPhone 13', 22000000, N'VND', 'iphone13.png', 'DT01', 'APPLE02'),
('P006', 'Samsung Galaxy S21', N'Điện thoại Samsung Galaxy S21', 20000000, N'cái', 'samsungs21.png', 'DT01', 'SS01'),
('P007', 'Xiaomi Mi 11', N'Điện thoại Xiaomi Mi 11', 12000000, N'VND', 'xiaomi11.png', 'DT01', 'XM01'),
('P008', 'Oppo Reno6', N'Điện thoại Oppo Reno6', 10000000, N'VND', 'opporeno6.png', 'DT01', 'OP01'),
('P009', 'iPad Pro', N'Máy tính bảng iPad Pro', 30000000, N'VND', 'ipadpro.png', 'MTB01', 'APPLE01'),
('P010', 'Lenovo Tab M10', N'Máy tính bảng Lenovo Tab M10', 7000000, N'VND', 'lenovotab.png', 'MTB01', 'LE01'),
('P011', 'Chuột Logitech', N'Chuột không dây Logitech', 500000, N'VND', 'logitechmouse.png', 'PK01', 'MSI01'),
('P012', 'Tai nghe Sony', N'Tai nghe Sony WH-1000XM4', 8000000, N'VND', 'sonyxm4.png', 'PK01', 'DELL01'),
('P013', 'Bàn phím Corsair', N'Bàn phím cơ Corsair K95', 3500000, N'VND', 'corsairkeyboard.png', 'PK01', 'APPLE01'),
('P014', 'MSI RTX 3080', N'Card đồ họa MSI RTX 3080', 15000000, N'VND', 'msi3080.png', 'L01', 'MSI01'),
('P015', 'Samsung Galaxy Z Flip', N'Điện thoại gập Samsung Galaxy Z Flip', 25000000, N'VND', 'galaxyzflip.png', 'DT01', 'SS01'),
('P016', 'Xiaomi Redmi Note 10', N'Điện thoại Xiaomi Redmi Note 10', 8000, N'VND', 'xiaominote10.png', 'DT01', 'XM01'),
('P017', 'Oppo A54', N'Điện thoại Oppo A54', 6000000, N'VND', 'oppoa54.png', 'DT01', 'OP01'),
('P018', 'MacBook Air', N'Laptop MacBook Air', 25000000, N'VND', 'macbookair.png', 'L01', 'APPLE01'),
('P019', 'Asus TUF Gaming', N'Laptop gaming Asus TUF', 20000000, N'VND', 'asustuf.png', 'L01', 'ASUS01'),
('P020', 'Dell Inspiron 14', N'Laptop Dell Inspiron 14', 18000000, N'VND', 'dellinspiron14.png', 'L01', 'DELL01'),
('P021', 'Lenovo Legion 5', N'Laptop gaming Lenovo Legion 5', 22000000, N'VND', 'lenovolegion5.png', 'L01', 'LE01'),
('P022', 'Iphone 12', N'Điện thoại iPhone 12', 18000000, N'cái', 'iphone12.png', 'DT01', 'APPLE02'),
('P023', 'Samsung Note 20', N'Điện thoại Samsung Note 20', 22000000, N'VND', 'samsungnote20.png', 'DT01', 'SS01'),
('P024', 'Xiaomi Mi Pad 5', N'Máy tính bảng Xiaomi Mi Pad 5', 10000000, N'VND', 'miPad5.png', 'MTB01', 'XM01'),
('P025', 'Oppo Find X3', N'Điện thoại Oppo Find X3', 19000000, N'VND', 'oppofindx3.png', 'DT01', 'OP01'),
('P026', 'Lenovo ThinkPad X1', N'Laptop Lenovo ThinkPad X1', 35000000, N'VND', 'lenovothinkpad.png', 'L01', 'LE01'),
('P027', 'Asus ROG Strix', N'Laptop gaming Asus ROG Strix', 28000000, N'VND', 'asusrog.png', 'L01', 'ASUS01'),
('P028', 'Iphone SE', N'Điện thoại iPhone SE', 12000000, 'VND', 'iphonese.png', 'DT01', 'APPLE02'),
('P029', 'Samsung Galaxy A52', N'Điện thoại Samsung Galaxy A52', 14000000, N'VND', 'samsunga52.png', 'DT01', 'SS01'),
('P030', 'Xiaomi Mi 10T', N'Điện thoại Xiaomi Mi 10T', 15000000, N'VND', 'xiaomi10t.png', 'DT01', 'XM01');

-- Thêm dữ liệu vào bảng Orders
INSERT INTO Orders (OrderId, OrderDate, ShipDate, OrderStatus) VALUES
('O001', '2024-11-01', '2024-11-05', 'Shipped'),
('O002', '2024-11-02', '2024-11-06', 'Pending'),
('O003', '2024-11-03', '2024-11-07', 'Completed'),
('O004', '2024-11-04', '2024-11-08', 'Processing'),
('O005', '2024-11-05', '2024-11-09', 'Cancelled');

-- Thêm dữ liệu vào bảng OrderDetails
INSERT INTO OrderDetails (OrderDetailsId, OrderId, ProductId, Quantity, Price) VALUES
('OD001', 'O001', 'P001', 1, 25000),
('OD002', 'O001', 'P005', 2, 22000),
('OD003', 'O002', 'P003', 1, 35000),
('OD004', 'O003', 'P007', 3, 12000),
('OD005', 'O004', 'P015', 1, 25000),
('OD006', 'O005', 'P022', 2, 12000);
DROP PROCEDURE IF EXISTS SearchProducts;
GO
CREATE PROCEDURE SearchProducts
    @ProductName NVARCHAR(100) = NULL,  -- Tên sản phẩm
    @BrandName NVARCHAR(100) = NULL,    -- Hãng sản phẩm
    @CategoryName NVARCHAR(100) = NULL, -- Danh mục sản phẩm
    @MinPrice DECIMAL(10, 2) = NULL,    -- Giá tối thiểu
    @MaxPrice DECIMAL(10, 2) = NULL     -- Giá tối đa
AS
BEGIN
    -- Truy vấn sản phẩm theo các tiêu chí
    SELECT 
        p.ProductId,
        p.ProductName,
        p.ProductDescription,
        p.Price,
        p.Unit,
        p.ProductImage,
        p.CategoryId,
        p.BrandId,
        p.CreateDate,    -- Thêm CreateDate
        c.CategoryName,
        b.BrandName
    FROM 
        Products p
    JOIN 
        Categories c ON p.CategoryId = c.CategoryId
    JOIN 
        Brands b ON p.BrandId = b.BrandId
    WHERE
        (p.ProductName LIKE '%' + ISNULL(@ProductName, p.ProductName) + '%') AND
        (b.BrandName LIKE '%' + ISNULL(@BrandName, b.BrandName) + '%') AND
        (c.CategoryName LIKE '%' + ISNULL(@CategoryName, c.CategoryName) + '%') AND
        (p.Price >= ISNULL(@MinPrice, 0)) AND
        (p.Price <= ISNULL(@MaxPrice, 99999999.99))
    ORDER BY
        p.ProductName;
END;
GO
CREATE PROCEDURE SearchUsers
    @Username NVARCHAR(100) = NULL,  -- Tên đăng nhập
    @Email NVARCHAR(100) = NULL,     -- Email
    @Role NVARCHAR(100) = NULL        -- Vai trò
AS
BEGIN
    -- Truy vấn người dùng theo các tiêu chí
    SELECT 
        u.UserId,
        u.Username,
        u.Email,
        u.Role
    FROM 
        Users u
    WHERE
        (u.Username LIKE '%' + ISNULL(@Username, u.Username) + '%') AND
        (u.Email LIKE '%' + ISNULL(@Email, u.Email) + '%') AND
        (u.Role LIKE '%' + ISNULL(@Role, u.Role) + '%')
    ORDER BY
        u.Username;
END;
GO
CREATE TRIGGER trg_InsertCustomer
ON Users
AFTER INSERT
AS
BEGIN
    INSERT INTO Customers (CustomerId, FullName, Email, PhoneNumber, CustomerAddress, PasswordHash)
    SELECT 
        'C' + CAST(UserId AS NVARCHAR(19)), -- CustomerId (Ghép tiền tố C và UserId)
        Username,                          -- FullName
        Email,                             -- Email
        PhoneNumber,                       -- PhoneNumber
        NULL,                              -- CustomerAddress
        PasswordHash                       -- PasswordHash
    FROM inserted
    WHERE Role = 'Customer';
END;






