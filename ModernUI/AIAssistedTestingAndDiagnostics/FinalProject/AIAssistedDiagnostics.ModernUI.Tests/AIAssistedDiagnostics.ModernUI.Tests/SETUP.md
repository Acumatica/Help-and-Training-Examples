# AI-Assisted Testing and Diagnostics with the Test SDK for the Modern UI (Test SDK Project Setup)

This document explains how to prepare and run the Test SDK project for the Modern UI used in Lessons/Activities 2.1–2.3 of the **AI-Assisted Testing & Diagnostics with the Test SDK for the Modern UI** training/help guide.

The project demonstrates automated browser-level validation for the **Repair Work Orders (RS301000)** form. 
The test opens work order `000001`, changes the labor quantity from `1` to `0`, triggers validation by saving and refreshing the Labor grid, and verifies that the quantity is restored to `1`.

## Audience

This setup is intended for facilitators who need to run this Test SDK project locally.


## What This Project Contains

The solution includes the prepared Test SDK project:

```text
AIAssistedDiagnostics.ModernUI.Tests.sln
AIAssistedDiagnostics.ModernUI.Tests\
```

The project includes:

```text
Wrappers\Generated\RS\RS301000.cs
Wrappers\Generated\RS301000_response.json
Wrappers\Custom\RepairWorkOrdersScreen.cs
Helpers\TestData.cs
Tests\StartTests.cs
Tests\RS301000_LaborQuantityTests.cs
artifacts\passing\
artifacts\failures\
```

The key generated wrapper members used by the tests added in `RS301000_LaborQuantityTests.cs` are:

```csharp
WorkOrders_fsColumnAOrder.OrderNbr
Labor_gridLabor
Labor_gridLabor.Row.Quantity
Save()
Labor_gridLabor.Refresh()
```

The custom helper class is:

```csharp
RepairWorkOrdersScreen
```

The helper wraps the generated wrapper members so the test code can use learner-friendly methods such as:

```csharp
Open()
OpenWorkOrder(string workOrderNbr)
GetFirstLaborQuantity()
SetFirstLaborQuantity(decimal quantity)
TriggerValidation()
```

## What This Project Does Not Include

The repository does **not** include:

- Acumatica ERP installer
- Test SDK ZIP file
- Real `config.xml` with credentials

## How to Build and Run the Project Locally

To build and run the project locally, install the following:

1. **Visual Studio 2022 or later**

   Recommended workload:

   ```text
   .NET desktop development
   ```

   Make sure the project can target:

   ```text
   .NET Framework 4.8
   ```

2. **GitHub Copilot for Visual Studio**

   Copilot is used in the learning activities related to this project to inspect wrapper code, generate UI test code (the `LaborQuantity_WhenBelowConfigured_RestoresConfiguredQuantity()` method), and classify failures.

3. **Acumatica ERP 2026 R1**

   For this project, the expected build is:

   ```text
   Acumatica ERP 2026 R1 build 26.100.0175 (https://acumatica-builds.s3.amazonaws.com/index.html?prefix=builds/26.1/26.100.0175/AcumaticaERP/)
   ```

4. **Matching Acumatica Test SDK**

   Use the Test SDK package that matches the Acumatica ERP build:

   ```text
   TestSDK_26_100_0175_183.zip (https://acumatica-builds.s3.amazonaws.com/index.html?prefix=builds/26.1/26.100.0175/TestSDK/)
   ```

   Extract it locally. A recommended path is:

   ```text
   C:\AcumaticaTestSDK\TestSDK_2026R1_26_100_0175\
   ```

5. **Local Acumatica site**

   The examples below assume a local site named:

   ```text
   SmartFix_T280
   ```

   Example URL:

   ```text
   http://localhost/SmartFix_T280
   ```

## Required Acumatica Site State

Before running the Test SDK project, the target Acumatica site must be prepared. You must deploy an instance for the **T280 Testing Business Logic with the Acumatica Unit Test Framework** training course by using the **Acumatica ERP Installer** downloaded in Step 3 above.

### Required customization

The `PhoneRepairShop` customization must be published to the deployed Acumatica ERP instance. This customization is automatically included in the **T280 Testing Business Logic with the Acumatica Unit Test Framework** training course instance.

This Test SDK project expects the published customization project to have the following behavior:

```text
On the **Repair Work Orders (RS301000)**, when a labor quantity lower than the configured value is entered,
the system restores the quantity to the configured value.
```

### Required screen

The user configured in `config.xml` of the extracted Test SDK pacakge must be able to open the following screen in the **Modern UI**:

```text
RS301000 - Repair Work Orders
```

### Required test data

The site must contain the following prepared work order:

| Item | Value |
|---|---|
| Screen | `RS301000` |
| Work order number | `000001` |
| Configured labor quantity | `1` |
| Test input quantity | `0` |
| Expected restored quantity | `1` |

Before running the automated tests, manually verify this behavior in Acumatica:

1. Sign in to the target site.
2. Open **Repair Work Orders (RS301000)**.
3. Open work order `000001`.
4. Confirm that the target labor line has quantity `1`.
5. Change the labor quantity to `0`.
6. Save the record and refresh the Labor grid.
7. Confirm that the quantity is restored to `1`.

