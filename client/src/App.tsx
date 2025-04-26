import { useEffect, useState } from "react";
import { Container, Typography, Button, Box, Dialog, DialogTitle, DialogContent, DialogActions } from "@mui/material";
import JobTable from "./components/JobTable";
import JobForm from "./components/JobForm";
import { fetchJobs, createJob, stopJob, restartJob, deleteJob, Job } from "./api/jobsApi";
import { HubConnectionBuilder } from "@microsoft/signalr";

function App() {
  const [jobs, setJobs] = useState<Job[]>([]);
  const [open, setOpen] = useState(false);
  const [deleteJobId, setDeleteJobId] = useState<number | null>(null);

  // Fetch jobs on load
  useEffect(() => {
    fetchJobs().then(setJobs);
  }, []);

  // Setup SignalR connection
  useEffect(() => {
    const hubConnection = new HubConnectionBuilder()
      .withUrl(`${process.env.REACT_APP_API_BASE_URL}/jobHub`)
      .withAutomaticReconnect()
      .build();

    hubConnection.on("JobUpdated", (job: Job) => {
      setJobs(prev => {
        const idx = prev.findIndex(j => j.id === job.id);
        if (idx !== -1) {
          const updated = [...prev];
          updated[idx] = job;
          return updated;
        } else {
          return [...prev, job];
        }
      });
    });

    hubConnection.start();

    return () => { hubConnection.stop(); };
  }, []);

  // Handlers
  const handleCreate = async (name: string, priority: number) => {
    const job = await createJob({ name, priority });
    setJobs(jobs => [...jobs, job]);
  };

  const handleStop = async (id: number) => {
    await stopJob(id);
  };

  const handleRestart = async (id: number) => {
    await restartJob(id);
  };

  const handleDelete = async (id: number) => {
    setDeleteJobId(id);
  };

  const confirmDelete = async () => {
    if (deleteJobId !== null) {
      await deleteJob(deleteJobId);
      setJobs(jobs => jobs.filter(j => j.id !== deleteJobId));
      setDeleteJobId(null);
    }
  };

  const cancelDelete = () => setDeleteJobId(null);

  return (
    <Container>
      <Box display="flex" justifyContent="space-between" alignItems="center" mt={4} mb={2}>
        <Typography variant="h4">Jobs</Typography>
        <Button variant="contained" onClick={() => setOpen(true)}>Add New Job</Button>
      </Box>
      <JobTable jobs={jobs} onStop={handleStop} onRestart={handleRestart} onDelete={handleDelete} />
      <JobForm open={open} onClose={() => setOpen(false)} onCreate={handleCreate} />
      <Dialog open={deleteJobId !== null} onClose={cancelDelete}>
        <DialogTitle>Delete Job</DialogTitle>
        <DialogContent>Are you sure you want to delete this job?</DialogContent>
        <DialogActions>
          <Button onClick={cancelDelete}>Cancel</Button>
          <Button onClick={confirmDelete} color="error">Delete</Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}

export default App;
