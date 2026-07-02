# Copilot Instructions for Modern UI Test SDK Testing

This working folder contains a prepared Acumatica Test SDK project for the AI-Assisted Diagnostics and Testing workshop/course/guide.

When generating or modifying code:

- Use the generated Test SDK wrappers or the custom helper classes.
- Do not invent wrapper members.
- Do not use direct DOM selectors unless no wrapper member or helper method exists.
- Do not add credentials, tenant names, local paths, or environment-specific URLs to generated code.
- Use deterministic test data from `TestData`.
- Preserve the existing Test SDK project structure.
- Do not modify `config.xml`, `launchSettings.json`, or `Test.cs` unless explicitly instructed.
- Use a review-first workflow: explain or plan first, edit second.
- Always list assumptions that the developer must verify.

## Workshop Scenario

The Test SDK test validates the same behavior that was fixed and validated by the unit tests:

- Screen: Repair Work Orders (`RS301000`)
- Prepared work order: `000001`
- Configured labor quantity: `1`
- Below-configured test quantity: `0`
- Expected restored quantity: `1`

## Confirmed Wrapper and Helper Members

Use the following generated or helper members:

- `RepairWorkOrdersScreen`
- `screen.Open()`
- `screen.OpenWorkOrder(TestData.PreparedWorkOrderNbr)`
- `screen.GetFirstLaborQuantity()`
- `screen.SetFirstLaborQuantity(TestData.BelowConfiguredLaborQuantity)`
- `screen.TriggerValidation()`
- `WorkOrders_fsColumnAOrder.OrderNbr`
- `Labor_gridLabor`
- `Labor_gridLabor.Row.Quantity`
- `Save()`
- `Labor_gridLabor.Refresh()`

Do not invent convenience wrapper members such as:

- `Labor`
- `LaborGrid`
- `QuantityField`
- `SetQuantity`
- `OpenRepairWorkOrder`