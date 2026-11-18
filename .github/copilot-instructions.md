# GitHub Copilot Instructions for Yayo's Bank (Continued) Mod

Welcome to the GitHub Copilot instructions for Yayo's Bank (Continued), a RimWorld mod that provides a dynamic banking system, including loans, war bonds, and economic incidents. Below are guidelines to help you navigate the codebase and contribute effectively.

## Mod Overview and Purpose

**Mod Name:** Yayo's Bank (Continued)  
**Author:** YAYO, Amnabi  
**PackageId:** Mlie.YayosBank  
**Description:**  
This mod updates the original YAYO mod and introduces a comprehensive banking system to RimWorld. Players can take loans from factions, face consequences if they can't repay in time, and engage with war bonds that can be sold or used for military aid. The mod includes a dynamic economy with economic incidents and a system for dividend arrivals and bond price changes based on quest success.

## Key Features and Systems

- **Loans:** Players can request loans from factions with a risk of attack if the debt is not repaid on time.
- **War Bonds:** Factions sell war bonds, which fluctuate in price and can be used for military aid or profit.
- **Economic Incidents:** Events like loan increases, bankruptcy, and creditor raids add depth to the economic system.
- **Dividend Arrivals:** Players receive items as dividends every quarter if they hold bonds.
- **Delisting:** Bonds are delisted if their price falls below $100.
- **Bond Price Changes:** Prices fluctuate based on quest success, creating a dynamic market.

## Coding Patterns and Conventions

- **Coding Language:** C#
- **Framework:** RimWorld modding framework
- **Libraries:** Harmony, HugsLib
- **Structure:** The codebase is organized into folders for different aspects of the mod, such as XML definitions, Harmony patches, and utility classes.

### Directory Structure

- **Source/rimstocks/**: Contains the main C# code.
  - **FactionData.cs**: Manages faction data.
  - **CustomGraphGroup.cs**: Handles custom graph rendering.
  - **FactionPriceData.cs**: Manages faction price data.
  - **HarmonyPatches/**: Contains Harmony patches for RimWorld.
- **1.7/Defs/**: Contains XML definitions for mod elements.
- **1.7/Patches/**: Contains XML patches for RimWorld.

### Naming Conventions

- **Classes:** PascalCase (e.g., `FactionData`, `CustomGraphGroup`)
- **Methods and Variables:** camelCase (e.g., `clear()`, `ExposeData`)
- **Constants:** UPPERCASE (e.g., `MAX_LOAN_AMOUNT`)

## XML Integration

Yayo's Bank (Continued) heavily relies on XML for defining game elements. Here’s how to work with XML files:

- **Structure:** Each XML file defines specific game elements like `ThingCategoryDef`, `GameConditionDef`, `IncidentDef`, and `letterDef`.
- **Naming:** Use descriptive names for XML elements to ensure clarity.
- **Location:** Place XML files in the appropriate directories within the mod folder.

### Example XML Definition

xml
<ThingCategoryDef Name="BankItems">
  <label>Bank Items</label>
  <parentName>Items</parentName>
  <thingCategories>
    <li>Money</li>
    <li>Bonds</li>
  </thingCategories>
</ThingCategoryDef>


## Harmony Patching

Harmony is a powerful tool for modding RimWorld, allowing you to modify the game's behavior without altering the original code. Here’s how to use Harmony patches in Yayo's Bank (Continued):

- **Patch Location:** Harmony patches are located in the `Source/rimstocks/HarmonyPatches/` directory.
- **Patch Types:** Prefixes and postfixes are used to modify method behavior before or after the original code.
- **Example Patch:**

csharp
[HarmonyPatch(typeof(FactionDialogMaker), "FactionDialogFor")]
public static class FactionDialogMaker_FactionDialogFor
{
    public static void Postfix(Faction faction, ref Dialog_Option[] __result)
    {
        // Add custom options to the faction dialog
        var newOption = new Dialog_Option("Request Loan", () =>
        {
            // Loan request logic
        });
        List<Dialog_Option> options = new List<Dialog_Option>(__result);
        options.Add(newOption);
        __result = options.ToArray();
    }
}


## Suggestions for Copilot

1. **Code Completion:** Use Copilot to complete methods, classes, and XML elements based on the existing codebase.
2. **Harmony Patches:** Generate Harmony patches for existing RimWorld methods using Copilot.
3. **XML Definitions:** Create new XML definitions for new game elements, such as new item categories or incidents.
4. **Error Handling:** Implement error handling in C# code to manage potential issues, especially during debt repayment or bond transactions.

## Contributing Guidelines

- **Testing:** Ensure your changes do not break existing functionality. Use RimSort to manage mod dependencies.
- **Documentation:** Update the README or create new documentation if you add new features.
- **Reporting Issues:** Report errors using the Discord channel for support, providing logs using the Log Uploader.

Feel free to modify and expand the mod. Your contributions are highly appreciated!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

