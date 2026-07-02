# Inspect an Acumatica Unit Test Failure

Use this prompt to inspect a failed Acumatica unit test after the failure pattern has been classified.

Do not edit files yet.

Use the open files as context:
- The failing test file.
- The implementation file that contains the graph or graph extension logic.
- Any message constants or helper files related to the failure.

For the current activity, inspect the failing `TestRepairWorkOrdersForm` unit test.

Relevant files:
- `PhoneRepairShop_Code.Tests/RSSVWorkOrderEntryTests.cs`
- `PhoneRepairShop_Code/RSSVWorkOrderEntry.cs`
- `PhoneRepairShop_Code/Helper/Messages.cs`

Expected business rule:
When `RSSVWorkOrderLabor.Quantity` is lower than the configured labor quantity, the system should attach the `QuantityTooSmall` warning and restore the quantity to the configured value.

Observed failure:
The test sets `woLabor.Quantity` to `1`.
The `QuantityTooSmall` warning is expected.
`woLabor.Quantity` should be restored to `3`.
The test fails at the quantity assertion.

Failing assertion:
`Assert.Equal(3, woLabor.Quantity)`

Actual value:
`1`

Please do the following:

1. Explain what the failing assertion means.
2. Identify the most likely root cause.
3. Point to the method or event handler to inspect first.
4. Explain why the warning can still appear while the quantity is not restored.
5. Propose the smallest safe fix as a plan only.
6. List assumptions the developer should verify before editing code.
7. Identify any Acumatica Framework convention or Acuminator rule that may be relevant.
8. Explain how the proposed fix should be validated.

Constraints:
- Do not edit files.
- Do not change the test expectation.
- Do not remove `Messages.QuantityTooSmall`.
- Do not change `PXErrorLevel.Warning`.
- Do not hardcode a value only to make the test pass.
- Do not rewrite unrelated graph logic.