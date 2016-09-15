TNTP Coding Exercise
===================

You've been selected as a candidate for the Software Developer position at TNTP. Great! This project is intended to see how you work in a fashion resembling real development. 

You'll be tasked with taking the instructions in this project (including a [user story](https://www.mountaingoatsoftware.com/agile/user-stories), [Behavior-Driven-Design scenarios](https://dannorth.net/introducing-bdd/), and some helpful pointers), and creating a working peice of software. We'll then review your submission for coding style, best practices, and accuracy in terms of meeting the requirements.

The project: "Simple Twitter", a small microblogging solution that will allow people to post a name, and a comment, and see other comments that other people have posted. Things that you'll need to include are a database (or other effective data storage mechanism) to house the comments and user names, a web-based front-end, and some validation to make sure garbage isn't making it into the data storage layer.

Let's get started!

## Story

```
As a user,
I would like to post comments that other people can see, and see comments that other people have posted
So that I can feel connected in this crazy disconnected world
```

## Scenarios
```
Given a user who has found the site
When that user wants to post a comment
Then that user should be able to supply their name, and a comment
```
```
Given a user who is posting a comment
When that user does not supply a name
Then they should not be able to post their comment
```
```
Given a user who is posting a comment
When that user attempts to exceed a limit of 140 characters in their comment
Then that user should not be able to post their comment
```
```
Given a user
When that user is viewing comments
Then they should see all of the comments in the system, in reverse chronological order (newer comments first)
```

## Technical Restrictions

- The project *must* be written in either C#/.NET (if so, please use .NET Framework version 4.5+, ASP.NET MVC 5 or above, or .NET Core), or NodeJS (or, a combination of the two is acceptable; this **is** a requirement).
- We recommend using SQL Server, PostgreSQL, or MongoDB (all software you'll be working with at TNTP) for your back-end (this is **not** a requirement)
- We highly encourage you to explore using cloud services such as Amazon Web Services and Microsoft Azure as part of your solution (this is **not** a requirement)
- Please include unit tests that cover the scenarios listed above (this **is** a requirement)

## Instructions

- If you don't already have one, [create an account](https://github.com/join) on Github, and [fork this project](https://help.github.com/articles/fork-a-repo/)
- Install a Git client ([Github provides one that's simple and intuitive](https://desktop.github.com/)) and [clone your fork](https://help.github.com/articles/cloning-a-repository/) of this project
- Create a branch
- Develop your solution
- When finished, ensure that your solution is in the cloned repository, and push your changes to your fork of the repository
- Do a [pull request](https://help.github.com/articles/about-pull-requests/) 

Feel free to reach out at any time to your contact at TNTP with any questions. **Good luck!**
