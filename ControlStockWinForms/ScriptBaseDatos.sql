create database InventarioDB;
go

use InventarioDB;

-- productos 
create table Productos(
id int identity(1,1) primary key,
Nombre nvarchar(100) not null,
Descripcion nvarchar(255),
Precio decimal(10,2) not null,
Cantidad int not null,
StockMinimo int not null default 5,
FechaIngreso datetime default getdate()
);


--ventas
create table Ventas(
id int identity(1,1) primary key,
Fecha datetime default getdate(),
Total decimal(10,2) not null
);

--detalles 
create table Detalles(
id int identity(1,1) primary key,
Ventaid int foreign key references Ventas(id),
Productoid int foreign key references Productos(id),
Cantidad int not null,
Precio decimal(10,2) not null
); 


INSERT INTO Productos (Nombre, Descripcion, Precio, Cantidad, StockMinimo)
VALUES
('Mouse Inalámbrico', 'Mouse óptico inalámbrico USB', 15.50, 50, 5),
('Teclado Mecánico', 'Teclado mecánico RGB', 45.00, 20, 5),
('Monitor 24"', 'Monitor LED 24 pulgadas', 120.00, 10, 6),
('Impresora Láser', 'Impresora láser monocromo', 200.00, 5, 5),
('Laptop Dell', 'Laptop Dell Inspiron 15"', 650.00, 8, 5),
('Memoria RAM 8GB', 'DDR4 8GB 3200MHz', 35.00, 25, 5),
('Disco SSD 512GB', 'SSD interno 512GB', 60.00, 15, 5),
('Auriculares Gaming', 'Auriculares con micrófono', 25.00, 30, 5),
('Cámara Web HD', 'Cámara web 1080p', 40.00, 12, 5),
('Regulador de Voltaje', 'Protector de voltaje 8 tomas', 18.00, 20, 5);


select * from Productos