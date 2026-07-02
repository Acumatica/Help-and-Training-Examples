# Generate an Acumatica Unit Test Regression Test

Generate one focused xUnit regression test for the repaired Acumatica customization behavior.

Use the open files as context:
- The existing test class.
- The implementation file that contains the repaired behavior.
- Message constants or helper files used by the existing tests.

Before generating code, briefly state:
1. The test goal.
2. The existing setup pattern you will reuse.
3. The assertions the test should make.
4. Any assumptions the developer should verify.

Then generate the test method.

The test should:

1. Follow the existing setup pattern already present in the test class.
2. Use the Acumatica Unit Test Framework.
3. Use Arrange, Act, Assert comments.
4. Verify the specific repaired behavior only.
5. Use existing message constants and helper methods where appropriate.
6. Use `PXCache.GetStateExt` and `PXFieldState` if the test inspects warnings or errors.
7. Use `PXCache.Locate` if validation changes the cached DAC object.
8. Avoid broad setup logic that hides the tested business rule.
9. Avoid hardcoded values unrelated to the existing test data.
10. Not change production code.
11. Not require a live database or browser session.
12. Keep the test readable.
13. Follow Acumatica Unit Test Framework patterns and Acuminator-friendly coding conventions.

After generating code, list what the developer should verify before running the test.