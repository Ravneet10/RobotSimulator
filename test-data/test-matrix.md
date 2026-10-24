# Test Matrix Data

This file contains reusable test inputs and expected outputs for the Toy Robot Simulator implementation plan.

## Official assessment examples

### Example A
Input:
PLACE 0,0,NORTH
MOVE
REPORT

Expected output:
0,1,NORTH

### Example B
Input:
PLACE 0,0,NORTH
LEFT
REPORT

Expected output:
0,0,WEST

### Example C
Input:
PLACE 1,1,EAST
MOVE
MOVE
LEFT
MOVE
REPORT

Expected output:
3,2,NORTH

## Additional required scenarios

### Report before placement
Input:
REPORT

Expected output:
No valid report output.

### Move before placement
Input:
MOVE

Expected output:
No state change.

### Invalid placement outside table
Input:
PLACE 5,0,NORTH
REPORT

Expected output:
No valid report output.

### Invalid orientation
Input:
PLACE 0,0,UP
REPORT

Expected output:
No valid report output.

### Boundary guard from north edge
Input:
PLACE 0,0,NORTH
MOVE
MOVE
MOVE
MOVE
MOVE
REPORT

Expected output:
0,4,NORTH

### Repeated placement replacement
Input:
PLACE 0,0,NORTH
PLACE 2,2,EAST
REPORT

Expected output:
2,2,EAST

### Empty input
Input:

Expected output:
No output.

### Unknown command
Input:
PLACE 0,0,NORTH
JUMP
REPORT

Expected output:
0,0,NORTH

## Proposed robustness checks

These are not approved assessment requirements and should be treated as proposed behaviour only until explicitly approved.

### Lowercase input
Input:
place 0,0,north
report

Expected output:
0,0,NORTH

### Blank lines or trailing spaces
Input:
PLACE 0,0,NORTH

Expected output:
No output for blank lines; valid output for the command when executed.
