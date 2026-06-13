-- =========================================================================
-- SEEDER: DATA DE PRUEBA PARA PRISMOD
-- =========================================================================

-- 0. MANAGER: SISTEMAS, FEATURES Y LÍMITES
INSERT INTO manager.systems (system_cen, name, description)
VALUES 
('SYS-CORE', 'CORE_ERP', 'Sistema central de inventario, compras y ventas base'),
('SYS-REST', 'RESTAURANT', 'Modulo de gestion de restaurantes, KDS y meseros'),
('SYS-APPA', 'APPAREL', 'Modulo de indumentaria, tallas y colores');

INSERT INTO manager.features (feature_cen, name, description)
VALUES 
('FEAT-BASE', 'BASE_ACCESS', 'Acceso base al sistema'),
('FEAT-IA', 'IA_SUGGESTIONS', 'Sugerencias con Inteligencia Artificial'),
('FEAT-PAGE', 'HOSTED_PAGE', 'Pagina web alojada'),
('FEAT-BOT', 'CHATBOT', 'Asistente virtual');

-- Vincular features a sistemas (ej. CORE tiene BASE y puede tener IA)
INSERT INTO manager.system_features (system_feature_cen, system_cen, feature_cen)
VALUES 
('SF-CORE-BASE', 'SYS-CORE', 'FEAT-BASE'),
('SF-CORE-IA', 'SYS-CORE', 'FEAT-IA'),
('SF-REST-BASE', 'SYS-REST', 'FEAT-BASE');

-- Definir límites para el feature BASE del CORE según nivel de acceso (1: Std, 2: Prem, 3: Ent)
INSERT INTO manager.system_features_limits (system_feature_cen, limit_name, limit_value, lvl_access)
VALUES 
-- Limites de Compañías por perfil de usuario (ejemplo, aunque se controle a nivel global)
('SF-CORE-BASE', 'MAX_COMPANIES', 1, 1),
('SF-CORE-BASE', 'MAX_COMPANIES', 3, 2),
('SF-CORE-BASE', 'MAX_COMPANIES', 999, 3),

-- Limites de Productos
('SF-CORE-BASE', 'PRODUCT', 10, 1),
('SF-CORE-BASE', 'PRODUCT', 1000, 2),
('SF-CORE-BASE', 'PRODUCT', 99999, 3),

-- Limites de Bodegas
('SF-CORE-BASE', 'WAREHOUSE', 1, 1),
('SF-CORE-BASE', 'WAREHOUSE', 10, 2),
('SF-CORE-BASE', 'WAREHOUSE', 999, 3),

-- Limites de Ordenes
('SF-CORE-BASE', 'ORDER', 50, 1),
('SF-CORE-BASE', 'ORDER', 5000, 2),
('SF-CORE-BASE', 'ORDER', 99999, 3);

-- 1. EMPRESAS (Nota: origin_system_cen asume que nacieron del CORE, owner_profile_cen asume un admin por defecto)
-- Para que las FK funcionen en la BD completa, insertamos un admin temporal
INSERT INTO manager.user_profiles (profile_cen, user_cen, first_name, last_name)
VALUES ('CEN-PROF-ADMIN', 'ADMIN-USER-ID', 'Super', 'Admin');

INSERT INTO inventory.companies (company_cen, name, origin_system_cen, owner_profile_cen, is_active)
VALUES 
('CEN-COMP-001', 'Restaurante Prismod Gourmet', 'SYS-CORE', 'CEN-PROF-ADMIN', TRUE),
('CEN-COMP-002', 'Mini Market Prismod', 'SYS-CORE', 'CEN-PROF-ADMIN', TRUE);

-- 2. BODEGAS
INSERT INTO inventory.warehouses (warehouse_cen, company_cen, name, is_active)
VALUES 
('CEN-WH-001', 'CEN-COMP-001', 'Bodega Principal - Restaurante', TRUE),
('CEN-WH-002', 'CEN-COMP-001', 'Cocina - Restaurante', TRUE),
('CEN-WH-003', 'CEN-COMP-002', 'Almacen General - Market', TRUE);

-- 3. CATEGORIAS
INSERT INTO inventory.categories (category_cen, company_cen, name, description, is_active)
VALUES 
('CEN-CAT-001', 'CEN-COMP-001', 'Bebidas', 'Refrescos, jugos y aguas', TRUE),
('CEN-CAT-002', 'CEN-COMP-001', 'Proteinas', 'Carnes, pollos y pescados', TRUE),
('CEN-CAT-003', 'CEN-COMP-001', 'Abarrotes', 'Ingredientes secos y condimentos', TRUE),
('CEN-CAT-004', 'CEN-COMP-002', 'Limpieza', 'Articulos de aseo general', TRUE);

