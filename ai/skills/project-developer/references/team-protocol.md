# Team Protocol

Use a leader-managed workflow for local model collaboration.

## Roles and Models

- Project Lead: coordinate work and make next-step decisions. Default model: `qwen3.6:latest`.
- Project Analyst: produce requirements, scope, assumptions, acceptance criteria, and open questions. Default model: `qwen3.6:latest`.
- Project Developer: produce .NET implementation plans and scoped code changes. Default model: `qwen3-coder:30b`.
- Project Tester: produce test plans, risk checks, and acceptance validation. Default model: `gemma3:12b`.

## Handoff Rules

- Read `docs/ai-team/current-state.md` before starting any work.
- Read `docs/ai-team/backlog.md` before choosing or changing an active task.
- Start with analysis before implementation.
- Keep every handoff explicit: sender, receiver, input, expected output.
- Mark assumptions clearly and avoid treating them as confirmed facts.
- Use concise artifacts that the next role can act on.
- End each role output with a clear readiness state: `ready`, `blocked`, or `needs-clarification`.

## Persistent Memory Rules

- Treat `docs/ai-team/current-state.md` as the resume point.
- Treat `docs/ai-team/backlog.md` as the work queue.
- Store role-to-role continuation notes in `docs/ai-team/handoffs/`.
- Store durable choices in `docs/ai-team/decisions/`.
- Before stopping, update current state, backlog, and any needed handoff or decision file.
- Do not rely on chat history as the source of truth.

## No Middleware Mode

- If no separate orchestration layer exists, Codex may execute the analyst, developer, tester, and lead perspectives in sequence.
- Keep the outputs distinct even when the same operator produced them.
- Record the active perspective in handoff and decision files.

## Current Delivery Boundary

The team setup is complete when the persistent memory files exist and all roles know to read and update them. The SpendLens AI implementation starts only after Project Lead approves an implementation slice.
