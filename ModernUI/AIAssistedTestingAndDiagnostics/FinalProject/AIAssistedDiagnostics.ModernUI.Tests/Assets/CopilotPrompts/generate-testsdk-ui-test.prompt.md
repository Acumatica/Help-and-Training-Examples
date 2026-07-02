# Generate a UI Test by using the Test SDK

Draft a UI Test by using the Test SDK for the Repair Work Orders labor quantity validation scenario.

Target screen:
`RS301000` Repair Work Orders

Scenario:
Open existing repair work order `000001` whose labor line has a configured/default quantity of `1`.
Change the labor quantity to `0`.
Trigger validation by using the project’s established save/update pattern.
Verify that the UI restores the labor quantity to `1`.
If the wrapper exposes field warning information, also verify that the `QuantityTooSmall` warning is shown.

Use the open files as context:
- The generated `RS301000` wrapper class.
- The `RepairWorkOrdersScreen` helper class.
- The `TestData` helper class.
- The existing Test SDK test class.

Before generating code, list:
1. The helper methods you plan to use.
2. The generated wrapper members that support those helper methods.
3. Any assumptions that must be verified.

Requirements:

1. Prefer the `RepairWorkOrdersScreen` helper methods where possible.
2. Use generated wrapper members only if a helper method does not already exist.
3. Do not use direct DOM selectors unless the wrappers do not expose the needed element.
4. Use deterministic test data from `TestData`.
5. Open work order `000001`.
6. Change the labor quantity from `1` to `0`.
7. Trigger validation.
8. Assert that the final labor quantity is `1`.
9. Assert the `QuantityTooSmall` warning only if the wrapper exposes a reliable way to inspect it.
10. Use Arrange, Act, Assert comments.
11. Avoid hardcoded credentials, tenant secrets, local paths, or environment-specific URLs.
12. Add TODO comments where wrapper names or test data must be verified.
13. Name the test method `LaborQuantity_WhenBelowConfigured_RestoresConfiguredQuantity`.

Constraints:

- Do not invent wrapper members.
- Do not rewrite existing helpers unless the helper has a clear TODO.
- Do not modify configuration files.
- Do not create broad setup logic that distracts from the test scenario.
- Show the generated method or diff before the test is run.