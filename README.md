# Distributed Job Management System

A distributed job management system built with React + TypeScript (client) and .NET Core (server).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Git](https://git-scm.com/)

## System Architecture

See the [Architecture Document](docs/architecture.md) for a detailed overview, diagram, and explanation of key components and flow.

## Project Structure

- `client/` - React + TypeScript frontend
- `server/` - .NET Core backend
- `docs/` - Project documentation

## Environment Configuration

### Server (.NET Core)
Create a `.env` file in the server root with:
```
DB_SERVER=your_database_server
```

### Client (React)
Create a `.env` file in the client root with:
```
REACT_APP_API_BASE_URL=http://localhost:5034
```

> Note: Add `.env` files to `.gitignore` to keep sensitive information secure.

## Getting Started

### Server Setup
1. Navigate to the server directory
2. Create `.env` file with database configuration
3. Run the following commands:
```bash
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

### Client Setup
1. Navigate to the client directory
2. Create `.env` file with API base URL
3. Run the following commands:
```bash
npm install
npm run dev
```

## Features

- Create, view, update, and delete jobs
- Real-time job status updates using SignalR
- Job priority management
- Job progress tracking
- Error handling and reporting

## Technologies Used

- Frontend: React, TypeScript, Material-UI, SignalR Client
- Backend: .NET Core, Entity Framework Core, SignalR
- Database: SQL Server

## API Documentation

### Jobs API

#### List Jobs
- **GET** `/api/jobs`
- Returns: Array of job objects
- Example response:
```json
[
  {
    "id": 1,
    "name": "Test Job",
    "priority": 0,
    "status": 0,
    "progress": 0,
    "startTime": null,
    "endTime": null,
    "error": null
  }
]
```

#### Create Job
- **POST** `/api/jobs`
- Body:
```json
{
  "name": "string",
  "priority": 0
}
```

#### Stop Job
- **POST** `/api/jobs/{id}/stop`

#### Restart Job
- **POST** `/api/jobs/{id}/restart`

#### Delete Job
- **DELETE** `/api/jobs/{id}`

### Real-Time Updates

- SignalR hub at `/jobHub`
- Event: `JobUpdated` (payload: job object)
- Example payload:
```json
{
  "id": 1,
  "name": "Test Job",
  "priority": 0,
  "status": 1,
  "progress": 50,
  "startTime": "2024-02-27T10:00:00Z",
  "endTime": null,
  "error": null
}
```

## Development Notes

- The server uses Entity Framework Core for database operations
- SignalR is used for real-time updates between server and client
- Jobs are processed by background workers with priority queue
- Error handling includes automatic retries and status updates

## Troubleshooting

### Common Issues

1. **Database Connection Issues**
   - Verify SQL Server is running
   - Check `.env` file configuration
   - Ensure database exists and migrations are applied

2. **SignalR Connection Issues**
   - Verify server is running
   - Check CORS settings
   - Ensure correct API base URL in client `.env`

3. **Build Issues**
   - Run `dotnet restore` and `npm install` to update dependencies
   - Clear npm cache: `npm cache clean --force`
   - Delete `node_modules` and reinstall

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

MIT 