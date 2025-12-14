// User types
export interface User {
  id: string;
  name: string;
  email: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: User;
}

// Task types
export type TaskStatus = 'Pending' | 'InProgress' | 'Completed';

export interface Task {
  id: string;
  title: string;
  description?: string;
  status: TaskStatus;
  dueDate: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateTaskDto {
  title: string;
  description?: string;
  dueDate: string;
}

export interface UpdateTaskDto {
  title: string;
  description?: string;
  dueDate: string;
  status: TaskStatus;
}

// Error types
export interface ApiError {
  error: string;
  statusCode: number;
}


