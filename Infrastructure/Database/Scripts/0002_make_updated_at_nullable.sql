-- Migration: make updated_at nullable across all mutable tables.
-- Rationale: updated_at should only be set when a record is actually updated,
-- not at creation time. NULL means "never updated since creation".

ALTER TABLE branches        ALTER COLUMN updated_at DROP NOT NULL;
ALTER TABLE stock_templates ALTER COLUMN updated_at DROP NOT NULL;
ALTER TABLE stock_locations ALTER COLUMN updated_at DROP NOT NULL;
ALTER TABLE item_locations  ALTER COLUMN updated_at DROP NOT NULL;
