-- =========================================================================
-- 02-SEED.SQL: ESCENARIO MASIVO DE RESTAURANTES (REVISADO Y CORREGIDO)
-- Simulación de Franquicias, Almacenes Centrales y Flujo Operativo Real
-- =========================================================================

-- =========================================================================
-- 1. COMPAÑÍAS
-- =========================================================================
INSERT INTO inventory.companies (company_cen, name) VALUES 
('RE-PIZZA', 'Luigi´s Authentic Pizzeria'),
('RE-STEAK', 'The Grill Master - Premium Steakhouse'),
('RE-SUSHI', 'Zen Fusion & Sushi Bar'),
('RE-COFFEE', 'Coffee & Beans Artisanal Bakery');

-- =========================================================================
-- 2. ALMACENES
-- =========================================================================
INSERT INTO inventory.warehouses (warehouse_cen, company_cen, name) VALUES 
('WH-PIZ-KIT', 'RE-PIZZA', 'Cocina de Producción'),
('WH-PIZ-BAR', 'RE-PIZZA', 'Barra y Bebidas'),
('WH-PIZ-STO', 'RE-PIZZA', 'Almacén de Secos y Harinas'),
('WH-STK-KIT', 'RE-STEAK', 'Línea de Fuego (Cocina)'),
('WH-STK-COL', 'RE-STEAK', 'Cámara Frigorífica (Cortes)'),
('WH-STK-BAR', 'RE-STEAK', 'Cava de Vinos'),
('WH-SUS-KIT', 'RE-SUSHI', 'Barra de Sushi'),
('WH-SUS-STO', 'RE-SUSHI', 'Depósito General'),
('WH-COF-MAI', 'RE-COFFEE', 'Barra Principal'),
('WH-COF-BAK', 'RE-COFFEE', 'Área de Panadería');

-- =========================================================================
-- 3. CATEGORÍAS
-- =========================================================================
INSERT INTO inventory.categories (category_cen, company_cen, name, description) VALUES 
('CAT-PIZ-ENT', 'RE-PIZZA', 'Entradas Italianas', 'Antipastos y Focaccias'),
('CAT-PIZ-MAI', 'RE-PIZZA', 'Pizzas Artesanales', 'Pizzas a la leña'),
('CAT-PIZ-PAS', 'RE-PIZZA', 'Pastas Tradicionales', 'Pastas frescas hechas en casa'),
('CAT-STK-COR', 'RE-STEAK', 'Cortes de Res', 'Cortes Premium'),
('CAT-STK-GUA', 'RE-STEAK', 'Guarniciones', 'Acompañamientos'),
('CAT-STK-VIN', 'RE-STEAK', 'Vinos de Cava', 'Selección de tintos'),
('CAT-SUS-ROL', 'RE-SUSHI', 'Special Rolls', 'Makis de autor'),
('CAT-SUS-NIG', 'RE-SUSHI', 'Nigiris & Sashimi', 'Pescados frescos'),
('CAT-COF-HOT', 'RE-COFFEE', 'Cafés Calientes', 'Espressos y Lattes'),
('CAT-COF-BAK', 'RE-COFFEE', 'Repostería Finas', 'Postres artesanales');

-- =========================================================================
-- 4. UNIDADES DE MEDIDA
-- =========================================================================
INSERT INTO inventory.units (unit_cen, company_cen, name, abbreviation) VALUES 
('UN-PIZ-UND', 'RE-PIZZA', 'Unidad', 'UND'),
('UN-PIZ-KG', 'RE-PIZZA', 'Kilogramo', 'KG'),
('UN-STK-UND', 'RE-STEAK', 'Unidad', 'UND'),
('UN-STK-GR', 'RE-STEAK', 'Gramo', 'GR'),
('UN-STK-BOT', 'RE-STEAK', 'Botella 750ml', 'BOT'),
('UN-SUS-UND', 'RE-SUSHI', 'Unidad', 'UND'),
('UN-SUS-KG', 'RE-SUSHI', 'Kilogramo', 'KG'),
('UN-COF-UND', 'RE-COFFEE', 'Unidad', 'UND'),
('UN-COF-OZ', 'RE-COFFEE', 'Onza', 'OZ');

