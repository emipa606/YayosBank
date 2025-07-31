# GitHub Copilot Instructions for RimWorld Modding Project

## Mod Overview and Purpose
This mod, presumably named "RimStocks," enhances the trading and economic aspects of RimWorld by introducing new systems for managing factions, tradeables, and price data. Its primary purpose is to provide an in-depth economic and trading system, allowing players to engage with more dynamic and realistic trade mechanics. The mod offers various features such as faction-specific price data management, customized dialog interfaces for factions, and detailed graphs of economic trends.

## Key Features and Systems
- **Faction Dialogs**: Custom interfaces for interacting with factions via `FactionDialogMaker_FactionDialogFor` class.
- **Price Management**: Real-time tracking and management of faction-specific price data using `FactionPriceData` and `FactionData` classes.
- **Graphical Representations**: Display economic trends and patterns through custom graphs using `CustomGraphGroup` class.
- **Quest Integration**: Modify quest outcomes and dynamics through classes like `Quest_End`.
- **Stack Management**: Enhanced item stacking logic via `Thing_TryAbsorbStack` class.
- **Trade System Enhancement**: Improve item trading mechanics by using `Tradeable_InitPriceDataIfNeeded`.

## Coding Patterns and Conventions
- **Class Naming**: Classes generally follow PascalCase, clearly indicating their purpose. For example, `FactionDialogMaker_FactionDialogFor` and `WorldComponent_PriceSaveLoad`.
- **Access Modifiers**: Public, internal, and static modifiers are used judiciously to control the accessibility and lifecycle of classes and methods.
- **Inheritance and Implementation**: Implementations of interfaces, such as `IExposable`, suggest a focus on data serialization and exposure.

## XML Integration
- The mod does not explicitly outline XML files, but integration with RimWorld's XML-driven system can be inferred. XML likely plays a role in defining game assets like items or factions. Ensure XML files are correctly placed within the mod's `Defs` folder structure and maintain clear tag and attribute naming.

## Harmony Patching
- **Instrumenting Def Generation**: The `harmonyPatch_core` class contains Harmony patches, particularly targeting methods like `DefGenerator_GenerateImpliedDefs_PreResolve`, to override or enhance game functionality.
- **Namespace-Based Patching**: Organize Harmony patches by functionality within namespaces, as seen in `Harmony_SomeNamespace`.

## Suggestions for Copilot
1. **Template Code Generation**: Utilize Copilot to generate scaffolding for new classes or methods, maintaining consistency with existing patterns. For example, new `MapComponent` subclasses should mirror the constructor pattern found in `Core(Map map) : MapComponent(map)`.

2. **Repeated Patterns**: For repetitive coding tasks such as implementing `IExposable`, Copilot can be leveraged to auto-generate boilerplate code ensuring consistent serialization logic mimicking the existing pattern in `FactionPriceData`.

3. **Harmony Patch Creation**: Encourage Copilot to assist in creating new Harmony patches by providing it with examples from `harmonyPatch_core`.

4. **Graphing Logic**: When developing new graphical elements, utilize Copilot for drawing logic that is similar in complexity to `CustomGraphGroup.DrawGraph`.

5. **Event Handling**: Facilitate Copilot to assist in writing event-driven code by drawing on similar handlers or setups, promoting uniformity across the mod.

6. **Code Documentation**: Use Copilot to draft XML-documentation comments for public APIs to improve code readability and maintainability.

By following these guidelines and utilizing GitHub Copilot effectively, developers can maintain a high level of consistency, efficiency, and quality throughout the modding project.
