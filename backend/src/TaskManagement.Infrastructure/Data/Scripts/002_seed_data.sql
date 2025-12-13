-- Insert test users
-- Password for all users: TestPassword123
-- Hashed using BCrypt

INSERT INTO users (id, name, email, password_hash, created_at)
VALUES 
    ('a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d', 'John Doe', 'john@example.com', '$2a$11$XKfkL3Z8P5wQJ9fJ7Y3VaeZQX5K4vX8N9wZYq5N6Z7P8Q9R0S1T2U', NOW()),
    ('b2c3d4e5-f6a7-5b6c-9d0e-1f2a3b4c5d6e', 'Jane Smith', 'jane@example.com', '$2a$11$XKfkL3Z8P5wQJ9fJ7Y3VaeZQX5K4vX8N9wZYq5N6Z7P8Q9R0S1T2U', NOW()),
    ('c3d4e5f6-a7b8-6c7d-0e1f-2a3b4c5d6e7f', 'Bob Wilson', 'bob@example.com', '$2a$11$XKfkL3Z8P5wQJ9fJ7Y3VaeZQX5K4vX8N9wZYq5N6Z7P8Q9R0S1T2U', NOW())
ON CONFLICT (id) DO NOTHING;

-- Insert test tasks for John Doe
INSERT INTO tasks (id, user_id, title, description, status, due_date, created_at)
VALUES
    ('d4e5f6a7-b8c9-7d8e-1f2a-3b4c5d6e7f8a', 'a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d', 
     'Complete project documentation', 
     'Write comprehensive documentation for the task management system', 
     'InProgress', 
     NOW() + INTERVAL '7 days', 
     NOW()),
    
    ('e5f6a7b8-c9d0-8e9f-2a3b-4c5d6e7f8a9b', 'a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d', 
     'Review pull requests', 
     'Review and merge pending pull requests from team members', 
     'Pending', 
     NOW() + INTERVAL '3 days', 
     NOW()),
    
    ('f6a7b8c9-d0e1-9f0a-3b4c-5d6e7f8a9b0c', 'a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d', 
     'Setup CI/CD pipeline', 
     'Configure automated testing and deployment pipeline', 
     'Completed', 
     NOW() + INTERVAL '1 day', 
     NOW())
ON CONFLICT (id) DO NOTHING;

-- Insert test tasks for Jane Smith
INSERT INTO tasks (id, user_id, title, description, status, due_date, created_at)
VALUES
    ('a7b8c9d0-e1f2-0a1b-4c5d-6e7f8a9b0c1d', 'b2c3d4e5-f6a7-5b6c-9d0e-1f2a3b4c5d6e', 
     'Design database schema', 
     'Create ERD and design normalized database schema', 
     'Completed', 
     NOW() + INTERVAL '5 days', 
     NOW()),
    
    ('b8c9d0e1-f2a3-1b2c-5d6e-7f8a9b0c1d2e', 'b2c3d4e5-f6a7-5b6c-9d0e-1f2a3b4c5d6e', 
     'Implement user authentication', 
     'Add JWT-based authentication with BCrypt password hashing', 
     'InProgress', 
     NOW() + INTERVAL '4 days', 
     NOW())
ON CONFLICT (id) DO NOTHING;

-- Insert test tasks for Bob Wilson
INSERT INTO tasks (id, user_id, title, description, status, due_date, created_at)
VALUES
    ('c9d0e1f2-a3b4-2c3d-6e7f-8a9b0c1d2e3f', 'c3d4e5f6-a7b8-6c7d-0e1f-2a3b4c5d6e7f', 
     'Write unit tests', 
     'Achieve 80% code coverage with meaningful tests', 
     'Pending', 
     NOW() + INTERVAL '6 days', 
     NOW()),
    
    ('d0e1f2a3-b4c5-3d4e-7f8a-9b0c1d2e3f4a', 'c3d4e5f6-a7b8-6c7d-0e1f-2a3b4c5d6e7f', 
     'Deploy to production', 
     'Deploy application to production environment', 
     'Pending', 
     NOW() + INTERVAL '10 days', 
     NOW())
ON CONFLICT (id) DO NOTHING;