-- =========================================================================
-- 5. PRODUCTOS (Menú y Materia Prima)
-- =========================================================================
INSERT INTO inventory.products (product_cen, company_cen, category_cen, unit_cen, sku, name, description, sale_price, cost_price, reorder_level, station_code) VALUES 
('PR-PZ-01', 'RE-PIZZA', 'CAT-PIZ-MAI', 'UN-PIZ-UND', 'PZ-MARG', 'Pizza Margherita', 'Tomate, Mozzarella', 14.50, 4.20, 0, 'ST-OVEN'),
('PR-PZ-02', 'RE-PIZZA', 'CAT-PIZ-MAI', 'UN-PIZ-UND', 'PZ-PEPP', 'Pizza Pepperoni', 'Doble pepperoni', 16.00, 5.10, 0, 'ST-OVEN'),
('PR-PZ-03', 'RE-PIZZA', 'CAT-PIZ-PAS', 'UN-PIZ-UND', 'PA-LASA', 'Lasagna de la Casa', 'Receta tradicional', 12.50, 4.80, 0, 'ST-KITCH'),
('PR-PZ-I01', 'RE-PIZZA', 'CAT-PIZ-MAI', 'UN-PIZ-KG', 'IN-HARINA', 'Harina 00', 'Harina importada', 0.00, 1.20, 50, 'ST-STORE'),
('PR-ST-01', 'RE-STEAK', 'CAT-STK-COR', 'UN-STK-UND', 'ST-RIBEYE', 'Ribeye Angus 400g', 'Corte premium', 38.00, 14.50, 10, 'ST-GRILL'),
('PR-ST-02', 'RE-STEAK', 'CAT-STK-COR', 'UN-STK-UND', 'ST-PICA', 'Picaña 350g', 'Sabor intenso', 32.00, 11.20, 15, 'ST-GRILL'),
('PR-ST-03', 'RE-STEAK', 'CAT-STK-VIN', 'UN-STK-BOT', 'VI-MALB', 'Malbec Reserva', 'Mendoza, Argentina', 55.00, 22.00, 12, 'ST-BAR'),
('PR-SU-01', 'RE-SUSHI', 'CAT-SUS-ROL', 'UN-SUS-UND', 'SU-ACEB', 'Acevichado Roll', 'Langostino y palta', 12.00, 3.80, 0, 'ST-SUSHI'),
('PR-SU-02', 'RE-SUSHI', 'CAT-SUS-ROL', 'UN-SUS-UND', 'SU-CALI', 'California Roll', 'Cangrejo y palta', 10.00, 3.20, 0, 'ST-SUSHI'),
('PR-SU-03', 'RE-SUSHI', 'CAT-SUS-NIG', 'UN-SUS-UND', 'SU-NIG-SA', 'Nigiri Salmón', 'Pescado fresco', 7.50, 2.10, 0, 'ST-SUSHI'),
('PR-CF-01', 'RE-COFFEE', 'CAT-COF-HOT', 'UN-COF-UND', 'CF-CAPT', 'Cappuccino', 'Leche vaporizada', 4.50, 0.90, 0, 'ST-BARISTA'),
('PR-CF-02', 'RE-COFFEE', 'CAT-COF-HOT', 'UN-COF-UND', 'CF-AMER', 'Americano', 'Blend especial', 3.00, 0.40, 0, 'ST-BARISTA'),
('PR-CF-03', 'RE-COFFEE', 'CAT-COF-BAK', 'UN-COF-UND', 'BK-CROIS', 'Croissant', 'Mantequilla pura', 2.80, 0.85, 20, 'ST-BAKERY');

