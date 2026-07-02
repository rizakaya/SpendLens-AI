# AI Team Operating Guide

This folder is the persistent memory for the SpendLens AI software team.
Every AI role must read this folder before starting work and update it before handing work off.

## Team Roles

- Project Lead: owns scope, sequencing, decisions, and readiness.
- Project Analyst: turns product intent into testable requirements.
- Project Developer: Codex; owns implementation, code changes, verification notes, and temporary role orchestration when no separate middle layer exists.
- Project Tester: owns acceptance checks, regression risks, and test readiness.

## Execution Mode

There is currently no separate orchestration layer in the product workflow.
Until one exists, Codex acts as the active operator:

- reads product docs
- triggers analyst-style requirement synthesis when needed
- triggers tester-style test planning when needed
- makes implementation progress directly
- records which role perspective was used for each handoff

## Always Read First

1. `docs/ai-team/current-state.md`
2. `docs/ai-team/backlog.md`
3. Latest relevant file in `docs/ai-team/handoffs/`
4. Relevant decision records in `docs/ai-team/decisions/`
5. Product documents in `docs/`

## Always Update Before Stopping

1. Update `docs/ai-team/current-state.md` with the latest phase, active task, blockers, and next step.
2. Add or update a handoff file under `docs/ai-team/handoffs/` when another role must continue.
3. Add a decision record under `docs/ai-team/decisions/` for choices that affect architecture, scope, data flow, dependencies, or delivery order.
4. Update `docs/ai-team/backlog.md` when a task is started, completed, blocked, or split.

## Working Agreement

- Keep one active task at a time unless the Project Lead explicitly splits work.
- Do not rely on chat history as the source of truth.
- Record assumptions separately from confirmed facts.
- End each role output with a clear state: `ready`, `blocked`, or `needs-clarification`.
- Prefer short, concrete updates over long narrative notes.
- When no orchestration layer exists, Codex may execute Project Lead, Analyst, Developer, and Tester responsibilities in sequence, but must still record them distinctly in `docs/ai-team/`.
