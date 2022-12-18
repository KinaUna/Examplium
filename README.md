# Examplium

## TLDR:

An open-source, free to use playground for learning by doing, learning by copying, and learning by experimentation, as well as inspiration, to better understand the practical aspects of starting, running, and working in a company (software, for now).

A reference project and product to use as a foundation for integration of work-related learning.

This is a fairly long and convoluted explanation as I don’t know how to present the ideas more succinct, yet. Sorry, but not sorry. It’s incomplete and could use some refinement, but like launching a startup or new product, getting something out there can be more useful than waiting for perfection.

<br />

## What the what?

Examplium is a project intended to provide an example for learning about working with (software) product development, using a realistic product as the starting point. One might imagine it like WordPress, Umbraco, or even Linux for work life.

The example product may not be profitable, or even viable, but it will be a functioning product none the less, and it should serve as a sample to use as a starting point for learning by doing, experimentation, and exploration, to help you get a better grasp of what work life entails.

The initial product concept is based on my first startup idea, KinaUna. An app I created to collect information for and about my daughter, to keep a record for her and to share some things with the extended family.

The first milestone is to build a minimal virtual company, like a one-person startup, and then steadily add features of bigger and bigger organizations as complexity increases. Seeing is believing, having something tangible to work with early on will make it a lot easier to explain what it is, and what it could become.

I will begin with building it my way, but others should be able to extend it, or copy the concept and rebuild it in a way that fits them better. For example, my preferred platform for app development is currently .Net, and I want to learn more about Blazor, so I will use that. But anyone with the appropriate skills can clone the project and then replace the code part of the project with Python, PHP, Node.js, or whatever framework suits them best. Similarly, if you prefer a different approach to documentation, collaboration, planning, and so on, you should be able to replace that too.

<br />

## Wherefore

One idea led to another, which led to another idea, and another… 

I was looking at jobs for software developers, contemplating what skills do I need and how I should learn to improve them to become a more likely candidate for the positions that I found interesting. It seemed like good idea to have repository to show some code, and I should somehow document what I learn, and what if I combined that, while doing something that potentially supports, promotes, and overlaps with my existing projects.

While there are thousands of excellent resources for learning how to write code, the primary function of a developer, finding resources to learn about the secondary and peripheral functions seems to lead nowhere. Or worse, tutorial hell. I imagine it is similar for other professions. 

There are a lot of tutorials, books, blogs, courses, and open-source projects about how to write code, how to improve it, but after learning the fundamentals it seems like you reach a desert in the landscape of learning resources. Sure, there are little oases of knowledge, patches of information, but most of the resources circle back to areas you already discovered. You get a different perspective, more knowledge about some area, but you do not learn more about its place in the world.

I get the need for repetition and digging deeper and deeper into the details of a subject to become more proficient, but there still seems to be very little help available to bridge gap between studying and producing something within an organization.

While some learning resources are good at explaining the concepts, theories, frameworks, and processes, it often seems like learning to ride a bike by learning about the laws of physics (I think it was Robert C. Martin who wrote about this in one of the Clean Coding books).

When starting out a new career it is difficult to imagine how a job is done, apart from the obvious core functions. You look at the job openings in your field and most of them ask for years of experience and often some list of skills that you are vaguely familiar with. Like most people who can drive a car probably have very limited knowledge of what professional driver does on a day-to-day basis. An experience requirement in itself can obviously only be met by doing real work, but what if you could obtain some of that knowledge and insight earlier? How about the other things, the “why don’t they teach you this at school” parts, can you get some kind of initial comprehension for those too?

As a self-taught ... well, everything, I never went beyond high school level, I am not sure if some of this is covered in various courses at higher education levels, but from what I see in various online courses, portfolios of newly graduated people, career discussions on the web, and from job postings, I can only assume it is rare and/or not covered enough to be useful.

As a side note, it could also be useful to discuss in more detail why experience is in such high demand. What is it employers expect to get from experienced employees that they cannot get from the inexperienced? But I will leave that discussion for another time and place, but the project here will hopefully implicitly answer some of that too.

<br />

Problems this project could explore for job seekers (and individual learners in general):

- Without hands-on experience working on products or services before I have a job, how can I learn about and prepare for a given role?
- I don’t know what it takes to build, deliver, maintain, and support a product from start to end.
- I don’t fully understand what roles are involved in delivering a product, and how people interact and communicate.
- Why, what, and where do you document?
- Who decides what, who decides who decides?

