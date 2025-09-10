---
title: Move around in a city in safety (fr_04) - Script
hide:
---

# Move around in a city in safety (fr_04) - Script
[Quest Index](./index.md) - Language: english - [french](./fr_04-script.fr.md) - [polish](./fr_04-script.pl.md) - [italian](./fr_04-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-04-road-safety-les-mans/40/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1892167235#gid=1892167235)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_04%20Le%20Mans%20Streets/FR_04%20Le%20Mans%20Streets%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// fr_04 | Road Safety (Les Mans)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - lemans_race (sports heritage)</span>
<span class="yarn-header-dim">// - race_car (automotive)</span>
<span class="yarn-header-dim">// - traffic_lights safety education)</span>
<span class="yarn-header-dim">// - stop_sign (safety education)</span>
<span class="yarn-header-dim">// - danger_sign (safety education)</span>
<span class="yarn-header-dim">// - zebra_crossing (safety education)</span>
<span class="yarn-header-dim">// - scooter (transportation)</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - Experience Le Mans race as spectator</span>
<span class="yarn-header-dim">// - Navigate city streets following traffic rules</span>
<span class="yarn-header-dim">// - Identify and respond to traffic signs correctly</span>
<span class="yarn-header-dim">// - Reach train station safely</span>
<span class="yarn-header-dim">// Words used: race, car, speed, traffic, lights, stop, danger, zebra crossing, scooter, train station, safety, street, road, pedestrian, red, yellow, green, triangle, stripes, careful</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-line">Welcome to Le Mans! <span class="yarn-meta">#line:0a4f3e1</span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great job! <span class="yarn-meta">#line:08cd696 </span></span>
<span class="yarn-line">You used the signs to stay safe. <span class="yarn-meta">#line:0298d64 </span></span>
<span class="yarn-line">Road safety can be fun! <span class="yarn-meta">#line:001499f </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Draw a map of the streets around your home! <span class="yarn-meta">#line:0acd0a1 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-friend-intro"></a>
## talk_friend_intro

<div class="yarn-node" data-title="talk_friend_intro"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the Le Mans car race. <span class="yarn-meta">#line:02f370e </span></span>
<span class="yarn-line">Cars are very fast! <span class="yarn-meta">#line:07f77f0 </span></span>
<span class="yarn-line">Only grown-ups can drive them. <span class="yarn-meta">#line:0d0cd3f </span></span>
<span class="yarn-line">Those race cars are so fast! <span class="yarn-meta">#line:0e54cfc </span></span>
<span class="yarn-line">You can only go that fast on the track. <span class="yarn-meta">#line:053328c </span></span>
<span class="yarn-line">On city streets we go slow. <span class="yarn-meta">#line:0b70620 </span></span>
<span class="yarn-line">Time to go home. <span class="yarn-meta">#line:06e4b9d </span></span>
<span class="yarn-cmd">&lt;&lt;jump talk_friend_scooter&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-friend-scooter"></a>
## talk_friend_scooter

<div class="yarn-node" data-title="talk_friend_scooter"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The race is over. Go home by train. <span class="yarn-meta">#line:0b834f8 </span></span>
<span class="yarn-line">Take my scooter to the station. <span class="yarn-meta">#line:00f2f65 </span></span>
<span class="yarn-cmd">&lt;&lt;card scooter&gt;&gt;</span>
<span class="yarn-line">I will wait there. <span class="yarn-meta">#line:0425fb5 </span></span>
<span class="yarn-line">Look at the signs. <span class="yarn-meta">#line:0b5ee19 </span></span>
<span class="yarn-cmd">&lt;&lt;action GIVE_SCOOTER&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump task_reach_station&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-reach-station"></a>
## task_reach_station

<div class="yarn-node" data-title="task_reach_station"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Get to the train station safely. <span class="yarn-meta">#line:fr04_task_station</span></span>
<span class="yarn-line">Stop at red. Go at green. <span class="yarn-meta">#line:fr04_task_station_2</span></span>

</code></pre></div>

<a id="ys-node-sign-stop"></a>
## sign_stop

<div class="yarn-node" data-title="sign_stop"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is a STOP sign. <span class="yarn-meta">#line:09ac224 </span></span>
<span class="yarn-cmd">&lt;&lt;card stop_sign&gt;&gt;</span>
<span class="yarn-line">Stop all the way. <span class="yarn-meta">#line:0fde84a </span></span>
<span class="yarn-line">Look both ways. <span class="yarn-meta">#line:0758dbd </span></span>


</code></pre></div>

<a id="ys-node-restart-level"></a>
## restart_level

<div class="yarn-node" data-title="restart_level"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You did not pay attention. Try again! <span class="yarn-meta">#line:0eed8ec </span></span>

</code></pre></div>

<a id="ys-node-sign-danger"></a>
## sign_danger

<div class="yarn-node" data-title="sign_danger"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is a DANGER sign. <span class="yarn-meta">#line:0a57038 </span></span>
<span class="yarn-cmd">&lt;&lt;card danger_sign&gt;&gt;</span>
<span class="yarn-line">It means slow down. <span class="yarn-meta">#line:02cfe75 </span></span>
<span class="yarn-line">Something may be on the road. <span class="yarn-meta">#line:07915e7 </span></span>

</code></pre></div>

<a id="ys-node-street-lights"></a>
## street_lights

<div class="yarn-node" data-title="street_lights"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">These are traffic lights. <span class="yarn-meta">#line:0355437 </span></span>
<span class="yarn-cmd">&lt;&lt;card traffic_lights&gt;&gt;</span>
<span class="yarn-line">RED - STOP <span class="yarn-meta">#line:0cb35d6</span></span>
<span class="yarn-line">YELLOW - SLOW <span class="yarn-meta">#line:0b0c56c </span></span>
<span class="yarn-line">GREEN - GO <span class="yarn-meta">#line:0c87bce </span></span>

</code></pre></div>

<a id="ys-node-zebra-crossing"></a>
## zebra_crossing

<div class="yarn-node" data-title="zebra_crossing"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">These white stripes are a zebra crossing. <span class="yarn-meta">#line:02fbb57 </span></span>
<span class="yarn-cmd">&lt;&lt;card zebra_crossing&gt;&gt;</span>
<span class="yarn-line">It gives pedestrians the right of way. <span class="yarn-meta">#line:085d81b </span></span>
<span class="yarn-line">Slow down near it. <span class="yarn-meta">#line:01cd74f </span></span>
<span class="yarn-line">Stop if someone wants to cross. <span class="yarn-meta">#line:04e20a8 </span></span>

</code></pre></div>

<a id="ys-node-danger-signs"></a>
## danger_signs

<div class="yarn-node" data-title="danger_signs"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">They are triangles. <span class="yarn-meta">#line:08c3073 </span></span>

</code></pre></div>

<a id="ys-node-bravo"></a>
## bravo

<div class="yarn-node" data-title="bravo"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You made it! <span class="yarn-meta">#line:03b112c </span></span>
<span class="yarn-line">You followed the rules. <span class="yarn-meta">#line:0662662 </span></span>
<span class="yarn-line">You are great at street safety! <span class="yarn-meta">#line:0c88cfb </span></span>

</code></pre></div>

<a id="ys-node-spawned-pedestrian"></a>
## spawned_pedestrian

<div class="yarn-node" data-title="spawned_pedestrian"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: pedestrian </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Always look both ways before crossing the street! <span class="yarn-meta">#line:0d65c60 </span></span>
<span class="yarn-line">Never run across the street, even if you're in a hurry! <span class="yarn-meta">#line:084a760 </span></span>
<span class="yarn-line">Always use a crosswalk or pedestrian crossing when available! <span class="yarn-meta">#line:07f8272 </span></span>
<span class="yarn-line">Wear bright or reflective clothing when walking at night! <span class="yarn-meta">#line:0587fd6 </span></span>
<span class="yarn-line">Never use your phone or headphones when crossing the street! <span class="yarn-meta">#line:044f000 </span></span>

</code></pre></div>

<a id="ys-node-spawned-race-lemans"></a>
## spawned_race_lemans

<div class="yarn-node" data-title="spawned_race_lemans"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: race </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Le Mans race lasts 24 hours! <span class="yarn-meta">#line:02e7a89 </span></span>
<span class="yarn-line">Race cars go fast on the track, not on streets. <span class="yarn-meta">#line:0518750 </span></span>
<span class="yarn-line">Speed is for the circuit. <span class="yarn-meta">#line:02e3d19 </span></span>
<span class="yarn-line">In the streets we go slowly. <span class="yarn-meta">#line:02f57a4 </span></span>

</code></pre></div>

<a id="ys-node-spawned-kids-walking"></a>
## spawned_kids_walking

<div class="yarn-node" data-title="spawned_kids_walking"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">We walk on the sidewalk. <span class="yarn-meta">#line:0999d95 </span></span>
<span class="yarn-line">We stop and look before we cross. <span class="yarn-meta">#line:04b17e0 </span></span>
<span class="yarn-line">We wait for green at the lights. <span class="yarn-meta">#line:0b0fe8c </span></span>
<span class="yarn-line">We look left and right for cars. <span class="yarn-meta">#line:0bbc9ed </span></span>
<span class="yarn-line">We do not play in the street. <span class="yarn-meta">#line:08097b8 </span></span>

</code></pre></div>


