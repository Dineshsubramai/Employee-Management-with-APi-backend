# Employee API

## Employee dashboard

The frontend is served by the same ASP.NET Core app at `/`. Open the API host in your browser (for example, `http://localhost:5080`). The dashboard uses the current host for its requests, so no separate frontend server or cross-origin setup is needed.

The dashboard lets a user create an account, sign in, search the employee directory, add employees, edit employee details, and delete employees. It also displays attendance and leave request summaries. Account passwords are stored as salted PBKDF2-SHA256 hashes. After sign-in, the page keeps the JWT in `sessionStorage` and sends it to protected employee GET routes.

Employee API routes used by the page:

- `POST /api/Auth/CreateAccount` and `POST /api/Auth/Login` for account creation and sign-in.
- `GET /api/employees` for the protected employee directory.
- `POST /api/employees`, `PUT /api/employees/{id}`, and `DELETE /api/employees/{id}` for employee changes.
- `GET /api/attendance` and `GET /api/leave-requests` for dashboard summaries.

## Run unit tests

From the solution folder, run:

```powershell
dotnet test EmployeeApi.slnx
```

The `EmployeeApi.UnitTests` project uses xUnit and Moq. Its controller test checks that `GET /api/employees` returns HTTP 200 and the employee list supplied by the service.

A simple ASP.NET Core API following the provided Tourist API pattern: controllers receive requests, DTOs validate input, services apply rules, EF Core accesses SQL Server, and JWT bearer authentication protects employee GET endpoints.

## Local employee webpage

Run `dotnet run` from this project folder. The browser opens the employee dashboard at `http://localhost:5080`. The Scalar API documentation is at `http://localhost:5080/scalar` and the OpenAPI document is at `/openapi/v1.json`. Sign in on the dashboard to load the protected employee list. Attendance and leave requests load without a token.

For local development, sign in with username `admin` and password `EmployeeDemo!2026`. The Development-only `POST /api/auth/token` endpoint returns a JWT. The dashboard stores the token for the current browser session and sends it with API requests. The same endpoint and demo credentials are disabled outside Development. Do not use these demo credentials or the sample signing key in production.

To use Scalar, first call `POST /api/auth/token` with this JSON body:

```json
{
  "username": "admin",
  "password": "EmployeeDemo!2026"
}
```

Copy `accessToken` from the response. Add this request header to employee GET endpoints in Scalar:

```text
Authorization: Bearer <paste-accessToken-here>
```

### Example: get employees with authorization

1. Start the app with `dotnet run` and open `http://localhost:5080/scalar`.
2. Open **POST `/api/auth/token`**. Send this body and click **Send**:

   ```json
   {
     "username": "admin",
     "password": "EmployeeDemo!2026"
   }
   ```

3. Copy the `accessToken` value from the response.
4. Open **GET `/api/employees`**. Under **Headers**, add:

   | Key | Value |
   |---|---|
   | `Authorization` | `Bearer paste-your-accessToken-here` |

   Replace the example text with the copied token. Include `Bearer`, one space, then the token.
5. Click **Send**. The response lists employees. If you receive `401 Unauthorized`, get a new token and check that the header value is correct.

Only `GET /api/employees` and `GET /api/employees/{id}` require a JWT. Employee POST, PUT, and DELETE, all attendance routes, and all leave-request routes are open in this project.

The dashboard shell can open even when SQL Server is unavailable, but employee data and saving require a working `EmployeeDb` connection string in `appsettings.json`.

## Request flow

`ASP.NET Core Web API → Controllers → Services → DTO validation → Entity Framework Core → SQL Server`

The database has three related tables: `Employees`, `Attendance`, and `LeaveRequests`.

## Run it

1. Set `ConnectionStrings:EmployeeDb` in `appsettings.json` to the SQL Server instance you use. The server must be running and the Windows account in the connection string must be allowed to create/open `EmployeeDb`.
2. Run these commands once from this project folder to create the database and its tables:

```powershell
dotnet restore
dotnet tool update --global dotnet-ef --version 10.0.13
dotnet ef database update
dotnet run
```

The `InitialCreate` migration is included, so `database update` creates the `Employees`, `Attendance`, and `LeaveRequests` tables. If SQL Server reports that the database cannot be opened, confirm the `Server` value and Windows login permissions in the connection string before rerunning the update.

In Development, the OpenAPI document is available at `/openapi/v1.json`.

## Routes

| Method | Route | Purpose | Authorization |
|---|---|---|---|
| POST | `/api/auth/token` | Get a Development-only JWT using the demo login | None |
| GET | `/api/employees` | List employees | JWT required |
| GET | `/api/employees/{id}` | Get one employee | JWT required |
| POST | `/api/employees` | Add employee | None |
| PUT | `/api/employees/{id}` | Update employee | None |
| DELETE | `/api/employees/{id}` | Delete employee and related records | None |
| GET | `/api/attendance?employeeId=1` | List attendance; employee filter is optional | None |
| POST | `/api/attendance` | Add attendance record | None |
| GET | `/api/leave-requests?employeeId=1` | List requests; employee filter is optional | None |
| POST | `/api/leave-requests` | Submit a leave request | None |
| PATCH | `/api/leave-requests/{id}/status` | Set status to Pending, Approved, or Rejected | None |

Example employee body:

```json
{
  "name": "Asha Kumar",
  "email": "asha@example.com",
  "department": "Engineering",
  "jobTitle": "Developer",
  "dateHired": "2025-04-01T00:00:00Z"
}
```

Use a real email address format and an ISO date (`YYYY-MM-DD` or `YYYY-MM-DDTHH:mm:ssZ`). A date such as `10/10/2026` may not parse in JSON requests.

Example attendance body:

```json
{
  "employeeId": 1,
  "workDate": "2026-10-20",
  "checkIn": "09:00:00",
  "checkOut": "17:30:00",
  "notes": "Office"
}
```

`workDate` may be omitted, set to an empty string, or supplied as `YYYY-MM-DD`. If it is omitted or blank, the API uses today's date. `employeeId` must refer to an employee already in the database.

Example leave request body:

```json
{
  "employeeId": 1,
  "startDate": "2026-11-02",
  "endDate": "2026-11-04",
  "reason": "Personal leave"
}
```