<br />

Problems this project could explore for organizations:

- How can we improve mutual understanding between teams?
- How can we prepare our people and new hires better for new roles?
- How can we better understand the roles within our organization?
- How do we reduce misperception, miscommunication between people, teams, and hierarchies?
- How do we improve the quality of relationships, internally and externally.
- How do others do things, where can we get inspiration when we feel stuck, confused, lost?
- What concepts could be missing in our mental/business/organizational models? Where do we learn about them, and whether they can help us improve?

<br />

These are just preliminary lists of problems to consider, more will be added as they emerge.

<br />

## The missing links

Some of the problems with the examples used in learning resources, that I have learned from so far are:

Familiarity, longevity, context, extendibility, relevance.

These are overlapping a lot with the issues for job interviews, as the purpose of a lot of learning material is to improve your chances of getting a job.

<br />

**Familiarity:** Often you start learning something new there are a lot of concepts you have get familiar with first, before you can start on the actual content. 

If you want to build a home, you are already familiar with what it is, and you hopefully have experience with living in one. You know about kitchens, bathrooms, living rooms, and so on. You don’t have to introduce a new house model every time you want to learn about some housing aspect. With software and business development it seems that every new topic introduces a new model to be assimilated before you can start learning the specialized content. If you are looking at housing, you can, most of the time, use any house as a reference for learning, even if the sample house looks nothing like your house. It still has doors, windows, plumbing, heating, and so forth, and the concepts involved are shared in the minds of most people. 

Imagine if there was a project that students in marketing, business analytics, software engineering, design, finance, and others, had a common understanding of. Employers of fresh students could use this knowledge to speed up the hiring and onboarding process. Some people, both employers and applicants, go through hundreds of interviews, and both sides must contend with a lot of uncertainty and variability regarding interview content. Companies can often not share what they work on, and even if they could an applicant going through multiple interviews would quickly be overloaded with learning how each company do what they do. So, interviews can at times revolve around conceptual problems that go from so cliché and trite only a psychoanalyst on LSD would feel inspired, to so complex and stealthy conspiracy theorist would get jealous. If you are too specific you could miss out on opportunities that would be a good match for you, if one side doesn’t get your references. If you are too generic, you can’t be sure if anything discussed helps you decide. Wouldn’t it be nice if both sides could have a common model to explore together without having to do tons of homework? 

It could go something like this: A manager in the medical industry want to make a career change and has a job interview for a management role in a media company, and both parties cannot disclose any details about their work. They could then agree to look at Examplium in preparation for the interview and use that as starting point for learning about what the other party needs and has to offer. The interviewer could ask how the applicant would do some part differently in a media company, perhaps how should performance be measured. Now, even if the result isn’t a match, both can learn something that they probably haven’t thought about before, and both could use the same example for their next round of interviews.

I am doubtful that this project will fully accomplish that anytime soon, it is simply too large a scope. That doesn’t mean I can’t make baby steps in this direction, and if such a thing is possible, the work done here could be a seed for others to create solutions that partially bridge the gaps in other ways. Those solutions could inspire someone to build something that stiches together more and more disciplines into a common system.

For now, this solution will focus on providing a solution for self-learners, so they themselves can have project a that they are familiar with, to use for practicing and implementing newly learned knowledge. If it works for them, they could suggest to their instructors, teachers, mentors, that their next material could perhaps be based on this project, or at least use it as reference for explaining some of the generic concepts used in their models. For example, a teacher might say “In a real project you would have to add a GDPR policy here and get the user’s consent. This would require collaboration with the product owner/manager, the Data Protection Officer (which may have to be appointed if one doesn’t exist), and possibly a designer, to define the specifications, requirements, etc. If you want to see an example of this, take a look at Examplium’s source code, requirements, and other documentation here, here, and here...”

<br />

**Longevity:** The solutions are not relevant to my current situation, so I won't use them or update them once the initial project is done.

<br />

**Context:** The examples are isolated. I don't learn much about the interaction with different work areas.

<br />

**Extendibility:** It is hard to apply the examples when trying to learn new subjects when they all have different context, scope, and are made for different audiences. When you have years of experience you can probably relate something new to some of your earlier problems and incorporate it into your current projects, but when you are new in a field it is difficult to try out some new ideas on something that has any significance to your own situation. Without real impact on your situation, you tend to forget most of what you learn, and either newer use it again or must re-learn things again later when you have enough experience to apply it in your current work.

