---
title: A Voyage on the Odra River (pl_03) - Script
hide:
---

# A Voyage on the Odra River (pl_03) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-03-a-voyage-on-the-odra-river/34/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=106202032#gid=106202032)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_03%20Wroclaw%20River/PL_03%20Wroclaw%20River%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_03 | Odra River (Wroclaw)</span>
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
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: odra_dock</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Welcome to WROCŁAW in POLAND.</span> <span class="yarn-meta">#line:03f03fe </span>
<span class="yarn-cmd">&lt;&lt;card place_odra_river&gt;&gt;</span>
<span class="yarn-line">This is the ODRA RIVER.</span> <span class="yarn-meta">#line:053bcb9 </span>
<span class="yarn-line">Ready to explore?</span> <span class="yarn-meta">#line:06099c1 </span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great trip on the ODRA.</span> <span class="yarn-meta">#line:02a257c </span>
<span class="yarn-line">You saw BRIDGES and BOATS.</span> <span class="yarn-meta">#line:0925273 </span>
<span class="yarn-line">You used a MAP and a QUIZ.</span> <span class="yarn-meta">#line:05d21d7 </span>
<span class="yarn-line">Want an extra task?</span> <span class="yarn-meta">#line:0913f94 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Draw a simple MAP.</span> <span class="yarn-meta">#line:0265d07 </span>
<span class="yarn-line">Mark ODRA and WISŁA.</span> <span class="yarn-meta">#line:08d1173 </span>
<span class="yarn-line">Add one ROAD BRIDGE.</span> <span class="yarn-meta">#line:0b3ae20 </span>
<span class="yarn-line">Add one TRAIN BRIDGE.</span> <span class="yarn-meta">#line:055fe3a </span>
<span class="yarn-line">Sketch a BARGE and a KAYAK.</span> <span class="yarn-meta">#line:0cd2507 </span>
<span class="yarn-line">Show it to a friend.</span> <span class="yarn-meta">#line:0774214 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-bridges-intro"></a>

## BRIDGES_INTRO

<div class="yarn-node" data-title="BRIDGES_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// PART 1 – CITY OF BRIDGES</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: city_of_bridges</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card bridge&gt;&gt;</span>
<span class="yarn-line">WROCŁAW has many BRIDGES.</span> <span class="yarn-meta">#line:041a469 </span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_bridges&gt;&gt;</span>
<span class="yarn-line">More than one hundred!</span> <span class="yarn-meta">#line:0522f41 </span>
<span class="yarn-line">Let’s find three types.</span> <span class="yarn-meta">#line:04082d6 </span>


</code>
</pre>
</div>

<a id="ys-node-bridge-foot-hint"></a>

## BRIDGE_FOOT_HINT

<div class="yarn-node" data-title="BRIDGE_FOOT_HINT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: footbridge_view</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card footbridge&gt;&gt;</span>
<span class="yarn-line">FOOTBRIDGE is for people.</span> <span class="yarn-meta">#line:0d0c6ff </span>
<span class="yarn-line">Hint: no CARS on it.</span> <span class="yarn-meta">#line:057b7a9 </span>

<span class="yarn-cmd">&lt;&lt;activity cleancanvas odra_footbridge tutorial&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-bridge-train-hint"></a>

## BRIDGE_TRAIN_HINT

<div class="yarn-node" data-title="BRIDGE_TRAIN_HINT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: railway_bridge</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">TRAIN BRIDGE carries TRAINS.</span> <span class="yarn-meta">#line:04b8fb2 </span>
<span class="yarn-line">Hint: look for TRACKS.</span> <span class="yarn-meta">#line:03bf613 </span>

<span class="yarn-cmd">&lt;&lt;activity memory odra_bridges tutorial&gt;&gt;</span>
<span class="yarn-line">Next: ROAD BRIDGE</span> <span class="yarn-meta">#line:08d07e5 </span>
    <span class="yarn-cmd">&lt;&lt;jump BRIDGE_CAR_HINT&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-bridge-car-hint"></a>