-- 4. UNIDADES
INSERT INTO inventory.units (unit_cen, company_cen, name, abbreviation, is_active)
VALUES 
('CEN-UNT-001', 'CEN-COMP-001', 'Unidad', 'unid', TRUE),
('CEN-UNT-002', 'CEN-COMP-001', 'Kilogramo', 'kg', TRUE),
('CEN-UNT-003', 'CEN-COMP-001', 'Litro', 'lt', TRUE);

-- 5. PRODUCTOS
INSERT INTO inventory.products (product_cen, company_cen, category_cen, unit_cen, sku, name, description, sale_price, cost_price, reorder_level, status, station_code)
VALUES 
('CEN-PROD-001', 'CEN-COMP-001', 'CEN-CAT-001', 'CEN-UNT-001', 'SOFT-001', 'Coca Cola 500ml', 'Refresco embotellado', 2.50, 1.20, 24.00, 'ACTIVE', 'BAR'),
('CEN-PROD-002', 'CEN-COMP-001', 'CEN-CAT-002', 'CEN-UNT-002', 'MEAT-001', 'Lomo de Res', 'Carne de res premium', 15.00, 8.50, 5.00, 'ACTIVE', 'GRILL'),
('CEN-PROD-003', 'CEN-COMP-001', 'CEN-CAT-003', 'CEN-UNT-002', 'DRY-001', 'Arroz Blanco', 'Arroz grano largo', 1.80, 0.90, 10.00, 'ACTIVE', 'KITCHEN'),
('CEN-PROD-004', 'CEN-COMP-001', 'CEN-CAT-001', 'CEN-UNT-003', 'BEER-001', 'Cerveza Artesanal', 'Botella 330ml', 4.50, 2.10, 12.00, 'ACTIVE', 'BAR');

-- 6. STOCK INICIAL
INSERT INTO inventory.stock (stock_cen, product_cen, warehouse_cen, available_quantity, reserved_quantity)
VALUES 
('CEN-STK-001', 'CEN-PROD-001', 'CEN-WH-001', 100.00, 0.00),
('CEN-STK-002', 'CEN-PROD-002', 'CEN-WH-002', 25.50, 0.00),
('CEN-STK-003', 'CEN-PROD-003', 'CEN-WH-001', 50.00, 0.00);

-- 7. METODOS DE PAGO (Ventas)
INSERT INTO sales.payment_methods (payment_method_code, name, is_active)
VALUES 
('CASH', 'Efectivo', TRUE),
('CARD', 'Tarjeta de Credito/Debito', TRUE),
('TRANSFER', 'Transferencia Bancaria', TRUE),
('QR', 'Pago Movil QR', TRUE);

-- 8. MESEROS Y EQUIPOS KDS (Ventas)
INSERT INTO sales.waiters (waiter_cen, company_cen, name)
VALUES 
('CEN-WAIT-001', 'CEN-COMP-001', 'Juan Perez'),
('CEN-WAIT-002', 'CEN-COMP-001', 'Maria Garcia');

INSERT INTO sales.kds_teams (team_cen, company_cen, name)
VALUES 
('CEN-KDS-001', 'CEN-COMP-001', 'Equipo de Parrilla'),
('CEN-KDS-002', 'CEN-COMP-001', 'Equipo de Bar');

INSERT INTO sales.kds_team_categories (team_cen, category_cen)
VALUES 
('CEN-KDS-001', 'CEN-CAT-002'), -- Parrilla -> Proteinas
('CEN-KDS-002', 'CEN-CAT-001'); -- Bar -> Bebidas

-- 9. CONFIGURACION DE IMPUESTOS (Ventas)
INSERT INTO sales.tax_configurations (company_cen, global_tax_percentage)
VALUES 
('CEN-COMP-001', 13.00),
('CEN-COMP-002', 13.00);

-- 10. PROVEEDORES (Compras)
INSERT INTO purchasing.suppliers (supplier_cen, company_cen, name)
VALUES 
('CEN-SUPP-001', 'CEN-COMP-001', 'Distribuidora Global Bebidas'),
('CEN-SUPP-002', 'CEN-COMP-001', 'Carnicos del Valle');
