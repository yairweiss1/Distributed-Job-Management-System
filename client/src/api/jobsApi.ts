const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'http://localhost:5034';
const API_URL = `${API_BASE_URL}/api/jobs`;

export interface Job {
  id: number;
  name: string;
  priority: number;
  status: number;
  startTime?: string;
  endTime?: string;
  progress: number;
  error?: string;
}

export async function fetchJobs(): Promise<Job[]> {
  const res = await fetch(API_URL);
  return res.json();
}

export async function createJob(job: { name: string; priority: number }) {
  const res = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(job),
  });
  return res.json();
}

export async function stopJob(id: number) {
  await fetch(`${API_URL}/${id}/stop`, { method: "POST" });
}

export async function restartJob(id: number) {
  await fetch(`${API_URL}/${id}/restart`, { method: "POST" });
}

export async function deleteJob(id: number) {
  await fetch(`${API_URL}/${id}`, { method: "DELETE" });
} 