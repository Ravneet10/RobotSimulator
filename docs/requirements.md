# Toy Robot Simulator Functional Specification

## 1. Scope and Objectives

This specification defines the functional behaviour for a toy robot simulator operating on a 5 x 5 tabletop. It is implementation-independent and is intended to support design, validation, and acceptance testing for the assessment.

The simulator shall be capable of:
- accepting commands for placing, moving, rotating, and reporting the robot;
- keeping the robot inactive until a valid placement has occurred;
- preventing invalid placements and boundary-crossing moves;
- reporting the current robot state in a consistent format;
- handling malformed or unsupported commands without altering the robot state.

The goal is to specify required behaviour without prescribing any particular technology, program structure, framework, or user interface.

## 2. Functional Requirements

### FR-001: Tabletop dimensions
The system shall define a rectangular tabletop with 5 columns and 5 rows, using coordinates X and Y where X = 0..4 and Y = 0..4.

### FR-002: Valid robot orientations
The robot shall support exactly four valid orientations: NORTH, SOUTH, EAST, and WEST.

### FR-003: Initial robot state
Before a valid PLACE command is processed, the robot shall be considered not placed. In this state, no movement, rotation, or report command shall change the robot's state or produce a valid position output.

### FR-004: Valid placement
The system shall accept a PLACE command containing an X coordinate, Y coordinate, and orientation. A placement shall be valid only if the coordinates are within the tabletop bounds and the orientation is one of the valid orientations.

### FR-005: Invalid placement handling
If a PLACE command specifies a coordinate outside the tabletop or an unsupported orientation, the command shall be ignored and the robot state shall remain unchanged.

### FR-006: MOVE command behaviour
When the robot is placed, a MOVE command shall advance the robot by one unit in the current orientation if the resulting position remains inside the tabletop bounds. If the move would cross the boundary, the command shall be ignored and the robot state shall remain unchanged.

### FR-007: LEFT command behaviour
When the robot is placed, a LEFT command shall rotate the robot 90 degrees counterclockwise without changing its position. The orientation cycle is NORTH -> WEST -> SOUTH -> EAST -> NORTH.

### FR-008: RIGHT command behaviour
When the robot is placed, a RIGHT command shall rotate the robot 90 degrees clockwise without changing its position. The orientation cycle is NORTH -> EAST -> SOUTH -> WEST -> NORTH.

### FR-009: REPORT command behaviour
When the robot is placed, a REPORT command shall output the robot's current state in the format: X,Y,ORIENTATION.

### FR-010: Unsupported or malformed commands
If a command is unsupported, malformed, or cannot be parsed, the system shall ignore it without modifying the robot state.

### FR-011: State persistence
Once a valid placement has been established, the robot's state shall persist across subsequent commands until a later valid placement changes it, subject to movement and rotation rules.

### FR-012: Repeated PLACE commands
A valid PLACE command shall be accepted at any time after the robot has been initialized. A later valid PLACE command overwrites the prior position and orientation, thereby redefining the robot's state.

### FR-013: Command sequence semantics
The system shall process a sequence of commands in order. Every command shall be evaluated using the current robot state at the time it is received, and only valid commands shall mutate state.

## 3. Business Rules and Constraints

### BR-001: Board constraints
The tabletop is limited to five columns and five rows, with coordinates constrained to X = 0..4 and Y = 0..4.

### BR-002: Orientation constraints
Only the values NORTH, SOUTH, EAST, and WEST are valid.

### BR-003: Placement before operation
Commands other than PLACE shall have no effect while the robot is not placed.

### BR-004: No boundary crossing
A MOVE command shall never place the robot outside the tabletop. The move shall be rejected if the next coordinate is outside the permitted range.

### BR-005: Rotation is orientation-only
LEFT and RIGHT affect only the robot's facing direction and never position or board validity.

### BR-006: Ignoring invalid commands
Invalid commands, malformed input, invalid placement attempts, and attempts to move outside the board shall be ignored without altering state or producing an error output.

