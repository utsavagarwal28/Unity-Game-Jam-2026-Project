# Project Instructions for OpenCode

## Project
- Unity project: Unity Game Jam 2026 Project
- Unity version: 6000.3.19f1
- Target platform: WebGL Desktop
- Input: Unity New Input System
- Project root: C:\Projects\Personal Projects\Unity Game Jam 2026 Project

## Collaboration Workflow
- ChatGPT is responsible for planning, technical decisions, reviewing changes, and coordinating Git.
- OpenCode is responsible for inspecting and implementing the approved task.
- Do not independently expand the task scope.
- Prefer simple, working implementations over premature abstractions.
- Do not create a Manager class for every system.
- Reuse existing project architecture and conventions where reasonable.

## Protect Existing Work
- Before making changes, inspect `git status` and the current diff.
- Identify and preserve all pre-existing modified and untracked files.
- Do not overwrite, delete, reset, stash, clean, or otherwise alter unrelated user work.
- Do not assume untracked files are disposable.
- Clearly distinguish pre-existing changes from changes made for the current task.

## Git Restrictions
- Never run `git add .` or `git add -A`.
- Do not stage, commit, push, merge, rebase, reset, or clean.
- Do not create or switch branches unless explicitly instructed.
- Do not modify Git history.
- Leave all implementation changes in the working tree for review.
- Report the exact files created, modified, or deleted.

## Unity Development
- Use the New Input System. Do not use the legacy `UnityEngine.Input` API.
- Follow the project's existing Unity and C# conventions.
- Preserve existing scene references, serialized fields, and Inspector assignments.
- Avoid unnecessary package changes.
- Do not modify ProjectSettings or package versions unless the task requires it.
- Consider WebGL browser limitations and test platform-specific code carefully.
- Do not claim something has been tested in a browser unless it actually has.

## Implementation Process
1. Inspect relevant files and existing architecture.
2. Report the proposed implementation briefly before making substantial changes.
3. Implement only the approved scope.
4. Check for compilation errors and obvious integration issues.
5. Review the final diff.
6. Report changed files, Inspector setup, testing performed, limitations, and anything requiring manual action.

## Important
If a requirement conflicts with existing project code or cannot be implemented reliably, explain the issue before making a risky change. Do not silently substitute a different behaviour.
