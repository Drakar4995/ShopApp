
SET IDENTITY_INSERT [dbo].[MetodoPago] ON
INSERT INTO [dbo].[MetodoPago] ([ID], [PaymentMethod_type], [Email], [Prefix], [Phone], [CreditCardNumber], [CCV], [ExpirationDate]) VALUES (9, N'PaymentMethod_CreditCard', NULL, NULL, NULL, N'1234567891234567', N'123', N'2023-10-12 00:00:00')
SET IDENTITY_INSERT [dbo].[MetodoPago] OFF
SET IDENTITY_INSERT [dbo].[Compra] ON
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (10, 205, N'2021-12-13 21:19:45', N'CALLE 2', N'3', 9)
SET IDENTITY_INSERT [dbo].[Compra] OFF


SET IDENTITY_INSERT [dbo].[ItemCompra] ON
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (30, 1, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (31, 2, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (32, 3, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (33, 4, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (34, 6, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (35, 7, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (36, 8, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (37, 9, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (38, 10, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (39, 11, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (40,12, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (41, 13, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (42, 14, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (43, 15, 31, 10)
INSERT INTO [dbo].[ItemCompra] ([Id], [PrendaID], [Cantidad], [CompraID]) VALUES (44, 16, 31, 10)
SET IDENTITY_INSERT [dbo].[ItemCompra] OFF
