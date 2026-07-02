# Classify an Acumatica Unit Test Failure

Use this prompt to classify a failed Acumatica unit test before asking for a root-cause analysis or code fix.

Do not edit files.

Use the failed test output and any open source files as context.

Please classify the failure as one of the following:

1. Failed assertion
2. Runtime exception
3. Missing dependency or service registration
4. Setup DAC or test data issue
5. Implementation defect

For the classification, provide:

1. The selected failure category.
2. The evidence that supports this classification.
3. Why the other categories are less likely.
4. The first file, method, or setup area to inspect next.
5. What should not be changed yet.
6. Whether this failure should be investigated as a test issue, setup issue, or implementation issue.

Do not propose a code change yet.
Do not modify files.
Do not change the test expectation.

Failure output:
[Paste the failed assertion, expected value, actual value, stack trace, or exception message.]