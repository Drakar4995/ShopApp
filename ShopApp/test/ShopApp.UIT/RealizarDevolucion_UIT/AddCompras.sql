SET IDENTITY_INSERT [dbo].[MetodoPago] ON
INSERT INTO [dbo].[MetodoPago] ([ID], [PaymentMethod_type], [Email], [Prefix], [Phone], [CreditCardNumber], [CCV], [ExpirationDate]) VALUES (1, N'PaymentMethod_CreditCard', NULL, NULL, NULL, N'1234567891234567', N'123', N'2023-10-12 00:00:00')
SET IDENTITY_INSERT [dbo].[MetodoPago] OFF

SET IDENTITY_INSERT [dbo].[Compra] ON
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (6, 205, N'2021-12-13 21:19:45', N'CALLE 2', N'3', 1)
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (7, 25, N'2021-12-13 21:20:31', N'CALLE 2', N'3', 1)
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (8, 10, N'2021-12-13 21:21:55', N'Calle 2', N'3', 1)
SET IDENTITY_INSERT [dbo].[Compra] OFF

SET IDENTITY_INSERT [dbo].[ItemCompra] ON
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (9, 7, 1, 6)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (10, 8, 1, 6)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (11, 9, 1, 7)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (12, 12, 1, 7)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (13, 1, 1, 8)
SET IDENTITY_INSERT [dbo].[ItemCompra] OFF