-- =========================================================================
-- 6. STOCK INICIAL
-- =========================================================================
INSERT INTO inventory.stock (stock_cen, product_cen, warehouse_cen, available_quantity, reserved_quantity) VALUES 
('SK-PZ-01', 'PR-PZ-I01', 'WH-PIZ-STO', 500.00, 0),
('SK-PZ-02', 'PR-PZ-01', 'WH-PIZ-KIT', 20.00, 0),
('SK-ST-01', 'PR-ST-01', 'WH-STK-COL', 45.00, 5.00),
('SK-ST-02', 'PR-ST-03', 'WH-STK-BAR', 60.00, 0),
('SK-SU-01', 'PR-SU-01', 'WH-SUS-KIT', 15.00, 0),
('SK-CF-01', 'PR-CF-03', 'WH-COF-BAK', 40.00, 0);

-- =========================================================================
-- 7. DOCUMENTOS E INVENTARIO
-- =========================================================================
INSERT INTO inventory.inventory_documents (document_cen, company_cen, warehouse_cen, document_type, status, title, reason, external_reference) VALUES 
('DOC-INV-001', 'RE-PIZZA', 'WH-PIZ-STO', 'PURCHASE_RECEIPT', 'COMPLETED', 'Llegada de Insumos', 'Orden Mensual', 'FAC-99812'),
('DOC-INV-002', 'RE-STEAK', 'WH-STK-KIT', 'TRANSFER_IN', 'COMPLETED', 'Reposición Cocina', 'Traslado desde Cámara', 'INT-442'),
('DOC-INV-003', 'RE-COFFEE', 'WH-COF-BAK', 'ADJUSTMENT_OUT', 'COMPLETED', 'Mermas Panadería', 'Productos dañados', 'ADJ-101');

INSERT INTO inventory.inventory_document_lines (line_cen, document_cen, product_cen, quantity, unit_cost) VALUES 
('L-INV-001', 'DOC-INV-001', 'PR-PZ-I01', 200.00, 1.15),
('L-INV-002', 'DOC-INV-002', 'PR-ST-01', 10.00, 14.50),
('L-INV-003', 'DOC-INV-003', 'PR-CF-03', 5.00, 0.85);

-- =========================================================================
-- 8. KARDEX
-- =========================================================================
INSERT INTO inventory.kardex_movements (movement_cen, company_cen, warehouse_cen, product_cen, document_cen, movement_type, quantity, unit_cost, reason) VALUES 
('KDX-001', 'RE-PIZZA', 'WH-PIZ-STO', 'PR-PZ-I01', 'DOC-INV-001', 'IN', 200.00, 1.15, 'Compra'),
('KDX-002', 'RE-STEAK', 'WH-STK-COL', 'PR-ST-01', 'DOC-INV-002', 'OUT', 10.00, 14.50, 'Traslado Salida'),
('KDX-003', 'RE-STEAK', 'WH-STK-KIT', 'PR-ST-01', 'DOC-INV-002', 'IN', 10.00, 14.50, 'Traslado Entrada');

-- =========================================================================
-- 9. COMPRAS
-- =========================================================================
INSERT INTO purchasing.suppliers (supplier_cen, company_cen, name) VALUES 
('SUP-IT-IMP', 'RE-PIZZA', 'Importadora La Italiana'),
('SUP-MEAT-P', 'RE-STEAK', 'Carnes del Sur S.A.'),
('SUP-FISH-D', 'RE-SUSHI', 'Ocean Fresh Fish Co.'),
('SUP-MILK-L', 'RE-COFFEE', 'Lácteos El Tambo');

INSERT INTO purchasing.purchase_orders (order_cen, company_cen, warehouse_cen, supplier_cen, status, created_at, confirmed_at) VALUES 
('PO-REST-001', 'RE-PIZZA', 'WH-PIZ-STO', 'SUP-IT-IMP', 1, NOW() - INTERVAL '10 days', NOW() - INTERVAL '9 days'),
('PO-REST-002', 'RE-STEAK', 'WH-STK-COL', 'SUP-MEAT-P', 1, NOW() - INTERVAL '2 days', NOW() - INTERVAL '1 day'),
('PO-REST-003', 'RE-SUSHI', 'WH-SUS-STO', 'SUP-FISH-D', 0, NOW(), NULL);

