# Copilot Instructions for AI-Assisted Diagnostics and Testing

This working folder contains Acumatica ERP customization code and automated test code for the AI-Assisted Diagnostics and Testing workshop/course/guide.

When generating or modifying code:

- Preserve Acumatica graph, DAC, cache, and event-handler patterns.
- Do not change test expectations unless the business rule is explicitly wrong.
- Do not hardcode values only to make a test pass.
- Do not remove validation logic, warnings, or errors without explanation.
- Prefer small, focused changes.
- Use a review-first workflow: explain or plan first, edit second.
- Always list assumptions that the developer must verify.

## Unit Test Framework Guidance

For Acumatica Unit Test Framework tests:

- Follow existing xUnit and Acumatica Unit Test Framework patterns.
- Use `Setup<TGraph>(...)` when setup DACs are required.
- Use `PXGraph.CreateInstance<TGraph>()` to create graph instances.
- Use graph caches and views consistently with the existing tests.
- Use `PXCache.GetStateExt` and `PXFieldState` when testing warnings or errors.
- Use `PXCache.Locate` when validation changes the cached DAC object.
- Use existing message constants instead of hardcoded warning or error text.
- Keep regression tests focused on one repaired behavior.
- Do not require a live database or browser session for unit tests.

## Acuminator Diagnostics Guidance

When reviewing Acuminator reports or Acumatica customization code:

- Use Acuminator diagnostic IDs when available.
- Explain the Acumatica Framework rule behind the diagnostic before suggesting a fix.
- Prefer the smallest safe code change.
- Do not suppress a diagnostic unless there is a clear justification.
- Do not rewrite unrelated code.
- Preserve existing business behavior.
- After suggesting a fix, explain how to verify it by building, running unit tests, and rerunning Acuminator.