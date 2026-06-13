-- =========================================================================
-- SEED DATA: MICROSERVICES
-- =========================================================================

-- 1. COMPANIES
INSERT INTO inventory.companies (company_cen, name) 
VALUES ('COMP-001', 'Restaurante El Gourmet');

-- 2. WAREHOUSES
INSERT INTO inventory.warehouses (warehouse_cen, company_cen, name)
VALUES ('WH-001', 'COMP-001', 'Almacén Central');

-- 3. CATEGORIES
INSERT INTO inventory.categories (category_cen, company_cen, name, description)
VALUES 
('CAT-001', 'COMP-001', 'Bebidas', 'Todo tipo de refrescos y licores'),
('CAT-002', 'COMP-001', 'Comidas', 'Platos principales y entradas');

-- 4. UNITS
INSERT INTO inventory.units (unit_cen, company_cen, name, abbreviation)
VALUES 
('UNIT-001', 'COMP-001', 'Unidad', 'UND'),
('UNIT-002', 'COMP-001', 'Kilogramo', 'KG');

-- 5. PRODUCTS
INSERT INTO inventory.products (product_cen, company_cen, category_cen, unit_cen, sku, name, sale_price, cost_price, reorder_level)
VALUES 
('PROD-001', 'COMP-001', 'CAT-001', 'UNIT-001', 'BEB-001', 'Coca Cola 500ml', 2.50, 1.20, 10.00),
('PROD-002', 'COMP-001', 'CAT-002', 'UNIT-001', 'COM-001', 'Hamburguesa Especial', 8.50, 4.00, 5.00);

-- 6. STOCK
INSERT INTO inventory.stock (stock_cen, product_cen, warehouse_cen, available_quantity)
VALUES 
('STK-001', 'PROD-001', 'WH-001', 50.00),
('STK-002', 'PROD-002', 'WH-001', 20.00);

-- 7. PURCHASING: SUPPLIERS
INSERT INTO purchasing.suppliers (supplier_cen, company_cen, name)
VALUES ('SUPP-001', 'COMP-001', 'Distribuidora Global');

-- 8. SALES: TAX CONFIG
INSERT INTO sales.tax_configurations (company_cen, global_tax_percentage)
VALUES ('COMP-001', 18.00);

-- 9. SALES: PAYMENT METHODS
INSERT INTO sales.payment_methods (payment_method_code, name)
VALUES 
('CASH', 'Efectivo'),
('CARD', 'Tarjeta de Crédito');

-- 10. SALES: WAITERS
INSERT INTO sales.waiters (waiter_cen, company_cen, name)
VALUES ('WAIT-001', 'COMP-001', 'Juan Perez');
