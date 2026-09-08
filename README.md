# Unity-Slot-Game-Assignment
Unity Slot Game Assignment for Underpin Services

# Game Overview

## Features
- Reel spinning animation handled via script.
- Win/lose animations for the slot machine.
- Popups for out of money and for payout.
- Two separate betting buttons - one for 100 and one for 500 (in-game currency).

## Win Situations:
### Triple match payout:
  1. Cherry = 2 * BetAmount
  2. Bar = 3 * BetAmount
  3. Bell = 5 * BetAmount
  4. Seven = 10 * BetAmount
### Pair match payout:
  1. Cherry = BetAmount / 5
  2. Bar = BetAmount / 3
  3. Bell = BetAmount / 2
  4. Seven = BetAmount

## Lose Situations:
### No match:
Payout amount = 0

# Thought Process and Approach
- Each reel has 4 main sprites, plus one copy appended above and two copies appended below the main list. This is needed for the wrap-around to feel like continuous spinning - without it, the slots just off-screen above or below the current one wouldn't be visible.
- Everything is decoupled by using channel-based ScriptableObject events.
- Each reel goes through phases - accelerating, constant spin at max speed, decelerating, then snapping to the target slot at a very small speed until it reaches the exact Y position.
- Enum-based approach to convert a stopped slot's position into a usable number.

# How to run the WebGL build (requires Python)
1. Clone this repo.
2. Open the Build/WebGL folder in a command prompt.
3. Run the command for your OS:
   a. Windows: `python -m http.server 8000`
   b. Linux/Mac: `python3 -m http.server 8000`
4. Open http://localhost:8000 in your browser.
5. Enjoy playing!
