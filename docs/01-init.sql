-- =========================================================================
-- 1. CREACIÓN DE ESQUEMAS
-- =========================================================================
CREATE SCHEMA IF NOT EXISTS manager;
CREATE SCHEMA IF NOT EXISTS inventory;
CREATE SCHEMA IF NOT EXISTS purchasing;
CREATE SCHEMA IF NOT EXISTS sales;

-- =========================================================================
-- MÓDULO: IDENTITY (Gestión de Accesos y Suscripciones)
-- =========================================================================

CREATE TABLE manager.user_profiles (
    id BIGSERIAL PRIMARY KEY,
    profile_cen VARCHAR(50) UNIQUE NOT NULL,
    user_cen VARCHAR(50) UNIQUE NOT NULL, -- Relación lógica con AspNetUsers
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE manager.access_company(
    id BIGSERIAL PRIMARY KEY,
    access_company_cen VARCHAR(50) UNIQUE NOT NULL,
    profile_cen VARCHAR(50) REFERENCES manager.user_profiles(profile_cen) NOT NULL,
    company_cen VARCHAR(50) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE manager.systems ( -- aqui definimos los sistemas que pueden ser suscritos (Restaurante, Indumentaria, etc)
    id BIGSERIAL PRIMARY KEY,
    system_cen VARCHAR(50) UNIQUE NOT NULL,
    name VARCHAR(50) NOT NULL,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE manager.features (
    id BIGSERIAL PRIMARY KEY,
    feature_cen VARCHAR(50) UNIQUE NOT NULL,
    name VARCHAR(50) NOT NULL,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE manager.system_features (
    id BIGSERIAL PRIMARY KEY,
    system_feature_cen VARCHAR(50) UNIQUE NOT NULL,
    system_cen VARCHAR(50) REFERENCES manager.systems(system_cen) NOT NULL,
    feature_cen VARCHAR(50) REFERENCES manager.features(feature_cen) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE manager.system_features_limits(
    id BIGSERIAL PRIMARY KEY,
    system_feature_cen VARCHAR(50) REFERENCES manager.system_features(system_feature_cen) NOT NULL,
    limit_name VARCHAR(50) NOT NULL,
    limit_value INT NOT NULL DEFAULT 0,
    lvl_access INT NOT NULL DEFAULT 1, -- 1: estandar_suscripción, 2: premium, 3: enterprise
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
    UNIQUE (system_feature_cen, limit_name)
);

CREATE TABLE manager.access_system (
    id BIGSERIAL PRIMARY KEY,
    access_system_cen VARCHAR(50) UNIQUE NOT NULL,
    profile_cen VARCHAR(50) REFERENCES manager.user_profiles(profile_cen) NOT NULL,
    system_cen VARCHAR(50) REFERENCES manager.systems(system_cen) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE manager.subscription(
    id BIGSERIAL PRIMARY KEY,
    subscription_cen VARCHAR(50) UNIQUE NOT NULL,
    access_system_cen VARCHAR(50) REFERENCES manager.access_system(access_system_cen) NOT NULL,
    lvl_access INT NOT NULL DEFAULT 1, -- 1: estandar_suscripción, 2: premium, 3: enterprise
    subscription_state VARCHAR(50) NOT NULL, -- SUBSCRIPTION_RENEWED, SUBSCRIPTION_CANCELLED, SUBSCRIPTION_CREATED, SUBSCRIPTION_EXPIRED, etc.
    description TEXT,
    start_upgrade TIMESTAMP WITH TIME ZONE NOT NULL,
    end_upgrade TIMESTAMP WITH TIME ZONE NOT NULL,
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE manager.usage_counters (
    id BIGSERIAL PRIMARY KEY,
    access_system_cen VARCHAR(50) REFERENCES manager.access_system(access_system_cen) NOT NULL,
    resource_type VARCHAR(50) NOT NULL, -- PRODUCT, WAREHOUSE, ORDER
    current_count INT DEFAULT 0 NOT NULL,
    last_reset_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL,
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
    UNIQUE (access_system_cen, resource_type)
);

CREATE TABLE manager.refresh_tokens (
    id BIGSERIAL PRIMARY KEY,
    token TEXT UNIQUE NOT NULL,
    user_cen VARCHAR(50) NOT NULL,
    expires_at TIMESTAMP WITH TIME ZONE NOT NULL,
    is_revoked BOOLEAN DEFAULT FALSE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

-- =========================================================================
-- MÓDULO: INVENTARIO (Compañero)
-- =========================================================================

CREATE TABLE inventory.companies (
    id BIGSERIAL PRIMARY KEY,
    company_cen VARCHAR(50) UNIQUE NOT NULL,
    name VARCHAR(150) NOT NULL,
    origin_system_cen VARCHAR(50) REFERENCES manager.systems(system_cen) NOT NULL,
    owner_profile_cen VARCHAR(50) REFERENCES manager.user_profiles(profile_cen) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE inventory.warehouses (
    id BIGSERIAL PRIMARY KEY,
    warehouse_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) REFERENCES inventory.companies(company_cen) ON DELETE CASCADE NOT NULL,
    name VARCHAR(100) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE NOT NULL
);

CREATE TABLE inventory.categories (
    id BIGSERIAL PRIMARY KEY,
    category_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) REFERENCES inventory.companies(company_cen) ON DELETE CASCADE NOT NULL,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE NOT NULL
);

CREATE TABLE inventory.units (
    id BIGSERIAL PRIMARY KEY,
    unit_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) REFERENCES inventory.companies(company_cen) ON DELETE CASCADE NOT NULL,
    name VARCHAR(50) NOT NULL,
    abbreviation VARCHAR(10),
    is_active BOOLEAN DEFAULT TRUE NOT NULL
);

CREATE TABLE inventory.products (
    id BIGSERIAL PRIMARY KEY,
    product_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) REFERENCES inventory.companies(company_cen) ON DELETE CASCADE NOT NULL,
    category_cen VARCHAR(50) REFERENCES inventory.categories(category_cen) ON DELETE SET NULL,
    unit_cen VARCHAR(50) REFERENCES inventory.units(unit_cen) ON DELETE SET NULL,
    sku VARCHAR(50) NOT NULL,
    name VARCHAR(150) NOT NULL,
    description TEXT,
    sale_price NUMERIC(12, 4) NOT NULL DEFAULT 0.0000,
    cost_price NUMERIC(12, 4) DEFAULT 0.0000,
    reorder_level NUMERIC(12, 4) NOT NULL DEFAULT 0.0000,
    status VARCHAR(20) NOT NULL DEFAULT 'ACTIVE', 
    station_code VARCHAR(50),
    CONSTRAINT uq_product_sku_company UNIQUE (company_cen, sku)
);

CREATE TABLE inventory.stock (
    id BIGSERIAL PRIMARY KEY,
    stock_cen VARCHAR(50) UNIQUE NOT NULL,
    product_cen VARCHAR(50) REFERENCES inventory.products(product_cen) ON DELETE CASCADE NOT NULL,
    warehouse_cen VARCHAR(50) REFERENCES inventory.warehouses(warehouse_cen) ON DELETE CASCADE NOT NULL,
    available_quantity NUMERIC(12, 4) NOT NULL DEFAULT 0.0000,
    reserved_quantity NUMERIC(12, 4) NOT NULL DEFAULT 0.0000,
    CONSTRAINT uq_stock_product_warehouse UNIQUE (product_cen, warehouse_cen)
);

CREATE TABLE inventory.inventory_documents (
    id BIGSERIAL PRIMARY KEY,
    document_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) REFERENCES inventory.companies(company_cen) ON DELETE CASCADE NOT NULL,
    warehouse_cen VARCHAR(50) REFERENCES inventory.warehouses(warehouse_cen) ON DELETE RESTRICT NOT NULL,
    document_type VARCHAR(30) NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'COMPLETED', 
    title VARCHAR(150) NOT NULL,
    reason TEXT,
    external_reference VARCHAR(100),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE inventory.inventory_document_lines (
    id BIGSERIAL PRIMARY KEY,
    line_cen VARCHAR(50) UNIQUE NOT NULL,
    document_cen VARCHAR(50) REFERENCES inventory.inventory_documents(document_cen) ON DELETE CASCADE NOT NULL,
    product_cen VARCHAR(50) REFERENCES inventory.products(product_cen) ON DELETE RESTRICT NOT NULL,
    quantity NUMERIC(12, 4) NOT NULL,
    unit_cost NUMERIC(12, 4)
);

CREATE TABLE inventory.kardex_movements (
    id BIGSERIAL PRIMARY KEY,
    movement_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) REFERENCES inventory.companies(company_cen) ON DELETE CASCADE NOT NULL,
    warehouse_cen VARCHAR(50) REFERENCES inventory.warehouses(warehouse_cen) ON DELETE RESTRICT NOT NULL,
    product_cen VARCHAR(50) REFERENCES inventory.products(product_cen) ON DELETE RESTRICT NOT NULL,
    document_cen VARCHAR(50) REFERENCES inventory.inventory_documents(document_cen) ON DELETE SET NULL,
    movement_type VARCHAR(20) NOT NULL,
    quantity NUMERIC(12, 4) NOT NULL,
    unit_cost NUMERIC(12, 4),
    reason TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

-- =========================================================================
-- MÓDULO: COMPRAS (Tu Esquema)
-- Referencias DESACOPLADAS hacia el inventario externo
-- =========================================================================

CREATE TABLE purchasing.suppliers (
    id BIGSERIAL PRIMARY KEY,
    supplier_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) NOT NULL, -- Referencia suave
    name VARCHAR(150) NOT NULL
);

CREATE TABLE purchasing.purchase_orders (
    id BIGSERIAL PRIMARY KEY,
    order_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) NOT NULL,   -- Referencia suave
    warehouse_cen VARCHAR(50) NOT NULL, -- Referencia suave
    supplier_cen VARCHAR(50) REFERENCES purchasing.suppliers(supplier_cen) ON DELETE RESTRICT NOT NULL, -- Relación interna fuerte
    status SMALLINT NOT NULL DEFAULT 0, 
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
    confirmed_at TIMESTAMP WITH TIME ZONE
);

CREATE TABLE purchasing.purchase_order_items (
    id BIGSERIAL PRIMARY KEY,
    item_cen VARCHAR(50) UNIQUE NOT NULL,
    order_cen VARCHAR(50) REFERENCES purchasing.purchase_orders(order_cen) ON DELETE CASCADE NOT NULL,
    product_cen VARCHAR(50) NOT NULL, -- Referencia suave
    quantity NUMERIC(12, 4) NOT NULL
);

-- =========================================================================
-- MÓDULO: VENTAS (Tu Esquema)
-- Referencias DESACOPLADAS hacia el inventario externo
-- =========================================================================

CREATE TABLE sales.tax_configurations (
    id BIGSERIAL PRIMARY KEY,
    company_cen VARCHAR(50) UNIQUE NOT NULL, -- Referencia suave (Unique 1 a 1)
    global_tax_percentage NUMERIC(5, 2) NOT NULL DEFAULT 0.00
);

CREATE TABLE sales.payment_methods (
    payment_method_code VARCHAR(20) PRIMARY KEY, -- Esta es de dominio/catálogo puro
    name VARCHAR(50) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE NOT NULL
);

CREATE TABLE sales.waiters (
    id BIGSERIAL PRIMARY KEY,
    waiter_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) NOT NULL, -- Referencia suave
    name VARCHAR(100) NOT NULL
);

CREATE TABLE sales.kds_teams (
    id BIGSERIAL PRIMARY KEY,
    team_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) NOT NULL, -- Referencia suave
    name VARCHAR(100) NOT NULL
);

CREATE TABLE sales.kds_team_categories (
    team_cen VARCHAR(50) REFERENCES sales.kds_teams(team_cen) ON DELETE CASCADE NOT NULL,
    category_cen VARCHAR(50) NOT NULL, -- Referencia suave
    PRIMARY KEY (team_cen, category_cen)
);

CREATE TABLE sales.tickets (
    id BIGSERIAL PRIMARY KEY,
    ticket_cen VARCHAR(50) UNIQUE NOT NULL,
    company_cen VARCHAR(50) NOT NULL, -- Referencia suave
    waiter_cen VARCHAR(50) REFERENCES sales.waiters(waiter_cen) ON DELETE SET NULL,
    daily_number INT NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'OPEN', 
    subtotal NUMERIC(12, 4) NOT NULL DEFAULT 0.0000,
    tax_amount NUMERIC(12, 4) NOT NULL DEFAULT 0.0000,
    total NUMERIC(12, 4) NOT NULL DEFAULT 0.0000,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE sales.ticket_items (
    id BIGSERIAL PRIMARY KEY,
    ticket_item_cen VARCHAR(50) UNIQUE NOT NULL,
    ticket_cen VARCHAR(50) REFERENCES sales.tickets(ticket_cen) ON DELETE CASCADE NOT NULL,
    product_cen VARCHAR(50) NOT NULL, -- Referencia suave
    quantity INT NOT NULL,
    unit_price NUMERIC(12, 4) NOT NULL,
    note VARCHAR(255),
    kds_status VARCHAR(20) NOT NULL DEFAULT 'CREATED',
    sent_at TIMESTAMP WITH TIME ZONE,
    resend_count INT NOT NULL DEFAULT 0
);

CREATE TABLE sales.sales (
    id BIGSERIAL PRIMARY KEY,
    sale_cen VARCHAR(50) UNIQUE NOT NULL,
    ticket_cen VARCHAR(50) UNIQUE REFERENCES sales.tickets(ticket_cen) ON DELETE RESTRICT NOT NULL,
    payment_method_code VARCHAR(20) REFERENCES sales.payment_methods(payment_method_code),
    inventory_document_cen VARCHAR(50), -- Comprobante devuelto por el compañero
    total NUMERIC(12, 4) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP NOT NULL
);