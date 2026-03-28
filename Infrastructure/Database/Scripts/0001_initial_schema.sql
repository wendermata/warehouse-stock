CREATE TABLE IF NOT EXISTS branches
(
    id         UUID         NOT NULL PRIMARY KEY,
    code       VARCHAR(50)  NOT NULL UNIQUE,
    name       VARCHAR(200) NOT NULL,
    created_at TIMESTAMPTZ  NOT NULL,
    updated_at TIMESTAMPTZ  NOT NULL
    );

-- 📍 Cadastro genérico (A, B, C, D...)
CREATE TABLE IF NOT EXISTS stock_templates
(
    id                 UUID         NOT NULL PRIMARY KEY,
    external_reference VARCHAR(50)  NOT NULL UNIQUE, -- A, B, C...
    description        VARCHAR(200),
    created_at         TIMESTAMPTZ  NOT NULL,
    updated_at         TIMESTAMPTZ  NOT NULL
    );

-- 🏢 Local físico por filial
CREATE TABLE IF NOT EXISTS stock_locations
(
    id                UUID         NOT NULL PRIMARY KEY,
    branch_id         UUID         NOT NULL REFERENCES branches (id),
    stock_template_id UUID         NOT NULL REFERENCES stock_templates (id),
    created_at        TIMESTAMPTZ  NOT NULL,
    updated_at        TIMESTAMPTZ  NOT NULL,

    UNIQUE (branch_id, stock_template_id)
    );

-- 📦 SKU dentro do local
CREATE TABLE IF NOT EXISTS item_locations
(
    id                 UUID         NOT NULL PRIMARY KEY,
    stock_location_id  UUID         NOT NULL REFERENCES stock_locations (id),
    sku                VARCHAR(100) NOT NULL,
    available_quantity INT          NOT NULL DEFAULT 0,
    created_at         TIMESTAMPTZ  NOT NULL,
    updated_at         TIMESTAMPTZ  NOT NULL,

    UNIQUE (stock_location_id, sku)
    );

-- 📘 Ledger de estoque
CREATE TABLE IF NOT EXISTS stock_movements
(
    id                 UUID         NOT NULL PRIMARY KEY,
    item_location_id   UUID         NOT NULL REFERENCES item_locations (id),
    type               SMALLINT     NOT NULL,
    quantity           INT          NOT NULL,
    balance_after      INT          NOT NULL,
    external_reference VARCHAR(200) NOT NULL,
    occurred_at        TIMESTAMPTZ  NOT NULL,

    UNIQUE (item_location_id, external_reference)
    );
