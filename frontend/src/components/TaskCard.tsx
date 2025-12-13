import React from 'react';
import {
  Card,
  CardContent,
  CardActions,
  Typography,
  IconButton,
  Chip,
  Box,
  Tooltip,
} from '@mui/material';
import { Edit, Delete, CalendarToday } from '@mui/icons-material';
import { format } from 'date-fns';
import type { Task } from '@/types/api.types';
import { TASK_STATUSES } from '@/utils/constants';

interface TaskCardProps {
  task: Task;
  onEdit: (task: Task) => void;
  onDelete: (task: Task) => void;
}

export const TaskCard: React.FC<TaskCardProps> = ({ task, onEdit, onDelete }) => {
  const statusInfo = TASK_STATUSES.find((s) => s.value === task.status);

  const formatDate = (dateString: string) => {
    try {
      return format(new Date(dateString), 'MMM dd, yyyy');
    } catch {
      return dateString;
    }
  };

  const isOverdue = () => {
    const dueDate = new Date(task.dueDate);
    const today = new Date();
    return task.status !== 'Completed' && dueDate < today;
  };

  return (
    <Card
      sx={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        transition: 'transform 0.2s, box-shadow 0.2s',
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: 4,
        },
        borderLeft: `4px solid ${statusInfo?.color || '#ccc'}`,
      }}
    >
      <CardContent sx={{ flexGrow: 1 }}>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 1 }}>
          <Typography variant="h6" component="h2" sx={{ wordBreak: 'break-word' }}>
            {task.title}
          </Typography>
          <Chip
            label={statusInfo?.label || task.status}
            size="small"
            sx={{
              backgroundColor: statusInfo?.color,
              color: 'white',
              fontWeight: 'bold',
              ml: 1,
            }}
          />
        </Box>

        {task.description && (
          <Typography
            variant="body2"
            color="text.secondary"
            sx={{
              mb: 2,
              display: '-webkit-box',
              WebkitLineClamp: 3,
              WebkitBoxOrient: 'vertical',
              overflow: 'hidden',
            }}
          >
            {task.description}
          </Typography>
        )}

        <Box sx={{ display: 'flex', alignItems: 'center', mt: 2 }}>
          <CalendarToday sx={{ fontSize: 16, mr: 0.5, color: isOverdue() ? 'error.main' : 'text.secondary' }} />
          <Typography
            variant="caption"
            color={isOverdue() ? 'error.main' : 'text.secondary'}
            sx={{ fontWeight: isOverdue() ? 'bold' : 'normal' }}
          >
            Due: {formatDate(task.dueDate)}
            {isOverdue() && ' (Overdue)'}
          </Typography>
        </Box>
      </CardContent>

      <CardActions sx={{ justifyContent: 'flex-end', pt: 0 }}>
        <Tooltip title="Edit task">
          <IconButton size="small" color="primary" onClick={() => onEdit(task)}>
            <Edit fontSize="small" />
          </IconButton>
        </Tooltip>
        <Tooltip title="Delete task">
          <IconButton size="small" color="error" onClick={() => onDelete(task)}>
            <Delete fontSize="small" />
          </IconButton>
        </Tooltip>
      </CardActions>
    </Card>
  );
};

