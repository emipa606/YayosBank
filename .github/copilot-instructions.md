# GitHub Copilot Instructions for Yayo's Bank (Continued)

## Mod Overview and Purpose
Yayo's Bank (Continued) is an economic-themed mod for RimWorld, originally created by YAYO and updated by Amnabis. The mod introduces complex economic systems where players can engage in financial activities with in-game factions, such as loans and war bonds. The mod is designed to add depth to the economic interactions within the game, offering players unique challenges and opportunities involving faction relations and financial management.

## Key Features and Systems
- **Loan System**: Players can take out loans from factions. Failure to repay loans with interest may result in hostile actions from the lender, including creditor raids.
  
- **War Bonds**: Factions sell war bonds, whose prices fluctuate over time. Bonds can be used to request military aid or held onto for item rewards. Players can benefit from economic incidents affecting bond prices.

- **Economic Incidents**:
  - Loan increases due to interest
  - Seizure bankruptcy
  - Creditor raids
  - Dividend arrivals, represented by items in drop pods every quarter
  - Bond price changes influenced by quest success

- **Bonds Utilization**: Use bonds to receive quarterly gifts from factions or request military aid. Bonds can be sold at a profit when their price rises.

## Coding Patterns and Conventions
- **C# Structure**: The mod follows typical C# object-oriented design principles using classes and methods to encapsulate functionality.
- **Naming Conventions**: CamelCase is used for public class names and methods. Internal classes and methods use a similar naming convention with appropriate access modifiers.
  
- **File Organization**: Source code is organized into specific files for different functionalities such as faction dialogues, harmony patches, quest handling, etc.

## XML Integration
- XML is used for defining the mod's static data, such as item definitions, events, and interactions with RimWorld's core systems. Ensure proper syntax and adherence to RimWorld’s expected data structure for seamless integration.

## Harmony Patching
- **Harmony Patches**: Utilized in `Harmony_SomeNamespace.cs`, this mod employs Harmony for patching existing game methods to inject additional functionality or modify behaviors at runtime.
- **Method Prefix/Postfix**: Use prefixes to inject logic before a method executes, and postfixes to modify behavior after a method runs.

## Suggestions for Copilot
1. **Automatic Naming Suggestions**: When declaring new classes or methods, Copilot can suggest meaningful names based on their roles within the mod.
2. **XML Template Generation**: Suggest XML templates based on defined C# objects to ensure seamless data integration.
3. **Code Completion**: Assist with code completion while writing complex economic logic or implementing harmony patches.
4. **Error Prevention**: Provide suggestions to prevent common errors in RimWorld modding, such as handling null references or incorrect XML schema usage.
5. **Documentation Enhancements**: Encourage inline comments and summaries for public methods to maintain comprehensive documentation.

## Author's Note
Feel free to modify and expand upon this mod. The source code is included for developers to experiment with and create new variations. If you're interested in contributing or discussing ideas, you’re encouraged to reach out via the RimWorld Discord.

## Support and Troubleshooting
- Use the Log Uploader for error logs.
- For support, utilize the designated Discord channel rather than GitHub discussions for timely assistance.
- If you discover a solution to a problem, contribute by posting it to the GitHub repository.
- Organize your mods with RimSort for optimal performance and compatibility. 

Tags: cash, money, stock, stonk, economics