INSERT INTO purchasing.purchase_order_items (item_cen, order_cen, product_cen, quantity) VALUES 
('POI-R-01', 'PO-REST-001', 'PR-PZ-I01', 500.00),
('POI-R-02', 'PO-REST-002', 'PR-ST-01', 50.00),
('POI-R-03', 'PO-REST-002', 'PR-ST-02', 30.00);

-- =========================================================================
-- 10. VENTAS CONFIG
-- =========================================================================
INSERT INTO sales.tax_configurations (company_cen, global_tax_percentage) VALUES 
('RE-PIZZA', 18.00), ('RE-STEAK', 18.00), ('RE-SUSHI', 15.00), ('RE-COFFEE', 10.00);

INSERT INTO sales.payment_methods (payment_method_code, name, is_active) VALUES 
('CASH', 'Efectivo', true), ('DEBIT', 'Tarjeta Débito', true), ('CREDIT', 'Tarjeta Crédito', true), ('QR-PAY', 'Pago QR', true);

INSERT INTO sales.waiters (waiter_cen, company_cen, name) VALUES 
('W-PIZ-01', 'RE-PIZZA', 'Franco'), ('W-PIZ-02', 'RE-PIZZA', 'Guido'),
('W-STK-01', 'RE-STEAK', 'Roberto'), ('W-STK-02', 'RE-STEAK', 'Elena'),
('W-SUS-01', 'RE-SUSHI', 'Kenji'), ('W-SUS-02', 'RE-SUSHI', 'Akira'),
('W-COF-01', 'RE-COFFEE', 'Lucía');

INSERT INTO sales.kds_teams (team_cen, company_cen, name) VALUES 
('KDS-PIZ-FUE', 'RE-PIZZA', 'Hornos'), ('KDS-PIZ-FRI', 'RE-PIZZA', 'Producción'),
('KDS-STK-GRI', 'RE-STEAK', 'Parrilla'), ('KDS-SUS-BAR', 'RE-SUSHI', 'Barra Sushi'),
('KDS-COF-BAR', 'RE-COFFEE', 'Baristas');

INSERT INTO sales.kds_team_categories (team_cen, category_cen) VALUES 
('KDS-PIZ-FUE', 'CAT-PIZ-MAI'), ('KDS-PIZ-FRI', 'CAT-PIZ-ENT'), ('KDS-STK-GRI', 'CAT-STK-COR'),
('KDS-SUS-BAR', 'CAT-SUS-ROL'), ('KDS-COF-BAR', 'CAT-COF-HOT');

-- =========================================================================
-- 11. FLUJO OPERATIVO INICIAL
-- =========================================================================
INSERT INTO sales.tickets (ticket_cen, company_cen, waiter_cen, daily_number, status, subtotal, tax_amount, total, created_at) VALUES 
('TK-PZ-101', 'RE-PIZZA', 'W-PIZ-01', 1, 'CLOSED', 61.00, 10.98, 71.98, NOW() - INTERVAL '3 hours'),
('TK-ST-201', 'RE-STEAK', 'W-STK-01', 1, 'OPEN', 131.00, 23.58, 154.58, NOW() - INTERVAL '45 minutes');

INSERT INTO sales.ticket_items (ticket_item_cen, ticket_cen, product_cen, quantity, unit_price, kds_status) VALUES 
('TI-PZ-01', 'TK-PZ-101', 'PR-PZ-01', 2, 14.50, 'SERVED'),
('TI-ST-01', 'TK-ST-201', 'PR-ST-01', 2, 38.00, 'PREPARING');

INSERT INTO sales.sales (sale_cen, ticket_cen, payment_method_code, total, created_at) VALUES 
('SL-PZ-01', 'TK-PZ-101', 'CREDIT', 71.98, NOW() - INTERVAL '2 hours');

-- =========================================================================
-- 12. GENERACIÓN MASIVA DE DATOS (HISTORIAL)
-- =========================================================================

-- Meseros adicionales (50 mas)
INSERT INTO sales.waiters (waiter_cen, company_cen, name) 
SELECT 
    'W-EX-' || i, 
    (CASE WHEN i % 4 = 0 THEN 'RE-PIZZA' WHEN i % 4 = 1 THEN 'RE-STEAK' WHEN i % 4 = 2 THEN 'RE-SUSHI' ELSE 'RE-COFFEE' END),
    'Mesero ' || i
