---
title: Poruszaj się bezpiecznie po mieście (fr_04) - Script
hide:
---

# Poruszaj się bezpiecznie po mieście (fr_04) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-04-road-safety-les-mans/40/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1892167235#gid=1892167235)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_04%20Le%20Mans%20Streets/FR_04%20Le%20Mans%20Streets%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_04 | Road Safety (Les Mans)</span>
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
<span class="yarn-line">Witamy w Le Mans!</span> <span class="yarn-meta">#line:0a4f3e1</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetna robota!</span> <span class="yarn-meta">#line:08cd696 </span>
<span class="yarn-line">Użyłeś znaków, aby zachować bezpieczeństwo.</span> <span class="yarn-meta">#line:0298d64 </span>
<span class="yarn-line">Bezpieczeństwo na drodze może być fajne!</span> <span class="yarn-meta">#line:001499f </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Narysuj mapę ulic w pobliżu swojego domu!</span> <span class="yarn-meta">#line:0acd0a1 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-friend-intro"></a>

## talk_friend_intro

<div class="yarn-node" data-title="talk_friend_intro">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To jest wyścig samochodowy Le Mans.</span> <span class="yarn-meta">#line:02f370e </span>
<span class="yarn-line">Samochody są bardzo szybkie!</span> <span class="yarn-meta">#line:07f77f0 </span>
<span class="yarn-line">Tylko osoby dorosłe mogą nimi jeździć.</span> <span class="yarn-meta">#line:0d0cd3f </span>
<span class="yarn-line">Te samochody wyścigowe są takie szybkie!</span> <span class="yarn-meta">#line:0e54cfc </span>
<span class="yarn-line">Tylko na torze możesz jechać tak szybko.</span> <span class="yarn-meta">#line:053328c </span>
<span class="yarn-line">Na ulicach miast poruszamy się powoli.</span> <span class="yarn-meta">#line:0b70620 </span>
<span class="yarn-line">Czas wracać do domu.</span> <span class="yarn-meta">#line:06e4b9d </span>
<span class="yarn-cmd">&lt;&lt;jump talk_friend_scooter&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-friend-scooter"></a>

## talk_friend_scooter

<div class="yarn-node" data-title="talk_friend_scooter">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wyścig się skończył. Wracaj do domu pociągiem.</span> <span class="yarn-meta">#line:0b834f8 </span>
<span class="yarn-line">Zabieram skuter na stację.</span> <span class="yarn-meta">#line:00f2f65 </span>
<span class="yarn-cmd">&lt;&lt;card scooter&gt;&gt;</span>
<span class="yarn-line">Poczekam tam.</span> <span class="yarn-meta">#line:0425fb5 </span>
<span class="yarn-line">Spójrz na znaki.</span> <span class="yarn-meta">#line:0b5ee19 </span>
<span class="yarn-cmd">&lt;&lt;action GIVE_SCOOTER&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump task_reach_station&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-reach-station"></a>

## task_reach_station

<div class="yarn-node" data-title="task_reach_station">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=TUTOR, task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dotrzyj bezpiecznie na stację kolejową.</span> <span class="yarn-meta">#line:fr04_task_station</span>
<span class="yarn-line">Zatrzymaj się na czerwonym. Jedź na zielonym.</span> <span class="yarn-meta">#line:fr04_task_station_2</span>

</code>
</pre>
</div>

<a id="ys-node-sign-stop"></a>

## sign_stop

<div class="yarn-node" data-title="sign_stop">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To jest znak STOP.</span> <span class="yarn-meta">#line:09ac224 </span>
<span class="yarn-cmd">&lt;&lt;card stop_sign&gt;&gt;</span>
<span class="yarn-line">Zatrzymaj się całkowicie.</span> <span class="yarn-meta">#line:0fde84a </span>
<span class="yarn-line">Spójrz w obie strony.</span> <span class="yarn-meta">#line:0758dbd </span>


</code>
</pre>
</div>

<a id="ys-node-restart-level"></a>

## restart_level

<div class="yarn-node" data-title="restart_level">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nie zwróciłeś uwagi. Spróbuj jeszcze raz!</span> <span class="yarn-meta">#line:0eed8ec </span>

</code>
</pre>
</div>

<a id="ys-node-sign-danger"></a>

## sign_danger

<div class="yarn-node" data-title="sign_danger">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To znak NIEBEZPIECZEŃSTWO.</span> <span class="yarn-meta">#line:0a57038 </span>
<span class="yarn-cmd">&lt;&lt;card danger_sign&gt;&gt;</span>
<span class="yarn-line">Oznacza to zwolnienie.</span> <span class="yarn-meta">#line:02cfe75 </span>
<span class="yarn-line">Coś może być na drodze.</span> <span class="yarn-meta">#line:07915e7 </span>

