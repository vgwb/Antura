---
title: Fleuve Oder (pl_03) - Script
hide:
---

# Fleuve Oder (pl_03) - Script
[Quest Index](./index.fr.md) - Language: [english](./pl_03-script.md) - french - [polish](./pl_03-script.pl.md) - [italian](./pl_03-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-03-a-voyage-on-the-odra-river/34/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=106202032#gid=106202032)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_03%20Wroclaw%20River/PL_03%20Wroclaw%20River%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// Quest: pl_03 | Odra River (Wroclaw)</span>
<span class="yarn-header-dim">// PL_03 - A Voyage on the Odra River</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// ---------------------------------------------</span>
<span class="yarn-header-dim">// WANTED:</span>
<span class="yarn-header-dim">// cleancanvas odra_footbridge  — Clean the FOOTBRIDGE image; concept: BRIDGE for people (no CARS).</span>
<span class="yarn-header-dim">// memory odra_bridges          — Match BRIDGE types; concepts: TRAIN BRIDGE (tracks), ROAD BRIDGE (CARS).</span>
<span class="yarn-header-dim">// jigsawpuzzle odra_barge      — Rebuild BARGE image; concept: BOAT that carries GOODS.</span>
<span class="yarn-header-dim">// jigsawpuzzle odra_houseboat  — Rebuild HOUSEBOAT image; concept: BOAT for LIVING.</span>
<span class="yarn-header-dim">// order odra_couples           — Pair BRIDGES→(people/CARS/TRAIN) and BOATS→(goods/live).</span>
<span class="yarn-header-dim">// order odra_map_triplet       — Tap/sequence ODRA, WISŁA, BALTIC; concept: rivers flow to SEA.</span>
<span class="yarn-header-dim">// quiz odra_facts              — Final assessment (at least 2 Qs): ODRA→BALTIC, longest river=WISŁA.</span>
<span class="yarn-header-dim">// Words used: Odra, river, bridge, island , water, Wrocław, navigation, ecosystem</span>
<span class="yarn-header-dim">// INTRO – DOCK</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: odra_dock</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome to WROCŁAW in POLAND] <span class="yarn-meta">#line:03f03fe </span></span>
<span class="yarn-line">[MISSING TRANSLATION: This is the ODRA RIVER] <span class="yarn-meta">#line:053bcb9 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Ready to explore?] <span class="yarn-meta">#line:06099c1 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-the-end"></a>
## the_end

<div class="yarn-node" data-title="the_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: This quest is complete.]
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-quest-proposal"></a>
## quest_proposal

<div class="yarn-node" data-title="quest_proposal"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: Why don't you draw the river?]
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-bridges-intro"></a>
## BRIDGES_INTRO

<div class="yarn-node" data-title="BRIDGES_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// PART 1 – CITY OF BRIDGES</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: city_of_bridges</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: WROCŁAW has many BRIDGES.] <span class="yarn-meta">#line:041a469 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: More than one hundred!] <span class="yarn-meta">#line:0522f41 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Let’s find three types.] <span class="yarn-meta">#line:04082d6 </span></span>
[MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-bridge-foot-hint"></a>
## BRIDGE_FOOT_HINT

<div class="yarn-node" data-title="BRIDGE_FOOT_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: footbridge_view</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: FOOTBRIDGE is for people] <span class="yarn-meta">#line:0d0c6ff </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Hint no CARS on it] <span class="yarn-meta">#line:057b7a9 </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;activity cleancanvas odra_footbridge tutorial&gt;&gt;</span>
[MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-bridge-train-hint"></a>
## BRIDGE_TRAIN_HINT

<div class="yarn-node" data-title="BRIDGE_TRAIN_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: railway_bridge</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: TRAIN BRIDGE carries TRAINS] <span class="yarn-meta">#line:04b8fb2 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Hint look for TRACKS] <span class="yarn-meta">#line:03bf613 </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;activity memory odra_bridges tutorial&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Next: ROAD BRIDGE] <span class="yarn-meta">#line:08d07e5 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BRIDGE_CAR_HINT&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-bridge-car-hint"></a>
## BRIDGE_CAR_HINT

<div class="yarn-node" data-title="BRIDGE_CAR_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: road_bridge</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: ROAD BRIDGE is for CARS] <span class="yarn-meta">#line:0f6e252 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Hint spot CARS crossing] <span class="yarn-meta">#line:03fcfdf </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;activity memory odra_bridges tutorial&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-boats-intro"></a>
## BOATS_INTRO

<div class="yarn-node" data-title="BOATS_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// PART 2 – RIVER BOATS</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: odra_river_traffic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Many BOATS on the ODRA.] <span class="yarn-meta">#line:0eb639b </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Find a BARGE first.] <span class="yarn-meta">#line:0d21cc3 </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Show me a BARGE] <span class="yarn-meta">#line:03495d2 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BOAT_BARGE_HINT&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-boat-barge-hint"></a>
## BOAT_BARGE_HINT

<div class="yarn-node" data-title="BOAT_BARGE_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: barge_photo</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: BARGE is low and flat] <span class="yarn-meta">#line:028dcdf </span></span>
<span class="yarn-line">[MISSING TRANSLATION: It carries GOODS] <span class="yarn-meta">#line:083423e </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle odra_barge tutorial&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Find a HOUSEBOAT] <span class="yarn-meta">#line:0a09f26 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-boat-house-hint"></a>
## BOAT_HOUSE_HINT

<div class="yarn-node" data-title="BOAT_HOUSE_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: houseboat</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: HOUSEBOAT is for living] <span class="yarn-meta">#line:0d77d00 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Windows like a small home] <span class="yarn-meta">#line:0d30896 </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle odra_houseboat tutorial&gt;&gt;</span>
[MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-boats-match"></a>
## BOATS_MATCH

<div class="yarn-node" data-title="BOATS_MATCH"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: odra_couples</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Match things to jobs] <span class="yarn-meta">#line:0d62913 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: BRIDGE -&gt; people/CARS/TRAIN] <span class="yarn-meta">#line:094d834 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: BOAT -&gt; goods/live] <span class="yarn-meta">#line:0e16bac </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;activity order odra_couples tutorial&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-map-intro"></a>
## MAP_INTRO

<div class="yarn-node" data-title="MAP_INTRO"><pre class="yarn-code"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// PART 3 – BIG MAP</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: Map</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: poland_river_map</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: ODRA is Poland’s 2 RIVER.] <span class="yarn-meta">#line:0ff7dfd </span></span>
<span class="yarn-line">[MISSING TRANSLATION: WISŁA is 1, the longest.] <span class="yarn-meta">#line:03fe124 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Both flow to the BALTIC SEA.] <span class="yarn-meta">#line:0941615 </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-line">[MISSING TRANSLATION: -&gt; Map check] <span class="yarn-meta">#line:0fe3ec3 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump MAP_ACTIVITY&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-map-activity"></a>
## MAP_ACTIVITY

<div class="yarn-node" data-title="MAP_ACTIVITY"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Map</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: map_icons</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Tap ODRA, WISŁA, BALTIC.] <span class="yarn-meta">#line:0ddc2c8 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Follow RIVERS to the SEA.] <span class="yarn-meta">#line:06237e3 </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;activity order odra_map_triplet tutorial&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-ending-dock"></a>
## ENDING_DOCK

<div class="yarn-node" data-title="ENDING_DOCK"><pre class="yarn-code"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ENDING – DOCK &amp; SIGN</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: odra_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: This blue SIGN means RIVER.] <span class="yarn-meta">#line:0b583df </span></span>
<span class="yarn-line">[MISSING TRANSLATION: You learned BRIDGES and BOATS.] <span class="yarn-meta">#line:0a2180f </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Ready for a quiz?] <span class="yarn-meta">#line:07d9d36 </span></span>
<span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-final-quiz"></a>
## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ"><pre class="yarn-code"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// FINAL QUIZ – ALWAYS LAST</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: odra_quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Answer two questions.] <span class="yarn-meta">#line:0afcdc6 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Tap the best choice.] <span class="yarn-meta">#line:0c9b102 </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;activity quiz odra_facts tutorial&gt;&gt;</span>
<span class="yarn-comment">// Suggested questions in preset:</span>
<span class="yarn-comment">// 1) ODRA flows into which SEA?  a) BLACK  b) BALTIC ✅</span>
<span class="yarn-comment">// 2) Longest RIVER in POLAND?    a) WISŁA ✅  b) ODRA</span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;jump QUEST_COMPLETE&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-quest-complete"></a>
## QUEST_COMPLETE

<div class="yarn-node" data-title="QUEST_COMPLETE"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: quest_complete</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Great work, RIVER explorer!] <span class="yarn-meta">#line:012f97c </span></span>
<span class="yarn-line">[MISSING TRANSLATION: See you on the next trip!] <span class="yarn-meta">#line:045c7d9 </span></span>
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;jump the_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>


