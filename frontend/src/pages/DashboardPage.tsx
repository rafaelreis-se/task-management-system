import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  Typography,
  Alert,
  Snackbar,
  CircularProgress,
  Tabs,
  Tab,
  Chip,
  Paper,
} from '@mui/material';
import { Add, Refresh } from '@mui/icons-material';
import { Layout } from '@/components/Layout';
import { TaskList } from '@/components/TaskList';
import { TaskDialog } from '@/components/TaskDialog';
import { DeleteConfirmDialog } from '@/components/DeleteConfirmDialog';
import { taskService } from '@/services/task.service';
import type { Task, CreateTaskDto, UpdateTaskDto, TaskStatus } from '@/types/api.types';

export const DashboardPage: React.FC = () => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [filteredTasks, setFilteredTasks] = useState<Task[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isDialogLoading, setIsDialogLoading] = useState(false);
  const [error, setError] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  
  // Dialog states
  const [isTaskDialogOpen, setIsTaskDialogOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedTask, setSelectedTask] = useState<Task | null>(null);
  
  // Filter state
  const [statusFilter, setStatusFilter] = useState<TaskStatus | 'All'>('All');

  useEffect(() => {
    loadTasks();
  }, []);

  useEffect(() => {
    // Apply filter
    if (statusFilter === 'All') {
      setFilteredTasks(tasks);
    } else {
      setFilteredTasks(tasks.filter((task) => task.status === statusFilter));
    }
  }, [tasks, statusFilter]);

  const loadTasks = async () => {
    try {
      setIsLoading(true);
      setError('');
      const data = await taskService.getAll();
      setTasks(data);
    } catch (err: any) {
      const message = err.response?.data?.error || 'Failed to load tasks';
      setError(message);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCreateTask = () => {
    setSelectedTask(null);
    setIsTaskDialogOpen(true);
  };

  const handleEditTask = (task: Task) => {
    setSelectedTask(task);
    setIsTaskDialogOpen(true);
  };

  const handleDeleteTask = (task: Task) => {
    setSelectedTask(task);
    setIsDeleteDialogOpen(true);
  };

  const handleSaveTask = async (data: CreateTaskDto | UpdateTaskDto) => {
    try {
      setIsDialogLoading(true);
      setError('');

      if (selectedTask) {
        // Update existing task
        await taskService.update(selectedTask.id, data as UpdateTaskDto);
        setSuccessMessage('Task updated successfully');
      } else {
        // Create new task
        await taskService.create(data as CreateTaskDto);
        setSuccessMessage('Task created successfully');
      }

      setIsTaskDialogOpen(false);
      await loadTasks();
    } catch (err: any) {
      const message = err.response?.data?.error || 'Failed to save task';
      setError(message);
    } finally {
      setIsDialogLoading(false);
    }
  };

  const handleConfirmDelete = async () => {
    if (!selectedTask) return;

    try {
      setIsDialogLoading(true);
      setError('');
      await taskService.delete(selectedTask.id);
      setSuccessMessage('Task deleted successfully');
      setIsDeleteDialogOpen(false);
      await loadTasks();
    } catch (err: any) {
      const message = err.response?.data?.error || 'Failed to delete task';
      setError(message);
    } finally {
      setIsDialogLoading(false);
    }
  };

  const getTaskCountByStatus = (status: TaskStatus) => {
    return tasks.filter((task) => task.status === status).length;
  };

  return (
    <Layout>
      <Box>
        {/* Header */}
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
          <Typography variant="h4" component="h1">
            My Tasks
          </Typography>
          <Box sx={{ display: 'flex', gap: 1 }}>
            <Button
              variant="outlined"
              startIcon={<Refresh />}
              onClick={loadTasks}
              disabled={isLoading}
            >
              Refresh
            </Button>
            <Button
              variant="contained"
              startIcon={<Add />}
              onClick={handleCreateTask}
            >
              New Task
            </Button>
          </Box>
        </Box>

        {/* Error Alert */}
        {error && (
          <Alert severity="error" onClose={() => setError('')} sx={{ mb: 3 }}>
            {error}
          </Alert>
        )}

        {/* Filter Tabs */}
        <Paper sx={{ mb: 3 }}>
          <Tabs
            value={statusFilter}
            onChange={(_, newValue) => setStatusFilter(newValue)}
            variant="scrollable"
            scrollButtons="auto"
          >
            <Tab
              label={
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  All
                  <Chip label={tasks.length} size="small" />
                </Box>
              }
              value="All"
            />
            <Tab
              label={
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  Pending
                  <Chip label={getTaskCountByStatus('Pending')} size="small" color="warning" />
                </Box>
              }
              value="Pending"
            />
            <Tab
              label={
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  In Progress
                  <Chip label={getTaskCountByStatus('InProgress')} size="small" color="info" />
                </Box>
              }
              value="InProgress"
            />
            <Tab
              label={
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  Completed
                  <Chip label={getTaskCountByStatus('Completed')} size="small" color="success" />
                </Box>
              }
              value="Completed"
            />
          </Tabs>
        </Paper>

        {/* Loading State */}
        {isLoading && (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
            <CircularProgress />
          </Box>
        )}

        {/* Task List */}
        {!isLoading && (
          <TaskList
            tasks={filteredTasks}
            onEdit={handleEditTask}
            onDelete={handleDeleteTask}
          />
        )}

        {/* Task Dialog */}
        <TaskDialog
          open={isTaskDialogOpen}
          task={selectedTask}
          onClose={() => setIsTaskDialogOpen(false)}
          onSave={handleSaveTask}
          isLoading={isDialogLoading}
        />

        {/* Delete Confirmation Dialog */}
        <DeleteConfirmDialog
          open={isDeleteDialogOpen}
          task={selectedTask}
          onClose={() => setIsDeleteDialogOpen(false)}
          onConfirm={handleConfirmDelete}
          isLoading={isDialogLoading}
        />

        {/* Success Snackbar */}
        <Snackbar
          open={!!successMessage}
          autoHideDuration={3000}
          onClose={() => setSuccessMessage('')}
          anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
        >
          <Alert severity="success" onClose={() => setSuccessMessage('')}>
            {successMessage}
          </Alert>
        </Snackbar>
      </Box>
    </Layout>
  );
};


