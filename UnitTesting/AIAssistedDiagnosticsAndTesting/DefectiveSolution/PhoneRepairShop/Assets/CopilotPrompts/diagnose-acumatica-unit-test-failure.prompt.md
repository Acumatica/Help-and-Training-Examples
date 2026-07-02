# Diagnose an Acumatica Unit Test Failure

Use this prompt to diagnose a failed Acumatica unit test.

Do not edit files yet.

Use the open files as context:
- The failing test file.
- The relevant graph or graph extension file.
- Any message constants or helper files related to the failure.

Please do the following:

1. Classify the failure pattern as one of the following:
   - Failed assertion
   - Runtime exception
   - Missing dependency or service registration
   - Setup DAC or test data issue
   - Implementation defect

2. Explain what evidence supports the classification.

3. Explain the failing assertion or exception in plain language.

4. Identify the first file, method, event handler, or test setup area to inspect.

5. Identify the most likely root cause.

6. Explain what should not be changed yet.

7. Suggest the smallest safe fix as a plan only. Do not modify files.

8. List assumptions that the developer must verify before applying any change.

9. If an Acuminator rule or Acumatica Framework convention is relevant, mention it and explain why it applies.

Failure output:
[Paste the failed assertion, expected value, actual value, stack trace, or exception message.]