# Integration Diagnostics Runner

This is a small console app that demonstrates selected Acumatica REST API workflows and writes one JSON file per scenario.

Project layout:

```text
Infrastructure\  Runtime session, scenario context, and JSON writer
Scenarios\       One JSON-producing scenario class per functional area
Support\         Hardcoded configuration, ID helpers, shared Acumatica helpers
```

Configuration is intentionally hardcoded in `Support\DiagnosticsConfig.cs`:

```csharp
public const string BaseUrl = "http://localhost:5555";
public const string Username = "admin";
public const string Password = "123";
public const string Company = "";
```

Build and run it with:

```powershell
dotnet build ProjectIntegration.sln
Project\bin\Debug\ProjectIntegration.exe
```

Output is written to:

```text
outputs\scenarios\
  session-login.json
  account-groups.json
  change-order-classes.json
  change-orders.json
  compound-filter.json
  cost-codes.json
  get-list-optimizations.json
  last-modified-date-times.json
  pro-forma.json
  project-budgets.json
  projects.json
  project-tasks.json
  project-templates.json
  project-template-tasks.json
  project-transactions.json
```

Each file contains scenario metadata, step results, and the captured REST request/response payloads. The runner does not delete records created during a run.
