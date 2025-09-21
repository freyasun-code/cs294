# Five Cuts Challenge

## Project Description
This project is a Unity AR mini-game called **Five Cuts Challenge**.  
The main idea is to simulate cutting a gold bar five times in a row. The yellow cube in the scene represents the gold bar, and a fast-moving knife moves back and forth beneath it.  

- Each time the player clicks, the knife cuts the gold bar.  
- The longer piece is discarded, and the shorter piece remains.  
- If the player successfully cuts the gold bar five times in a row, they win the challenge.  
- If the player misses even once, the challenge fails.  
- A text element above the bar records how many cuts have been made.  

---

## Project Structure
- **Cube (Gold Bar)** → controlled by `BreadController.cs`  
- **Knife** → controlled by `KnifeController.cs`  
- **Game Manager** → overall logic handled by `GameManager.cs`  

---
