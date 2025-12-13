/// <reference types="cypress" />

describe('Task Management', () => {
  beforeEach(() => {
    cy.clearTestData();
    // Register a new user and login via API (faster)
    cy.ensureLoggedIn();
  });

  describe('View Tasks', () => {
    it('should display dashboard with tasks section', () => {
      cy.url().should('include', '/dashboard');
      cy.contains('My Tasks').should('be.visible');
    });

    it('should have filter tabs', () => {
      cy.contains('All').should('be.visible');
      cy.contains('Pending').should('be.visible');
      cy.contains('In Progress').should('be.visible');
      cy.contains('Completed').should('be.visible');
    });

    it('should have New Task button', () => {
      cy.contains('button', 'New Task').should('be.visible');
    });
  });

  describe('Create Task', () => {
    it('should open create task dialog', () => {
      cy.contains('button', 'New Task').click();
      cy.get('[role="dialog"]').should('be.visible');
      cy.contains('Create New Task').should('be.visible');
    });

    it('should create a new task', () => {
      const taskTitle = `Test Task ${Date.now()}`;
      
      cy.contains('button', 'New Task').click();
      
      // Fill the form
      cy.get('[role="dialog"]').within(() => {
        cy.get('input[name="title"]').type(taskTitle);
        cy.get('textarea[name="description"]').type('This is a test task description');
        cy.contains('button', 'Create').click();
      });
      
      // Dialog should close and task should appear
      cy.get('[role="dialog"]').should('not.exist');
      cy.contains(taskTitle).should('be.visible');
    });

    it('should show validation error for empty title', () => {
      cy.contains('button', 'New Task').click();
      
      // Try to submit without filling title - clear any default
      cy.get('[role="dialog"]').within(() => {
        cy.get('input[name="title"]').clear();
        cy.contains('button', 'Create').click();
      });
      
      // Should still show dialog (validation failed)
      cy.get('[role="dialog"]').should('be.visible');
    });
  });

  describe('Edit Task', () => {
    beforeEach(() => {
      // Create a task first
      const taskTitle = `Edit Test ${Date.now()}`;
      cy.contains('button', 'New Task').click();
      cy.get('[role="dialog"]').within(() => {
        cy.get('input[name="title"]').type(taskTitle);
        cy.contains('button', 'Create').click();
      });
      cy.get('[role="dialog"]').should('not.exist');
      cy.contains(taskTitle).should('be.visible');
    });

    it('should edit an existing task', () => {
      const updatedTitle = `Updated Task ${Date.now()}`;
      
      // Find a task card and click edit (the edit icon button)
      cy.get('.MuiCard-root').first().within(() => {
        cy.get('[aria-label="Edit task"]').click();
      });
      
      // Edit dialog should appear
      cy.get('[role="dialog"]').should('be.visible');
      cy.contains('Edit Task').should('be.visible');
      
      cy.get('[role="dialog"]').within(() => {
        cy.get('input[name="title"]').clear().type(updatedTitle);
        cy.contains('button', 'Update').click();
      });
      
      // Task should be updated
      cy.get('[role="dialog"]').should('not.exist');
      cy.contains(updatedTitle).should('be.visible');
    });
  });

  describe('Delete Task', () => {
    beforeEach(() => {
      // Create a task first
      const taskTitle = `Delete Test ${Date.now()}`;
      cy.contains('button', 'New Task').click();
      cy.get('[role="dialog"]').within(() => {
        cy.get('input[name="title"]').type(taskTitle);
        cy.contains('button', 'Create').click();
      });
      cy.get('[role="dialog"]').should('not.exist');
      cy.contains(taskTitle).should('be.visible');
    });

    it('should delete a task', () => {
      // Click delete on first task
      cy.get('.MuiCard-root').first().within(() => {
        cy.get('[aria-label="Delete task"]').click();
      });
      
      // Confirm deletion in dialog
      cy.get('[role="dialog"]').should('be.visible');
      cy.contains('Delete Task').should('be.visible');
      cy.get('[role="dialog"]').within(() => {
        cy.contains('button', 'Delete').click();
      });
      
      // Dialog should close
      cy.get('[role="dialog"]').should('not.exist');
      
      // Success message should appear
      cy.contains('Task deleted successfully').should('be.visible');
    });

    it('should cancel task deletion', () => {
      // Get initial count
      cy.get('.MuiCard-root').then(($cards) => {
        const initialCount = $cards.length;
        
        // Click delete on first task
        cy.get('.MuiCard-root').first().within(() => {
          cy.get('[aria-label="Delete task"]').click();
        });
        
        // Cancel deletion
        cy.get('[role="dialog"]').within(() => {
          cy.contains('button', 'Cancel').click();
        });
        
        // Dialog should close
        cy.get('[role="dialog"]').should('not.exist');
        
        // Task count should remain the same
        cy.get('.MuiCard-root').should('have.length', initialCount);
      });
    });
  });

  describe('Filter Tasks', () => {
    beforeEach(() => {
      // Create a task first
      cy.contains('button', 'New Task').click();
      cy.get('[role="dialog"]').within(() => {
        cy.get('input[name="title"]').type(`Filter Test ${Date.now()}`);
        cy.contains('button', 'Create').click();
      });
      cy.get('[role="dialog"]').should('not.exist');
    });

    it('should filter tasks by status', () => {
      // Click on Pending tab
      cy.contains('[role="tab"]', 'Pending').click();
      
      // Should filter (tab should be selected)
      cy.contains('[role="tab"]', 'Pending').should('have.attr', 'aria-selected', 'true');
      
      // Click on All tab
      cy.contains('[role="tab"]', 'All').click();
      cy.contains('[role="tab"]', 'All').should('have.attr', 'aria-selected', 'true');
    });
  });
});
