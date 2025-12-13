/// <reference types="cypress" />

/**
 * Full E2E Flow Test
 * 
 * This test simulates a complete user journey:
 * 1. Register a new account
 * 2. Create a task
 * 3. Edit a task
 * 4. Delete a task (create another first)
 * 5. Logout
 * 6. Login again
 * 7. Verify data persisted
 */
describe('Complete User Flow', () => {
  const timestamp = Date.now();
  const testUser = {
    name: 'E2E Test User',
    email: `e2e.fullflow.${timestamp}@example.com`,
    password: 'TestPassword123',
  };

  const originalTitle = `My Task ${timestamp}`;
  const editedTitle = `EDITED: ${originalTitle}`;
  const taskToDelete = `Delete Me ${timestamp}`;

  before(() => {
    cy.clearTestData();
  });

  it('should complete full user journey', () => {
    // ═══════════════════════════════════════════════════════════
    // Step 1: Register new account
    // ═══════════════════════════════════════════════════════════
    cy.log('**Step 1: Register new account**');
    cy.register(testUser.name, testUser.email, testUser.password);
    cy.url().should('include', '/dashboard', { timeout: 15000 });
    cy.contains('My Tasks').should('be.visible');

    // ═══════════════════════════════════════════════════════════
    // Step 2: Create a task
    // ═══════════════════════════════════════════════════════════
    cy.log('**Step 2: Create a task**');
    
    cy.contains('button', 'New Task').click();
    cy.get('[role="dialog"]').should('be.visible');
    cy.get('[role="dialog"]').within(() => {
      cy.get('input[name="title"]').type(originalTitle);
      cy.get('textarea[name="description"]').type('This is my first task');
      cy.contains('button', 'Create').click();
    });
    cy.get('[role="dialog"]').should('not.exist');
    cy.contains(originalTitle).should('be.visible');

    // ═══════════════════════════════════════════════════════════
    // Step 3: Edit the task
    // ═══════════════════════════════════════════════════════════
    cy.log('**Step 3: Edit the task**');
    
    cy.contains(originalTitle)
      .closest('.MuiCard-root')
      .within(() => {
        cy.get('[aria-label="Edit task"]').click();
      });
    
    cy.get('[role="dialog"]').should('be.visible');
    cy.get('[role="dialog"]').within(() => {
      cy.get('input[name="title"]').clear().type(editedTitle);
      cy.contains('button', 'Update').click();
    });
    
    cy.get('[role="dialog"]').should('not.exist');
    cy.contains(editedTitle).should('be.visible');

    // ═══════════════════════════════════════════════════════════
    // Step 4: Create another task and delete it
    // ═══════════════════════════════════════════════════════════
    cy.log('**Step 4: Create and delete a task**');
    
    // Create task to delete
    cy.contains('button', 'New Task').click();
    cy.get('[role="dialog"]').should('be.visible');
    cy.get('[role="dialog"]').within(() => {
      cy.get('input[name="title"]').type(taskToDelete);
      cy.get('textarea[name="description"]').type('This will be deleted');
      cy.contains('button', 'Create').click();
    });
    cy.get('[role="dialog"]').should('not.exist');
    cy.contains(taskToDelete).should('be.visible');
    
    // Delete it
    cy.contains(taskToDelete)
      .closest('.MuiCard-root')
      .within(() => {
        cy.get('[aria-label="Delete task"]').click();
      });
    
    cy.get('[role="dialog"]').should('be.visible');
    cy.get('[role="dialog"]').within(() => {
      cy.contains('button', 'Delete').click();
    });
    
    cy.get('[role="dialog"]').should('not.exist');
    cy.contains(taskToDelete).should('not.exist');

    // ═══════════════════════════════════════════════════════════
    // Step 5: Logout
    // ═══════════════════════════════════════════════════════════
    cy.log('**Step 5: Logout**');
    cy.logout();

    // ═══════════════════════════════════════════════════════════
    // Step 6: Login again
    // ═══════════════════════════════════════════════════════════
    cy.log('**Step 6: Login again**');
    cy.login(testUser.email, testUser.password);

    // ═══════════════════════════════════════════════════════════
    // Step 7: Verify data persisted
    // ═══════════════════════════════════════════════════════════
    cy.log('**Step 7: Verify data persistence**');
    
    // Edited task should exist
    cy.contains(editedTitle).should('exist');
    
    // Deleted task should NOT exist
    cy.contains(taskToDelete).should('not.exist');
    
    cy.log('**✅ Full user flow completed successfully!**');
  });
});
