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
<span class="yarn-line">Bienvenue à WROCŁAW en POLOGNE. <span class="yarn-meta">#line:03f03fe </span></span>
<span class="yarn-cmd">&lt;&lt;card place_odra_river&gt;&gt;</span>
<span class="yarn-line">C'est la RIVIÈRE ODRA. <span class="yarn-meta">#line:053bcb9 </span></span>
<span class="yarn-line">Prêt à explorer ? <span class="yarn-meta">#line:06099c1 </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Super voyage sur l'ODRA. <span class="yarn-meta">#line:02a257c </span></span>
<span class="yarn-line">Vous avez vu des PONTS et des BATEAUX. <span class="yarn-meta">#line:0925273 </span></span>
<span class="yarn-line">Vous avez utilisé une CARTE et un QUIZ. <span class="yarn-meta">#line:05d21d7 </span></span>
<span class="yarn-line">Vous voulez une tâche supplémentaire ? <span class="yarn-meta">#line:0913f94 </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dessinez une CARTE simple. <span class="yarn-meta">#line:0265d07 </span></span>
<span class="yarn-line">Mark ODRA et WISŁA. <span class="yarn-meta">#line:08d1173 </span></span>
<span class="yarn-line">Ajoutez un PONT ROUTIER. <span class="yarn-meta">#line:0b3ae20 </span></span>
<span class="yarn-line">Ajoutez un PONT TRAIN. <span class="yarn-meta">#line:055fe3a </span></span>
<span class="yarn-line">Dessinez une BARGE et un KAYAK. <span class="yarn-meta">#line:0cd2507 </span></span>
<span class="yarn-line">Montre-le à un ami. <span class="yarn-meta">#line:0774214 </span></span>
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
<span class="yarn-line">WROCŁAW a de nombreux PONTS. <span class="yarn-meta">#line:041a469 </span></span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_bridges&gt;&gt;</span>
<span class="yarn-line">Plus d'une centaine ! <span class="yarn-meta">#line:0522f41 </span></span>
<span class="yarn-line">Trouvons trois types. <span class="yarn-meta">#line:04082d6 </span></span>


</code></pre></div>

<a id="ys-node-bridge-foot-hint"></a>
## BRIDGE_FOOT_HINT

<div class="yarn-node" data-title="BRIDGE_FOOT_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: footbridge_view</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card footbridge&gt;&gt;</span>
<span class="yarn-line">La PASSERELLE est pour les gens. <span class="yarn-meta">#line:0d0c6ff </span></span>
<span class="yarn-line">Indice : pas de VOITURES dessus. <span class="yarn-meta">#line:057b7a9 </span></span>

<span class="yarn-cmd">&lt;&lt;activity cleancanvas odra_footbridge tutorial&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-bridge-train-hint"></a>
## BRIDGE_TRAIN_HINT

<div class="yarn-node" data-title="BRIDGE_TRAIN_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: railway_bridge</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">Le PONT TRAIN transporte des TRAINS. <span class="yarn-meta">#line:04b8fb2 </span></span>
<span class="yarn-line">Astuce : recherchez TRACKS. <span class="yarn-meta">#line:03bf613 </span></span>

<span class="yarn-cmd">&lt;&lt;activity memory odra_bridges tutorial&gt;&gt;</span>
<span class="yarn-line">Suivant : PONT ROUTIER <span class="yarn-meta">#line:08d07e5 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BRIDGE_CAR_HINT&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-bridge-car-hint"></a>
## BRIDGE_CAR_HINT

<div class="yarn-node" data-title="BRIDGE_CAR_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Bridges</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: road_bridge</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card road_bridge&gt;&gt;</span>
<span class="yarn-line">Le PONT ROUTIER est pour les VOITURES. <span class="yarn-meta">#line:0f6e252 </span></span>
<span class="yarn-line">Indice : voir le passage CARS. <span class="yarn-meta">#line:03fcfdf </span></span>

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
<span class="yarn-line">De nombreux BATEAUX sur l'ODRA. <span class="yarn-meta">#line:0eb639b </span></span>
<span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">Trouvez d’abord une BARGE. <span class="yarn-meta">#line:0d21cc3 </span></span>

<span class="yarn-line">Montre-moi une BARGE <span class="yarn-meta">#line:03495d2 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BOAT_BARGE_HINT&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-boat-barge-hint"></a>
## BOAT_BARGE_HINT

<div class="yarn-node" data-title="BOAT_BARGE_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: barge_photo</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La BARGE est basse et plate. <span class="yarn-meta">#line:028dcdf </span></span>
<span class="yarn-line">Il transporte des MARCHANDISES. <span class="yarn-meta">#line:083423e </span></span>

<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle odra_barge tutorial&gt;&gt;</span>
<span class="yarn-line">Trouver une péniche <span class="yarn-meta">#line:0a09f26 </span></span>

</code></pre></div>

<a id="ys-node-boat-house-hint"></a>
## BOAT_HOUSE_HINT

<div class="yarn-node" data-title="BOAT_HOUSE_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: houseboat</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">La péniche est faite pour vivre. <span class="yarn-meta">#line:0d77d00 </span></span>
<span class="yarn-line">Des fenêtres comme une petite maison. <span class="yarn-meta">#line:0d30896 </span></span>

<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle odra_houseboat tutorial&gt;&gt;</span>

<span class="yarn-line">Montre-moi un KAYAK <span class="yarn-meta">#line:046e06a </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BOAT_KAYAK_HINT&gt;&gt;</span>
<span class="yarn-line">Prêt à correspondre <span class="yarn-meta">#line:0b48adf </span></span>
    <span class="yarn-cmd">&lt;&lt;jump BOATS_MATCH&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-boat-kayak-hint"></a>