## BRIDGE_CAR_HINT

<div class="yarn-node" data-title="BRIDGE_CAR_HINT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: road_bridge</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card road_bridge&gt;&gt;</span>
<span class="yarn-line">ROAD BRIDGE is for CARS.</span> <span class="yarn-meta">#line:0f6e252 </span>
<span class="yarn-line">Hint: see CARS crossing.</span> <span class="yarn-meta">#line:03fcfdf </span>

<span class="yarn-cmd">&lt;&lt;activity memory odra_bridges tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-boats-intro"></a>

## BOATS_INTRO

<div class="yarn-node" data-title="BOATS_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// PART 2 – RIVER BOATS</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: odra_river_traffic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat&gt;&gt;</span>
<span class="yarn-line">Many BOATS on the ODRA.</span> <span class="yarn-meta">#line:0eb639b </span>
<span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">Find a BARGE first.</span> <span class="yarn-meta">#line:0d21cc3 </span>

<span class="yarn-line">Show me a BARGE</span> <span class="yarn-meta">#line:03495d2 </span>
    <span class="yarn-cmd">&lt;&lt;jump BOAT_BARGE_HINT&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-boat-barge-hint"></a>

## BOAT_BARGE_HINT

<div class="yarn-node" data-title="BOAT_BARGE_HINT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: barge_photo</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">BARGE is low and flat.</span> <span class="yarn-meta">#line:028dcdf </span>
<span class="yarn-line">It carries GOODS.</span> <span class="yarn-meta">#line:083423e </span>

<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle odra_barge tutorial&gt;&gt;</span>
<span class="yarn-line">Find a HOUSEBOAT</span> <span class="yarn-meta">#line:0a09f26 </span>

</code>
</pre>
</div>

<a id="ys-node-boat-house-hint"></a>

## BOAT_HOUSE_HINT

<div class="yarn-node" data-title="BOAT_HOUSE_HINT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: houseboat</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">HOUSEBOAT is for living.</span> <span class="yarn-meta">#line:0d77d00 </span>
<span class="yarn-line">Windows like a small home.</span> <span class="yarn-meta">#line:0d30896 </span>

<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle odra_houseboat tutorial&gt;&gt;</span>

<span class="yarn-line">Show me a KAYAK</span> <span class="yarn-meta">#line:046e06a </span>
    <span class="yarn-cmd">&lt;&lt;jump BOAT_KAYAK_HINT&gt;&gt;</span>
<span class="yarn-line">Ready to match</span> <span class="yarn-meta">#line:0b48adf </span>
    <span class="yarn-cmd">&lt;&lt;jump BOATS_MATCH&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-boat-kayak-hint"></a>

## BOAT_KAYAK_HINT

<div class="yarn-node" data-title="BOAT_KAYAK_HINT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: kayak</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card kayak&gt;&gt;</span>
<span class="yarn-line">KAYAK is small and light</span> <span class="yarn-meta">#line:06a76b7 </span>
<span class="yarn-line">One person can paddle it.</span> <span class="yarn-meta">#line:0b0ccde </span>
<span class="yarn-line">Move fast on calm water.</span> <span class="yarn-meta">#line:0e4d09c </span>

</code>
</pre>
</div>

<a id="ys-node-boats-match"></a>

## BOATS_MATCH

<div class="yarn-node" data-title="BOATS_MATCH">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: odra_couples</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Match things to jobs.</span> <span class="yarn-meta">#line:0d62913 </span>
<span class="yarn-line">BRIDGE -&gt; people/CARS/TRAIN.</span> <span class="yarn-meta">#line:094d834 </span>
<span class="yarn-line">BOAT -&gt; goods/live.</span> <span class="yarn-meta">#line:0e16bac </span>

<span class="yarn-cmd">&lt;&lt;activity order odra_couples tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-map-intro"></a>

## MAP_INTRO

