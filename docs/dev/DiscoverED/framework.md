# DiscoverED Framework
_A Unity-based framework for educational games creation_

Imagine if educators and game designers could seamlessly collaborate to transform any didactical content into 3D adventure games. This is the vision behind a new, open-source videogame framework designed to empower creators to craft immersive learning experiences mixing exploration, dialogues, interactive puzzles and engaging quests. At the heart of its accessible design is an external node-based dialogue editor, simplifying the creation and translation of the narrative content.

This framework aims to democratize educational game development, providing intuitive tools for educators to bring their curricula to life and for game designers to lend their expertise in crafting experiences that are not only fun but also deeply impactful for learning.

## Core Idea: Learn by Playing, Create with Ease

The fundamental idea is to bridge the gap between educational theory and engaging gameplay. The framework provides an environment where:

- **Educators** can easily input their subject matter, learning objectives and assessment criteria.
- **Game Designers** can then use these inputs to design compelling game mechanics, narratives and level designs.
- **Players (Students)** embark on 3D adventures where they actively explore environments, interact with characters and objects, solve puzzles that reinforce learning concepts and complete quests that guide them through the educational material.

## Key Goals

- **empower educators & designers:** provide tools that are accessible and user-friendly, regardless of extensive programming knowledge.
- **transform learning:** convert traditional educational material into interactive quests and narratives.
- **foster engagement:** leverage the inherent appeal of 3D adventure games – exploration, storytelling and puzzle-solving – to motivate learners.
- **promote collaboration:** facilitate a smooth workflow between content experts and game designers.
- **open & extensible:** offer an open-source foundation that can be adapted and expanded by the community.

## Workflow: from concept to playable adventure

1. **content definition (educator/designer):** the educator or designer starts by outlining the learning goals and gameplay. They use "Homer" to write the dialogues, define the characters and structure the quests that will guide the learning journey.
2. **narrative & quest design (educator/designer with Homer):** the game designer takes the educational content and crafts engaging narratives, branching dialogues, and multi-step quests. They focus on making the learning objectives feel like natural parts of an unfolding story.  
3. **world building & gameplay implementation (designer/developer in Unity):** the "Homer" data is imported into the Unity framework. Designers then use the framework's tools to build the 3D world, place characters and interactive elements, customize puzzles and link the narrative components to in-game actions and environments.  
4. **iteration & testing (educator & designer):** educators and designers can easily playtest the adventure, identify areas for improvement in both gameplay and educational clarity, and make quick revisions using "Homer" and the Unity framework.
5. **testing & iteration (beta players / educators):**  
    - playtest the adventure to ensure it's engaging, the educational goals are met, and the game mechanics are sound.
    - refine content in "Homer" and game elements in Unity based on feedback.
6. **deployment & learning (players / students):** the final Quest is deployed for players (students) to explore, interact with, and learn from in an engaging and immersive way.

## Key Components of the Framework

### Unity-Powered core engine

- **open-source nature:** the framework itself is open-source, fostering a community of developers and educators who can contribute to its growth, share resources, and adapt it to specific needs.
- **modular design:** built with modularity in mind, allowing for the easy integration of new features, educational mechanics, and custom assets.
- **pre-built templates & assets:** includes a library of common 3D environments, characters, interaction systems and puzzle templates to accelerate development.

**Componenents**

**3D World Engine:**  
The framework supports the creation of rich and explorable 3D environments. This includes tools and templates for:

- **scene management:** easy setup of different game locations.
- **character control:** pre-configured player controllers for navigation (walking, running, interacting).
- **interaction system:** a straightforward way to define interactive objects within the game world (e.g., items to pick up, levers to pull, information points).

**quest management system:**  
this system interprets the Quest Data structured in "Homer" and manage the player's progress. it handles:

- **quest activation & tracking:** triggering quests, displaying objectives, and monitoring completion.
- **conditional logic:** implementing prerequisites for quests or specific quest steps (e.g., "talk to character a before item b can be found").
- **reward & feedback mechanisms:** integrating ways to acknowledge player achievement and provide educational reinforcement.

