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
