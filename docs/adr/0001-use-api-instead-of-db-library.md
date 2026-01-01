# ADR 0001: Replace Direct Database Access with API Layer

## Status
Accepted

## Context
The current WinForms application communicates with the database
via a shared custom library referenced directly by the UI layer.
This creates tight coupling between UI, data access, and business logic.

Problems with the current approach:
- UI depends directly on database-related abstractions
- Business logic is mixed with presentation logic
- Difficult to test business rules in isolation
- Future UI technology changes require deep refactoring
- No clear system boundary

The application is a legacy system and must be modernized incrementally
without a full rewrite.

## Decision
We will introduce a dedicated API layer responsible for:
- Data access
- Business orchestration
- Validation and rules enforcement

The WinForms application will communicate with the backend exclusively
via network calls (HTTP), not via direct database access or shared DB libraries.

The API will act as a system boundary and integration point.

Existing Domain and ApplicationLogic layers are the single source of truth
for business rules and workflows.

The API layer must reuse these layers via direct references.
Do NOT reimplement business logic, validations, or workflows
inside API controllers or API-specific services.

Controllers must be thin and delegate all behavior
to ApplicationLogic services.

## Consequences

### Positive
- Clear separation of concerns
- UI becomes a client, not a business owner
- Backend logic becomes testable independently
- Enables future UI replacement (e.g., WPF, MAUI) with minimal impact
- Enables additional clients (CLI, batch jobs, other services)

### Negative
- Initial implementation effort
- Need to handle network failures and latency
- Requires async-first mindset in UI

## Notes
This decision is foundational and should be respected by all future changes.
Any new functionality must be implemented behind the API boundary.

Controllers must remain thin and delegate all business behavior
to ApplicationLogic services.