<div class="yarn-node" data-title="MAP_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// PART 3 – BIG MAP</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: Map</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: poland_river_map</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">ODRA is Poland’s 2 RIVER.</span> <span class="yarn-meta">#line:0ff7dfd </span>
<span class="yarn-cmd">&lt;&lt;card place_vistula_river&gt;&gt;</span>
<span class="yarn-line">WISŁA is 1, the longest.</span> <span class="yarn-meta">#line:03fe124 </span>
<span class="yarn-line">Both flow to the BALTIC SEA.</span> <span class="yarn-meta">#line:0941615 </span>

    <span class="yarn-cmd">&lt;&lt;jump MAP_ACTIVITY&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-map-activity"></a>

## MAP_ACTIVITY

<div class="yarn-node" data-title="MAP_ACTIVITY">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Map</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">image: map_icons</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroklaw_map&gt;&gt;</span>
<span class="yarn-line">Tap ODRA, WISŁA, BALTIC.</span> <span class="yarn-meta">#line:0ddc2c8 </span>
<span class="yarn-line">Follow RIVERS to the SEA.</span> <span class="yarn-meta">#line:06237e3 </span>

<span class="yarn-cmd">&lt;&lt;activity jigsaw_odra_map jigsaw_odra_map_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-jigsaw-odra-map-done"></a>

## jigsaw_odra_map_done

<div class="yarn-node" data-title="jigsaw_odra_map_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Map</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great work, RIVER explorer!</span> <span class="yarn-meta">#line:012f97c</span>

</code>
</pre>
</div>

<a id="ys-node-ending-dock"></a>

## ENDING_DOCK

<div class="yarn-node" data-title="ENDING_DOCK">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: odra_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">This blue SIGN means RIVER.</span> <span class="yarn-meta">#line:0b583df </span>
<span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">You learned BRIDGES and BOATS.</span> <span class="yarn-meta">#line:0a2180f </span>
<span class="yarn-line">Ready for a quiz?</span> <span class="yarn-meta">#line:07d9d36 </span>
<span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz"></a>

## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// FINAL QUIZ – ALWAYS LAST</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">image: odra_quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Answer two questions.</span> <span class="yarn-meta">#line:0afcdc6 </span>
<span class="yarn-line">Tap the best choice.</span> <span class="yarn-meta">#line:0c9b102 </span>

<span class="yarn-cmd">&lt;&lt;activity order_odra_facts order_odra_facts_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-order-odra-facts-done"></a>

## order_odra_facts_done

<div class="yarn-node" data-title="order_odra_facts_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: quest_complete</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great work, RIVER explorer!</span> <span class="yarn-meta">#line:0117d8b </span>
<span class="yarn-line">See you on the next trip!</span> <span class="yarn-meta">#line:045c7d9 </span>

<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-child"></a>

## spawned_child

<div class="yarn-node" data-title="spawned_child">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawned automatically in the scene</span>
<span class="yarn-header-dim">// each time you meet them they say one random line</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I like the small KAYAK.</span> <span class="yarn-meta">#line:0f36b7f </span>
<span class="yarn-line">Paddle fast on calm water.</span> <span class="yarn-meta">#line:07ff8c5 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">So many BRIDGES in this city.</span> <span class="yarn-meta">#line:0577d80 </span>
<span class="yarn-line">My photo count is huge.</span> <span class="yarn-meta">#line:089ea37 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-fisher"></a>

## spawned_fisher

<div class="yarn-node" data-title="spawned_fisher">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Quiet water is good for fish.</span> <span class="yarn-meta">#line:0207b2a </span>
<span class="yarn-line">I watch boats glide past.</span> <span class="yarn-meta">#line:04d0dac </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-birdwatcher"></a>

## spawned_birdwatcher

<div class="yarn-node" data-title="spawned_birdwatcher">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Birds rest on the old bridge.</span> <span class="yarn-meta">#line:05d28c9 </span>
<span class="yarn-line">I note them in my book.</span> <span class="yarn-meta">#line:0b1b835 </span>

</code>
</pre>
</div>


