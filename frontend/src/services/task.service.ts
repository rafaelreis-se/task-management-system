import { api } from './api';
import type { Task, CreateTaskDto, UpdateTaskDto } from '@/types/api.types';

export const taskService = {
  /**
   * Get all tasks for the authenticated user
   */
  getAll: async (): Promise<Task[]> => {
    const response = await api.get<Task[]>('/tasks');
    return response.data;
  },

  /**
   * Get a single task by ID
   */
  getById: async (id: string): Promise<Task> => {
    const response = await api.get<Task>(`/tasks/${id}`);
    return response.data;
  },

  /**
   * Create a new task
   */
  create: async (data: CreateTaskDto): Promise<Task> => {
    const response = await api.post<Task>('/tasks', data);
    return response.data;
  },

  /**
   * Update an existing task
   */
  update: async (id: string, data: UpdateTaskDto): Promise<Task> => {
    const response = await api.put<Task>(`/tasks/${id}`, data);
    return response.data;
  },

  /**
   * Delete a task
   */
  delete: async (id: string): Promise<void> => {
    await api.delete(`/tasks/${id}`);
  },
};


