---
title: Move around in a city in safety (fr_04) - Script
hide:
---

# Move around in a city in safety (fr_04) - Script
[Quest Index](./index.md) - Language: english - [french](./fr_04-script.fr.md) - [polish](./fr_04-script.pl.md) - [italian](./fr_04-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-04-road-safety-les-mans/40/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1892167235#gid=1892167235)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_04%20Le%20Mans%20Streets/FR_04%20Le%20Mans%20Streets%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// FR_04 LE_MANS - Racing &amp; Road Safety</span>
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

<a id="ys-node-talk-friend"></a>
## talk_friend

<div class="yarn-node" data-title="talk_friend"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the race of Le Mans <span class="yarn-meta">#line:02f370e </span></span>
<span class="yarn-line">Cars are very fast! <span class="yarn-meta">#line:07f77f0 </span></span>
<span class="yarn-line">Only adults can drive them. <span class="yarn-meta">#line:0d0cd3f </span></span>
<span class="yarn-line">The race is finished. Go back home by train! <span class="yarn-meta">#line:0b834f8 </span></span>
<span class="yarn-line">Wow, those race cars are fast! <span class="yarn-meta">#line:0e54cfc </span></span>
<span class="yarn-line">But remember, the racetrack is the only place to go that fast. <span class="yarn-meta">#line:053328c </span></span>
<span class="yarn-line">On city streets, safety is what's most important. <span class="yarn-meta">#line:0b70620 </span></span>
<span class="yarn-line">It's time to go home! <span class="yarn-meta">#line:06e4b9d </span></span>
<span class="yarn-line">Here, take my scooter to get to the train station. <span class="yarn-meta">#line:00f2f65 </span></span>
<span class="yarn-line">I'll wait for you there. <span class="yarn-meta">#line:0425fb5 </span></span>
<span class="yarn-line">Be very careful and pay attention to the signs! <span class="yarn-meta">#line:0b5ee19 </span></span>

</code></pre></div>

<a id="ys-node-sign-stop"></a>
## sign_stop

<div class="yarn-node" data-title="sign_stop"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is a STOP sign. <span class="yarn-meta">#line:09ac224 </span></span>
<span class="yarn-line">It means you must always come to a complete stop <span class="yarn-meta">#line:0fde84a </span></span>
<span class="yarn-line">and look both ways before you can go. <span class="yarn-meta">#line:0758dbd </span></span>


</code></pre></div>

<a id="ys-node-restart-level"></a>
## restart_level

<div class="yarn-node" data-title="restart_level"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">you didn't pay attention! retry! <span class="yarn-meta">#line:0eed8ec </span></span>

</code></pre></div>

<a id="ys-node-sign-danger"></a>
## sign_danger

<div class="yarn-node" data-title="sign_danger"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is a DANGER sign! <span class="yarn-meta">#line:0a57038 </span></span>
<span class="yarn-line">It warns you to be careful and slow down <span class="yarn-meta">#line:02cfe75 </span></span>
<span class="yarn-line">because there might be something unexpected on the road. <span class="yarn-meta">#line:07915e7 </span></span>

</code></pre></div>

<a id="ys-node-street-lights"></a>
## street_lights

<div class="yarn-node" data-title="street_lights"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">These are traffic lights. <span class="yarn-meta">#line:0355437 </span></span>
<span class="yarn-line">RED: STOP <span class="yarn-meta">#line:0cb35d6</span></span>
<span class="yarn-line">YELLOW: SLOW DOWN <span class="yarn-meta">#line:0b0c56c </span></span>
<span class="yarn-line">GREEN: GO! <span class="yarn-meta">#line:0c87bce </span></span>

</code></pre></div>

<a id="ys-node-zebra-crossing"></a>
## zebra_crossing

<div class="yarn-node" data-title="zebra_crossing"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR, </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">These white stripes are a zebra crossing. <span class="yarn-meta">#line:02fbb57 </span></span>
<span class="yarn-line">It gives pedestrians the right way. <span class="yarn-meta">#line:085d81b </span></span>
<span class="yarn-line">So you must slow down when approaching it <span class="yarn-meta">#line:01cd74f </span></span>
<span class="yarn-line">and stop if someone is trying to cross! <span class="yarn-meta">#line:04e20a8 </span></span>

</code></pre></div>

<a id="ys-node-danger-signs"></a>
## danger_signs

<div class="yarn-node" data-title="danger_signs"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">They are TRIANGLE <span class="yarn-meta">#line:08c3073 </span></span>

</code></pre></div>

<a id="ys-node-bravo"></a>
## bravo

<div class="yarn-node" data-title="bravo"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">=&gt; You made it! <span class="yarn-meta">#line:03b112c </span></span>
<span class="yarn-line">=&gt; you followed all the rules perfectly. <span class="yarn-meta">#line:0662662 </span></span>
<span class="yarn-line">=&gt; You're a street safety expert! Have a good trip! <span class="yarn-meta">#line:0c88cfb </span></span>

</code></pre></div>


