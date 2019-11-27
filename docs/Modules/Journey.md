# Journey

Represents the didactical journey.

## Journey position

The Journey position is defined as a hierarchical structure, made of Stages, Learning Blocks, and Play Sessions.

* **Stages** define overall learning goals. Each stage is assigned to a specific Map.
* **Learning Blocks** define general learning requirements for a set of play sessions.
* **Play Sessions** define single play instances, composed of several minigames and a result. A Play Session may be considered an **Assessment**, in this case the value is always 100.

Each is defined by a sequential integer value.
A combination of three values identifies a single playing session, which is referred to as **Journey Position**.

A Journey Position is thus identified by a the sequence **X.Y.Z** where X is the Stage, Y the Learning Block, and Z the Play Session.

## Configuration

The learning progression can be configured through two main sources:

1) Editing the PlaySessionData and LearningBlockData tables in the static database. These define the progression of the learning content from lower to higher stages.
The learning content should be distributed so that harder content appears at higher stages.
The system will make sure to use this information when **filtering** dictionary content.
*Example: content at higher learning blocks cannot appear at lower play sessions*

2) Editing the ConfigAI weight constants. These values define how much weight to give to each rule when selecting contents.
The system will make sure to use this information when **weighing** dictionary content.
*Example: content that has been seen recently may appear less often*
