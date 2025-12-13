export const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export const TOKEN_KEY = 'auth_token';
export const USER_KEY = 'user_data';

export const TASK_STATUSES = [
  { value: 'Pending', label: 'Pending', color: '#FFA726' },
  { value: 'InProgress', label: 'In Progress', color: '#42A5F5' },
  { value: 'Completed', label: 'Completed', color: '#66BB6A' },
] as const;