<br />

**Relevance:** I don’t use the example solutions myself. For example, e-Commerce applications are popular in web development tutorials, because a lot of companies need them, but I have never personally used an e-Commerce project I created while learning. I just abandon the code as soon as I finish the course/tutorial/book. Right after creating the first version of KinaUna, I started using it, and have used it every day since, which means I get a very good feeling for and understanding of the good, the bad, and the ugly sides of it, and I will always be motivated to keep making improvements when I expect to use it repeatedly myself. If you can get others to use your product too you will also learn to deal with feedback (or lack thereof) and get more practice in communicating with end users.

Also, while working in other roles, I have been feeling like banging against a wall when trying to learn about the work of other teams and departments. Mostly because I and other people are too busy and overloaded to spend time on showing someone how we do our work, but sometimes also because of silos in the organization, where each department refuses to let outsiders see what they are doing. Maybe it is some sort of “security by obscurity” mentality, if no one knows how we do things, we will be safe from criticism, blame, and finger pointing – if no one else knows how we do things we are irreplaceable, indispensable. It’s been a while since I worked in large organizations, so things have probably improved by now, at least judging by what I see Stack Overflow and GitHub users produce these days.

Another benefit of relevance is collaboration. Say you want to learn about product management, but don’t want to learn how to code. If the product is relevant to others, you have much better odds of getting help from or team up with someone who can code.

<br />

## The product

I think KinaUna can be adapted to serve other purposes, to make it more relevant for a lot of people. For example, instead of using it for a family member you could adapt several features to work for pets, cars, households, hobbies, and so on. Want to have an app for managing sports activities, keeping track of your favorite mushroom locations, your collection of something, Examplium should work well as a starting point. It will take a lot of time and effort to create everything you want, but to just get started on something that interests you should be manageable. If you can’t write code you will need to get help with that, but the existing features and documentation should be enough to help you get started on planning, creating specifications, requirements, which will make it a lot easier to get someone to help with the code, just like in a real company. If enough people learn about Examplium, odds will improve that someone who can code is interested in a similar feature and want to make it anyway.

It won’t work for everything, but I think it will be possible to use Examplium as the foundation, for something that you can use yourself, no matter what your interests are. Having a personal project for something that you are already has your devotion and dedication, to use for experimenting and trying out what you learn, can make a huge difference when learning something complex over a long time. 
For those who can code it should be straight forward how to build and deploy the app, so you can use it yourself. For non-technical people there will be a fully functional app made available that you can use too, but obviously it will only have some generic features and you won’t be able to modify it directly like some programmers. If you don’t want to deal with code and want the app to work differently, you can also submit a request for a change or new feature, or if you discover a problem, you can also submit an issue ticket, like any other open-source project, and see how that works.

<br />

## Key objectives for the project (not the product): 

**Clean:** Easy to read, and easy to modify. Like clean code, the project itself should be easy to read, and equally important it should be easy for anyone to change to their needs, to change to fix problems, and to change by adding new features. Like a clean room it should be easy to get an overview of where things belong, easy to put something new into the room in a place that makes sense, not just to yourself, but also anyone entering the room.

<br />

**Useful now and in the future:** The project should deliver a product that has practical (positive) effects on your life and can continuously be extended as you learn new subjects and have new ideas.

This also means the product delivered must be complete, that is fully functional (although all features may not be implemented yet). For learning you could then clone the solution and mess it up by adding obscure bugs or deleting entire parts of it, to see if you can fix it again.

<br />

**Track changes:** So everyone can see how things evolved. 

<br />

**Fully documented:** Everything should be well documented. 

<br />

## Integration of integration

I believe integration is a key element to work, and life in general.

Learning, creating products, organizing work all involves a lot of integration.

But integration is hard to deal with in practical terms, we all have some understanding of it and naturally integrate all sorts of things in our daily activities, yet many of us find it very challenging to put it into words, much less make practical use of it.
Much like traffic, we know what it is, we deal with it frequently, complain about it, teach our children about it, yet very few people can describe it and analyze it well enough to do anything effective with it.

Despite the challenges of wrestling with the concept of integration I think it is worth the effort to at least attempt to use it as a guiding principle for this project.

