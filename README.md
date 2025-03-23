# Checkers AI 💻 
This is a project is a monte-carlo search tree based checkers AI made for my CMP304 module at Abertay uni. I had a great time working on this project 
(this was actually one of my favourite modules) and I got to explore an area of programming I had been interested in for quite a while. A project I
have always been interested in is making a chess engine which is capable of beating me at chess (I maintain a solid 1800+ rating on chess.com), and
this program makes use of a lot of core principles and logic which I could apply to chess if I chose.

## The Development Process 🛠 

### 🐣 Start At The Start
Looking at what I wanted to create for this project, I very quickly noticed the immediate problem. I wanted to make a checkers AI, but I had no checkers.
My starting point was clear. In my head, this project would take place in two parts: the making of checkers, and the creation of the AI. I thought I could
power on ahead making checkers immediately with relatively little thought for the future, and it would all come together in the end. As I will explain 
shortly, this line of thought was deeply flawed, but some lessons have to be experienced to be learned. For now I was happily on my way to making a quick 
version of checkers, following along with the first [tutorial](https://www.youtube.com/watch?v=-0vg5gopetE) I found on youtube. Using a tutorial to make 
the game was actually advised by the uni, as this was not the main focus of the project, and this part of my development went relatively quickly. Little 
did I know, I had already made my first major mistake of the project...

### 🤯 It All Goes Horribly Wrong
So, I have a version of checkers, and I want to add an AI to it. What could the problem possibly be, you may ask? Well, here's the issue. I had made a
game of checkers where the data and UI were so tightly integrated together that it was impossible to separate the two. Through blindly following a tutorial
(which did a perfectly fine job of showing how to make a game of checkers) without considering my own use case, I had created something which actually
didn't help me at all. I tried to think about ways around this, but for an AI like mine being able to simulate moves and games without changing UI elements
was absolutely essential, and there was no avoiding that fact. After a while I was forced to accept that there was only one thing left to do. I had to 
take the lessons I had learned, and start again.

### ♻ Start At The Start, With A Plan
At this point in the project I was back to square one, but having been through this before I had a much clearer idea in my head of how I wanted to architect
the whole program. I wanted the board to be stored separately, and have other parts of the code simply look in from the outside. I wanted the AI to be able 
to have it's own instances of the board, which it could run its own fun little game on off in it's own world, and this wouldn't affect the UI as the UI 
would be an outsider looking in only at the one board it cared to see (the one the main game was played on). Armed with this knowledge, and enough independant
thought to not even want a tutorial, I set to work making my new checkers program. The lesson of separating the UI from the data stuck with me after this
project, and was something that would stick with me moving forward into many of my future projects, and I think this experience taught me a valuable lesson
about planning the whole project from the start properly.

For the sake of this write up, I will skip over the making of checkers. I am working in unity, and it honestly wasn't that hard. there was a bug here and a 
bug there, but I got them working in the end. Now, onto the AI!

### 👾 The AI Revolution
At this stage in the project, we have a fully working version of checkers. It is capable of limiting players to legal moves, moving pieces, allowing for taking
and all the other basic things you would want from a checkers program. Now, lets get the silly stuff out the way. Checkers is a solved game. You can play it
from a lookup table.

Now let's be honest. It may be effective, but that's not what we're here for.

Neural network based AI was another alternative I looked in to. These AI can be remarkably good at board games like checkers, but the trade off is often in the 
fine control over difficulty and behaviour. In my [project presentation](https://docs.google.com/presentation/d/1HZwHD338rQBVY-RWUqO-Pr8NIWZQCU44g4i5naiNzSc/edit?usp=sharing), 
I talk about how I could have chosen to make use a deep reinforcement learning algorithm. They are both perfectly viable solutions, but two things really made 
my mind up on this matter. I am working to a deadline for my uni project, and it just isn't where my passions lie. For the deadline, I am sure I could have made
the deep reinforcement learning work. However, I have a more fundamental understanding of the principles behind more conventional AI techniques - something that
makes me more cofidentin my ability to make them work to a deadline. But the real kicker is that I just truly don't enjoy working with machine learning the same 
way I do conventionalAI logic. I've tried it out, and it's fine - not the worst thing in the world. But I didn't become a programmer to automate the problem 
solving phase. It's what I fell in love with coding for, and I want to understand everything my code is doing and why it's so cool and what all the clever little 
optimisations are.

With my decision narrowed down to conventional techniques I decided (as you will know if you looked at the presentation earlier) to make a monte-carlo search tree
based algorithm. I had an interest in these algorithms from my time looking into chess AI, and was aware of their use in other board games like scrabble as well.
The technique was fit for my purpose (a two player perfect information game is where this technique thrives), and seemed like a very reasonable scope for this uni 
project. There was even plenty of room for further optimisation if I got my work done and fancied a fun little project for a higher grade.

### 💾 Finally Making An AI
This write up has dragged on a bit, but as I learned from this project when you start with nothing there is actually quite a lot of thinking to do before you make
an AI. However, at this stage, with the plans completed, it was time to make the program. The monte carlo search tree can be broken down into 4 phases: selection,
expansion, simulation and backpropegation. My initial approach to this project was to make each phase as simple as possible, and adding optimisations later on to
measure improvement and gain and understanding of how to tune to skill level of the AI.

#### 🤔 Selection
My selection stage was intially set up to be a random choice of path down to a leaf node of the tree. At each node, I simply selected a random child to traverse to
from the available children. This inital approach, however, does create a bug where because the program selects only one path down the tree. The program always 
traverses to a leaf, and therefore it wont select a new move that hasn't been played yet from a node that already has one possible option explored. However, at this 
early stage in development I unfortunately did not catch this issue, so be aware that it is present for now. Fundamentally selection should stop at a node which 
is not fully expanded, rather than a leaf node.

#### 🧩 Expansion
The initial expansion algorithm also worked through random values, although this time there is no fundamental logical issue with that. We pick a random legal move, 
and expand our move tree in the AI to include it. This move tree works to store the calculations and scores of the moves the AI is considering playing, and to keep 
track of what paths have been fully explored already.

#### 👨‍💻 Simluation
My simulation step was also designed to be as simple as I could imagine. The purposes of simulation is to create a score that can be used in backpropegation to 
add or take from the potential value of the move our AI is considering playing. For this purposes, the AI would simulate a full game all the way to an end state
through purely random moves (you may be seeing a pattern in my interpretation of simplicity here).

#### 🔙 Backpropegation
Backpropegation is the process of moving back up the tree and adjusting the scores of all the nodes up to the root, keeping track of the culmulative score of all
the games simulated from each point. For my simple implementation, I gave a score of 1 for a win, 0 for a draw and -1 for a loss.

There were some bugs with the initial implementation of these (a fun one was simulating games on the main board due to confusion about how C# handles passing by 
value vs by reference, causing the board to immediate jump to a finished state where the AI decided the winner of the game after the first move), but after some
work the simple version of the AI was completed and put into the unity project.

### 📈 Upgrading My Boy
Now that the basic version of the AI was made, I had some time remaining before this project was to be handed in. I wanted to look a bit deeper into upgrading my
AI and making him a bit better at checkers. Due to it's simple nature (and that pesky bug in the selection phase), it is actually quite terrible at the game I'm
asking it to play. After watching the AI play, I realised it was at its worst in the endgames and when captures were available on the board. Since I had no idea
about the bug in selection, my initial idea was to add a caviat to the simulation phase. The idea was to make the AI always favour captures when available to it,
on the basis that generally speaking this would more often than not be a better move.

This change didn't work as I expected. Likely because of the bug in selection, the AI still sucked at everything and there was no noticeable improvement or change
in its moves. However, I found that moving this change to the expansion phase did cause a positive impact. Little did I know, this was because I was not expanding
to the right moves because of the bug in selection, but this proved to be a valuable piece of evidence that let me work out what was happening. 

The last change to the project, which actually happened after the hand in date of the project while I was working on it in my own time, was finally finding and
fixing that bug in selection. At long last, it was finally spotted and solved.

### 🏁 Reflections And Takeaways
My number one takeaway from this project was debugging. It is *ABSOLUTELY ESSENTIAL* to not allow any parts of complex programs like this to act as a black box 
where I don't know whats happening. That selection bug could have been spotted far earlier if I had created strong debugging tools for myself and looked at 
exactly what the algorithm was doing. Furthermore, I realised this was actually one of my favourite modules up to this point in my course. Partly for the freedom 
it offered me to explore existing interests, and partly because I genuinely enjoyed the problem solving process creating an AI like this offered. I actually had
a lot more ideas I wish to explore at the time of this write up. I am interested in changing my scoring system to be based off how many pieces are remaining on 
each side as a way to account for how dominant a specific win is, and to add more features in the simulation phase to weight towards good moves (eg rather than
always playing a capture, just weight more heavily towards it and keep a chance of not playing it).

All in all I had a great time working on this project and learned about C# language features, AI algorithms and good debugging practices.
