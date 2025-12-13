/// <reference types="cypress" />

describe('Authentication', () => {
  beforeEach(() => {
    cy.clearTestData();
  });

  describe('Login', () => {
    it('should display login form', () => {
      cy.visit('/');
      cy.get('#email').should('be.visible');
      cy.get('#password').should('be.visible');
      cy.get('button[type="submit"]').should('contain.text', 'Sign In');
    });

    it('should login with valid credentials', () => {
      // First register a user
      const timestamp = Date.now();
      const email = `login.test.${timestamp}@example.com`;
      
      cy.register('Login Test User', email, 'TestPassword123');
      cy.url().should('include', '/dashboard', { timeout: 15000 });
      
      // Logout
      cy.logout();
      
      // Now test login
      cy.login(email, 'TestPassword123');
      cy.url().should('include', '/dashboard');
      cy.contains('My Tasks').should('be.visible');
    });

    it('should stay on login page with invalid credentials', () => {
      cy.visit('/');
      cy.get('#email').clear().type('nonexistent@email.com');
      cy.get('#password').clear().type('wrongpassword123');
      cy.get('button[type="submit"]').click();
      // Should stay on login page (not redirect to dashboard)
      cy.wait(2000); // Wait for potential response
      cy.url().should('not.include', '/dashboard');
    });

    it('should navigate to register page', () => {
      cy.visit('/');
      cy.contains("Don't have an account? Sign Up").click();
      cy.url().should('include', '/register');
    });
  });

  describe('Register', () => {
    it('should display register form', () => {
      cy.visit('/register');
      cy.get('#name').should('be.visible');
      cy.get('#email').should('be.visible');
      cy.get('#password').should('be.visible');
      cy.get('#confirmPassword').should('be.visible');
    });

    it('should register a new user', () => {
      const timestamp = Date.now();
      const email = `register.test.${timestamp}@example.com`;
      
      cy.register('Register Test User', email, 'TestPassword123');
      
      // Should redirect to dashboard after successful registration
      cy.url().should('include', '/dashboard', { timeout: 15000 });
    });

    it('should show error when passwords do not match', () => {
      cy.visit('/register');
      cy.get('#name').type('Test User');
      cy.get('#email').type('test@example.com');
      cy.get('#password').type('Password123');
      cy.get('#confirmPassword').type('DifferentPassword');
      cy.get('button[type="submit"]').click();
      // Should show validation error
      cy.contains('Passwords do not match').should('be.visible');
    });

    it('should navigate back to login page', () => {
      cy.visit('/register');
      cy.contains('Already have an account? Sign In').click();
      cy.url().should('eq', Cypress.config().baseUrl + '/');
    });
  });

  describe('Logout', () => {
    it('should logout successfully', () => {
      // Register and login first
      const timestamp = Date.now();
      const email = `logout.test.${timestamp}@example.com`;
      
      cy.register('Logout Test User', email, 'TestPassword123');
      cy.url().should('include', '/dashboard', { timeout: 15000 });
      
      // Logout
      cy.logout();
      
      // Should redirect to login page
      cy.url().should('eq', Cypress.config().baseUrl + '/');
    });
  });
});