### BR-007: Command grammar constraint
The accepted command set is limited to PLACE, MOVE, LEFT, RIGHT, and REPORT. Any other command is not part of the valid grammar for this specification.

### BR-008: Output format constraint
A valid REPORT output shall use the current X coordinate, Y coordinate, and orientation in the exact order X,Y,ORIENTATION.

### BR-009: Edge and corner coverage
The four table edges are:
- North edge: Y = 4
- South edge: Y = 0
- East edge: X = 4
- West edge: X = 0

The four table corners are:
- (0,0)
- (0,4)
- (4,0)
- (4,4)

A move that would leave the tabletop from any edge or corner must be rejected and the robot remains unchanged.

## 4. Input and Output Requirements

### Input requirements
- Input shall be provided as a sequence of commands.
- Supported commands include:
  - PLACE X,Y,F
  - MOVE
  - LEFT
  - RIGHT
  - REPORT
- Supported orientation values are NORTH, SOUTH, EAST, and WEST.
- Coordinate values must be integers within the table bounds.
- Inputs that do not match the command grammar or contain invalid arguments shall be rejected as invalid.

### Output requirements
- A valid REPORT command shall produce the robot state in the format: X,Y,ORIENTATION.
- Example output format: 0,0,NORTH
- Before the first valid PLACE, no valid report shall be produced.
- Invalid commands shall not produce an error message under this specification unless a specific external system contract explicitly requires one; the baseline requirement is to ignore them silently.

## 5. Acceptance Criteria

### AC-001: Board initialization
Given a newly initialized simulator
When no valid PLACE command has been processed
Then the robot is unplaced and no command other than PLACE alters its state.

### AC-002: Valid placement on board
Given a valid PLACE command such as PLACE 0,0,NORTH
When the coordinates are within bounds and the orientation is valid
Then the robot is placed at X=0, Y=0, facing NORTH.

### AC-003: Invalid placement outside board
Given a PLACE command such as PLACE 5,0,NORTH or PLACE 0,5,NORTH
When the position is outside the valid table bounds
Then the placement is ignored and the robot remains unplaced or unchanged.

### AC-004: Invalid orientation on placement
Given a PLACE command with an unsupported orientation
When the orientation is not NORTH, SOUTH, EAST, or WEST
Then the placement is ignored.

### AC-005: LEFT rotation cycle
Given a robot placed at any valid position and facing a valid orientation
When LEFT is received
Then the robot rotates 90 degrees counterclockwise according to the orientation cycle NORTH -> WEST -> SOUTH -> EAST -> NORTH without moving.

### AC-006: RIGHT rotation cycle
Given a robot placed at any valid position and facing a valid orientation
When RIGHT is received
Then the robot rotates 90 degrees clockwise according to the orientation cycle NORTH -> EAST -> SOUTH -> WEST -> NORTH without moving.

### AC-007: Movement within bounds
Given a robot placed at 0,0,NORTH
When MOVE is received
Then the robot moves to 0,1,NORTH because the destination remains within the table bounds.

### AC-008: Movement beyond boundary is ignored
Given a robot placed at 0,4,NORTH
When MOVE is received
Then the robot does not move and remains at 0,4,NORTH.

### AC-009: REPORT output
Given a robot is placed and has a current state
When REPORT is received
Then output is in the form X,Y,ORIENTATION.

### AC-010: Ignore commands before placement
Given the robot has not been placed
When MOVE, LEFT, RIGHT, or REPORT is received
Then no state change occurs and no valid report output is produced.

### AC-011: Repeated PLACE command
Given a robot has already been placed
When a later valid PLACE command is received
Then the robot's position and orientation are replaced by the new valid placement.

### AC-012: Malformed input handling
Given a command sequence containing unsupported or malformed input
When the command cannot be parsed or is not in the accepted set
Then the command is ignored without changing the robot state.

### AC-013: Edge and corner behaviour
Given a robot is placed on any edge or corner and a MOVE would leave the table
When the next coordinate is outside the valid range
Then the move is rejected and the robot remains in the previous valid position.

