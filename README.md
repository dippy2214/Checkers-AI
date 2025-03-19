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

### 👨‍💻 The AI Revolution
At this stage in the project, we have a fully working version of checkers. It is capable of limiting players to legal moves, moving pieces, allowing for taking
and all the other basic things you would want from a checkers program. Now, lets get the silly stuff out the way. Checkers is a solved game. You can play it
from a lookup table.

Now let's be honest. It may be effective, but that's not what we're here for.

Neural network based AI was another alternative I looked in to. These AI can be remarkably good at board games like checkers. In my 
[project presentation](https://docs.google.com/presentation/d/1HZwHD338rQBVY-RWUqO-Pr8NIWZQCU44g4i5naiNzSc/edit?usp=sharing), I talk about how I could have 
chosen to make use a deep reinforcement learning algorithm. They are both perfectly viable solutions, but two things really made my mind up on this matter. I 
am working to a deadline for my uni project, and it just isn't where my passions lie. For the deadline, I am sure I could have made the deep reinforcement 
learning work. However, I have a more fundamental understanding of the principles behind more conventional AI techniques - something that makes me more cofident
in my ability to make them work to a deadline. But the real kicker is that I just truly don't enjoy working with machine learning the same way I do conventional
AI logic. I've tried it out, and it's fine - not the worst thing in the world. But I didn't become a programmer to automate the problem solving phase. It's what 
I fell in love with coding for, and I want to understand everything my code is doing and why it's so cool and what all the clever little optimisations are.

With my decision narrowed down to conventional techniques I decided (as you will know if you looked at the presentation earlier) to make a monte-carlo search tree
based algorithm. I had an interest in these algorithms from my time looking into chess AI, and was aware of their use in other board games like scrabble as well.
The technique was fit for my purpose (a two player perfect information game is where this technique thrives), and seemed like a very reasonable scope for this uni 
project. There was even plenty of room for further optimisation if I got my work done and fancied a fun little project for a higher grade.

### 💾 Finally Making An AI


