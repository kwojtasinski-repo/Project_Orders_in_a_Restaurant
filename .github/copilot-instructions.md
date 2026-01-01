# Copilot Instructions – Legacy WinForms Modernization

## Project Context

This repository contains a legacy Windows Forms application.

Current state:
- WinForms application
- Legacy architecture with mixed concerns
- UI tightly coupled to data access
- Migration in progress

Target direction:
- Incremental modernization
- Clear separation of concerns
- Network-based communication (API) instead of direct database access
- No full rewrite

Do NOT suggest:
- Blazor
- WebView
- MVVM frameworks for WinForms
- UI technology replacement unless explicitly requested

The existing internal library already follows a layered architecture
(Domain, ApplicationLogic, Infrastructure) and must be preserved.

---

## Architectural Boundaries (Strict Rules)

The following rules MUST be respected:

- UI layer (WinForms) must NOT contain business logic
- UI layer must NOT access the database directly
- UI layer must NOT reference data access implementations
- Business logic must NOT be implemented as static classes
- All cross-layer communication must go through interfaces

Violations of these rules are considered incorrect solutions.

Architectural decisions are documented in `docs/adr/` and must be respected.

Existing Domain and ApplicationLogic layers are the single source of truth
for business rules and workflows.

The API layer must reuse these layers via direct references.
Do NOT reimplement business logic, validations, or workflows
inside API controllers or API-specific services.

Controllers must be thin and delegate all behavior
to ApplicationLogic services.

---

## WinForms Safety Rules

When working with WinForms:

- NEVER modify `InitializeComponent()`
- NEVER manually edit designer-generated code
- All UI design changes must be done via the Designer
- Event handler logic must be outside of designer code
- Forms should contain only:
  - event handling
  - UI state management
  - calls to injected services

Do NOT introduce business logic into Forms.

---

## Async & Dependency Injection

- All I/O and service calls must be asynchronous
- Always use `async` / `await`
- NEVER use `.Result` or `.Wait()`
- Dependencies must be injected via constructors
- Property injection is not allowed
- Prefer interfaces over concrete implementations
- Synchronous wrappers around async code are not allowed.

The codebase is being migrated incrementally; solutions must be compatible with this approach.
