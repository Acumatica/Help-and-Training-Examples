# Inspect a Test SDK Wrapper for a Modern UI form

Inspect the generated Test SDK wrapper for the `RS301000` form.

Do not modify files.

Use the open files as context:
- `Wrappers\Generated\RS\RS301000.cs`
- `Wrappers\Custom\RepairWorkOrdersScreen.cs`
- `Helpers\TestData.cs`
- `ModernUIWrapperNotes.md`, if available.

Please identify:

1. The generated screen class and namespace.
2. The member used for the Order Nbr. field.
3. The member used for the Labor grid.
4. The member used for the Labor Quantity field.
5. The methods used to save and refresh the screen or grid.
6. The helper methods in `RepairWorkOrdersScreen` that wrap these generated members.
7. Any wrapper members that are missing or not confirmed.
8. Any member names that Copilot should not invent.

Use exact member names from the generated wrapper.

If a member does not clearly exist, say that it is missing instead of inventing one.