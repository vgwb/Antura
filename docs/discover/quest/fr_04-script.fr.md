---
title: Se déplacer en ville en toute sécurité (fr_04) - Script
hide:
---

# Se déplacer en ville en toute sécurité (fr_04) - Script
[Quest Index](./index.fr.md) - Language: [english](./fr_04-script.md) - french - [polish](./fr_04-script.pl.md) - [italian](./fr_04-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-04-road-safety-les-mans/40/1)  
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
<span class="yarn-line">Bienvenue au Mans ! <span class="yarn-meta">#line:0a4f3e1</span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Excellent travail ! <span class="yarn-meta">#line:08cd696 </span></span>
<span class="yarn-line">Vous avez utilisé les panneaux pour rester en sécurité. <span class="yarn-meta">#line:0298d64 </span></span>
<span class="yarn-line">La sécurité routière peut être amusante ! <span class="yarn-meta">#line:001499f </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dessinez une carte des rues autour de votre maison ! <span class="yarn-meta">#line:0acd0a1 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-friend-intro"></a>
## talk_friend_intro

<div class="yarn-node" data-title="talk_friend_intro"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">C'est la course automobile du Mans. <span class="yarn-meta">#line:02f370e </span></span>
<span class="yarn-line">Les voitures sont très rapides ! <span class="yarn-meta">#line:07f77f0 </span></span>
<span class="yarn-line">Seuls les adultes peuvent les conduire. <span class="yarn-meta">#line:0d0cd3f </span></span>
<span class="yarn-line">Ces voitures de course sont si rapides ! <span class="yarn-meta">#line:0e54cfc </span></span>
<span class="yarn-line">Sur la piste, on ne peut aller qu'à cette vitesse. <span class="yarn-meta">#line:053328c </span></span>
<span class="yarn-line">Dans les rues de la ville, nous roulons lentement. <span class="yarn-meta">#line:0b70620 </span></span>
<span class="yarn-line">Il est temps de rentrer à la maison. <span class="yarn-meta">#line:06e4b9d </span></span>
<span class="yarn-cmd">&lt;&lt;jump talk_friend_scooter&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-friend-scooter"></a>
## talk_friend_scooter

<div class="yarn-node" data-title="talk_friend_scooter"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La course est terminée. Rentrez chez vous en train. <span class="yarn-meta">#line:0b834f8 </span></span>
<span class="yarn-line">J'emmène mon scooter à la gare. <span class="yarn-meta">#line:00f2f65 </span></span>
<span class="yarn-cmd">&lt;&lt;card scooter&gt;&gt;</span>
<span class="yarn-line">J'attendrai là-bas. <span class="yarn-meta">#line:0425fb5 </span></span>
<span class="yarn-line">Regardez les panneaux. <span class="yarn-meta">#line:0b5ee19 </span></span>
<span class="yarn-cmd">&lt;&lt;action GIVE_SCOOTER&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump task_reach_station&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-reach-station"></a>
## task_reach_station

<div class="yarn-node" data-title="task_reach_station"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Arrivez à la gare en toute sécurité. <span class="yarn-meta">#line:fr04_task_station</span></span>
<span class="yarn-line">Arrêtez-vous au feu rouge. Continuez au feu vert. <span class="yarn-meta">#line:fr04_task_station_2</span></span>

</code></pre></div>

<a id="ys-node-sign-stop"></a>
## sign_stop

<div class="yarn-node" data-title="sign_stop"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ceci est un panneau STOP. <span class="yarn-meta">#line:09ac224 </span></span>
<span class="yarn-cmd">&lt;&lt;card stop_sign&gt;&gt;</span>
<span class="yarn-line">Arrêtez-vous complètement. <span class="yarn-meta">#line:0fde84a </span></span>
<span class="yarn-line">Regardez dans les deux sens. <span class="yarn-meta">#line:0758dbd </span></span>


</code></pre></div>

<a id="ys-node-restart-level"></a>
## restart_level

<div class="yarn-node" data-title="restart_level"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vous n'avez pas fait attention. Réessayez ! <span class="yarn-meta">#line:0eed8ec </span></span>

</code></pre></div>

<a id="ys-node-sign-danger"></a>
## sign_danger

<div class="yarn-node" data-title="sign_danger"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ceci est un panneau de DANGER. <span class="yarn-meta">#line:0a57038 </span></span>
<span class="yarn-cmd">&lt;&lt;card danger_sign&gt;&gt;</span>
<span class="yarn-line">Cela signifie ralentir. <span class="yarn-meta">#line:02cfe75 </span></span>
<span class="yarn-line">Il se peut qu'il y ait quelque chose sur la route. <span class="yarn-meta">#line:07915e7 </span></span>