## BOAT_KAYAK_HINT

<div class="yarn-node" data-title="BOAT_KAYAK_HINT"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: kayak</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card kayak&gt;&gt;</span>
<span class="yarn-line">KAYAK est petit et léger <span class="yarn-meta">#line:06a76b7 </span></span>
<span class="yarn-line">Une seule personne peut le pagayer. <span class="yarn-meta">#line:0b0ccde </span></span>
<span class="yarn-line">Déplacez-vous rapidement sur l'eau calme. <span class="yarn-meta">#line:0e4d09c </span></span>

</code></pre></div>

<a id="ys-node-boats-match"></a>
## BOATS_MATCH

<div class="yarn-node" data-title="BOATS_MATCH"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Boats</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: odra_couples</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Associez les choses aux emplois. <span class="yarn-meta">#line:0d62913 </span></span>
<span class="yarn-line">PONT -&gt; personnes/VOITURES/TRAIN. <span class="yarn-meta">#line:094d834 </span></span>
<span class="yarn-line">BATEAU -&gt; marchandises/vivants. <span class="yarn-meta">#line:0e16bac </span></span>

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
<span class="yarn-line">L'ODRA est le deuxième fleuve de Pologne. <span class="yarn-meta">#line:0ff7dfd </span></span>
<span class="yarn-cmd">&lt;&lt;card place_vistula_river&gt;&gt;</span>
<span class="yarn-line">WISŁA est 1, le plus long. <span class="yarn-meta">#line:03fe124 </span></span>
<span class="yarn-line">Les deux se jettent dans la MER BALTIQUE. <span class="yarn-meta">#line:0941615 </span></span>

    <span class="yarn-cmd">&lt;&lt;jump MAP_ACTIVITY&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-map-activity"></a>
## MAP_ACTIVITY

<div class="yarn-node" data-title="MAP_ACTIVITY"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Map</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">image: map_icons</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroklaw_map&gt;&gt;</span>
<span class="yarn-line">Appuyez sur ODRA, WISŁA, BALTIC. <span class="yarn-meta">#line:0ddc2c8 </span></span>
<span class="yarn-line">Suivez les RIVIÈRES jusqu'à la MER. <span class="yarn-meta">#line:06237e3 </span></span>

<span class="yarn-cmd">&lt;&lt;activity jigsaw_odra_map jigsaw_odra_map_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-jigsaw-odra-map-done"></a>
## jigsaw_odra_map_done

<div class="yarn-node" data-title="jigsaw_odra_map_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Map</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Excellent travail, explorateur RIVER ! <span class="yarn-meta">#line:012f97c</span></span>

</code></pre></div>

<a id="ys-node-ending-dock"></a>
## ENDING_DOCK

<div class="yarn-node" data-title="ENDING_DOCK"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: odra_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">Ce PANNEAU bleu signifie RIVIÈRE. <span class="yarn-meta">#line:0b583df </span></span>
<span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">Vous avez appris les PONTS et les BATEAUX. <span class="yarn-meta">#line:0a2180f </span></span>
<span class="yarn-line">Prêt pour un quiz ? <span class="yarn-meta">#line:07d9d36 </span></span>
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
<span class="yarn-line">Répondez à deux questions. <span class="yarn-meta">#line:0afcdc6 </span></span>
<span class="yarn-line">Appuyez sur le meilleur choix. <span class="yarn-meta">#line:0c9b102 </span></span>

<span class="yarn-cmd">&lt;&lt;activity order_odra_facts order_odra_facts_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-order-odra-facts-done"></a>
## order_odra_facts_done

<div class="yarn-node" data-title="order_odra_facts_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=BoatCaptain</span>
<span class="yarn-header-dim">image: quest_complete</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Excellent travail, explorateur RIVER ! <span class="yarn-meta">#line:0117d8b </span></span>
<span class="yarn-line">À bientôt pour le prochain voyage ! <span class="yarn-meta">#line:045c7d9 </span></span>

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
<span class="yarn-line">J'aime le petit KAYAK. <span class="yarn-meta">#line:0f36b7f </span></span>
<span class="yarn-line">Pagayez vite sur l'eau calme. <span class="yarn-meta">#line:07ff8c5 </span></span>

</code></pre></div>

<a id="ys-node-spawned-tourist"></a>
## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=Tourist</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tant de PONTS dans cette ville. <span class="yarn-meta">#line:0577d80 </span></span>
<span class="yarn-line">Mon nombre de photos est énorme. <span class="yarn-meta">#line:089ea37 </span></span>

</code></pre></div>

<a id="ys-node-spawned-fisher"></a>
## spawned_fisher

<div class="yarn-node" data-title="spawned_fisher"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=Fisher</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L’eau calme est bonne pour les poissons. <span class="yarn-meta">#line:0207b2a </span></span>
<span class="yarn-line">Je regarde les bateaux glisser. <span class="yarn-meta">#line:04d0dac </span></span>

</code></pre></div>

<a id="ys-node-spawned-birdwatcher"></a>
## spawned_birdwatcher

<div class="yarn-node" data-title="spawned_birdwatcher"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">tags: actor=Birdwatcher</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Les oiseaux se reposent sur le vieux pont. <span class="yarn-meta">#line:05d28c9 </span></span>
<span class="yarn-line">Je les note dans mon livre. <span class="yarn-meta">#line:0b1b835 </span></span>

</code></pre></div>


