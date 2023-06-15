INSERT INTO Product(ProductId, ProductName, ProductDescription)
VALUES	('ca449bda-9bcd-4f2f-a2c4-60700a577bbb', 'Coke', 'Americas favorite beverage'),
		('0cca6a49-7186-43c9-9688-1e2068b01686', 'Pepsi', 'Americas favorate beverage to hate'),
		('a77031e4-f4b5-41f5-9978-ba7b622aa663', 'Sprite', 'The real best beverage')
GO

		INSERT INTO Store(StoreId, StoreName, StoreDescription)
VALUES	('fbb39e2a-1b01-4832-96b8-5cc41c22f48f', 'Walmart', 'Americas favorite supermarket to hate'),
		('2df9925c-f431-4a01-8f88-db83b9ac6f46', 'Target', 'Your girlfriends favorate supermarket'),
		('6c17772b-4e2d-4a33-af36-25b6b551a862', 'Walgreens', 'Not even a supermarket')
GO

INSERT INTO StoreProduct(StoreId, ProductId)
VALUES	('fbb39e2a-1b01-4832-96b8-5cc41c22f48f', 'a77031e4-f4b5-41f5-9978-ba7b622aa663'),
		('6c17772b-4e2d-4a33-af36-25b6b551a862', 'ca449bda-9bcd-4f2f-a2c4-60700a577bbb')
GO