## 6. Required Example Scenarios and Expected Results

The original challenge includes the following three official examples.

### Example 1: Basic Move
Command sequence:
- PLACE 0,0,NORTH
- MOVE
- REPORT

Expected result:
- 0,1,NORTH

### Example 2: Left turn
Command sequence:
- PLACE 0,0,NORTH
- LEFT
- REPORT

Expected result:
- 0,0,WEST

### Example 3: Multi-step movement and turn
Command sequence:
- PLACE 1,1,EAST
- MOVE
- MOVE
- LEFT
- MOVE
- REPORT

Expected result:
- 3,2,NORTH

Additional boundary and invalid-input examples may be used for supplementary testing, but they are not replacements for the official challenge examples.

## 7. Edge Cases and Negative Scenarios

- A robot is created and receives REPORT immediately.
- A robot is created and receives LEFT before any valid PLACE.
- A robot is created and receives RIGHT before any valid PLACE.
- A robot receives MOVE after a failed PLACE attempt.
- A robot receives PLACE with X outside the range 0..4.
- A robot receives PLACE with Y outside the range 0..4.
- A robot receives PLACE with orientation not in the valid set.
- A robot attempts to move from the north edge to Y=5.
- A robot attempts to move from the south edge to Y=-1.
- A robot attempts to move from the east edge to X=5.
- A robot attempts to move from the west edge to X=-1.
- A robot is placed at a corner and attempts to move in a direction that would leave the board.
- A robot receives an empty command.
- A robot receives a command with too few arguments.
- A robot receives an unsupported command such as JUMP.
- A robot receives a valid PLACE after an initial invalid PLACE attempt; only the second valid placement should count.
- A robot receives a valid PLACE after an earlier valid placement; the new placement replaces the previous state.

## 8. Assumptions and Unresolved Questions

The specification below distinguishes required behaviour from matters that could be product-level choices but are not part of the assessment.

### Required behaviour (assessment-based)
- The robot is initially unplaced.
- Valid commands are limited to PLACE, MOVE, LEFT, RIGHT, and REPORT.
- The robot remains stationary when a move would cross the tabletop boundary.
- Invalid placement attempts and invalid commands are ignored.
- A valid PLACE command after an earlier valid placement overwrites the previous state.

### Pending decision: output on invalid commands
The assessment does not prescribe a user-visible error message for invalid commands. This document treats invalid inputs as ignored no-ops consistent with the challenge's requirement to ignore invalid commands without changing robot state.

### Pending decision: case sensitivity
The assessment does not specify whether commands or orientations are case-sensitive. This specification assumes the canonical uppercase forms and treats any non-canonical input as invalid unless the assessment explicitly approves a different rule.

## 9. Assessment Review Findings

The following findings were identified while reviewing this document against the assessment. Each item includes severity, the affected requirement ID, the issue, and the proposed correction.

### Finding 1
- Severity: High
- Affected requirement ID: FR-003, FR-010, AC-010
- Problem: The requirement states that commands before first valid placement do not affect state, but the document did not explicitly state that no report may be produced before placement. This is a critical assessment requirement and should be stated directly.
- Proposed correction: Add explicit language in FR-003 and AC-010: "Commands before the first valid PLACE shall not change the robot state and shall not produce a valid REPORT output."

### Finding 2
- Severity: High
- Affected requirement ID: FR-007, FR-008, AC-005, AC-006
- Problem: Rotation semantics were not fully specified for the full 90-degree orientation cycle, especially the wraparound between NORTH and WEST and between NORTH and EAST.
- Proposed correction: Replace the rotation wording with explicit cycle mappings: LEFT = NORTH -> WEST -> SOUTH -> EAST -> NORTH; RIGHT = NORTH -> EAST -> SOUTH -> WEST -> NORTH.

### Finding 3
- Severity: Medium
- Affected requirement ID: BR-009, AC-013
- Problem: The assessment requires consideration of all four table edges and all four corners. These were not explicitly named and were only implied in edge-case text.
- Proposed correction: Add BR-009 naming each edge and corner and add AC-013 covering boundary attempts from any edge or corner.