</code>
</pre>
</div>

<a id="ys-node-street-lights"></a>

## street_lights

<div class="yarn-node" data-title="street_lights">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To są sygnalizacje świetlne.</span> <span class="yarn-meta">#line:0355437 </span>
<span class="yarn-cmd">&lt;&lt;card traffic_lights&gt;&gt;</span>
<span class="yarn-line">CZERWONY - STOP</span> <span class="yarn-meta">#line:0cb35d6</span>
<span class="yarn-line">ŻÓŁTY - WOLNY</span> <span class="yarn-meta">#line:0b0c56c </span>
<span class="yarn-line">ZIELONY - START</span> <span class="yarn-meta">#line:0c87bce </span>

</code>
</pre>
</div>

<a id="ys-node-zebra-crossing"></a>

## zebra_crossing

<div class="yarn-node" data-title="zebra_crossing">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags:  </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Te białe paski to przejście dla pieszych.</span> <span class="yarn-meta">#line:02fbb57 </span>
<span class="yarn-cmd">&lt;&lt;card zebra_crossing&gt;&gt;</span>
<span class="yarn-line">Daje pieszym pierwszeństwo przejazdu.</span> <span class="yarn-meta">#line:085d81b </span>
<span class="yarn-line">Zwolnij w jego pobliżu.</span> <span class="yarn-meta">#line:01cd74f </span>
<span class="yarn-line">Zatrzymaj się jeśli ktoś chce przejść.</span> <span class="yarn-meta">#line:04e20a8 </span>

</code>
</pre>
</div>

<a id="ys-node-danger-signs"></a>

## danger_signs

<div class="yarn-node" data-title="danger_signs">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To są trójkąty.</span> <span class="yarn-meta">#line:08c3073 </span>

</code>
</pre>
</div>

<a id="ys-node-bravo"></a>

## bravo

<div class="yarn-node" data-title="bravo">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Udało Ci się!</span> <span class="yarn-meta">#line:03b112c </span>
<span class="yarn-line">Postępowałeś zgodnie z zasadami.</span> <span class="yarn-meta">#line:0662662 </span>
<span class="yarn-line">Świetnie radzisz sobie z bezpieczeństwem na ulicy!</span> <span class="yarn-meta">#line:0c88cfb </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-pedestrian"></a>

## spawned_pedestrian

<div class="yarn-node" data-title="spawned_pedestrian">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: pedestrian </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Zawsze rozglądaj się w obie strony zanim przejdziesz przez ulicę!</span> <span class="yarn-meta">#line:0d65c60 </span>
<span class="yarn-line">Nigdy nie przebiegaj przez ulicę, nawet jeśli się spieszysz!</span> <span class="yarn-meta">#line:084a760 </span>
<span class="yarn-line">Zawsze korzystaj z przejścia dla pieszych lub chodnika, jeśli jest ono dostępne!</span> <span class="yarn-meta">#line:07f8272 </span>
<span class="yarn-line">Noś jasne i odblaskowe ubrania, gdy spacerujesz nocą!</span> <span class="yarn-meta">#line:0587fd6 </span>
<span class="yarn-line">Nigdy nie używaj telefonu ani słuchawek przechodząc przez ulicę!</span> <span class="yarn-meta">#line:044f000 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-race-lemans"></a>

## spawned_race_lemans

<div class="yarn-node" data-title="spawned_race_lemans">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: race </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wyścig Le Mans trwa 24 godziny!</span> <span class="yarn-meta">#line:02e7a89 </span>
<span class="yarn-line">Samochody wyścigowe są szybkie na torze, a nie na ulicach.</span> <span class="yarn-meta">#line:0518750 </span>
<span class="yarn-line">Prędkość jest istotna dla toru.</span> <span class="yarn-meta">#line:02e3d19 </span>
<span class="yarn-line">Na ulicach chodzimy powoli.</span> <span class="yarn-meta">#line:02f57a4 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kids-walking"></a>

## spawned_kids_walking

<div class="yarn-node" data-title="spawned_kids_walking">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Idziemy po chodniku.</span> <span class="yarn-meta">#line:0999d95 </span>
<span class="yarn-line">Zatrzymujemy się i rozglądamy zanim przejdziemy.</span> <span class="yarn-meta">#line:04b17e0 </span>
<span class="yarn-line">Czekamy na zielone światło.</span> <span class="yarn-meta">#line:0b0fe8c </span>
<span class="yarn-line">Rozglądamy się na lewo i prawo za samochodami.</span> <span class="yarn-meta">#line:0bbc9ed </span>
<span class="yarn-line">Nie bawimy się na ulicy.</span> <span class="yarn-meta">#line:08097b8 </span>

</code>
</pre>
</div>


