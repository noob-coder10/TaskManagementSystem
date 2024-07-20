# TaskManagementSystem

## Overview

This project is a Task Management System developed using ASP.NET Core Web API. It provides functionalities for managing employees, projects, tasks, notes, and documents, with role-based authentication and authorization for employees, managers, and admins.

## Table of Contents
- [Features](#features)
- [Database Schema](#database-schema)
- [API Endpoints](#api-endpoints)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [Testing the API](#testing-the-api)

## Features

- **Authentication and Authorization**: Implemented using JWT (JSON Web Tokens) for secure access.
- **Role-based Access Control**: Three types of roles: Employee, Manager, and Admin.
- **Entities**: Employee, Project, Task, Note, and Document.
- **Functionalities**:
  - Admin can create, modify, and remove employees.
  - Managers can create, modify, and remove projects.
  - Managers can add employees to projects as project members.
  - Managers can assign tasks to project members and modify/remove tasks they created.
  - Project members and Manager of that project can add notes and attach/detach documents to/from tasks assigned to them.
  - Admins have full access and can track all activities.

## Database Schema

### Employee

- **Attributes**:
  - EmpId (PK)
  - FullName
  - Email
  - Role

### Project

- **Attributes**:
  - ProjectId (PK)
  - ProjectName
  - ProjectDescription
  - StartDate
  - EndDate
  - Status
  - ManagerId (FK to Employee)
 
 ### EmployeeProject (for many-to-many relation)
 
- **Attributes**:
  - ProjectMembersEmpId (FK to Employee)
  - ProjectsProjectId (FK to Project)
      
### Task

- **Attributes**:
  - TaskId (PK)
  - Title
  - Description
  - Status
  - CreatedAt
  - DueDate
  - ProjectId (FK to Project)
  - AssignedToEmpId (FK to Employee)

### Note

- **Attributes**:
  - NoteId (PK)
  - Description
  - CreatedAt
  - TaskId (FK to Task)
  - CreatedByEmpId (FK to Employee)

### Document

- **Attributes**:
  - DocumentId (PK)
  - Name
  - ContentType
  - Size
  - DocumentPath
  - UploadedAt
  - NoteId (FK to Note)

## API Endpoints

### Auth

- `/api/auth/login`: POST - Authenticates user and returns JWT token.
- `/api/auth/register`: POST - Registers a new user.

### Employee

- `/api/employee`: GET - Retrieves all employees.
- `/api/employee/{id}`: GET - Retrieves an employee by Id.
- `/api/employee`: POST - Add an employee by Admin.
- `/api/employee/{id}`: PUT - Updates employee details.
- `/api/employee/{id}`: DELETE - Deletes an employee.

### Project

- `/api/project/{id}`: GET - Retrieves a project by Id.
- `/api/project`: POST - Creates a new project.
- `/api/project/{id}`: PUT - Updates project details.
- `/api/project/{id}`: DELETE - Deletes a project.
- `/api/project/projectmember`: POST - Add a member to a project.
- `/api/project/projectmember`: DELETE - Remove a member to a project.

### Task

- `/api/task/{projectid}/{requesterid}`: GET - Get all tasks by projectId and requesterId.
- `/api/task`: POST - Creates a new task.
- `/api/task/{id}`: PUT - Updates task details.
- `/api/task/{taskid}/{requesterid}`: DELETE - Deletes a task.

### Note

- `/api/note/{taskId}/{requesterid}`: GET - Retrieves all notes for a task.
- `/api/note`: POST - Adds a new note to a task.
- `/api/note/{id}`: PUT - Update note details.
- `/api/note/{taskId}/{requesterid}`: DELETE - Deletes a note from a task.

### Document

- `/api/document/{noteid}`: GET - Retrieves all documents attached to a note.
- `/api/document/upload`: POST - Adds a new document to a note.
- `/api/document/{documentid}/{requesterid}`: DELETE - Deletes a document from a note.


## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or another compatible database)

## Installation

1. **Clone the Repository**
    ```bash
    git clone git@github.com:noob-coder10/TaskManagementSystem.git
    cd TaskManagementSystem
    ```

2. **Restore NuGet Packages**
    ```bash
    dotnet restore
    ```

## Database Setup

1. **Configure Database Connection String**
   - Open `appsettings.json` and configure the TaskManagementConnectionString and TaskManagementAuthConnectionString to your SQL Server database. And configure the RootPath as per your system's folderpath where you want to store the uploaded documents.
     ```json
     "ConnectionStrings": {
          "TaskManagementConnectionString": "Server=server_name;Database=database_name;Trusted_Connection=True;TrustServerCertificate=True",
          "TaskManagementAuthConnectionString": "Server=server_name;Database=auth_database_name;Trusted_Connection=True;TrustServerCertificate=True"
        },
        "FileUpload": {
          "RootPath": "root-path-for-storing-the-uploaded-files"
        }
     ```

2. **Apply Migrations**
   - Run the following command to apply the database migrations:
     ```bash
     cd BookManagementApp
     dotnet tool install --global dotnet-ef
     dotnet ef database update
     ```

## Running the Application

1. **Run the Application**
    ```bash
    dotnet run
    ```

2. **Access the API**
   - The swagger API will be accessible at `https://localhost:7191/swagger/index.html`.


## Testing the API

You can test the endpoints from swagger .


```

