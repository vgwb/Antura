---
title: Il grande salvataggio dei nani di Breslavia (pl_02) - Script
hide:
---

# Il grande salvataggio dei nani di Breslavia (pl_02) - Script
[Quest Index](./index.it.md) - Language: [english](./pl_02-script.md) - [french](./pl_02-script.fr.md) - [polish](./pl_02-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-02-the-great-wroclaw-dwarf-rescue/33/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1721014062#gid=1721014062)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_02%20Wroclaw%20Dwarves/PL_02%20Wroclaw%20Dwarves%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// pl_02 | Dwarves (Wroclaw)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// ---------------------------------------------</span>
<span class="yarn-header-dim">// WANTED:</span>
<span class="yarn-header-dim">// - wroclaw_dwarf_statue (cultural symbol)</span>
<span class="yarn-header-dim">// - PolishDwarf (cultural tradition)</span>
<span class="yarn-header-dim">// - wroclaw_market_square (city center)</span>
<span class="yarn-header-dim">// - WroclawOldTownHall (historical building)</span>
<span class="yarn-header-dim">// - WroclawCathedral (religious architecture)</span>
<span class="yarn-header-dim">// - WroclawSkyTower (modern landmark)</span>
<span class="yarn-header-dim">tags: type=Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $found = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $need = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_1_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_2_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_3_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_4_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_5_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_6_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_7_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_8_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_9_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_10_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $top_met = false&gt;&gt;</span>

<span class="yarn-line">Benvenuti a Breslavia! <span class="yarn-meta">#line:023b330 </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questa missione è completata. <span class="yarn-meta">#line:04eb3f4 </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Perché non disegni il tuo Nano? <span class="yarn-meta">#line:0e7c76a </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npg-task-dwarves"></a>
## npg_task_dwarves

<div class="yarn-node" data-title="npg_task_dwarves"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oh no! Antura è bloccato in cima alla Sky Tower. <span class="yarn-meta">#line:0a18e6c </span></span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_sky_tower zoom&gt;&gt;</span>
<span class="yarn-line">I nani bloccarono l'ascensore. <span class="yarn-meta">#line:08bcfba </span></span>
<span class="yarn-line">Trova 10 nani nei dintorni di Breslavia. Ci aiuteranno. <span class="yarn-meta">#line:07471d4 </span></span>
<span class="yarn-cmd">&lt;&lt;card wroklaw_map zoom&gt;&gt;</span>
<span class="yarn-line">Esplora la città. Parla con ogni nano che incontri. <span class="yarn-meta">#line:09148d9 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_DWARVES task_dwarves_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-dwarves-done"></a>
## task_dwarves_done

<div class="yarn-node" data-title="task_dwarves_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai trovato tutti i nani! <span class="yarn-meta">#line:03df148 </span></span>
<span class="yarn-line">Adesso sali con l'ascensore! <span class="yarn-meta">#line:0364c03 </span></span>

</code></pre></div>

<a id="ys-node-dwarf-1-origin"></a>
## dwarf_1_origin

<div class="yarn-node" data-title="dwarf_1_origin"><pre class="yarn-code"><code><span class="yarn-header-dim">// 1) The origin of dwarves</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_dwarf_statue zoom&gt;&gt;</span>
<span class="yarn-line">Breslavia è la città degli gnomi. Li puoi trovare ovunque. <span class="yarn-meta">#line:007686b </span></span>
<span class="yarn-line">Sono piccoli e gentili. Amano fare scherzi. <span class="yarn-meta">#line:0143908 </span></span>
<span class="yarn-line">Vuoi giocare con me? <span class="yarn-meta">#line:0e460fe </span></span>
<span class="yarn-line">SÌ <span class="yarn-meta">#line:02a4fb8 </span></span>
	<span class="yarn-cmd">&lt;&lt;activity jigsaw_dwarf_origin dwarf_1_origin_done&gt;&gt;</span>
<span class="yarn-line">NO <span class="yarn-meta">#line:0f7786d </span></span>
<span class="yarn-line">	Oh, ok. Forse più tardi. <span class="yarn-meta">#line:0e236f0 </span></span>
	<span class="yarn-cmd">&lt;&lt;SetActive dwarf_1 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-1-origin-done"></a>
## dwarf_1_origin_done

<div class="yarn-node" data-title="dwarf_1_origin_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ottimo lavoro! Hai risolto il puzzle. <span class="yarn-meta">#line:0a87d03 </span></span>
<span class="yarn-line">Adesso vengo con te. <span class="yarn-meta">#line:0f49b50 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_1_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_1 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-2-town-hall"></a>
## dwarf_2_town_hall

<div class="yarn-node" data-title="dwarf_2_town_hall"><pre class="yarn-code"><code><span class="yarn-header-dim">// 2) Old Town Hall</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset wroclaw_old_town_hall&gt;&gt;</span>
<span class="yarn-line">Questo è il vecchio municipio. Qui lavorano i dirigenti della città. <span class="yarn-meta">#line:02cbbf0 </span></span>
<span class="yarn-line">Le riunioni si svolgono all'interno. L'orologio è molto vecchio. <span class="yarn-meta">#line:0ca6131 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_town_hall dwarf_2_town_hall_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-2-town-hall-done"></a>
## dwarf_2_town_hall_done

<div class="yarn-node" data-title="dwarf_2_town_hall_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ben fatto! <span class="yarn-meta">#line:004110a </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_2_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_2 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-3-cathedral"></a>
## dwarf_3_cathedral

<div class="yarn-node" data-title="dwarf_3_cathedral"><pre class="yarn-code"><code><span class="yarn-header-dim">// 3) Cathedral with quiz</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card bishop_dwarf&gt;&gt;</span>
<span class="yarn-line">Questa è la cattedrale. È una chiesa grande e importante. <span class="yarn-meta">#line:00f2132 </span></span>
<span class="yarn-line">Sai cos'è una chiesa? <span class="yarn-meta">#line:080f821 </span></span>
<span class="yarn-line">SÌ <span class="yarn-meta">#line:07550a5 </span></span>
<span class="yarn-line">NO <span class="yarn-meta">#line:09e7452 </span></span>
 <span class="yarn-cmd">&lt;&lt;detour info_church&gt;&gt;</span>
<span class="yarn-line">Presenta alte torri e vetrate colorate. <span class="yarn-meta">#line:08871e4 </span></span>
<span class="yarn-cmd">&lt;&lt;jump dwarf_3_quiz&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-info-church"></a>
## info_church

<div class="yarn-node" data-title="info_church"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card church&gt;&gt;</span>
<span class="yarn-line">Una chiesa è un luogo in cui le persone pregano. <span class="yarn-meta">#line:0b1f4e1</span></span>

</code></pre></div>

<a id="ys-node-dwarf-3-quiz"></a>
## dwarf_3_quiz

<div class="yarn-node" data-title="dwarf_3_quiz"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cosa fanno le persone in una cattedrale? <span class="yarn-meta">#line:05ea00d </span></span>
<span class="yarn-line">Fanno la spesa. <span class="yarn-meta">#line:0c891cc </span></span>
<span class="yarn-line">   Non qui. Riprova. <span class="yarn-meta">#line:0d84e58 </span></span>
   <span class="yarn-cmd">&lt;&lt;jump dwarf_3_quiz&gt;&gt;</span>
<span class="yarn-line">Pregano. <span class="yarn-meta">#line:01ac8e6 </span></span>
<span class="yarn-line">   Sì. Puoi collezionare il nano. <span class="yarn-meta">#line:071c92a </span></span>
   <span class="yarn-cmd">&lt;&lt;jump dwarf_3_activity&gt;&gt;</span>
<span class="yarn-line">Pilotano aerei. <span class="yarn-meta">#line:09d9b34 </span></span>
<span class="yarn-line">   No. Riprova. <span class="yarn-meta">#line:06ebc0c </span></span>
   <span class="yarn-cmd">&lt;&lt;jump dwarf_3_quiz&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-3-activity"></a>
## dwarf_3_activity

<div class="yarn-node" data-title="dwarf_3_activity"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Facciamo un gioco. <span class="yarn-meta">#line:0c5d8d3 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_wroclaw_cathedral dwarf_3_activity_done&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-dwarf-3-activity-done"></a>
## dwarf_3_activity_done

<div class="yarn-node" data-title="dwarf_3_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Fantastico! È un altro nano. <span class="yarn-meta">#line:04886ba </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_3_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_3 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-4-zoo"></a>
## dwarf_4_zoo

<div class="yarn-node" data-title="dwarf_4_zoo"><pre class="yarn-code"><code><span class="yarn-header-dim">// 4) ZOO</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Visitate lo zoo di Breslavia. È il più grande della Polonia. <span class="yarn-meta">#line:0198e80 </span></span>
<span class="yarn-line">Ospita molti animali provenienti da tutto il mondo. <span class="yarn-meta">#line:01b020e </span></span>
<span class="yarn-cmd">&lt;&lt;jump dwarf_4_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-4-activity"></a>
## dwarf_4_activity

<div class="yarn-node" data-title="dwarf_4_activity"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Facciamo un gioco. <span class="yarn-meta">#line:0f921e7 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_wroclaw_zoo dwarf_4_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-4-activity-done"></a>
## dwarf_4_activity_done

<div class="yarn-node" data-title="dwarf_4_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Grande! <span class="yarn-meta">#line:0c852a7 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_4_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_4 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-5"></a>
## dwarf_5

<div class="yarn-node" data-title="dwarf_5"><pre class="yarn-code"><code><span class="yarn-header-dim">// 5) Centennial Hall</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La Centennial Hall è enorme. All'interno si possono vedere degli spettacoli. <span class="yarn-meta">#line:0ebb952 </span></span>
<span class="yarn-line">Il tetto sembra una cupola gigante. <span class="yarn-meta">#line:0f4189b </span></span>
<span class="yarn-cmd">&lt;&lt;jump dwarf_5_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-5-activity"></a>
## dwarf_5_activity

<div class="yarn-node" data-title="dwarf_5_activity"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Facciamo un gioco. <span class="yarn-meta">#line:06b4a16 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle centennial_hall dwarf_5_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-5-activity-done"></a>
## dwarf_5_activity_done

<div class="yarn-node" data-title="dwarf_5_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Grande! <span class="yarn-meta">#line:0abf458 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_5_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_5 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-6"></a>
## dwarf_6

<div class="yarn-node" data-title="dwarf_6"><pre class="yarn-code"><code><span class="yarn-header-dim">// 6) Multimedia Fountain</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Qui l'acqua danza con musica e luci. <span class="yarn-meta">#line:0daf76d </span></span>
<span class="yarn-line">Gli spettacoli sono bellissimi nelle notti d'estate. <span class="yarn-meta">#line:006ed40 </span></span>
<span class="yarn-cmd">&lt;&lt;jump dwarf_6_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-6-activity"></a>
## dwarf_6_activity

<div class="yarn-node" data-title="dwarf_6_activity"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Facciamo un gioco. <span class="yarn-meta">#line:0cdb4f5 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_multimedia_fountain dwarf_6_activity_done&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-dwarf-6-activity-done"></a>
## dwarf_6_activity_done

<div class="yarn-node" data-title="dwarf_6_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Grande! <span class="yarn-meta">#line:0818eec </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_6_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_6 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-7"></a>
## dwarf_7

<div class="yarn-node" data-title="dwarf_7"><pre class="yarn-code"><code><span class="yarn-header-dim">// 7) Panorama Racławicka</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Un gigantesco dipinto raffigurante una battaglia ti circonda. <span class="yarn-meta">#line:0de7f17 </span></span>
<span class="yarn-line">Ti trovi dentro la storia. <span class="yarn-meta">#line:0f8436b </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle panorama_raclawicka dwarf_7_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-8"></a>
## dwarf_8

<div class="yarn-node" data-title="dwarf_8"><pre class="yarn-code"><code><span class="yarn-header-dim">// 8) Olga Tokarczuk</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Olga Tokarczuk è una famosa scrittrice di Breslavia. <span class="yarn-meta">#line:0496de5 </span></span>
<span class="yarn-line">Ha vinto il premio Nobel per la letteratura. <span class="yarn-meta">#line:00ae354 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle olga_tokarczuk dwarf_8_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-9"></a>
## dwarf_9

<div class="yarn-node" data-title="dwarf_9"><pre class="yarn-code"><code><span class="yarn-header-dim">// 9) Sky Tower Plaza Dwarf</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questa è la piazza moderna vicino alla Sky Tower. <span class="yarn-meta">#line:096a9ee </span></span>
<span class="yarn-line">Qui le persone si incontrano per parlare e giocare. <span class="yarn-meta">#line:0c899a5 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle plaza_dwarf dwarf_9_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-10"></a>
## dwarf_10

<div class="yarn-node" data-title="dwarf_10"><pre class="yarn-code"><code><span class="yarn-header-dim">// 9) Sky Tower Dwarf</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La Sky Tower è molto alta. <span class="yarn-meta">#line:0ccd434 </span></span>
<span class="yarn-line">Dall'alto si può vedere molto lontano. <span class="yarn-meta">#line:0170463 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle sky_tower_dwarf dwarf_10_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-elevator-keymaster"></a>
## elevator_keymaster

<div class="yarn-node" data-title="elevator_keymaster"><pre class="yarn-code"><code><span class="yarn-header-dim">// Keymaster at the elevator</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Faccio la guardia all'ascensore. <span class="yarn-meta">#line:0dd986c </span></span>
&lt;&lt;if $found &lt; $need&gt;&gt;
<span class="yarn-line">	Hai trovato {0} / {1} nani. Continua a esplorare. <span class="yarn-meta">#line:08dfa28 </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Fantastico! Li hai trovati tutti. Apro la porta con la mia chiave. <span class="yarn-meta">#line:0fdc177 </span></span>
	<span class="yarn-cmd">&lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-elevator-up"></a>
## elevator_up

<div class="yarn-node" data-title="elevator_up"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L'ascensore sale. Ding! <span class="yarn-meta">#line:0abe973 </span></span>
<span class="yarn-line">La vista è bellissima. <span class="yarn-meta">#line:008b500 </span></span>

</code></pre></div>

<a id="ys-node-npg-rescue-top"></a>
## npg_rescue_top

<div class="yarn-node" data-title="npg_rescue_top"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=ANTURA</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $top_met&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_intro&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $top_met = true&gt;&gt;</span>
<span class="yarn-line">	AHhh Antura era qui. <span class="yarn-meta">#line:0708555 </span></span>
<span class="yarn-line">	Ma è semplicemente scomparso! <span class="yarn-meta">#line:0f710dd </span></span>
<span class="yarn-line">	Forse la prossima volta ci riuscirai! <span class="yarn-meta">#line:081c124 </span></span>
<span class="yarn-line">	Ma non è bellissima la vista? <span class="yarn-meta">#line:079ea46 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_intro&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-assessment-intro"></a>
## assessment_intro

<div class="yarn-node" data-title="assessment_intro"><pre class="yarn-code"><code><span class="yarn-header-dim">// Final Assessment</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vorresti rispondere a due brevi domande? <span class="yarn-meta">#line:07982da </span></span>
<span class="yarn-line">SÌ <span class="yarn-meta">#line:012a7d1 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-line">NO <span class="yarn-meta">#line:0d0b965 </span></span>
<span class="yarn-line">	Ok. Torna da me per terminare il gioco. <span class="yarn-meta">#line:0b51184 </span></span>

</code></pre></div>

<a id="ys-node-assessment-q1"></a>
## assessment_q1

<div class="yarn-node" data-title="assessment_q1"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: assessment, actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Qual è il simbolo di Breslavia? <span class="yarn-meta">#line:020c66a </span></span>
<span class="yarn-line">un cane <span class="yarn-meta">#line:03ffcee </span></span>
<span class="yarn-line">	Non questo. <span class="yarn-meta">#line:07344d2 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-line">una scimmia <span class="yarn-meta">#line:0760b20 </span></span>
<span class="yarn-line">	NO. <span class="yarn-meta">#line:0868c5e </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-line">un nano <span class="yarn-meta">#line:0972bf0 </span></span>
<span class="yarn-line">	Corretto! <span class="yarn-meta">#line:088b881 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-assessment-q2"></a>
## assessment_q2

<div class="yarn-node" data-title="assessment_q2"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: assessment, actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Breslavia è la ... città più grande della Polonia. <span class="yarn-meta">#line:09d303d </span></span>
<span class="yarn-line">Primo <span class="yarn-meta">#line:0ce9571 </span></span>
<span class="yarn-line">	NO. <span class="yarn-meta">#line:039419a </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-line">secondo <span class="yarn-meta">#line:0fea2e7 </span></span>
<span class="yarn-line">	Non questo. <span class="yarn-meta">#line:028148c </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-line">terzo <span class="yarn-meta">#line:0ebe219 </span></span>
<span class="yarn-line">	Esatto! Ben fatto. <span class="yarn-meta">#line:0802440 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-assessment-end"></a>
## assessment_end

<div class="yarn-node" data-title="assessment_end"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Fantastico! Hai completato la missione! <span class="yarn-meta">#line:0fc1bad </span></span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-7-activity-done"></a>
## dwarf_7_activity_done

<div class="yarn-node" data-title="dwarf_7_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">// Added completion nodes for dwarves 7-10</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buon lavoro! <span class="yarn-meta">#line:074a28b </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_7_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_7 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-8-activity-done"></a>
## dwarf_8_activity_done

<div class="yarn-node" data-title="dwarf_8_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buon lavoro! <span class="yarn-meta">#line:0ac938a </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_8_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_8 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-9-activity-done"></a>
## dwarf_9_activity_done

<div class="yarn-node" data-title="dwarf_9_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buon lavoro! <span class="yarn-meta">#line:0f70de9 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_9_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_9 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-10-activity-done"></a>
## dwarf_10_activity_done

<div class="yarn-node" data-title="dwarf_10_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buon lavoro! <span class="yarn-meta">#line:054d57b </span></span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_10_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_10 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-child"></a>
## spawned_child

<div class="yarn-node" data-title="spawned_child"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawned automatically in the scene</span>
<span class="yarn-header-dim">// each time you meet them they say one random line</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Mi piace contare i nani con il mio amico. <span class="yarn-meta">#line:05166a6 </span></span>
<span class="yarn-line">Alziamo lo sguardo verso la Sky Tower. <span class="yarn-meta">#line:068b00a </span></span>
<span class="yarn-line">Di notte lo spettacolo delle fontane è luminoso. <span class="yarn-meta">#line:096ce85 </span></span>
<span class="yarn-line">Lo zoo ospita animali provenienti da molti paesi. <span class="yarn-meta">#line:03c5f8f </span></span>

</code></pre></div>

<a id="ys-node-spawned-tourist"></a>
## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ho una mappa per trovare altri nani. <span class="yarn-meta">#line:0738670 </span></span>
<span class="yarn-line">Le torri della cattedrale sembrano molto alte. <span class="yarn-meta">#line:0eaaf50 </span></span>
<span class="yarn-line">Il grande dipinto ci circonda. <span class="yarn-meta">#line:0606b35 </span></span>

</code></pre></div>

<a id="ys-node-spawned-local"></a>
## spawned_local

<div class="yarn-node" data-title="spawned_local"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">spawn_group: locals </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La piazza del mercato è affollata e luminosa. <span class="yarn-meta">#line:052bf17 </span></span>
<span class="yarn-line">La sala ha un tetto alto e rotondo. <span class="yarn-meta">#line:05c1526 </span></span>
<span class="yarn-line">Il guardiano delle chiavi sorveglia la porta dell'ascensore. <span class="yarn-meta">#line:0209b93 </span></span>

</code></pre></div>

<a id="ys-node-spawned-reader"></a>
## spawned_reader

<div class="yarn-node" data-title="spawned_reader"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GIRL</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ho letto un libro di Olga. <span class="yarn-meta">#line:0246124 </span></span>
<span class="yarn-line">Olga ha vinto un grande premio. <span class="yarn-meta">#line:0eeaf85 </span></span>
<span class="yarn-line">Un giorno voglio scrivere storie. <span class="yarn-meta">#line:08c1def </span></span>

</code></pre></div>


