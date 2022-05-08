SET IDENTITY_INSERT [dbo].[Marca] ON
INSERT INTO [dbo].[Marca] ([MarcaID], [Nombre]) VALUES (1, 'Nike')
INSERT INTO [dbo].[Marca] ([MarcaID], [Nombre]) VALUES (2, 'Adidas')
INSERT INTO [dbo].[Marca] ([MarcaID], [Nombre]) VALUES (3, 'Joma')
INSERT INTO [dbo].[Marca] ([MarcaID], [Nombre]) VALUES (4, 'Kappa')
INSERT INTO [dbo].[Marca] ([MarcaID], [Nombre]) VALUES (5, 'Versache')
INSERT INTO [dbo].[Marca] ([MarcaID], [Nombre]) VALUES (6, 'Puma')

SET IDENTITY_INSERT [dbo].[Marca] OFF

SET IDENTITY_INSERT [dbo].[Prenda] ON
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (1, 'Camiseta', 10, '12/10/2020',10, 1, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (2, 'Abrigo', 40, '12/09/2000',10, 2, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (3, 'Pantalon', 15, '12/09/1998',12, 2, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (4, 'Sudadera', 20, '12/09/2017',120, 3, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (5, 'Calcetines', 5, '12/09/1998',120, 1, 'True')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (6, 'Deportivas', 100, '12/09/2017',120, 4, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (7, 'Camiseta con Capucha', 5, '12/09/2017',120, 1, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (8, 'Sudadera con Capucha', 200, '12/09/2017',120, 5, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (9, 'Prenda1', 5, '12/09/2017',120, 1, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (10, 'Prenda2', 5, '12/09/2017',120, 2, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (11, 'Prenda3', 5, '12/09/2017',120, 3, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (12, 'Prenda4', 20, '12/09/2017',120, 4, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (13, 'Gorra', 15, '12/09/2017',120, 2, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (14, 'Pantalon de Camuflaje', 30, '12/09/1998',120, 1, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (15, 'Camiseta Manga Corta', 20, '12/10/2020',120, 6, 'False')
INSERT INTO [dbo].[Prenda] ([PrendaID], [Nombre], [PrecioPrenda], [FechaLanzamiento], [CantidadCompra], [MarcaID], [isRetired]) VALUES (16, 'Pantalon Deportivo', 40, '12/09/1998',120, 1, 'False')




SET IDENTITY_INSERT [dbo].[Prenda] OFF


SET IDENTITY_INSERT [dbo].[Categoria] ON
INSERT INTO [dbo].[Categoria] ([CategoriaID], [Nombre]) VALUES (1, 'Originals')
INSERT INTO [dbo].[Categoria] ([CategoriaID], [Nombre]) VALUES (2, 'Kids')
INSERT INTO [dbo].[Categoria] ([CategoriaID], [Nombre]) VALUES (3, 'Sport')
INSERT INTO [dbo].[Categoria] ([CategoriaID], [Nombre]) VALUES (4, 'Urban')
SET IDENTITY_INSERT [dbo].[Categoria] OFF

SET IDENTITY_INSERT [dbo].[NewsLetter] ON
INSERT INTO [dbo].[NewsLetter] ([Id], [Titulo], [Descripcion], [MarcaID], [CategoriaID]) VALUES (1, 'Newsletter1','desc1', 1, 1)
INSERT INTO [dbo].[NewsLetter] ([Id], [Titulo], [Descripcion], [MarcaID], [CategoriaID]) VALUES (2, 'Newsletter2','desc2', 2, 2)
INSERT INTO [dbo].[NewsLetter] ([Id], [Titulo], [Descripcion], [MarcaID], [CategoriaID]) VALUES (3, 'Newsletter3','desc3', 3, 3)
INSERT INTO [dbo].[NewsLetter] ([Id], [Titulo], [Descripcion], [MarcaID], [CategoriaID]) VALUES (4, 'Newsletter4','desc4', 4, 4)
SET IDENTITY_INSERT [dbo].[NewsLetter] OFF


SET IDENTITY_INSERT [dbo].[MetodoPago] ON
INSERT INTO [dbo].[MetodoPago] ([ID], [PaymentMethod_type], [Email], [Prefix], [Phone], [CreditCardNumber], [CCV], [ExpirationDate]) VALUES (1, N'PaymentMethod_CreditCard', NULL, NULL, NULL, N'1234567891234567', N'123', N'2023-10-12 00:00:00')
INSERT INTO [dbo].[MetodoPago] ([ID], [PaymentMethod_type], [Email], [Prefix], [Phone], [CreditCardNumber], [CCV], [ExpirationDate]) VALUES (6, N'PaymentMethod_CreditCard', NULL, NULL, NULL, N'1234567891234567', N'365', N'2024-10-12 00:00:00')
SET IDENTITY_INSERT [dbo].[MetodoPago] OFF

SET IDENTITY_INSERT [dbo].[Compra] ON
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (6, 205, N'2021-12-13 21:19:45', N'CALLE 2', N'3', 1)
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (7, 25, N'2021-12-13 21:20:31', N'CALLE 2', N'3', 1)
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (8, 10, N'2021-12-13 21:21:55', N'Calle 2', N'3', 1)
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (2, 850, N'2021-12-13 18:26:51', N'iaff', N'3', 6)
SET IDENTITY_INSERT [dbo].[Compra] OFF

SET IDENTITY_INSERT [dbo].[ItemCompra] ON
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (9, 7, 1, 6)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (10, 8, 1, 6)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (11, 9, 1, 7)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (12, 12, 1, 7)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (13, 1, 1, 8)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (3, 2, 1, 2)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (4, 14, 28, 2)
SET IDENTITY_INSERT [dbo].[ItemCompra] OFF

