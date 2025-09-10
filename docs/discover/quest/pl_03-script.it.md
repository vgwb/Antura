---
title: Fiume Odra (pl_03) - Script
hide:
---

# Fiume Odra (pl_03) - Script
[Quest Index](./index.it.md) - Language: [english](./pl_03-script.md) - [french](./pl_03-script.fr.md) - [polish](./pl_03-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-03-a-voyage-on-the-odra-river/34/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=106202032#gid=106202032)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_03%20Wroclaw%20River/PL_03%20Wroclaw%20River%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// pl_03 | Odra River (Wroclaw)</span>
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
<span class="yarn-line">Benvenuti a WROCŁAW in POLONIA. <span class="yarn-meta">#line:03f03fe </span></span>
<span class="yarn-cmd">&lt;&lt;card place_odra_river&gt;&gt;</span>
<span class="yarn-line">Questo è il fiume ODRA. <span class="yarn-meta">#line:053bcb9 </span></span>
<span class="yarn-line">Pronti per esplorare? <span class="yarn-meta">#line:06099c1 </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bellissimo viaggio sull'ODRA. <span class="yarn-meta">#line:02a257c </span></span>
<span class="yarn-line">Hai visto PONTI e BARCHE. <span class="yarn-meta">#line:0925273 </span></span>
<span class="yarn-line">Hai utilizzato una MAPPA e un QUIZ. <span class="yarn-meta">#line:05d21d7 </span></span>
<span class="yarn-line">Vuoi un compito extra? <span class="yarn-meta">#line:0913f94 </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Disegna una MAPPA semplice. <span class="yarn-meta">#line:0265d07 </span></span>
<span class="yarn-line">Mark ODRA e WISŁA. <span class="yarn-meta">#line:08d1173 </span></span>
<span class="yarn-line">Aggiungere un PONTE STRADALE. <span class="yarn-meta">#line:0b3ae20 </span></span>
<span class="yarn-line">Aggiungere un PONTE FERROVIARIO. <span class="yarn-meta">#line:055fe3a </span></span>
<span class="yarn-line">Disegna una CHIATTA e un KAYAK. <span class="yarn-meta">#line:0cd2507 </span></span>
<span class="yarn-line">Mostralo a un amico. <span class="yarn-meta">#line:0774214 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

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
<span class="yarn-cmd">&lt;&lt;card bridge&gt;&gt;</span>
<span class="yarn-line">WROCŁAW ha molti PONTI. <span class="yarn-meta">#line:041a469 </span></span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_bridges&gt;&gt;</span>
<span class="yarn-line">Più di cento! <span class="yarn-meta">#line:0522f41 </span></span>
<span class="yarn-line">Troviamo tre tipi. <span class="yarn-meta">#line:04082d6 </span></span>


</code></pre></div>

<a id="ys-node-bridge-foot-hint"></a>
## BRIDGE_FOOT_HINT

<div class="yarn-node" data-title="BRIDGE_FOOT_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: footbridge_view</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card footbridge&gt;&gt;</span>
<span class="yarn-line">FOOTBRIDGE è per le persone. <span class="yarn-meta">#line:0d0c6ff </span></span>
<span class="yarn-line">Suggerimento: non ci sono AUTO. <span class="yarn-meta">#line:057b7a9 </span></span>

<span class="yarn-cmd">&lt;&lt;activity cleancanvas odra_footbridge tutorial&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-bridge-train-hint"></a>
## BRIDGE_TRAIN_HINT

<div class="yarn-node" data-title="BRIDGE_TRAIN_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: railway_bridge</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">Il PONTE FERROVIARIO trasporta TRENI. <span class="yarn-meta">#line:04b8fb2 </span></span>
<span class="yarn-line">Suggerimento: cerca le TRACCE. <span class="yarn-meta">#line:03bf613 </span></span>

<span class="yarn-cmd">&lt;&lt;activity memory odra_bridges tutorial&gt;&gt;</span>
<span class="yarn-line">Successivo: PONTE STRADALE <span class="yarn-meta">#line:08d07e5 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BRIDGE_CAR_HINT&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-bridge-car-hint"></a>
## BRIDGE_CAR_HINT

<div class="yarn-node" data-title="BRIDGE_CAR_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: road_bridge</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card road_bridge&gt;&gt;</span>
<span class="yarn-line">Il PONTE STRADALE è per le AUTO. <span class="yarn-meta">#line:0f6e252 </span></span>
<span class="yarn-line">Suggerimento: vedi attraversamento CARS. <span class="yarn-meta">#line:03fcfdf </span></span>

<span class="yarn-cmd">&lt;&lt;activity memory odra_bridges tutorial&gt;&gt;</span>

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
<span class="yarn-cmd">&lt;&lt;card boat&gt;&gt;</span>
<span class="yarn-line">Molte BARCHE sull'ODRA. <span class="yarn-meta">#line:0eb639b </span></span>
<span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">Trova prima una BARCA. <span class="yarn-meta">#line:0d21cc3 </span></span>

<span class="yarn-line">Mostrami una BARCA <span class="yarn-meta">#line:03495d2 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BOAT_BARGE_HINT&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-boat-barge-hint"></a>
## BOAT_BARGE_HINT

<div class="yarn-node" data-title="BOAT_BARGE_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: barge_photo</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">BARGE è basso e piatto. <span class="yarn-meta">#line:028dcdf </span></span>
<span class="yarn-line">Trasporta MERCI. <span class="yarn-meta">#line:083423e </span></span>

<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle odra_barge tutorial&gt;&gt;</span>
<span class="yarn-line">Trova una CASA GALLEGGIANTE <span class="yarn-meta">#line:0a09f26 </span></span>

</code></pre></div>

<a id="ys-node-boat-house-hint"></a>
## BOAT_HOUSE_HINT

<div class="yarn-node" data-title="BOAT_HOUSE_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: houseboat</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">HOUSEBOAT è una casa in cui vivere. <span class="yarn-meta">#line:0d77d00 </span></span>
<span class="yarn-line">Finestre come una piccola casa. <span class="yarn-meta">#line:0d30896 </span></span>

<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle odra_houseboat tutorial&gt;&gt;</span>

<span class="yarn-line">Mostrami un KAYAK <span class="yarn-meta">#line:046e06a </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BOAT_KAYAK_HINT&gt;&gt;</span>
<span class="yarn-line">Pronti per la partita <span class="yarn-meta">#line:0b48adf </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BOATS_MATCH&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-boat-kayak-hint"></a>
## BOAT_KAYAK_HINT

<div class="yarn-node" data-title="BOAT_KAYAK_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: kayak</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card kayak&gt;&gt;</span>
<span class="yarn-line">KAYAK è piccolo e leggero <span class="yarn-meta">#line:06a76b7 </span></span>
<span class="yarn-line">Una sola persona può pagaiare. <span class="yarn-meta">#line:0b0ccde </span></span>
<span class="yarn-line">Muoviti velocemente in acque calme. <span class="yarn-meta">#line:0e4d09c </span></span>

</code></pre></div>

<a id="ys-node-boats-match"></a>
## BOATS_MATCH

<div class="yarn-node" data-title="BOATS_MATCH"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: odra_couples</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Abbina le cose ai lavori. <span class="yarn-meta">#line:0d62913 </span></span>
<span class="yarn-line">PONTE -&gt; persone/AUTOMOBILI/TRENO. <span class="yarn-meta">#line:094d834 </span></span>
<span class="yarn-line">BARCA -&gt; merci/vive. <span class="yarn-meta">#line:0e16bac </span></span>

<span class="yarn-cmd">&lt;&lt;activity order odra_couples tutorial&gt;&gt;</span>

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
<span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">ODRA è il secondo fiume della Polonia. <span class="yarn-meta">#line:0ff7dfd </span></span>
<span class="yarn-cmd">&lt;&lt;card place_vistula_river&gt;&gt;</span>
<span class="yarn-line">WISŁA è 1, la più lunga. <span class="yarn-meta">#line:03fe124 </span></span>
<span class="yarn-line">Entrambi sfociano nel MAR BALTICO. <span class="yarn-meta">#line:0941615 </span></span>

    <span class="yarn-cmd">&lt;&lt;jump MAP_ACTIVITY&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-map-activity"></a>
## MAP_ACTIVITY

<div class="yarn-node" data-title="MAP_ACTIVITY"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Map</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">image: map_icons</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroklaw_map&gt;&gt;</span>
<span class="yarn-line">Tocca ODRA, WISŁA, BALTIC. <span class="yarn-meta">#line:0ddc2c8 </span></span>
<span class="yarn-line">Segui i FIUMI fino al MARE. <span class="yarn-meta">#line:06237e3 </span></span>

<span class="yarn-cmd">&lt;&lt;activity jigsaw_odra_map jigsaw_odra_map_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-jigsaw-odra-map-done"></a>
## jigsaw_odra_map_done

<div class="yarn-node" data-title="jigsaw_odra_map_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Map</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro, esploratore del FIUME! <span class="yarn-meta">#line:012f97c</span></span>

</code></pre></div>

<a id="ys-node-ending-dock"></a>
## ENDING_DOCK

<div class="yarn-node" data-title="ENDING_DOCK"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: odra_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">Questo CARTELLO blu significa FIUME. <span class="yarn-meta">#line:0b583df </span></span>
<span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">Hai imparato PONTI e BARCHE. <span class="yarn-meta">#line:0a2180f </span></span>
<span class="yarn-line">Pronti per un quiz? <span class="yarn-meta">#line:07d9d36 </span></span>
<span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-final-quiz"></a>
## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ"><pre class="yarn-code"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// FINAL QUIZ – ALWAYS LAST</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">image: odra_quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Rispondi a due domande. <span class="yarn-meta">#line:0afcdc6 </span></span>
<span class="yarn-line">Tocca la scelta migliore. <span class="yarn-meta">#line:0c9b102 </span></span>

<span class="yarn-cmd">&lt;&lt;activity order_odra_facts order_odra_facts_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-order-odra-facts-done"></a>
## order_odra_facts_done

<div class="yarn-node" data-title="order_odra_facts_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: quest_complete</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro, esploratore del FIUME! <span class="yarn-meta">#line:0117d8b </span></span>
<span class="yarn-line">Ci vediamo al prossimo viaggio! <span class="yarn-meta">#line:045c7d9 </span></span>

<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-child"></a>
## spawned_child

<div class="yarn-node" data-title="spawned_child"><pre class="yarn-code"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawned automatically in the scene</span>
<span class="yarn-header-dim">// each time you meet them they say one random line</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=Child</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi piace il KAYAK piccolo. <span class="yarn-meta">#line:0f36b7f </span></span>
<span class="yarn-line">Pagaia velocemente in acque calme. <span class="yarn-meta">#line:07ff8c5 </span></span>

</code></pre></div>

<a id="ys-node-spawned-tourist"></a>
## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=Tourist</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ci sono così tanti PONTI in questa città. <span class="yarn-meta">#line:0577d80 </span></span>
<span class="yarn-line">Il numero delle mie foto è enorme. <span class="yarn-meta">#line:089ea37 </span></span>

</code></pre></div>

<a id="ys-node-spawned-fisher"></a>
## spawned_fisher

<div class="yarn-node" data-title="spawned_fisher"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=Fisher</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L'acqua calma fa bene ai pesci. <span class="yarn-meta">#line:0207b2a </span></span>
<span class="yarn-line">Guardo le barche che scivolano via. <span class="yarn-meta">#line:04d0dac </span></span>

</code></pre></div>

<a id="ys-node-spawned-birdwatcher"></a>
## spawned_birdwatcher

<div class="yarn-node" data-title="spawned_birdwatcher"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=Birdwatcher</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Gli uccelli riposano sul vecchio ponte. <span class="yarn-meta">#line:05d28c9 </span></span>
<span class="yarn-line">Li annoto nel mio libro. <span class="yarn-meta">#line:0b1b835 </span></span>

</code></pre></div>


