import React, { useEffect } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  MenuItem,
  Box,
  CircularProgress,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import type { Task, CreateTaskDto, UpdateTaskDto, TaskStatus } from '@/types/api.types';
import { TASK_STATUSES } from '@/utils/constants';

interface TaskDialogProps {
  open: boolean;
  task?: Task | null;
  onClose: () => void;
  onSave: (data: CreateTaskDto | UpdateTaskDto) => Promise<void>;
  isLoading?: boolean;
}

export const TaskDialog: React.FC<TaskDialogProps> = ({
  open,
  task,
  onClose,
  onSave,
  isLoading = false,
}) => {
  const isEditMode = !!task;

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<UpdateTaskDto>({
    defaultValues: {
      title: '',
      description: '',
      dueDate: '',
      status: 'Pending',
    },
  });

  useEffect(() => {
    if (task) {
      // Format date for input field (YYYY-MM-DD)
      const formattedDate = task.dueDate.split('T')[0];
      reset({
        title: task.title,
        description: task.description || '',
        dueDate: formattedDate,
        status: task.status,
      });
    } else {
      // Default to tomorrow for new tasks
      const tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      const formattedDate = tomorrow.toISOString().split('T')[0];
      reset({
        title: '',
        description: '',
        dueDate: formattedDate,
        status: 'Pending',
      });
    }
  }, [task, reset]);

  const onSubmit = async (data: UpdateTaskDto) => {
    // Convert date to ISO format
    const isoDate = new Date(data.dueDate).toISOString();
    
    if (isEditMode) {
      await onSave({
        ...data,
        dueDate: isoDate,
      });
    } else {
      // For create, we don't send status
      const { status, ...createData } = data;
      await onSave({
        ...createData,
        dueDate: isoDate,
      });
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{isEditMode ? 'Edit Task' : 'Create New Task'}</DialogTitle>
      <Box component="form" onSubmit={handleSubmit(onSubmit)}>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Title"
            fullWidth
            required
            error={!!errors.title}
            helperText={errors.title?.message}
            {...register('title', {
              required: 'Title is required',
              minLength: {
                value: 3,
                message: 'Title must be at least 3 characters',
              },
            })}
          />
          <TextField
            margin="dense"
            label="Description"
            fullWidth
            multiline
            rows={4}
            error={!!errors.description}
            helperText={errors.description?.message}
            {...register('description')}
          />
          <TextField
            margin="dense"
            label="Due Date"
            type="date"
            fullWidth
            required
            InputLabelProps={{
              shrink: true,
            }}
            error={!!errors.dueDate}
            helperText={errors.dueDate?.message}
            {...register('dueDate', {
              required: 'Due date is required',
            })}
          />
          {isEditMode && (
            <Controller
              name="status"
              control={control}
              rules={{ required: 'Status is required' }}
              render={({ field }) => (
                <TextField
                  {...field}
                  margin="dense"
                  label="Status"
                  select
                  fullWidth
                  required
                  error={!!errors.status}
                  helperText={errors.status?.message}
                >
                  {TASK_STATUSES.map((status) => (
                    <MenuItem key={status.value} value={status.value}>
                      {status.label}
                    </MenuItem>
                  ))}
                </TextField>
              )}
            />
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose} disabled={isLoading}>
            Cancel
          </Button>
          <Button type="submit" variant="contained" disabled={isLoading}>
            {isLoading ? <CircularProgress size={24} /> : isEditMode ? 'Update' : 'Create'}
          </Button>
        </DialogActions>
      </Box>
    </Dialog>
  );
};

