/// <reference types="cypress" />

// ***********************************************
// Custom commands for Task Management E2E tests
// ***********************************************

declare global {
  namespace Cypress {
    interface Chainable {
      /**
       * Login via UI
       */
      login(email: string, password: string): Chainable<void>;

      /**
       * Register a new user via UI
       */
      register(name: string, email: string, password: string): Chainable<void>;

      /**
       * Ensure a test user exists and is logged in
       */
      ensureLoggedIn(): Chainable<void>;

      /**
       * Logout the current user
       */
      logout(): Chainable<void>;

      /**
       * Clear all test data
       */
      clearTestData(): Chainable<void>;
    }
  }
}

// Login via UI
Cypress.Commands.add('login', (email: string, password: string) => {
  cy.visit('/');
  cy.get('#email').clear().type(email);
  cy.get('#password').clear().type(password);
  cy.get('button[type="submit"]').click();
  cy.url().should('include', '/dashboard', { timeout: 15000 });
});

// Register via UI
Cypress.Commands.add('register', (name: string, email: string, password: string) => {
  cy.visit('/register');
  cy.get('#name').type(name);
  cy.get('#email').type(email);
  cy.get('#password').type(password);
  cy.get('#confirmPassword').type(password);
  cy.get('button[type="submit"]').click();
});

// Ensure logged in - register a new user via UI (more reliable than API)
Cypress.Commands.add('ensureLoggedIn', () => {
  const timestamp = Date.now();
  const user = {
    name: 'Test User',
    email: `test.${timestamp}@example.com`,
    password: 'TestPassword123',
  };
  
  cy.register(user.name, user.email, user.password);
  cy.url().should('include', '/dashboard', { timeout: 15000 });
});

// Logout - click on account icon, wait for menu, then click Logout
Cypress.Commands.add('logout', () => {
  // Click the account icon to open the menu
  cy.get('[aria-label="account of current user"]').click();
  
  // Wait for the menu to appear and be interactive
  cy.get('#menu-appbar').should('be.visible');
  
  // Click Logout menu item
  cy.get('#menu-appbar').contains('Logout').click({ force: true });
  
  // Verify we're logged out
  cy.url().should('eq', Cypress.config().baseUrl + '/', { timeout: 10000 });
});

// Clear test data (localStorage)
Cypress.Commands.add('clearTestData', () => {
  cy.window().then((win) => {
    win.localStorage.clear();
  });
});

export {};