If this manual check does not pass, fix the Acumatica site or customization before running the Test SDK project.

## Recommended Local Folder Structure

Use a stable local folder structure. For example:

```text
C:\AIAssistedTesting\ModernUITestSDK\  - For the Test SDK project downloaded from GitHub (see Step 1 below)
C:\AcumaticaTestSDK\TestSDK_2026R1_26_100_0175\ - For the extracted Test SDK ZIP file (see Item 4 above)
C:\AcuTestInstances\SmartFix_T280\ - For the deployed Acumatica site
C:\share\logs\ - For the Test SDK log files
```

The exact paths can differ, but the project, Test SDK configuration, and launch profile must point to the same locations.

## Step 1: Clone or Download the Repository

Clone or download the 2026R2 branch of the Help-and-Training-Examples repository from this link: https://github.com/Acumatica/Help-and-Training-Examples.
Copy the `ModernUI\TestSDK\AIAssistedTesting\FinalProject\AIAssistedDiagnostics.ModernUI.Tests` solution folder from the repository to a local folder.

Example:

```text
C:\AIAssistedTesting\ModernUITestSDK\
```

Open the solution:

```text
AIAssistedDiagnostics.ModernUI.Tests.sln
```

## Step 2: Add the Test SDK Packages as a NuGet Source

The project depends on packages from the matching Test SDK.

In Visual Studio:

1. Open **Tools > NuGet Package Manager > Package Manager Settings**.
2. Select **NuGet Package Manager > Package Sources**.
3. Add a new source.

Example:

```text
Name: Acumatica TestSDK 2026R1 26.100.0175
Source: C:\AcumaticaTestSDK\TestSDK_2026R1_26_100_0175\packages
```

4. Save the package source.
5. Restore NuGet packages for the solution.

If the project cannot restore packages, verify that the Test SDK path and package source are correct.

## Step 3: Configure the Test SDK `config.xml`

Use the `config.xml` file from the extracted Test SDK folder.

Example location:

```text
C:\AcumaticaTestSDK\TestSDK_2026R1_26_100_0175\config.xml
```

Update it for your local site. The exact XML structure depends on your Test SDK package, so use the existing file as the starting point.

Verify these values:

| Setting | Example |
|---|---|
| Site URL | `http://localhost/SmartFix_T280` |
| Tenant | `Company` |
| Username | `admin` or a dedicated Test SDK user |
| Password | Local test password |
| Browser path | Test SDK Chrome path |
| Log path | `C:\share\logs` |
| Download path | `C:\share\download` |
| Physical site path, if required | `C:\SmartFix_T280` |


## Step 4: Configure `launchSettings.json`

Open the project launch settings file:

```text
AIAssistedDiagnostics.ModernUI.Tests\Properties\launchSettings.json
```

Make sure the launch profile points to the Test SDK `config.xml`.

Example:

```json
{
  "profiles": {
    "AIAssistedDiagnostics.ModernUI.Tests": {
      "commandName": "Project",
      "commandLineArgs": "C:\\AcumaticaTestSDK\\TestSDK_2026R1_26_100_0175\\config.xml"
    }
  }
}
```

Use your actual Test SDK path.


## Step 5: Confirm the Project Builds

In Visual Studio:

1. Restore NuGet packages.
2. Build the solution.
3. Confirm that `AIAssistedDiagnostics.ModernUI.Tests` builds successfully.

The project should target:

```text
.NET Framework 4.8
```

If you see errors related to nullable reference types, confirm the project uses a C# language version that supports nullable reference syntax.

Recommended project settings:

```xml
<LangVersion>9.0</LangVersion>
<Nullable>enable</Nullable>
```

## Step 6: Verify `Test.cs`

The `Test.cs` file should simply sign in to the deployed instance and run `StartTests`.
The wrapper for the RS301000 screen has already been generated.

Recommended structure:

```csharp
using Core.Login;
using Core.TestExecution;

namespace AIAssistedDiagnostics.ModernUI.Tests
{
    public class Test : Check
    {
        public override void Execute()
        {
            PxLogin.LoginToDestinationSite();

            StartTests startTests = new StartTests();
            startTests.Execute();
        }
    }
}
```

## Step 7: Verify `StartTests.cs`

Open:

```text
Tests\StartTests.cs
```

This file should execute only the `RS301000` tests.

Recommended structure:

```csharp
using AIAssistedDiagnostics.ModernUI.Tests.Tests;
using Core.TestExecution;

namespace AIAssistedDiagnostics.ModernUI.Tests
{
    internal class StartTests
    {
        public void Execute()
        {
            using (TestExecution.CreateTestStepGroup("RS301000 smoke test"))
            {
                var rs301000Tests = new RS301000_LaborQuantityTests();

                rs301000Tests.RepairWorkOrdersScreen_CanOpenPreparedWorkOrder();
            }

            using (TestExecution.CreateTestStepGroup("RS301000 labor quantity validation test"))
            {
                var rs301000Tests = new RS301000_LaborQuantityTests();

                rs301000Tests.LaborQuantity_WhenBelowConfigured_RestoresConfiguredQuantity();
            }
        }
    }
}
```

