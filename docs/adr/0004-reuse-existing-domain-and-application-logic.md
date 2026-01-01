# ADR 0004: Reuse Existing Domain and ApplicationLogic Layers

## Status
Accepted

## Context
The system already contains a well-structured internal library
with the following layers:
- Domain
- ApplicationLogic
- Infrastructure

These layers encapsulate business rules, use cases, and data access.
Rewriting this logic inside the API layer would introduce duplication,
inconsistency, and unnecessary risk.

## Decision
The existing Domain and ApplicationLogic layers are the single source of truth
for all business behavior.

The API layer will:
- Reference ApplicationLogic and Infrastructure projects
- Act only as a transport and orchestration layer
- Expose existing use cases via HTTP endpoints

Business rules, validations, and workflows must NOT be reimplemented
in API controllers or API-specific services.

## Consequences

### Positive
- No duplication of business logic
- Faster API implementation
- Lower regression risk
- Clear ownership of business rules

### Negative
- API lifecycle is coupled to ApplicationLogic
- Requires careful dependency management

## Notes
Any proposal to reimplement business logic in the API
must be accompanied by a new ADR.
