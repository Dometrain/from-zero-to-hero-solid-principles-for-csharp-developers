INSERT INTO users (first_name, last_name, email, password, delivery_address, country_code, is_admin)
VALUES
('Admin', 'User', 'admin.user@solid.com', 'AQAAAAIAAYagAAAAEHF1gNPofEztyupY6SuUvtgcfr1Txf4v87lbtlqJUdF2uitAGYz3eI2rs0kvHBFwLg==', '123 Main St, Pasadena, CA', 'USA', true),
('Normal', 'User', 'normal.user@example.com', 'AQAAAAIAAYagAAAAED4O09ibNDeqK/KIENSNzREdBOu23IbZ+f7X3yImJ9mK84ewCeVQ36kT1RhcAJzUIA==', '12 Digby Avenue, Stook, Avonshire', 'GBR', false);

INSERT INTO products (name, price, description)
VALUES
('iPhone 15', 999.99, 'Apple smartphone'),
('Samsung Galaxy S21', 899.99, 'Samsung smartphone'),
('Google Pixel 5', 699.99, 'Google smartphone'),
('Sony PlayStation 5', 499.99, 'Gaming console'),
('Microsoft Xbox Series X', 499.99, 'Gaming console'),
('MacBook Pro', 1999.99, 'Apple laptop'),
('Dell XPS 15', 1499.99, 'Dell laptop'),
('HP Envy 13', 1199.99, 'HP laptop'),
('Lenovo ThinkPad X1 Carbon', 1799.99, 'Lenovo laptop'),
('Canon EOS R5', 3799.99, 'Professional camera'),
('Nikon Z7 II', 2999.99, 'Professional camera'),
('Sony Alpha A7R IV', 3199.99, 'Professional camera'),
('AirPods Pro', 249.99, 'Apple wireless earphones'),
('Sony WH-1000XM4', 349.99, 'Wireless noise-canceling headphones'),
('Bose QuietComfort 35 II', 299.99, 'Wireless noise-canceling headphones'),
('Samsung 65-inch 4K QLED TV', 1499.99, 'Smart TV'),
('LG 55-inch OLED TV', 1999.99, 'Smart TV'),
('Sony 75-inch 4K LED TV', 2499.99, 'Smart TV'),
('Nintendo Switch', 299.99, 'Gaming console'),
('Microsoft Surface Pro 7', 1199.99, 'Windows tablet'),
('Fitbit Charge 4', 149.99, 'Fitness tracker'),
('Garmin Fenix 6 Pro', 799.99, 'Fitness smartwatch'),
('GoPro HERO 9 Black', 449.99, 'Action camera'),
('Bose SoundLink Revolve+', 299.99, 'Portable Bluetooth speaker'),
('JBL Flip 5', 129.99, 'Portable Bluetooth speaker'),
('Sennheiser HD 660 S', 499.99, 'High-end headphones'),
('Logitech MX Master 3', 99.99, 'Wireless mouse'),
('Razer Huntsman Elite', 199.99, 'Mechanical gaming keyboard'),
('SteelSeries Arctis 7', 149.99, 'Gaming headset'),
('Samsung T7 Portable SSD', 199.99, 'External Solid State Drive'),
('WD My Passport External HDD', 129.99, 'External Hard Drive');

INSERT INTO orders (user_id, order_total, delivery_cost, tax)
VALUES
(1, 2799.97, 3.99, 0.00),
(2, 699.99, 3.99, 0.00);

INSERT INTO shopping_cart (user_id) VALUES
(1),
(2);

INSERT INTO shopping_cart_items (cart_id, product_id, quantity)
VALUES
(1, 1, 1),
(1, 2, 2),
(1, 3, 1),
(2, 3, 1);

INSERT INTO ordered_products (order_id, product_id, quantity) VALUES
(1, 1, 1),
(1, 2, 2),
(2, 3, 1);