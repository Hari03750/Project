# EventEase

A Blazor Server app for browsing events, registering attendees, and tracking check-ins on event day.

## Features

- **Event Card component** (`Shared/EventCard.razor`) — reusable card with two-way bound `IsRegistered` state, so toggling registration on the card updates both the parent page and the shared session state.
- **Routing** — `@page` routes for Home (`/`), Events (`/events`), Registration (`/register/{EventId:int}`), and Attendance Tracker (`/attendance`), plus a custom `NotFound` fallback in `App.razor` for invalid routes.
- **Input validation** — `AttendeeModel` uses `DataAnnotations` (`[Required]`, `[EmailAddress]`, `[Phone]`, `[Range]`), rendered via `EditForm` + `DataAnnotationsValidator` + `ValidationSummary` on the Registration page.
- **State management** — `UserSessionService` (scoped, per-user "session") tracks login name and which events the user has registered for; `EventService` (singleton) holds the shared event catalog so capacity counts stay consistent across users.
- **Attendance Tracker** — `AttendanceTracker.razor` lists attendees per event with a check-in toggle and a live checked-in count.

## Project structure

```
EventEaseApp/
├── Models/            EventModel.cs, AttendeeModel.cs
├── Services/          UserSessionService.cs, EventService.cs, AttendanceService.cs
├── Shared/            MainLayout.razor, NavMenu.razor, EventCard.razor, SessionBadge.razor
├── Pages/             Index.razor, Events.razor, Register.razor, AttendanceTracker.razor, Error.cshtml, _Host.cshtml
├── wwwroot/css/       site.css
├── App.razor
├── Program.cs
└── EventEaseApp.csproj
```

## Running locally

```bash
dotnet restore
dotnet run
```

Then open the URL shown in the console (e.g. `https://localhost:5001`).

---

## How Copilot assisted throughout development

*(Adapt the notes below to reflect your own Copilot conversations from Activities 1–3 — this is a template summarizing the kind of assistance Copilot typically provides at each stage.)*

**1. Generating the foundational Event Card component (Activity 1)**
Copilot was prompted to scaffold a Blazor component representing a single event, with fields for name, description, date, location, and capacity. It suggested using `[Parameter]` properties for the event data and an `EventCallback<T>` pair (`IsRegistered` / `IsRegisteredChanged`) to implement two-way binding cleanly, following Blazor's `@bind-Value`/`@bind-Value:event` convention rather than manually wiring events. This saved time versus writing the binding boilerplate from scratch and caught an early mistake where the callback wasn't being invoked, so the parent page never learned about registration changes.

**2. Debugging and implementing routing (Activity 2)**
When the Registration page initially threw a `NullReferenceException` for unknown event IDs, Copilot helped diagnose that `OnInitialized` ran once and didn't respond to route parameter changes, and suggested moving the lookup into `OnParametersSet`. It also proposed adding a custom `<NotFound>` template in `App.razor` so invalid URLs show a friendly message instead of a blank page or crash — turning a routing bug into a proper routing-error-handling pattern.

**3. Performance and validation optimization (Activity 2)**
Copilot flagged that fetching the full event list on every render was wasteful and recommended loading it once in `OnInitialized` (and using a singleton `EventService` so shared counts don't get re-fetched per card). For validation, it suggested `DataAnnotations` attributes (`[Required]`, `[EmailAddress]`, `[Phone]`, `[Range]`) paired with `EditForm`/`DataAnnotationsValidator` instead of manual if/else checks, which reduced code and gave consistent, built-in error messages. It also pointed out a capacity race condition — a user could register into an already-full event between page load and submit — and suggested a defensive re-check in `HandleValidSubmit`.

**4. Advanced features: Registration Form, session state, Attendance Tracker (Activity 3)**
Copilot helped design `UserSessionService` as a scoped service with a change-notification event (`OnChange`) so any component (like the `SessionBadge`) can react when login state changes, following the standard Blazor state-container pattern. For the Attendance Tracker, it suggested keeping a separate `AttendanceService` with a `ToggleCheckIn` method and a computed checked-in count, keeping the UI components free of business logic. It also proposed the guest-count `[Range]` validation and the "Event Full" badge/disabled-checkbox pattern on the Event Card to prevent over-registration at the UI level as well as the service level.

**Overall**, Copilot was most useful for (a) suggesting idiomatic Blazor patterns — parameter/callback pairs for two-way binding, `OnParametersSet` for route-parameter-driven data loading, and scoped services for session state — and (b) catching edge cases (invalid routes, full-capacity races, re-render inefficiencies) that weren't obvious until they were pointed out. Manual review was still needed to make sure suggested code matched the app's actual service registrations and to simplify a few over-engineered suggestions (e.g. an initially proposed `IStateContainer` interface was dropped in favor of a plain scoped class, since the app didn't need that level of abstraction).
