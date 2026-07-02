# MVP Requirements

Date: 2026-07-02
Prepared by: Codex acting in Project Analyst mode
State: ready

## Goal

Build the first working MVP of SpendLens AI so a user can import transaction data, preview and normalize it, inspect categorized spending, and receive basic AI-assisted financial insights.

## Users

- Individual users who want to upload bank or credit card activity and understand spending patterns.

## Scope

- Import transaction data from CSV or Excel files.
- Preview imported data before full import.
- Map source columns to the internal transaction model.
- Normalize merchant descriptions.
- Suggest categories for transactions.
- Show monthly summaries, category distribution, and top merchants.
- Detect recurring payments and subscription candidates.

## Out Of Scope

- Direct bank integrations.
- Mobile apps.
- OCR or PDF statement parsing.
- Financial advice, investment recommendations, or automated cancellation actions.

## Functional Requirements

- FR-1: The backend must accept import content and return a structured preview before persistence.
- FR-2: The system must identify key transaction fields such as date, description, amount, and currency.
- FR-3: The system must allow mapping source columns to internal fields when automatic detection is not enough.
- FR-4: Financial calculations must be handled by backend logic, not by the LLM.
- FR-5: AI usage must be limited to interpretation, normalization, classification, and narrative explanation.

## Acceptance Criteria

- AC-1: A valid CSV payload returns preview rows, headers, and warnings when applicable.
- AC-2: The preview response clearly reports the detected delimiter and total parsed rows.
- AC-3: Invalid or incomplete import content returns a client-visible validation failure.
- AC-4: The import workflow can be extended to file upload and persistence without replacing the current preview contract.

## Notes

- The current implementation covers FR-1 and part of FR-2 through `POST /api/imports/preview`.
- File upload, column mapping, and persistence are the next slice.