### Finding 4
- Severity: Medium
- Affected requirement ID: FR-012, AC-011
- Problem: Repeated valid PLACE commands were not explicitly defined as supported behaviour. The document implied persistence but did not state that a later valid PLACE overwrites prior state.
- Proposed correction: Add FR-012 and AC-011 stating that a valid PLACE at any time replaces the previous position and direction.

### Finding 5
- Severity: Medium
- Affected requirement ID: FR-010, BR-006, AC-012
- Problem: Invalid input handling was defined but not explicit enough for the assessment's required no-op rule across malformed PLACE syntax, unsupported commands, and non-numeric positions.
- Proposed correction: Clarify that malformed input, unsupported commands, and invalid PLACE arguments shall be ignored without state change and without producing a valid report.

### Finding 6
- Severity: Medium
- Affected requirement ID: Section 6
- Problem: The document included more examples than the challenge supplies. The assessment specifically references three official example scenarios; the requirement set should capture those exactly and avoid alternative examples being treated as authoritative requirements.
- Proposed correction: Retain the three canonical examples and label additional examples as supplementary non-official test cases only.

### Finding 7
- Severity: Low
- Affected requirement ID: Section 8
- Problem: The assumptions section introduced product-level decisions that were not required by the assessment, which could be mistaken for functional requirements.
- Proposed correction: Keep unresolved or product-level issues clearly labelled as pending decisions and clearly separate them from assessment-derived requirements.

## 10. Traceability Mapping: Requirements to Acceptance Criteria

| Requirement ID | Acceptance Criteria | Coverage Summary |
| --- | --- | --- |
| FR-001 | AC-001, AC-013 | Defines the 5x5 board and coordinate bounds. |
| FR-002 | AC-005, AC-006 | Valid orientations and rotation behaviour. |
| FR-003 | AC-001, AC-010 | Initial unplaced state and commands before first valid placement. |
| FR-004 | AC-002, AC-011 | Valid PLACE behaviour and placement replacement. |
| FR-005 | AC-003, AC-004, AC-012 | Invalid placement is ignored. |
| FR-006 | AC-007, AC-008, AC-013 | Movement and boundary safety. |
| FR-007 | AC-005 | LEFT rotation semantics. |
| FR-008 | AC-006 | RIGHT rotation semantics. |
| FR-009 | AC-009 | REPORT output format. |
| FR-010 | AC-012 | Unsupported and malformed commands are ignored. |
| FR-011 | AC-011 | Persistence across valid commands. |
| FR-012 | AC-011 | Repeated PLACE support. |
| FR-013 | AC-011, AC-012 | Sequence processing order and no-op invalid commands. |
| BR-001 | AC-001, AC-002, AC-003, AC-008, AC-013 | Board bounds. |
| BR-002 | AC-004, AC-005, AC-006 | Allowed orientations. |
| BR-003 | AC-001, AC-010 | Pre-placement command constraints. |
| BR-004 | AC-007, AC-008, AC-013 | Boundary crossing prevention. |
| BR-006 | AC-003, AC-004, AC-008, AC-012 | Invalid commands and invalid motions are ignored. |
| BR-007 | AC-012 | Supported command set. |
| BR-008 | AC-009 | Output format constraint. |
| BR-009 | AC-013 | Edges and corners considered. |

## 11. Completion Status

This document reflects the assessment requirements and the review findings needed to close gaps in the previous draft. It remains a specification only and does not include implementation code or any design decisions beyond the required functional definition.

## 12. Unresolved Decisions

These are not assessment requirements but may require product- or interface-level confirmation before implementation:
- Whether invalid commands should emit a visible error message or be ignored silently.
- Whether lowercase command names or orientations are accepted as valid input.
- Whether an external report-history log is required beyond the current robot state.

These items are explicitly labelled as pending and are not treated as approved functional requirements.
