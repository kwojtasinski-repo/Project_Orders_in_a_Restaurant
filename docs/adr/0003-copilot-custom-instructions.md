# ADR 0003: Use Custom GitHub Copilot Instructions

## Status
Accepted

## Context
The project is undergoing incremental modernization while maintaining
a large legacy codebase. GitHub Copilot is used to assist development.

Without guidance, Copilot may:
- Introduce modern patterns incompatible with legacy constraints
- Suggest full rewrites or UI technology changes
- Generate code that violates architectural boundaries

## Decision
We will define repository-level GitHub Copilot instructions
to guide code generation and refactoring.

These instructions will:
- Define architectural boundaries
- Enforce WinForms safety rules
- Enforce async and dependency injection patterns
- Prevent undesired technology suggestions

## Consequences

### Positive
- More consistent generated code
- Reduced architectural violations
- Faster refactoring with less review overhead
- Shared understanding between human developers and tooling

### Negative
- Instructions must be maintained as architecture evolves

## Notes
Copilot instructions are not a replacement for code review,
but an architectural guardrail.
