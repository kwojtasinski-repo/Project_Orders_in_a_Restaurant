# ADR 0002: Keep WinForms UI During Modernization

## Status
Deprecated

## Context
The application UI is currently implemented using Windows Forms (.NET Framework 4.8).
While the UI technology is dated, it is stable, well-understood, and heavily integrated
with the existing system.

A full UI rewrite would:
- Introduce high risk
- Delay delivery of business value
- Require rewriting logic that is already unstable architecturally

The primary architectural issues lie in coupling and layering, not in UI technology.

## Decision
We will keep WinForms as the UI technology during the modernization phase.

The focus of the migration will be:
- Extracting business logic out of the UI
- Introducing a clean backend boundary (API)
- Improving testability and maintainability

UI technology replacement is explicitly out of scope for the initial migration.

## Consequences

### Positive
- Lower migration risk
- Faster feedback cycle
- Business continuity
- Ability to validate backend architecture using an existing client

### Negative
- WinForms limitations remain in the short term
- UI improvements are constrained by legacy patterns

## Notes
Once the backend architecture is stabilized and API contracts are mature,
alternative UI technologies (e.g., WPF or MAUI) may be evaluated as separate initiatives.

This ADR has fulfilled its purpose during the modernization phase.
WinForms remains supported, but UI technology decisions are no longer
governed by this ADR and require new decisions.
