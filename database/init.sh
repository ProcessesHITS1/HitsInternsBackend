#!/bin/bash
set -e

echo "Creating multiple databases..."

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
    CREATE DATABASE "auth-service-db";
    CREATE DATABASE "company-management-service-db";
    CREATE DATABASE "interview-db";
EOSQL

echo "Databases created successfully."