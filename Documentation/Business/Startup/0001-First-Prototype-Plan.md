# Examplium - Scion First Product Prototype Plan

<br/>

## Objective

Create a simple prototype solution to use as the base for product development.

<br/>

## Key outcomes

A test web app with log in/out, sign up, and add, edit, and delete notes.

<br/>

## Status

In progress.

<br/>

## Problem statement

There is no sample product to work with yet.
Start with adding features for notes, as they are universally useful, and most likely have properties that can be reused for other features in the future. 

<br/>

## Scope

### Must have

- Accounts, with login, logout, sign up.
- List notes for current user.
- Add note for current user.
- Edit note for currrent user.
- Delete note for current user.
- Local database for persistence.
- Form data validation.
- Unit tests

<br/>

### Nice to have

- Confirm email address for account sign-up.
- Password reset.
- Allow users to edit their own account information.
- Allow users to delete their own account.
- Import/Export account and note data for quick set up and/or reset of sample data in the future. This could also be useful when GDPR compliance functions have to be added.

<br/>

### Not in scope
- Release ready code. This is just for internal testing, it just needs to run on a local machine.
  - Design. No need to update menus, layout, and other design elements yet.
  - Privacy, GDPR features. It is only for setting up the basic skeleton of the app and will only be used for tests.
  - DevOps, infrastructure, it just needs to run on a local machine with a local database.
- Analytics. 
- Mobile apps. A mobile solution can be added when the APIs are better understood and clearly defined. Also authentication/authorization for mobile apps need to be implemented first.
- Data backup/recovery. Only fake test data will be used.

<br/>

## Milestones

Create solution in Visual Studio, remove elements from the template that are not needed. In progress.

Add/update email service(s) for user account features. Not started.

Add/Update server API for user account operations. Not started.

Add/Update client pages for user accounts. Not started.

Add/Update server API for note operations. Not started.

Add/Update client pages for notes. Not started.


<br/>

## Reference materials

This document was inspired by this Atlassian Confluence Template: [Project Plan Template](https://www.atlassian.com/software/confluence/templates/project-plan)
