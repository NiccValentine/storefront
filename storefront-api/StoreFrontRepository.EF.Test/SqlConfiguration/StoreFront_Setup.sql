-- Product Record Creation
INSERT INTO Product(ProductId, ProductName, ProductDescription)
VALUES	('f1c2cf3e-b65f-4991-84a9-eca8dca3a08a', 'Product1', 'This is a test description 1'),
		('a7c29719-d5ed-400c-ae26-be578ee22b38', 'Product2', 'This is a test description 2'),
		('f0743f41-4c22-432a-9501-1855bdc5f887', 'Product3', 'This is a test description 3')

-- Store Record Creation
INSERT INTO Store(StoreId, StoreName, StoreDescription)
VALUES	('c25ef7e4-5641-40ac-b945-306fd3efc04b', 'Store1', 'This is a test description 1'),
		('ff0c12cc-c79a-428d-aafc-7a93970b01cd', 'Store2', 'This is a test description 2'),
		('7d43aab0-4a94-4556-b2ae-b131e86f4c25', 'Store3', 'This is a test description 3')

-- StoreProduct Record Creation
INSERT INTO StoreProduct(StoreId, ProductId)
VALUES	('c25ef7e4-5641-40ac-b945-306fd3efc04b', 'f1c2cf3e-b65f-4991-84a9-eca8dca3a08a'),
		('ff0c12cc-c79a-428d-aafc-7a93970b01cd', 'a7c29719-d5ed-400c-ae26-be578ee22b38')