This project aims to improve integration, even though I am not sure how to do that, yet I am hopeful that it will slowly evolve over time and become clear enough to at least serve as a starting point for others to consider how to optimize for integration.  

Daniel J. Siegel's Mindsight books have the most comprehensive and practical approach to integration I have come across so far. So, I will borrow a lot of ideas and concepts from there, most of which are explained well in the "Pocket Guide to Interpersonal Neurobiology".

So, borrowing from the Mindsight concept of domains of integration this is a first attempt to dissect integration as framework for managing and organizing work. I won’t be thinking about integration at every step, and I wouldn’t expect anyone else to do so, but occasionally it will be useful to take step back and view things through the lens of integration. 

<br />

### Domains of integration

Vertical Integration: Integration from top to bottom. Integration from CEOs to interns. Integration of big picture concepts and the smallest implementation details.

<br />

Lateral Integration: Integration of and across teams, organizations, units, co-workers, colleagues.

<br />

Temporal Integration: Integration over time.

<br />

Interpersonal Integration: Integration of people, roles, jobs, tasks.

<br />

State integration: Integration of projects, working hours, workloads.

<br />

Transpirational Integration: Integration of values, goals, and objectives.

<br />

This project will not start out with elegant solutions, maybe not even good solutions, because that takes too much time, it isn’t realistic in most projects anyhow, and I probably don’t have the skills (yet). Also, Integration over time can be shown more clearly if I start by just jumping into it and building it like some of us do when we have a “great” idea for an app, with limited knowledge and experience. Focusing on making something that works first, creating a prototype or MVP, seems more reasonable. If the prototype work well enough one can iteratively improve the documentation, processes, organization, management planning, and the product itself. 

The objective is to learn the craft rather than creating something perfect or becoming a perfect something. The aim is to build habits for continuous improvement, making ourselves, our teams, and our solutions increasingly flexible, adaptive, coherent, energetic, and stable (the FACES acronym is another idea borrowed from Daniel J. Siegel and Mindsight).

The concept of integration also lends itself well to learn from other areas of life. For instance, we can ask what other areas of life we could integrate knowledge from in our work. Sport and fitness are different from work for most of us, yet like work it is something many people do and understand on a personal level. Could we perhaps create links between knowledge of physical activities and work-related activities? As I understand it, to maintain fitness you need sustainable activities that have an appropriate intensity and volume. To improve fitness, you will need to add progressive overload, that is continuously add a little intensity or volume. If you add too much intensity it will reduce sustainability and/or volume. If you add too much volume it will reduce intensity and/or sustainability too. Could the something similar apply to work? I think so, and I believe it will be worthwhile to explore this more, how can we integrate knowledge from a variety of areas of life with other areas.

<br />

## Complexity

Keep it simple, they say. 

No. But also yes.

<br />

Complexity has a bad reputation, I guess because it is often confused with complication. 
There is a lot of value in the KISS principle, but when we say “keep it simple stupid” we are too often ignoring the hidden consequences of it: Making something simpler also makes it more complex.

Maybe for simple problems we can keep it simple, but the problems that have the greatest impact on our lives tend to be complex. To solve complex problems more complex solutions are needed. Even if the problem is that something is too complex for someone to use, any solution that reduces the complexity for the user will in itself be more complex than the original problem.

To reduce complexity in some part of a system, you must increase the complexity of another part, and this often increases the overall complexity of the system. 
A modern car is simpler to operate and maintain than older cars, because of fuel efficiency calculations, low tire pressure warning, ABS, traction control, etc. But the car itself is more complex, the engineering tools used to design, develop, test, and produce the car are more complex. All in all, cars are more complex than ever, and will continue to increase in complexity. The same pattern should be evident for a lot of other product categories.

Good project plans and management deals well with complexity. A good product effectively rearranges complexity, reducing complexity for the user by adding complexity for the producer/creator of the product. I think great businesses, amazing employers, and stelar employees don’t ignore or avoid complexity, they utilize it and cultivate it. 

So, making the project part of Examplium simple won’t be helpful, it will need a healthy dose of complexity to solve anything but the most insignificant problems. On the other hand, the sample product Examplium is developing should be appropriately simple to use. Presentations and documentation are hard to get right for everyone, and the appropriate level of complexity depends on the audience, I think it makes most sense to just wait and see what the feedback from people reading them will be, and then reevaluate where, when, and for whom it is too complex or too simplistic.

