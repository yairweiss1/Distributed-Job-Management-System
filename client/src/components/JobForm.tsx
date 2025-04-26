import { useState } from "react";
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField, Checkbox, FormControlLabel } from "@mui/material";

interface Props {
  open: boolean;
  onClose: () => void;
  onCreate: (name: string, priority: number) => void;
}

export default function JobForm({ open, onClose, onCreate }: Props) {
  const [name, setName] = useState("");
  const [highPriority, setHighPriority] = useState(false);

  const handleSubmit = () => {
    onCreate(name, highPriority ? 1 : 0);
    setName("");
    setHighPriority(false);
    onClose();
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>New Job</DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          margin="dense"
          label="Job Name"
          fullWidth
          value={name}
          onChange={e => setName(e.target.value)}
        />
        <FormControlLabel
          control={<Checkbox checked={highPriority} onChange={e => setHighPriority(e.target.checked)} />}
          label="High Priority"
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleSubmit} disabled={!name.trim()}>Create</Button>
      </DialogActions>
    </Dialog>
  );
} 