FROM generate_series(10, 60) i;

-- Productos de repostería (100 mas)
INSERT INTO inventory.products (product_cen, company_cen, category_cen, unit_cen, sku, name, sale_price, cost_price, reorder_level, station_code)
SELECT 
    'PR-BK-' || i, 
    'RE-COFFEE', 
    'CAT-COF-BAK', 
    'UN-COF-UND', 
    'SKU-BK-' || i, 
    'Postre Artesanal #' || i, 
    (random() * 5 + 3)::numeric(12,2), 
    (random() * 2 + 0.5)::numeric(12,2),
    10,
    'ST-BAKERY'
FROM generate_series(1, 100) i;

-- Stock para esos productos
INSERT INTO inventory.stock (stock_cen, product_cen, warehouse_cen, available_quantity)
SELECT 
    'SK-BK-' || i, 
    'PR-BK-' || i, 
    'WH-COF-BAK', 
    (random() * 40)::numeric(12,2)
FROM generate_series(1, 100) i;

-- Kardex Histórico (300 movimientos)
INSERT INTO inventory.kardex_movements (movement_cen, company_cen, warehouse_cen, product_cen, movement_type, quantity, reason, created_at)
SELECT 
    'KDX-H-' || i, 
    'RE-PIZZA', 
    'WH-PIZ-STO', 
    'PR-PZ-I01', 
    (CASE WHEN i % 10 = 0 THEN 'IN' ELSE 'OUT' END), 
    (random() * 15 + 1)::numeric(12,2), 
    'Movimiento operativo diario',
    NOW() - (i || ' hours')::interval
FROM generate_series(1, 300) i;

-- Tickets Históricos (200 mas)
INSERT INTO sales.tickets (ticket_cen, company_cen, waiter_cen, daily_number, status, subtotal, tax_amount, total, created_at)
SELECT 
    'TK-H-' || i, 
    (CASE WHEN i % 3 = 0 THEN 'RE-PIZZA' WHEN i % 3 = 1 THEN 'RE-STEAK' ELSE 'RE-SUSHI' END),
    'W-PIZ-01',
    (i % 100) + 1, 
    'CLOSED', 
    (random() * 120 + 25)::numeric(12,4),
    0, 0, -- Se calcularan en el siguiente paso
    NOW() - (i * 3 || ' hours')::interval
FROM generate_series(1, 200) i;

-- Actualización de montos para consistencia
UPDATE sales.tickets 
SET tax_amount = subtotal * 0.18, 
    total = subtotal * 1.18 
WHERE ticket_cen LIKE 'TK-H-%';

-- Ventas para tickets históricos (CORREGIDO: Sin usar 'i' fuera de contexto)
INSERT INTO sales.sales (sale_cen, ticket_cen, payment_method_code, total, created_at)
SELECT 
    'SL-H-' || SUBSTRING(ticket_cen FROM 6), 
    ticket_cen, 
    (CASE WHEN (SUBSTRING(ticket_cen FROM 6)::int) % 3 = 0 THEN 'CASH' WHEN (SUBSTRING(ticket_cen FROM 6)::int) % 3 = 1 THEN 'DEBIT' ELSE 'CREDIT' END),
    total,
    created_at
FROM sales.tickets 
WHERE ticket_cen LIKE 'TK-H-%';

-- Items para tickets históricos (300 items)
INSERT INTO sales.ticket_items (ticket_item_cen, ticket_cen, product_cen, quantity, unit_price, kds_status)
SELECT 
    'TI-H-' || i, 
    'TK-H-' || ((i % 200) + 1), 
    (CASE WHEN i % 2 = 0 THEN 'PR-PZ-01' ELSE 'PR-ST-01' END),
    (random() * 2 + 1)::int,
    15.00,
    'SERVED'
FROM generate_series(1, 300) i;

-- =========================================================================
-- FIN DEL SEEDER
-- =========================================================================
