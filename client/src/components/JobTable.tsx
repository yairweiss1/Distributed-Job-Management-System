import { Job } from "../api/jobsApi";
import {
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button, Chip, LinearProgress
} from "@mui/material";

interface Props {
  jobs: Job[];
  onStop: (id: number) => void;
  onRestart: (id: number) => void;
  onDelete: (id: number) => void;
}

const statusColor = (status: number) => {
  switch (status) {
    case 0: return "info";      // Pending
    case 1: return "warning";   // Running
    case 2: return "success";   // Completed
    case 3: return "error";     // Failed
    case 4: return "default";   // Canceled
    case 5: return "default";   // Stopped
    default: return "default";
  }
};

const statusLabel = (status: number) => [
  "Pending", "Running", "Completed", "Failed", "Canceled", "Stopped"
][status] || "Unknown";

export default function JobTable({ jobs, onStop, onRestart, onDelete }: Props) {
  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>ID</TableCell>
            <TableCell>Job Name</TableCell>
            <TableCell>Priority</TableCell>
            <TableCell>Status</TableCell>
            <TableCell>Start Time</TableCell>
            <TableCell>End Time</TableCell>
            <TableCell>Progress</TableCell>
            <TableCell>Actions</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {jobs.map(job => (
            <TableRow key={job.id}>
              <TableCell>{job.id}</TableCell>
              <TableCell>{job.name}</TableCell>
              <TableCell>{job.priority === 1 ? "High" : "Regular"}</TableCell>
              <TableCell>
                <Chip label={statusLabel(job.status)} color={statusColor(job.status)} />
              </TableCell>
              <TableCell>{job.startTime ? new Date(job.startTime).toLocaleString() : ""}</TableCell>
              <TableCell>{job.endTime ? new Date(job.endTime).toLocaleString() : ""}</TableCell>
              <TableCell style={{ minWidth: 120 }}>
                <LinearProgress variant="determinate" value={job.progress} />
                {job.progress}%
              </TableCell>
              <TableCell>
                {job.status === 1 && (
                  <Button size="small" color="warning" onClick={() => onStop(job.id)}>Stop</Button>
                )}
                {(job.status === 3 || job.status === 5) && (
                  <Button size="small" color="info" onClick={() => onRestart(job.id)}>Restart</Button>
                )}
                {(job.status === 2 || job.status === 3) && (
                  <Button size="small" color="error" onClick={() => onDelete(job.id)}>Delete</Button>
                )}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
} 