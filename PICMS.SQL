CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
    available BOOLEAN NOT NULL DEFAULT true
);

CREATE TABLE departments (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(255),
    available BOOLEAN NOT NULL DEFAULT true
);

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role_id INT REFERENCES roles(id),
    created_at TIMESTAMP DEFAULT now(),
    updated_at TIMESTAMP,
    available BOOLEAN NOT NULL DEFAULT true
);

CREATE TABLE applications (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(255),
    url VARCHAR(255),
    server VARCHAR(255),
    repository VARCHAR(255),
    owner_id INT REFERENCES users(id),
    support_email VARCHAR(100) NOT NULL,
    sme_email VARCHAR(100),
    db_server VARCHAR(255),
    created_at TIMESTAMP DEFAULT now(),
    available BOOLEAN NOT NULL DEFAULT true
);

CREATE TABLE tickets (
    id SERIAL PRIMARY KEY,
    user_id INT REFERENCES users(id),
    application_id INT REFERENCES applications(id),
    title VARCHAR(200) NOT NULL,
    departments_id INT REFERENCES departments(id),
    description TEXT,
    status VARCHAR(50) NOT NULL,
    created_at TIMESTAMP DEFAULT now(),
    updated_at TIMESTAMP,
    closed_by INT REFERENCES users(id),
    available BOOLEAN NOT NULL DEFAULT true
);

CREATE TABLE ticket_comments (
    id SERIAL PRIMARY KEY,
    ticket_id INT REFERENCES tickets(id),
    user_id INT REFERENCES users(id),
    comment TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT now(),
    available BOOLEAN NOT NULL DEFAULT true
);