<br />

## Consilience

In science, and some other fields, there is an important principle pertaining to knowledge from separate disciplines: consilience.

If some result is supported by other results, it is more likely to be valid. If a measurement agrees with other measurements, performed in different ways, the measurement is more likely to be the real deal. If some results, measurements, predictions, or outcomes don’t agree with each other, there is either something wrong with our understanding of how it should work, or there is a problem with the tools or method used. 

In business there are very few absolutes, we just don’t have the resources to analyze, theorize about, and measure everything. The scientific principle won’t work if we try to copy it directly, but I would like to think that it can still be a useful concept and like integration, it could be worthwhile to explore what consilience means for your job and your organization every now and then. 

Examplium should also aspire to adhere to the principle of consilience. Like integration, it is a difficult concept to implement in practice, I still haven’t figured that part out yet, but the idea is always floating around in my mind, that it should be possible and needs to be tried out somehow, someway.

<br />

## X-Main(First_Business_Class)

This project will have many sub-projects, you might say it is a meta-project or projectception. I will begin with the basic app idea and go straight to development as the first steps, but later the plan is to add all sorts of other roles and responsibilities as needed, and as well as I am able. Others with more expertise in any given subject can add their input and solutions as well.

So, in the spirit of inception, and to break down the complexity into smaller chunks, I thought it would be a good exercise to try to create the startup as a program. It will be very abstract and without any functional implementation, as it’s just one way to sift and sort through the crazy number of concepts filling up my head and it could potentially help in getting over creative obstructions (like writer’s block, but for project management. Is there a word for that?).

If you are a developer, could you describe your workplace in code form? Have you ever tried to simulate your work in (high level abstracted) code? It can seem like an absurdly complex exercise, but it might be a good way to discover things that could be automated, rearranged or optimized in some other way, even if you never get close to an accurate model. At least you will have to think hard about what a business is, what a product is, what a founder is, what a customer/end user is, and so on. Should you complete the code, well then you will even have a complete solution for a business as a service product.