## Step 8: Verify `TestData.cs`

Open:

```text
Helpers\TestData.cs
```

Confirm that it contains:

```csharp
namespace AIAssistedDiagnostics.ModernUI.Tests.Helpers
{
    public static class TestData
    {
        public const string PreparedWorkOrderNbr = "000001";

        public const decimal ConfiguredLaborQuantity = 1m;

        public const decimal BelowConfiguredLaborQuantity = 0m;

        public const decimal ExpectedRestoredLaborQuantity = 1m;
    }
}
```

## Step 9: Verify the Test Class Requirement

Open:

```text
Tests\RS301000_LaborQuantityTests.cs
```

The test class must inherit from `Core.Wrapper`.

Recommended declaration:

```csharp
using Core;

public sealed class RS301000_LaborQuantityTests : Wrapper
```

If the test class does not inherit from `Wrapper`, the generated `RS301000` wrapper may fail during initialization with an `InvalidCastException`.

## Step 10: Run the Smoke Test

The smoke test confirms that the project can sign in, open `RS301000`, open work order `000001`, and read the configured labor quantity.

Smoke test:

```csharp
RepairWorkOrdersScreen_CanOpenPreparedWorkOrder()
```

To run only the smoke test:

1. Open `StartTests.cs`.
2. Keep the validation test commented out.
3. Run the project from Visual Studio.

Example:

```csharp
rs301000Tests.RepairWorkOrdersScreen_CanOpenPreparedWorkOrder();

// rs301000Tests.LaborQuantity_WhenBelowConfigured_RestoresConfiguredQuantity();
```

Expected behavior:

```text
Log in to Acumatica
Open RS301000
Type Order Nbr. = 000001
Read the labor quantity
Confirm that the quantity is 1
```

Expected result:

```text
Exit code: 0
```

## Step 11: Run the Validation Test

After the smoke test passes, enable the validation test.

Validation test:

```csharp
LaborQuantity_WhenBelowConfigured_RestoresConfiguredQuantity()
```

Expected behavior:

```text
Log in to Acumatica
Open RS301000
Type Order Nbr. = 000001
Type Quantity = 0
Click Save
Click Refresh
Read the labor quantity
Confirm that the quantity is restored to 1
```

Expected result:

```text
Exit code: 0
```

## Expected Passing Output

A passing run should match the sanitized artifact:

```text
artifacts\passing\modern-ui-testsdk-passing-output.txt
```

Expected summary:

```text
Test: LaborQuantity_WhenBelowConfigured_RestoresConfiguredQuantity
Result: Passed

Screen:
RS301000 Repair Work Orders

Prepared record:
000001

Scenario:
The test opened Repair Work Orders, opened work order 000001, changed the labor quantity from 1 to 0, triggered validation by saving and refreshing the Labor grid, and verified that the labor quantity was restored to 1.

Observed operations:
- Open screen: RS301000
- Type into input: Order Nbr., value: 000001
- Type into input: Quantity, value: 0
- Click toolbar button: Save
- Click toolbar button: Refresh

Expected final quantity:
1

Observed final quantity:
1

Warning assertion:
Not asserted through wrapper. The runnable Test SDK assertion validates the restored quantity.

Exit code:
0
```

## Prepared Failure Artifacts

The project includes sanitized failure artifacts.

Expected files:

```text
artifacts\failures\wrapper-member-not-found-output.txt
artifacts\failures\validation-not-triggered-output.txt
artifacts\failures\test-data-not-found-output.txt
artifacts\failures\timing-wait-timeout-output.txt
artifacts\failures\environment-config-error-output.txt
```

These files are used to practice failure classification with Copilot.


## Troubleshooting

### Login fails

Check:

- Site URL in `config.xml`
- Tenant name
- Username
- Password
- User access rights
- Whether the site is running

### RS301000 does not open

Check:

- The user can open `RS301000` manually.
- The user sees `RS301000` in the Modern UI.
- The generated wrapper file exists:

  ```text
  Wrappers\Generated\RS\RS301000.cs
  ```

- `RepairWorkOrdersScreen.Open()` calls the correct screen-opening method.

### Work order `000001` cannot be found

Check:

- Work order `000001` exists in the target tenant.
- The Test SDK user can open it manually.
- `TestData.PreparedWorkOrderNbr` is set to `000001`.
- The site was not reset or reconfigured before the test.

### Validation test fails with `Expected: 1 / Actual: 0`

Likely cause:

```text
Validation was not triggered by the test.
```

Check:

- `TriggerValidation()` calls `Save()`.
- `TriggerValidation()` calls `Labor_gridLabor.Refresh()`.
- The UI has enough time to save and refresh.
- The customization contains the fixed business logic.

### Warning text is not asserted

This is expected.

The generated wrapper exposes:

```csharp
Labor_gridLabor.Row.Quantity
```

but warning inspection has not been confirmed through the wrapper/control API.

The runnable validation test asserts the restored quantity, not the warning text.