</code></pre></div>

<a id="ys-node-street-lights"></a>
## street_lights

<div class="yarn-node" data-title="street_lights"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ce sont des feux de circulation. <span class="yarn-meta">#line:0355437 </span></span>
<span class="yarn-cmd">&lt;&lt;card traffic_lights&gt;&gt;</span>
<span class="yarn-line">ROUGE - STOP <span class="yarn-meta">#line:0cb35d6</span></span>
<span class="yarn-line">JAUNE - LENT <span class="yarn-meta">#line:0b0c56c </span></span>
<span class="yarn-line">VERT - ALLER <span class="yarn-meta">#line:0c87bce </span></span>

</code></pre></div>

<a id="ys-node-zebra-crossing"></a>
## zebra_crossing

<div class="yarn-node" data-title="zebra_crossing"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ces rayures blanches sont un passage piéton. <span class="yarn-meta">#line:02fbb57 </span></span>
<span class="yarn-cmd">&lt;&lt;card zebra_crossing&gt;&gt;</span>
<span class="yarn-line">Il donne la priorité aux piétons. <span class="yarn-meta">#line:085d81b </span></span>
<span class="yarn-line">Ralentissez à proximité. <span class="yarn-meta">#line:01cd74f </span></span>
<span class="yarn-line">Arrêtez-vous si quelqu'un veut traverser. <span class="yarn-meta">#line:04e20a8 </span></span>

</code></pre></div>

<a id="ys-node-danger-signs"></a>
## danger_signs

<div class="yarn-node" data-title="danger_signs"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ce sont des triangles. <span class="yarn-meta">#line:08c3073 </span></span>

</code></pre></div>

<a id="ys-node-bravo"></a>
## bravo

<div class="yarn-node" data-title="bravo"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tu as réussi ! <span class="yarn-meta">#line:03b112c </span></span>
<span class="yarn-line">Vous avez suivi les règles. <span class="yarn-meta">#line:0662662 </span></span>
<span class="yarn-line">Vous êtes excellent en matière de sécurité routière ! <span class="yarn-meta">#line:0c88cfb </span></span>

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
<span class="yarn-line">Regardez toujours des deux côtés avant de traverser la rue ! <span class="yarn-meta">#line:0d65c60 </span></span>
<span class="yarn-line">Ne traversez jamais la rue en courant, même si vous êtes pressé ! <span class="yarn-meta">#line:084a760 </span></span>
<span class="yarn-line">Utilisez toujours un passage pour piétons ou un passage pour piétons lorsqu'il est disponible ! <span class="yarn-meta">#line:07f8272 </span></span>
<span class="yarn-line">Portez des vêtements clairs ou réfléchissants lorsque vous marchez la nuit ! <span class="yarn-meta">#line:0587fd6 </span></span>
<span class="yarn-line">N'utilisez jamais votre téléphone ou vos écouteurs lorsque vous traversez la rue ! <span class="yarn-meta">#line:044f000 </span></span>

</code></pre></div>

<a id="ys-node-spawned-race-lemans"></a>
## spawned_race_lemans

<div class="yarn-node" data-title="spawned_race_lemans"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: race </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La course du Mans dure 24 heures ! <span class="yarn-meta">#line:02e7a89 </span></span>
<span class="yarn-line">Les voitures de course vont vite sur la piste, pas dans la rue. <span class="yarn-meta">#line:0518750 </span></span>
<span class="yarn-line">La vitesse est pour le circuit. <span class="yarn-meta">#line:02e3d19 </span></span>
<span class="yarn-line">Dans les rues, nous avançons lentement. <span class="yarn-meta">#line:02f57a4 </span></span>

</code></pre></div>

<a id="ys-node-spawned-kids-walking"></a>
## spawned_kids_walking

<div class="yarn-node" data-title="spawned_kids_walking"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nous marchons sur le trottoir. <span class="yarn-meta">#line:0999d95 </span></span>
<span class="yarn-line">Nous nous arrêtons et regardons avant de traverser. <span class="yarn-meta">#line:04b17e0 </span></span>
<span class="yarn-line">Nous attendons le feu vert. <span class="yarn-meta">#line:0b0fe8c </span></span>
<span class="yarn-line">Nous regardons à gauche et à droite pour voir les voitures. <span class="yarn-meta">#line:0bbc9ed </span></span>
<span class="yarn-line">Nous ne jouons pas dans la rue. <span class="yarn-meta">#line:08097b8 </span></span>

</code></pre></div>