**dialogue engine integration:**  
seamlessly imports and processes the dialogue and narrative structures created in "Homer." this includes:

- **displaying conversations:** presenting text and audio voice-overs.
- **player choices:** enabling branching dialogues based on player input.
- **event triggering:** allowing dialogues to trigger in-game events (e.g., starting a puzzle, updating a quest).

**puzzle & minigame mechanics:**  
a library of adaptable puzzle templates or a system for easily integrating custom-built puzzles relevant to the educational content. examples could include:

- sequencing challenges
- matching games
- riddles and logic puzzles
- simple physics-based interactions

**educational content hooks:**  
specific functionalities to tie game events and achievements directly to learning outcomes. This might include in-game encyclopedias, quizzes triggered by discoveries, or summaries of learned concepts.

**modularity & extensibility:**  
being open-source, the framework is designed for easy customization. Developers can add new features, integrate different asset packs, or tailor the system to specific educational needs.
  
### External node-based dialogue and quest editor

- **intuitive visual scripting:** "Homer" is a dedicated external free platform designed for writing and structuring game narratives. Its node-based interface allows creators to visually map out conversations, branching dialogues, and quest progressions without needing to write complex code.
- **dialogue management:** supports features crucial for rich storytelling, such as character-specific dialogue, emotional tone indicators, and conditional branching based on player choices or in-game events.
- **quest creation:** facilitates the design of quests by linking dialogue nodes with in-game objectives, item requirements, npc interactions, and puzzle triggers. educators can directly translate learning steps into quest objectives.
- **translation support:** a key feature of "Homer" is its integrated translation capabilities, making it easier to adapt educational adventures for different languages.
- **seamless Unity integration:** the framework includes robust tools to import data from "Homer" directly into Unity, automatically generating the corresponding dialogue trees, quest logic, and NPC behaviors within the 3D game environment. 

**Components**

**Visual Scripting for narratives:**

- **node-based dialogue trees:** users can visually map out conversations, branching choices, and character responses by connecting nodes. each node can represent a line of dialogue, a player choice, or a narrative beat.
- **character management:** define characters, assign their dialogue, and manage their roles within the story.

**Quest design & structuring:**

- **quest creation nodes:** define quests, sub-quests, objectives, and conditions for completion within the same visual interface.
- **linking quests to dialogue:** seamlessly connect dialogue outcomes to quest progression (e.g., a specific dialogue choice might initiate a new quest or complete an existing objective).
- **educational content integration:** special nodes or fields to embed learning points, questions, or factual information directly within the quest flow.

**Translation management:**

- **built-in translation tools:** facilitate the translation of all dialogue and quest text into multiple languages that can be voice synthetized via AI and specific scripts.
- **export/import functionality:** easy export of text for external translation and import of translated content back into "Homer."

**Data export for Unity:**

- **standardized format:** "Homer" exports dialogue, quest data, and translations in a format specifically designed to be easily parsed by the Unity framework. this ensures a smooth pipeline from content creation to in-game implementation.
- **version control & collaboration:** features to support multiple users working on the same project and managing different versions of the narrative.

## Why this framework: benefits and impact

- **addresses a real need:** there's a growing demand for high-quality, engaging educational games that go beyond simple gamification.
- **enhanced learning outcomes:** by immersing players in interactive problem-solving and narrative, the framework aims to foster deeper understanding and knowledge retention.
- **accessible game creation:** lowers the barrier to entry for educators and designers wishing to create high-quality educational games.
- **focus on content:** allows educators to directly shape the learning experience.
- **flexible & adaptable:** the open-source nature and modular design allow for customization across various subjects and learning levels.
- **collaborative potential:** facilitates collaboration between educators with subject matter expertise and game designers skilled in creating engaging experiences.
- **power of visual scripting:** "Homer" provides an intuitive way to manage complex narrative and quest structures.
- **open-source community potential:** the open nature encourages contributions, improvements, and a supportive community of users and developers.
