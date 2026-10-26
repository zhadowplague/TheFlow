---
title: Structured prompting
lead: Implemented and documented by AI, for AI
published: 2026-10-26
tags: [AI]
authors:
    - name: "CodeBin"
      gitHubUserName: "zhadowplague"
---


Using AI on an existing codebase with no documentation is not a trivial task.
You have to pray that the AI understands the code correctly when it reads it and that changes are in accordance with your coding standards.
In other words, a lot of manual code review is required. A colleague tipped me off about https://www.aihero.dev/, and after testing it, I found that it works rather well.
You still have to verify that the AI follows the process, but the result is a lot more upfront decisions and better code output. Since the workflow also creates documentation for every feature written, you will (hopefully) never have to explain something twice.


Aside from just mentioning it in this post, I would like to give a short summary of the workflow here.
1. Request a new change in the repo through a prompt in a fresh session: "/grill-me-with-docs *change*"
1. Answer questions (just a yes is fine) and verify that the AI writes GLOSSARY.md (if no files are written, remind the AI to follow the process with "/wait-what")
1. Once the questioning is done, follow up in the same session with "/to-spec"
1. Then run "/to-tickets"
1. Start a fresh chat session
1. Run "/implement tickets"