So, for those who can read code (C# in this example, perhaps others can copy the concept and create examples in other programming languages) I have created a repository to work on here: [Business as Code](https://github.com/KinaUna/BusinessAsCode)

<br />

In plain language the first draft might boil down to this:

To run a business, I produce and sell one or more products. If I don’t have a product, I will create one. (To do: If I have a product…)

To create a product a developer will build a prototype based on the product requirements. If I don’t have a developer, I will create one (hire, find co-founder, etc.). 
If don’t have the product requirements, I will create them.

To create a developer, if I have enough money, I will hire a new developer, else I will get a co-founder who can code, and if that doesn’t work out, I will code it myself.

To create the product requirements, I will create (come up with) an idea, validate it, and create requirements for it.

To create an idea, I will...

To validate an idea, I will…

To create requirements, I will…

<br />

This is just a first attempt, and it will need to be revised (refactored, redesigned, rearchitected) and extended many times, as new requirements are discovered and my understanding of relationships (dependencies) between entities change, maybe I want to incorporate a different strategy, methodology, or approach to product development.

For example, you might want to make a Minimum Viable Product instead of, or after, the prototype. You could outsource all programming tasks. Some people would prefer to focus on market research early on and would add a lot more steps in that area. For others, privacy, social media, mental health, elimination of discrimination, scalability, safety could be a big priority, which would change the structure of the initial program/organization architecture. 

There should still be some more or less universal aspects of creating business or organization around delivering a product, and I hope I can make it generic enough to be useful for many other product and business ideas. In any case, the project can be forked at any point and developed in different directions. Maybe someone can start a branch to focus on small business problems, while another on medium sized, one on enterprise problems, yet another on government and intergovernmental problems. The real difficulty here is to somehow keep these forked branches aligned and linked in the future, so they keep adding to the universal knowledge in concert with each other. This is one area I haven’t figured out yet, but that shouldn’t prevent anyone from going ahead with creating a branch and make it work for their situation as best they can, it is just something to keep in mind and an ideal to strive for.

I suppose the ultimate goal of any project, business, product, or organization is to make the world a better place, for someone, in some way. I would also say most of the world’s problems today depends on people, so to make the world a better place we need to make better people. This is the core of KinaUna: To make the world better, make better people. To Make better people, make better parents, better families, better schools, better… … To make better X, make better tools for X. KinaUna is my first exploration into making better (software) tools for parenting and family. 

As there are a lot of tools for making better businesses, I thought perhaps I could copy some of the ideas from there and apply them to family life. I have only made tiny steps towards that end so far, it is a bigger venture than I can handle alone, so I hope another benefit of this project will be that I will get more feedback and help, that I can adopt for KinaUna, or this project could make KinaUna obsolete and spawn other tools for making better people entirely on its own.

From the rough description above I will begin my implementation of Examplium from the “To create a prototype” level, as I have the initial rough idea defined, and for learning purposes I will pretend it was validated. I should probably put on a Business Analyst hat and write the requirements first, but I personally find that fiddling around with code first helps me organize my thoughts better. Kind of like a movie director drawing sketches of scenes to get a feel for a story.

<br />

## The hard problem of problem-solving
Every job, every project, and every organization are about problem-solving. Every employer wants to hire problem-solvers, every job applicant aspires to (maybe not, but they really should) be a problem-solver, every parent wants their children to be problem-solvers. Problem-solving - big, small, narrow, wide, personal, global, and everything in between - is a common thread of everything in this project, and many other projects too.

Many people find it difficult to get a handle on problem-solving, to gain experience and level up skills in a systematic way, even though we all solve problems all the time. I suspect having several approaches to discover, analyze a problem space, and different implementations of solutions to similar problems will provide some insights into this tough problem too.

I realize it may be a futile endeavor, and I most likely won’t be able to solve many of the problems described here, and some things will only matter to a fraction of people, but this will all be open source so others can help with, copy, transform, any part or all of it if they want, or even create their own entire solutions based on the ideas used here. At least I expect to learn something, whether it is a success or failure, and I expect others can too.
Additionally, I believe improvements and increases in automation, scalability of solutions, maybe the expanse of SAAS solutions will lead to a reduction in work that relies mainly on procedural knowledge, know-how. Which means more and more jobs will favor people with good problem-solving skills. Maybe it has limited effect for the time being, but for parents, or educators, it could be important to consider what it implies for the next generation.

<br />

## What, who, when, where?

Like with KinaUna (and a lot of other ideas), the main obstacles are time and energy, especially given this is currently a hobby project. I have a family, jobs to find and apply for, a cat to entertain. It will be a long and laborious process before even the basics are in place, so we will just have to see how much time and energy I will have and/or how much others will chip in and contribute. 

The outcome will depend a lot on the help of people who know more than me. I am oblivious when it comes to finance, marketing, law, among other things. I also don’t have the resources to add specific content that could be useful to many. For example, I know that there are some popular documentation tools that would be interesting to use, but as far as I know they do not have any free options that would allow me to use their products as open-source documentation tools. The free versions should work fine for small teams, but for access to unlimited, open, fully featured collaboration features I think I would need an enterprise license.

I have no idea if the EU, UN, US-EDA, and similar have any kind of funds for supporting this kind of work, so someone could work full time on it, maybe someone out there knows a way? Perhaps some entrepreneurial talent can see a way to turn it into a profitable business? If someone can make YouTube, LinkedIn, Vimeo, or whatever tutorials or presentations inspired by Examplium, showing how something is done here, maybe how they would do it differently, it would be great.

To me it doesn’t matter if it monetized or not. There are likely many possibilities I haven’t considered, but essentially, I don’t care if someone copies my ideas and/or work from Examplium and benefits from it, as long as it makes the world a better place for me, my family, my friends, and anyone else. I would appreciate it if they do keep it as open as they possibly can though, so as many as possible can learn from it too.

Even though I would love this to be a full functioning solution tomorrow, I will be quite happy if by the time my daughter start thinking about career choices, she can use this project to explore and prepare for working life. 

Maybe it fizzles out into oblivion, maybe someone smarter and more resourceful than me picks up on this idea and takes it to the next level, whatever that may entail, maybe it just proceeds as planned. We shall see. 

Meanwhile anyone can add their voice, if you have knowledge to share you can work directly on this repository or clone it, if you don’t know anything useful you can always help by asking questions or leave a comment in the discussion area. Anyone can suggest bugs that needs to be fixed, improvements that are needed, or ask for new features to implement using the issues section. And, of course, you can always help by spreading the word to anyone you think might be interested in this.

<br />

<br />

First published 18th of December 2022 by Per Rosing von Philipsborn

Last updated 18th of December 2022.


