# Decision: Persistent AI Team Memory

Date: 2026-07-02
Status: accepted
Owner: Codex / Project Developer

## Context

AI tools can lose conversational context between sessions. The project needs a durable way for any role to resume work without relying on chat history.

## Decision

Use `docs/ai-team/` as the source of truth for team memory:

- `current-state.md` records the latest resume point.
- `backlog.md` records work status.
- `handoffs/` records role-to-role continuation notes.
- `decisions/` records durable project decisions.
- `templates/` provides consistent formats.

## Consequences

- Every role must read the memory files before starting.
- Every role must update the memory files before stopping.
- Chat history is useful context but not authoritative.

## Alternatives Considered

- Rely only on chat history: rejected because it is not durable enough for continuation.
- Keep state inside skill files: rejected because skills define behavior, while `docs/ai-team/` should track changing project state.

